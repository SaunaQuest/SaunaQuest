var distance;
var target: Transform;
var lookAtDistance = 15.0;
var chaseRange = 10.0;
var moveSpeed = 5.0;
var damping = 6.0;
var attackRange = 15.0;
var isItAttacking = false;

function Update() {
/*

    distance = Vector3.Distance(target.position, transform.position);

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
    
    */
}


function lookAt() {
    var rotation = Quaternion.LookRotation(target.position - transform.position);
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
}

function move() {
	var enemyRenderer = transform.GetComponentInChildren.<Renderer>();
	enemyRenderer.material.color = Color.gray;
    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
}

function attack(){
	var enemyRenderer = transform.GetComponentInChildren.<Renderer>();
	enemyRenderer.material.color = Color.red;
}