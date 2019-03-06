using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerETCController : MonoBehaviour {

	protected Animator animator;
	public float speed = 0;
	public float direction = 0;
	public Locomotion locomotion = null;
	float camRayLength = 100f;
	int floorMask;

	//Handling
	public float rotationSpeed = 450;
	public float walkSpeed = 5;
	public float runSpeed = 8;
	public float jumpSpeed = 6;
    public float testSpeed = 10;
    public Transform _spawn;

	public bool playAnim = true;

	//System
	private Quaternion targetRotation;

	//Components
	private CharacterController controller;
	public MeleeSystem meleeScript;
	public AudioSource audioS;
	public AudioClip walkSound;

	public ETCJoystick speedController;

    // Use this for initialization
    void Start ()
    {
		controller = GetComponent<CharacterController> ();
		floorMask = LayerMask.GetMask("Defalut");
		animator = GetComponent<Animator>();
		locomotion = new Locomotion(animator);
        gameObject.transform.position = _spawn.position;
	}


	
	// Update is called once per frame
	void Update () {



		Vector3 input = new Vector3 (ETCInput.GetAxis ("Vertical"), 0, ETCInput.GetAxis ("Horizontal"));

		if (input != Vector3.zero) {
			targetRotation = Quaternion.LookRotation (input);
			//transform.eulerAngles = Vector3.up* Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
	
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (meleeScript.stopOtherAnim == false) {
				GetComponent<Animation> ().Play ("run");
			} 

		} else {
			if(meleeScript.stopOtherAnim == false){
				GetComponent<Animation> ().Play ("Take 001");
			}
		}

		if (controller.velocity.magnitude > 0) 
		{


			//Debug.Log ("playing sound");
			if (!audioS.isPlaying) 
			{
                audioS.Play ();
			}
		}
			
	
		//Vector3 motion = input;
		//motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)?.7f:1;
		//motion *= (Input.GetButton("Run"))?runSpeed:walkSpeed;
		//motion *= walkSpeed;
		//motion += Vector3.up * -8;

		//controller.Move (motion * Time.deltaTime);
	}
	public void AngelFeatherEnabled()
	{
		speedController.axisX.speed += 1;
		speedController.axisY.speed += 1;
	}

	public void AngelFeatherDisabled()
	{
		speedController.axisX.speed -= 1;
		speedController.axisY.speed -= 1;
	}

	public void SpeedySabatonsEnabled()
	{
		speedController.axisX.speed += 1;
		speedController.axisY.speed += 1;
	}

	public void SpeedySabatonsDisabled()
	{
		speedController.axisX.speed -= 1;
		speedController.axisY.speed -= 1;
	}

	public void AcidSkullSpeedEnabled(){
		speedController.axisX.speed += 2;
		speedController.axisY.speed += 2;
	}

	public void AcidSkullSpeedDisabled(){
		speedController.axisX.speed -= 2;
		speedController.axisY.speed -= 2;
	}

	public void SetPlayerSpeedStat(int upgradedSpeed){
		speedController.axisX.speed += 0.5f * (CloudVariables.Values[5] + upgradedSpeed);
		speedController.axisY.speed += 0.5f * (CloudVariables.Values[5] + upgradedSpeed);
	}

	public void UpgradeSpeedStat(){
		speedController.axisX.speed += 0.5f;
		speedController.axisY.speed += 0.5f;
	}

}
