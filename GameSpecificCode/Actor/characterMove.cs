using UnityEngine;
using System.Collections;

public class characterMove : MonoBehaviour {

	CharacterController controller;
	[HideInInspector]
	public Animator anim;
	public float turnSpeed=7f;
	public float acceleration=35f;
	public float deceleration=45f;
	public float moveSpeed=16f;
	public bool SpeedChangeOnAttack=false;
	public float attackingMoveSpeed=16f;
	public float gravity=30;
	public float jumpForce=12;
	public Transform body;
	Vector3 velocity;
	float turn = 1;
	public bool canReceiveKnockback=true;
	public bool canJump;
	[HideInInspector]
	public bool canMove;
	[HideInInspector]
	public Vector3 moveAxis;
	
	[HideInInspector]
	public float currentMoveSpeed;
	[HideInInspector]
	public float speedBuff;
	float distToGround;
	bool isPlayer=false;
	
	void Start() {
		controller = GetComponent<CharacterController>();
		controller.Move(Vector3.down);
		anim = GetComponentInChildren<Animator>();
		distToGround = GetComponent<Collider>().bounds.extents.y;
		currentMoveSpeed = moveSpeed;
		if(!body)
			body=transform;
		if(GetComponent<PlayerController>())
			isPlayer=true;
		EnableControl();
	}
 
    IEnumerator Knockback(Vector3 velocity, float duration)	{
		bool Init=false;
		Vector3 kbMoveAxis=Vector3.zero;
		while (duration > 0)
		{
			if(!Init) {
				kbMoveAxis=Vector3.zero;
				kbMoveAxis=velocity;
				Init=true;
			}
			var mult = (Vector3.Dot(kbMoveAxis, body.transform.forward) + 1) / 2;
			mult += turn * (1 - mult);
			
			Vector3 kbVelocity = Vector3.MoveTowards(velocity, kbMoveAxis * 10 * mult, acceleration * Time.deltaTime);
			controller.Move(kbVelocity * Time.deltaTime);

			duration -= Time.deltaTime;
			yield return 0;
		}
	}
	
	public void TakeKnockback(Vector3 kb,float duration) {
		if(!canReceiveKnockback)
			return;
		StartCoroutine(Knockback(kb,duration));
	}
	
	public void TryMovement(float h, float v) {
		if(!canMove)
			return;
		
		moveAxis = Camera.main.transform.TransformDirection(new Vector3(h,0,v));
		moveAxis.y = 0;
		
		if (!moveAxis.Approximately(Vector3.zero))
			moveAxis.Normalize();
		else
			moveAxis = Vector3.zero;
		
		TryApplyMovement();
		
	}
	
	public void TryMovement(float h, float v, bool LookWhereMove) {
		TryMovement(h,v);
		
		if(isPlayer&&anim) {
			if(LookWhereMove) {
				anim.SetFloat("VelX", 0);
				anim.SetFloat("VelZ", 0);
			} else {
				GetDirectionForBlendtree(h,v);
			}
		}
		
		if(LookWhereMove)
			RotateTo(moveAxis);
			
	}
	
	public bool TryApplyMovement() {
		addGravity();
		if (moving)	{
			var mult = (Vector3.Dot(moveAxis, body.transform.forward) + 1) / 2;
			mult += turn * (1 - mult);
			
			moveVelocity = Vector3.MoveTowards(moveVelocity, moveAxis * (currentMoveSpeed+speedBuff) * mult, acceleration * Time.deltaTime);
			controller.Move(moveVelocity * Time.deltaTime);
			if(anim)	        	
	        	anim.SetBool("Moving",true);
			return true;
		}
		else {
			moveVelocity = Vector3.MoveTowards(moveVelocity, Vector3.zero, deceleration * Time.deltaTime);
			if(anim)
	        	anim.SetBool("Moving",false);
			if (!moveVelocity.Approximately(Vector3.zero))
				controller.Move(moveVelocity * Time.deltaTime);
			return false;
		}
	}
	
	public void RotateTo(Vector3 moveDir, bool instant=false) {
		if(!body || moveDir.IsZero())
			return;
		
		Quaternion newRotation;
		
		if(instant) {
			newRotation = Quaternion.LookRotation(moveDir);
			body.rotation = Quaternion.Euler( 0, newRotation.eulerAngles.y, 0);
			return;
		}
				
		turn = Mathf.MoveTowards(turn, 1, 3 * Time.deltaTime);
		
		newRotation = Quaternion.Lerp(body.rotation, Quaternion.LookRotation(moveDir,Vector3.up), turnSpeed * Time.deltaTime);
		body.rotation = Quaternion.Euler( 0, newRotation.eulerAngles.y, 0);
	}
	
	public void Jump() {
		if (!IsGrounded()||!canJump)
			return;
		velocity.y = jumpForce;
		if(anim)
			anim.Play("Jump",2);
	}
	
	public void DisableControl() {
		canMove = false;
		velocity = Vector3.zero;
	}
	
	public void EnableControl()	{
		canMove = true;
	}
	
	void GetDirectionForBlendtree(float inputX, float inputY) {    
		Vector3 inputDirec=new Vector3(inputX,inputY,0).normalized;
	    Vector3 leftStickInputAxis = inputDirec;
	    var a = SignedAngle(new Vector3(leftStickInputAxis.x, 0, leftStickInputAxis.y), body.transform.forward);

	    if (a < 0)
	        a *= -1;
	    else
	        a = 360 - a;

	    a += Camera.main.transform.eulerAngles.y;
	
	    var aRad = Mathf.Deg2Rad*a;
	    
	    leftStickInputAxis = new Vector2(Mathf.Sin(aRad), Mathf.Cos(aRad));

	    anim.SetFloat("VelX", leftStickInputAxis.x);
	    anim.SetFloat("VelZ", leftStickInputAxis.y);
	}
	
	void addGravity() {
		velocity.y -= gravity * Time.deltaTime;
		controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
	}
		
	Vector3 moveVelocity {
		get { return new Vector3(velocity.x, 0, velocity.z); }
		set {
			velocity.x = value.x;
			velocity.z = value.z;
		}
	}
	
	public void isAttacking(bool attacking) {
		if(attacking) {
			if(SpeedChangeOnAttack)
				currentMoveSpeed=attackingMoveSpeed;
		} else {
			if(SpeedChangeOnAttack)
				currentMoveSpeed=moveSpeed;
		}
	}
	
	public bool IsGrounded() {
	  return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}
	
	private float SignedAngle(Vector3 a, Vector3 b)
	{
	    return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Cross(a, b).y);
	}
	
	bool moving {
		get { return !moveAxis.Approximately(Vector3.zero); }
	}
}
