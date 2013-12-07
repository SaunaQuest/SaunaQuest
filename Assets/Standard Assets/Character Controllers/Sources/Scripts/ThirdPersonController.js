
// Require a character controller to be attached to the same game object
@script RequireComponent(CharacterController)

public var idleAnimation : AnimationClip;
public var walkAnimation : AnimationClip;
public var runAnimation : AnimationClip;
public var jumpPoseAnimation : AnimationClip;
public var deadAnimation : AnimationClip;

public var walkMaxAnimationSpeed : float = 0.75;
public var trotMaxAnimationSpeed : float = 1.0;
public var runMaxAnimationSpeed : float = 1.0;
public var jumpAnimationSpeed : float = 1.15;
public var landAnimationSpeed : float = 1.0;
public var deadAnimationSpeed : float = 1.0;

public var whiskyHeatAmount : float = 20.0;

private var applySlippery : boolean =false;
private var applyMud : boolean = false;

public var gameEnds : boolean = false;

private var _animation : Animation;

enum CharacterState {
	Idle = 0,
	Walking = 1,
	Trotting = 2,
	Running = 3,
	Jumping = 4,
	Dead = 5
}

private var _characterState : CharacterState;

// The speed when walking
var walkSpeed = 50;
// after trotAfterSeconds of walking we trot with trotSpeed
var trotSpeed = 50;
// when pressing "Fire3" button (cmd) we start running
var runSpeed = 90;
var normalWalkSpeed = 50;
var normalTrotSpeed = 50;
var normalRunSpeed = 90;

var inAirControlAcceleration = 3.0;

// How high do we jump when pressing jump and letting go immediately
var jumpHeight = 0.5;

// The gravity for the character
var gravity = 20.0;
// The gravity in controlled descent mode
var speedSmoothing = 10.0;
var rotateSpeed = 100.0;
var trotAfterSeconds = 3.0;

var canJump = true;

private var jumpRepeatTime = 0.05;
private var jumpTimeout = 0.15;
private var groundedTimeout = 0.25;

// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
private var lockCameraTimer = 0.0;

// The current move direction in x-z
private var moveDirection = Vector3.zero;
// The current vertical speed
private var verticalSpeed = 0.0;
// The current x-z move speed
private var moveSpeed = 0.0;

// The last collision flags returned from controller.Move
private var collisionFlags : CollisionFlags; 

// Are we jumping? (Initiated with jump button and not grounded yet)
private var jumping = false;
private var jumpingReachedApex = false;

// Are we moving backwards (This locks the camera to not do a 180 degree spin)
private var movingBack = false;
// Is the user pressing any keys?
private var isMoving = false;
// When did the user start walking (Used for going into trot after a while)
private var walkTimeStart = 0.0;
// Last time the jump button was clicked down
private var lastJumpButtonTime = -10.0;
// Last time we performed a jump
private var lastJumpTime = -1.0;


// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
private var lastJumpStartHeight = 0.0;


private var inAirVelocity = Vector3.zero;

private var lastGroundedTime = 0.0;


private var isControllable = true;

// If the player ends up on a slope which is at least the Slope Limit as set on the character controller, then he will slide down
var slideWhenOverSlopeLimit = true;
// If checked and the player is on an object tagged "Slide", he will slide down it regardless of the slope limit
var slideOnTaggedObjects = true;
var slideSpeed=10;
private var hit : RaycastHit;
private var rayDistance : float;
private var slideLimit : float;
private var contactPoint : Vector3;
private var isSliding = false;
private var slideDirection: Vector3 = Vector3.zero;

private var slipperyDirection: Vector3 = Vector3.zero;

function Awake ()
{
	moveDirection = transform.TransformDirection(Vector3.forward);
	
	_animation = GetComponent(Animation);
	if(!_animation)
		Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
	
	/*
public var idleAnimation : AnimationClip;
public var walkAnimation : AnimationClip;
public var runAnimation : AnimationClip;
public var jumpPoseAnimation : AnimationClip;	
	*/
	if(!idleAnimation) {
		_animation = null;
		Debug.Log("No idle animation found. Turning off animations.");
	}
	if(!walkAnimation) {
		_animation = null;
		Debug.Log("No walk animation found. Turning off animations.");
	}
	if(!runAnimation) {
		_animation = null;
		Debug.Log("No run animation found. Turning off animations.");
	}
	if(!jumpPoseAnimation && canJump) {
		_animation = null;
		Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
	}
	if(!deadAnimation) {
		_animation = null;
		Debug.Log("No dead animation found. Turning off animations.");
	}	
			
}

