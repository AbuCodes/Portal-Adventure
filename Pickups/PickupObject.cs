using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour {

	public int ScoreValue=1;
	public Transform PickupFX;
	public string PickupSound;
	public float MagnetRadius=15;
	public float MagnetForce=90;
	public float DistanceToPickUp=2;
	public float CantPickUpOnSpawn=0;
	public LayerMask characterMask=1 << 8;
	[HideInInspector]
	public float TimeOfStart;
	Transform currentTarget;
	Rigidbody rb;
	
	void Start() {
		if(PickupFX)
			PickupFX.CreatePool();
		
		if(!GetComponent<Rigidbody>()) {
			rb = gameObject.AddComponent<Rigidbody>();
			rb.isKinematic=true;
			rb.useGravity=false;
		} else {
			rb = gameObject.GetComponent<Rigidbody>();
		}
		
		if(!GetComponent<Collider>()||!GetComponent<Collider>().isTrigger) {
			var m_Collider = gameObject.AddComponent<SphereCollider>();
			m_Collider.isTrigger=true;
			m_Collider.radius=15f;
		}
		currentTarget=null;
		TimeOfStart=Time.time;
	}
	
	void OnEnable() {
		InvokeRepeating("CheckForPlayer", CantPickUpOnSpawn, 0.25F);
	}
	
	void FixedUpdate() {
		if(!currentTarget)
			return;
		if(TimeOfStart + CantPickUpOnSpawn > Time.time)
			return;
			
		if(Vector3.Distance(currentTarget.transform.position, transform.position) < DistanceToPickUp) {
		        if(PickupFX)
					PickupFX.Spawn(transform.position);
		        
				SoundManager.instance.Play(PickupSound);
		        
		        PlayerController pC = currentTarget.gameObject.GetComponent<PlayerController>();
		        PickupEffect(pC);
		}
		if(MagnetForce>0) {
			Vector3 relativePos = currentTarget.transform.position - transform.position;
			rb.AddForce(MagnetForce * relativePos.normalized);		
		}
	}
	
	void CheckForPlayer() {
		if (Physics.CheckSphere(transform.position, MagnetRadius, characterMask))
			{
				Collider collider = null;
				var dist = float.MaxValue;
				
				foreach (var overlap in Physics.OverlapSphere(transform.position, MagnetRadius, characterMask))
				{
					var d = Vector3.Distance(overlap.GetComponent<Collider>().transform.position, transform.position);
					if (d < dist && overlap.GetComponent<Collider>() != this.gameObject.GetComponent<Collider>())
					{
						dist = d;
						collider = overlap.GetComponent<Collider>();
					}
				}
				
				
				if(collider) {
					if(currentTarget==null)
						currentTarget=collider.transform;
				} else {
					if(currentTarget!=null)
						currentTarget=null;
				}
			}
	}
    
    public virtual void PickupEffect(PlayerController pC) {
        GameManager.instance.playerInfo[pC.pID].score += ScoreValue;
        HealthBarManager.instance.UpdateScore(pC.pID);
        
        this.transform.Recycle();
    }
}
