
var distance;
var targetChar: Transform;
var lookAtDistance = 15.0;
var chaseRange = 10.0;
var moveSpeed = 5.0;
var normalMoveSpeed = 1.0;
var damping = 6.0;
var attackRange = 15.0;
var isItAttacking = false;
var moveDirection : Vector3;
var applyMud :boolean;
var damagePerSecond = 2;
private var verticalSpeed=0.0;
private var gravity = 20.0;
private var collisionFlags : CollisionFlags; 

private var character : CharacterController;
     
function Start (){
    character = GetComponent(CharacterController);
}
    
    

function Update() {
	ApplyGravity();

    distance = Vector3.Distance(targetChar.position, transform.position);

    if (distance < lookAtDistance) {
        isItAttacking = false;
        lookAt();
    }
    if (distance > lookAtDistance) {
    }
    if (distance < chaseRange && distance > attackRange) {
        move();
    }
    if (distance < attackRange) {
        attack();
    }
    
}


function lookAt() {
    var rotation = Quaternion.LookRotation(targetChar.position - transform.position);
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
}

function move() {

	if(applyMud){
		moveSpeed = normalMoveSpeed/5;
	}
	else{
		moveSpeed = normalMoveSpeed;
	}
	var enemyRenderer = transform.GetComponentInChildren.<Renderer>();
	enemyRenderer.material.color = Color.gray;
    moveDirection = targetChar.position - transform.position;
    moveDirection.y = 0;
    collisionFlags = character.Move(moveDirection * moveSpeed * Time.deltaTime + Vector3 (0, verticalSpeed, 0));
}

function attack(){
//Play an instance using attached AudioSource component
 audio.Play();
 
	var enemyRenderer = transform.GetComponentInChildren.<Renderer>();
	enemyRenderer.material.color = Color.red;


 
	targetChar.GetComponent("ThirdPersonController").attackedByEnemy(damagePerSecond);
}
 
function ApplyGravity ()
{
		if (IsGrounded ())
			verticalSpeed = 0.0;
		else
			verticalSpeed = -gravity * Time.deltaTime;
}




function OnTriggerEnter(Hit : Collider)
{
   if(Hit.name == "Mud")
   {
 		applyMud = true;
   }      
}

function IsGrounded () {
	return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
}