function Update() {
	
	if(gameEnds){
		if (Input.GetButtonDown ("Jump"))
		{
			
			Application.LoadLevel("GameOnScene");
		} 
	}
	
	
	if (!isControllable)
	{
		// kill all inputs if not controllable.
		Input.ResetInputAxes();
	}
	
	if (Input.GetButtonDown ("Jump"))
	{
		lastJumpButtonTime = Time.time;
	} 
	
	if(gameEnds){
		walkSpeed=0;
		trotSpeed=0;
		runSpeed=0;
	}
	else if(applyMud){
		walkSpeed = normalWalkSpeed/10;
		trotSpeed = normalTrotSpeed/10;
		runSpeed = normalRunSpeed/10;
	}
	else{
		walkSpeed = normalWalkSpeed;
		trotSpeed = normalTrotSpeed;
		runSpeed = normalRunSpeed;
	}
	
	//Apply Slide
	ApplySlide();
	
	UpdateSmoothedMovementDirection();  
 
	
	// Apply gravity
	// - extra power jump modifies gravity
	// - controlledDescent mode modifies gravity
	ApplyGravity ();

	// Apply jumping logic
	ApplyJumping ();
	
	var controller : CharacterController = GetComponent(CharacterController);	

	// Calculate actual motion
	
	var movement;
	if(!movingBack){
		movement = moveDirection * moveSpeed + Vector3 (0, verticalSpeed, 0) + inAirVelocity + slideDirection*50;
	}
	else{
		movement = -moveDirection* moveSpeed + Vector3 (0, verticalSpeed, 0) + inAirVelocity + slideDirection*50;
	}
	
	movement *= Time.deltaTime;
	
	if(applySlippery && !applyMud){
		slipperyDirection = moveDirection * Random.RandomRange(5,10);
	}
	else{
		slipperyDirection = Vector3.zero;
	}
	collisionFlags = controller.Move(slipperyDirection*Time.deltaTime);
	collisionFlags = controller.Move(movement);
	
	
	
	// ANIMATION sector
	if(_animation) {
		if(_characterState == CharacterState.Dead){
				_animation[jumpPoseAnimation.name].wrapMode = WrapMode.Once;	
				_animation.CrossFade(deadAnimation.name);
		 } 	
		if(_characterState == CharacterState.Jumping) 
		{
			if(!jumpingReachedApex) {
				_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
				_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
				_animation.CrossFade(jumpPoseAnimation.name);
			} else {
				_animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
				_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
				_animation.CrossFade(jumpPoseAnimation.name);				
			}
		} 
		else 
		{
			if(controller.velocity.sqrMagnitude < 0.1) {
				_animation.CrossFade(idleAnimation.name);
			}
			else 
			{
				if(_characterState == CharacterState.Running) {
					_animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0, runMaxAnimationSpeed);
					_animation.CrossFade(runAnimation.name);	
				}
				else if(_characterState == CharacterState.Trotting) {
					_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0, trotMaxAnimationSpeed);
					_animation.CrossFade(walkAnimation.name);	
				}
				else if(_characterState == CharacterState.Walking) {
					_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0, walkMaxAnimationSpeed);
					_animation.CrossFade(walkAnimation.name);	
				}
				
			}
		}
	}
	// ANIMATION sector
	
	// Set rotation to the move direction
	if (IsGrounded())
	{
			transform.rotation = Quaternion.LookRotation(moveDirection);	
	}	
	else
	{
		var xzMove = movement;
		xzMove.y = 0;
		if (xzMove.sqrMagnitude > 0.001)
		{
			transform.rotation = Quaternion.LookRotation(xzMove);
		}
	}	
	
	// We are in jump mode but just became grounded
	if (IsGrounded())
	{
		lastGroundedTime = Time.time;
		inAirVelocity = Vector3.zero;
		if (jumping)
		{
			jumping = false;
			SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
		}
	}
}

