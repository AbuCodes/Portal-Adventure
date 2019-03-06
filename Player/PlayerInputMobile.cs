using UnityEngine;
using System.Collections;

public class PlayerInputMobile : MonoBehaviour {

	/*
	
	private CNJoystick movejoystick;
	private CNJoystick attackjoystick;
	private Camera mainCamera;
    private Vector3 totalMove;
    private Vector3 totalAttack;
    private bool moveTweakedLastFrame;
    private bool attackTweakedLastFrame;
    
    PlayerInput pI;
    
	void Start () {
		pI = GetComponent<PlayerInput>();
		if(movejoystick==null)
			movejoystick = GameObject.FindWithTag("MovementJoystick").GetComponent<CNJoystick>();
		movejoystick.JoystickMovedEvent += JoystickMovedEventHandler;
        movejoystick.FingerLiftedEvent += StopMoving;
        
        if(attackjoystick==null)
			attackjoystick = GameObject.FindWithTag("AttackJoystick").GetComponent<CNJoystick>();
		attackjoystick.JoystickMovedEvent += AttackJoystickMovedEventHandler;
        attackjoystick.FingerLiftedEvent += StopAttacking;
        
        totalMove = Vector3.zero;
        moveTweakedLastFrame = false;
	}

	void Update () {
		if(!moveTweakedLastFrame)
            totalMove = Vector3.zero;
            
        pI.mobileH = totalMove.x;
		pI.mobileV = totalMove.z;
		
		moveTweakedLastFrame = false;
		
		if(!attackTweakedLastFrame)
            totalAttack = Vector3.zero;
            
        pI.mobileHAttack = totalAttack.x;
		pI.mobileVAttack = totalAttack.z;
		
		attackTweakedLastFrame = false;
	}
	
	void StopMoving()
    {
        totalMove = Vector3.zero;
    }

    private void JoystickMovedEventHandler(Vector3 dragVector)
    {
        dragVector.z = dragVector.y;
        dragVector.y = 0f;
        Vector3 movement = Camera.main.transform.TransformDirection(dragVector);
        movement.y = 0f;
        movement.Normalize();
        
        float h = 0;
		float v = 0;
		
		if(pI.canInput) {
			h += movement.x;
			v += movement.z;
		}
		
		totalMove.x = h;
        totalMove.z = v;

        moveTweakedLastFrame = true;
    }
    
    void StopAttacking()
    {
        totalMove = Vector3.zero;
    }

    private void AttackJoystickMovedEventHandler(Vector3 dragVector)
    {
        dragVector.z = dragVector.y;
        dragVector.y = 0f;
        Vector3 movement = Camera.main.transform.TransformDirection(dragVector);
        movement.y = 0f;
        movement.Normalize();
        
        float h = 0;
		float v = 0;
		
		if(pI.canInput) {
			h += movement.x;
			v += movement.z;
		}
		
		totalAttack.x = h;
        totalAttack.z = v;

        attackTweakedLastFrame = true;
    }
    
	*/
	
}
