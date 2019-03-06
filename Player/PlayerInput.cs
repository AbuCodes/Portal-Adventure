using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	public enum InputMethod {
			SingleplayerKeyboard,
			SingleplayerMouse,
			mpKeyboard1,
			mpKeyboard2,
			Gamepad1,
			Gamepad2,
			Gamepad3,
			Gamepad4
	}
	
	[HideInInspector]
	public characterMove cM;
	PlayerController pC;
	
	string hMoveAxis;
	string vMoveAxis;
	string hFireAxis;
	string vFireAxis;
	
	float timeOfAttack;
	
	Transform reticle;
	
	[HideInInspector]
	public Vector3 attackDirection;
	
	[HideInInspector]
	public bool canInput=true;
	
	public InputMethod currentInput;

	[HideInInspector]
	public float mobileH;
	[HideInInspector]
	public float mobileV;
	
	[HideInInspector]
	public float mobileHAttack;
	[HideInInspector]
	public float mobileVAttack;
    
    KeyCode StartButton = KeyCode.Escape;
    KeyCode JumpButton = KeyCode.C;
    KeyCode DropWeaponButton = KeyCode.E;
    KeyCode SwapWeaponButton = KeyCode.Q;
    
    
    PauseScreen Pause;
    
	void Start () {
		cM=GetComponent<characterMove>();
		pC=GetComponent<PlayerController>();
		
		Pause=Camera.main.GetComponent<PauseScreen>();
		
		SetInput(currentInput);

		timeOfAttack = Time.time - 2;
	}

	public void SetInput(InputMethod newIM)
	{
		currentInput = newIM;
		
		if(reticle)
			Destroy(reticle.gameObject);
		
		switch (currentInput)
		{
			case InputMethod.SingleplayerKeyboard:
					hMoveAxis="HorizontalA";
					vMoveAxis="VerticalA";
					hFireAxis="HorizontalArrow";
					vFireAxis="VerticalArrow";
					StartButton=KeyCode.Escape;
					JumpButton=KeyCode.C;
					DropWeaponButton=KeyCode.E;
					SwapWeaponButton=KeyCode.Q;
				break;
			case InputMethod.SingleplayerMouse:
					hMoveAxis="HorizontalA";
					vMoveAxis="VerticalA";
					StartButton=KeyCode.Escape;
					JumpButton=KeyCode.C;
					DropWeaponButton=KeyCode.E;
					SwapWeaponButton=KeyCode.Q;
					SpawnReticle();
				break;
			case InputMethod.mpKeyboard1:
					hMoveAxis="HorizontalA";
					vMoveAxis="VerticalA";
					hFireAxis="HorizontalH";
					vFireAxis="VerticalH";
					StartButton=KeyCode.Escape;
					JumpButton=KeyCode.C;
					DropWeaponButton=KeyCode.E;
					SwapWeaponButton=KeyCode.Q;
				break;
			case InputMethod.mpKeyboard2:
					hMoveAxis="HorizontalArrow";
					vMoveAxis="VerticalArrow";
					hFireAxis="Horizontal1";
					vFireAxis="Vertical1";
					StartButton=KeyCode.KeypadMinus;
					JumpButton=KeyCode.Keypad0;
					DropWeaponButton=KeyCode.Keypad9;
					SwapWeaponButton=KeyCode.Keypad7;
				break;
			case InputMethod.Gamepad1:
					hMoveAxis="HorizontalAGP1";
					vMoveAxis="VerticalAGP1";
					if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer) {
						hFireAxis="HorizontalArrowGP1Mac";
						vFireAxis="VerticalArrowGP1Mac";
						StartButton=KeyCode.Joystick1Button9;
						JumpButton=KeyCode.Joystick1Button16;
						DropWeaponButton=KeyCode.Joystick1Button17;
						SwapWeaponButton=KeyCode.Joystick1Button18;
					} else {
						hFireAxis="HorizontalArrowGP1";
						vFireAxis="VerticalArrowGP1";
						StartButton=KeyCode.Joystick1Button7;
						JumpButton=KeyCode.Joystick1Button0;
						DropWeaponButton=KeyCode.Joystick1Button1;
						SwapWeaponButton=KeyCode.Joystick1Button2;
					}
				break;
			case InputMethod.Gamepad2:
					hMoveAxis="HorizontalGP2";
					vMoveAxis="VerticalGP2";
					if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer) {
						hFireAxis="HorizontalArrowGP2Mac";
						vFireAxis="VerticalArrowGP2Mac";
						StartButton=KeyCode.Joystick2Button9;
						JumpButton=KeyCode.Joystick2Button16;
						DropWeaponButton=KeyCode.Joystick2Button17;
						SwapWeaponButton=KeyCode.Joystick2Button18;
					} else {
						hFireAxis="HorizontalArrowGP2";
						vFireAxis="VerticalArrowGP2";
						StartButton=KeyCode.Joystick2Button7;
						JumpButton=KeyCode.Joystick2Button0;
						DropWeaponButton=KeyCode.Joystick2Button1;
						SwapWeaponButton=KeyCode.Joystick2Button2;
					}
				break;
			case InputMethod.Gamepad3:
					hMoveAxis="HorizontalGP3";
					vMoveAxis="VerticalGP3";
					if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer) {
						hFireAxis="HorizontalArrowGP3Mac";
						vFireAxis="VerticalArrowGP3Mac";
						StartButton=KeyCode.Joystick3Button9;
						JumpButton=KeyCode.Joystick3Button16;
						DropWeaponButton=KeyCode.Joystick3Button17;
						SwapWeaponButton=KeyCode.Joystick3Button18;
					} else {
						hFireAxis="HorizontalArrowGP3";
						vFireAxis="VerticalArrowGP3";
						StartButton=KeyCode.Joystick3Button7;
						JumpButton=KeyCode.Joystick3Button0;
						DropWeaponButton=KeyCode.Joystick3Button1;
						SwapWeaponButton=KeyCode.Joystick3Button2;
					}
				break;
			case InputMethod.Gamepad4:
					hMoveAxis="HorizontalGP4";
					vMoveAxis="VerticalGP4";
					if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer) {
						hFireAxis="HorizontalArrowGP4Mac";
						vFireAxis="VerticalArrowGP4Mac";
						StartButton=KeyCode.Joystick4Button9;
						JumpButton=KeyCode.Joystick4Button16;
						DropWeaponButton=KeyCode.Joystick4Button17;
						SwapWeaponButton=KeyCode.Joystick4Button18;
					} else {
						hFireAxis="HorizontalArrowGP4";
						vFireAxis="VerticalArrowGP4";
						StartButton=KeyCode.Joystick4Button7;
						JumpButton=KeyCode.Joystick4Button0;
						DropWeaponButton=KeyCode.Joystick4Button1;
						SwapWeaponButton=KeyCode.Joystick4Button2;
					}
				break;
		}
	}
	
	void Update () {
		if(pC.cH.Dead)
			return;

		GetInput();
		
		if(currentInput==InputMethod.SingleplayerMouse)
			GetMouseInput();
	}
		
	void GetInput() {
		float h = 0;
		float v = 0;
		float hAttack = 0;
		float vAttack = 0;
		
		if(Pause) {
			if (Input.GetKeyDown(StartButton))
				Pause.TogglePauseMenu();
		}
		
		if(canInput&&Time.timeScale!=0) {
			h += Input.GetAxisRaw(hMoveAxis);
			v += Input.GetAxisRaw(vMoveAxis);
			
			h += mobileH;
			v += mobileV;
			hAttack += mobileHAttack;
			vAttack += mobileVAttack;
			
			if(currentInput != InputMethod.SingleplayerMouse) {
				hAttack += Input.GetAxisRaw(hFireAxis);
				vAttack += Input.GetAxisRaw(vFireAxis);
			}
		
			if(Input.GetKeyDown(JumpButton))
				cM.Jump();
			
			if (Input.GetKeyDown(DropWeaponButton))
        	   pC.weaponManager.DropWeapon();
        	   
        	if (Input.GetKeyDown(SwapWeaponButton))
        	   pC.weaponManager.TrySwapWeapon();
		}
		
		cM.TryMovement(h,v,LookWhereMove());

		TryAttack(hAttack,vAttack);
	}
	
	void GetMouseInput() {
		ReticleMove();
		
		Vector3 ReticleDir = transform.DirectionTo(reticle.transform);

		if(Input.GetMouseButtonDown(1)) {
			//Do something on button 2 press?
		}
		if(Input.GetMouseButton(0)) {
			TryAttack(ReticleDir.x,ReticleDir.z);
		}
		
		if(!LookWhereMove()&&transform.DistanceTo(reticle.transform)>5)
			cM.RotateTo(ReticleDir,usingMouse());
	}
	
	void TryAttack(float h, float v) {
		if(h == 0f && v == 0f)
			return;

		attackDirection = new Vector3(h,0,v).normalized;
		
		cM.RotateTo(attackDirection,usingMouse());
		
		pC.weaponManager.currentWeapon.Attack(attackDirection);
		
		timeOfAttack = Time.time;
	}
	
	void SpawnReticle() {
		if(reticle)
			return;
			
		GameObject reticleInst=Instantiate(
				Resources.Load("Reticle"),
				Vector3.zero,
				Quaternion.identity
		) as GameObject;

		reticle=reticleInst.transform;
	}
	
	void ReticleMove() {
		if(!reticle)
			SpawnReticle();

		Plane plane=new Plane(Vector3.up,Vector3.zero);
		float hitDist=0f;
		Ray ray;
		Vector3 dest=Vector3.zero;

		ray=Camera.main.ScreenPointToRay(Input.mousePosition);

		if (plane.Raycast(ray,out hitDist)) {
			dest=ray.GetPoint(hitDist);
			
			dest.y += 0.05f;

			if(reticle&&Time.timeScale==1f)
				reticle.transform.position=dest;
		}
	}
	
	bool usingMouse() {
		if(currentInput==InputMethod.SingleplayerMouse)
			return true;
		else
			return false;
	}

	public bool LookWhereMove() {
		if(timeOfAttack + 1.5f < Time.time)
			return true;
		else
			return false;
	}
	
	public void OnDeath() {
		if(reticle)
			Destroy(reticle.gameObject);
		GameManager.instance.PlayerDied(pC.pID);
	}
}