function UpdateSmoothedMovementDirection ()
{
	var cameraTransform = Camera.main.transform;
	var grounded = IsGrounded();
	
	// Forward vector relative to the camera along the x-z plane
	var forward = cameraTransform.TransformDirection(Vector3.forward);
	forward.y = 0;
	forward = forward.normalized;

	// Right vector relative to the camera
	// Always orthogonal to the forward vector
	var right = Vector3(forward.z, 0, -forward.x);
	
	var v = Input.GetAxisRaw("Vertical");
	var h = Input.GetAxisRaw("Horizontal");

	// Are we moving backwards or looking backwards
	if (v < -0.2){
		movingBack = true;
	}
	else{
		movingBack = false;
	}

	
	var wasMoving = isMoving;
	isMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;
		
	// Target direction relative to the camera
	var targetDirection;
	 targetDirection = h * right + v * forward;
	// Grounded controls
	if (grounded)
	{

		// Lock camera for short period when transitioning moving & standing still
		lockCameraTimer += Time.deltaTime;
		if (isMoving != wasMoving)
			lockCameraTimer = 0.0;

		// We store speed and direction seperately,
		// so that when the character stands still we still have a valid forward direction
		// moveDirection is always normalized, and we only update it if there is user input.
		if (targetDirection != Vector3.zero && !movingBack)
		{
			// If we are really slow, just snap to the target direction
			if (moveSpeed < walkSpeed *1.5 && grounded)
			{
				moveDirection = Vector3.RotateTowards(moveDirection*0.1, targetDirection, rotateSpeed*3 * Mathf.Deg2Rad * Time.deltaTime, 1000);		
				moveDirection = moveDirection.normalized;
			}
			// Otherwise smoothly turn towards it
			else
			{
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);							
				moveDirection = moveDirection.normalized;
			}

		}
		
		// Smooth the speed based on the current target direction
		var curSmooth = speedSmoothing * Time.deltaTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0);
	
		_characterState = CharacterState.Idle;
		
		// Pick speed modifier
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{
			targetSpeed *= runSpeed;
			_characterState = CharacterState.Running;
		}
		
		else if (Time.time - trotAfterSeconds > walkTimeStart)
		{
			targetSpeed *= trotSpeed;
			_characterState = CharacterState.Trotting;
		}
		else
		{
			targetSpeed *= walkSpeed;
			_characterState = CharacterState.Walking;
		}
		
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		
		// Reset walk time start when we slow down
		if (moveSpeed < walkSpeed * 0.3)
			walkTimeStart = Time.time;
	}
	// In air controls
	else
	{
		// Lock camera while in air
		if (jumping)
			lockCameraTimer = 0.0;

		if (isMoving)
			inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
	}
	

		
}



function ApplyJumping ()
{
	// Prevent jumping too fast after each other
	if (lastJumpTime + jumpRepeatTime > Time.time)
		return;

	if (IsGrounded() && !IsSliding()) {
		// Jump
		// - Only when pressing the button down
		// - With a timeout so you can press the button slightly before landing		
		if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
			verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
			SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
		}
	}
}

function ApplySlide()
    {
        if (!IsGrounded())
		 return;
            
        slideDirection = Vector3.zero;

        var hitInfo : RaycastHit ;
        
        var controller : CharacterController = GetComponent(CharacterController);

        if (Physics.Raycast(contactPoint+Vector3.up, Vector3.down,hitInfo))
        {
        	var currentSlope = Vector3.Angle(hitInfo.normal, Vector3.up);
			if (currentSlope > controller.slopeLimit){		
                slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
                isSliding=true;
             }
        	else{
        		isSliding =false;
       		 }
        }


        
    }

function ApplyGravity ()
{
	if (isControllable)	// don't move player at all if not controllable.
	{
		
		
		// When we reach the apex of the jump we send out a message
		if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0)
		{
			jumpingReachedApex = true;
			SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
		}
	
		if (IsGrounded ())
			verticalSpeed = 0.0;
		else
			verticalSpeed -= gravity * Time.deltaTime;
	}
}

function CalculateJumpVerticalSpeed (targetJumpHeight : float)
{
	// From the jump height and gravity we deduce the upwards speed 
	// for the character to reach at the apex.
	return Mathf.Sqrt(2 * targetJumpHeight * gravity);
}

function DidJump ()
{
	jumping = true;
	jumpingReachedApex = false;
	lastJumpTime = Time.time;
	lastJumpStartHeight = transform.position.y;
	lastJumpButtonTime = -10;
	
	_characterState = CharacterState.Jumping;
}



function OnControllerColliderHit (hit : ControllerColliderHit )
{

	contactPoint = hit.point;
	if (hit.moveDirection.y > 0.01) 
		return;
}

function GetSpeed () {
	return moveSpeed;
}

function IsJumping () {
	return jumping;
} 

function IsSliding(){
	 return isSliding;
}

function IsGrounded () {
	return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
}

function GetDirection () {
	return moveDirection;
}

function IsMovingBackwards () {
	return movingBack;
}

function GetLockCameraTimer () 
{
	return lockCameraTimer;
}

function IsMoving ()  : boolean
{
	return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5;
}

function HasJumpReachedApex ()
{
	return jumpingReachedApex;
}

function IsGroundedWithTimeout ()
{
	return lastGroundedTime + groundedTimeout > Time.time;
}

function Reset ()
{
	gameObject.tag = "Player";
}


function OnCollisionEnter(collision : Collision)
{

}

function OnCollisionExit(collision : Collision)
{

}



function OnTriggerEnter(Hit : Collider)
{
   if(Hit.name == "Mud")
   {
 		//walkSpeed /= 10; 
 		//runSpeed /= 10;
 		applyMud = true;
   }
   if(Hit.name == "Ice")
   {
   		applySlippery = true;
   		//slipperyDirection = Vector3(Random.Range(5,10),0,Random.Range(5,10)); 
   		slipperyDirection = moveDirection;
   }       
   if(Hit.name == "Whisky")
   {
   		Destroy(Hit.gameObject);
   		if(transform.GetComponent("HealthBarScript").curHealth+whiskyHeatAmount > 
   		transform.GetComponent("HealthBarScript").maxHealth){
   			transform.GetComponent("HealthBarScript").curHealth = 
   			transform.GetComponent("HealthBarScript").maxHealth;
   		}
   		else{
   			transform.GetComponent("HealthBarScript").curHealth += whiskyHeatAmount;
   		}
   }
   if(Hit.name == "Finish")
   {
   		transform.GetComponent("TimerDisplayScript").disableTimer();
   		transform.GetComponent("HealthBarScript").disableHealthBar();
   		setGameCompleted();
   }
}

function OnTriggerExit(Hit : Collider)
{
   if(Hit.name == "Mud")
   {
 		//walkSpeed *= 10;
 		//runSpeed *= 10;
 		applyMud = false;
   }
   
   if(Hit.name == "Ice") // you can compare tags instead: if (Hit.tag = "Player")
   {
   		applySlippery=false;
   		//slipperyDirection = Vector3.zero;    
   }       

}

function getCharacterState(){
	return _characterState;
}

function setCharacterDead(){
	_characterState=CharacterState.Dead;
	gameEnds = true;
	var text : String= "You have failed!!!\n";
	text+= "\n";
	text+= "Press space to retry again\n";
	transform.GetComponent("GameOverDisplay").textToShow = text;
	transform.GetComponent("GameOverDisplay").showText = true;
	
}

function setGameCompleted(){
	_characterState=CharacterState.Dead;
	gameEnds = true;
	var text : String= "Congratulations! You have completed the game!\n";
	text+= "\n";
	text+= "Your score is:";
	var displayMinutes=transform.GetComponent("TimerDisplayScript").displayMinutes;
	var displaySeconds=transform.GetComponent("TimerDisplayScript").displaySeconds;	
	text+=displayMinutes+" minutes and "+displaySeconds+ " seconds\n";
	text+= "Press space to retry again\n";	
	transform.GetComponent("GameOverDisplay").textToShow = text;
	transform.GetComponent("GameOverDisplay").showText = true;
}