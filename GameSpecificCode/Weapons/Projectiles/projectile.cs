using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {
	public Transform hitEffect;
	public string hitSound;
	public TrailEffect trailEffect;
	public float radius=0.5f;
	public int damage=1;
	public float force=0.1f;
	public float forceDuration=0.4f;
	public float hitCamShake=0f;
	
	public LayerMask characterMask;
	public LayerMask WallMask;
	
	[HideInInspector]
	public Transform Owner;
	
	TrailEffect currentTrail;
	
	Vector3 hitDir;
	
	void Start() {
		if(hitEffect)
			hitEffect.CreatePool();
		if(trailEffect)
			trailEffect.CreatePool();
	}
	
	public virtual  void startShoot(Vector3 velocity, float distance) {
		StartCoroutine(Shoot(velocity,distance));
		if(trailEffect)
			startTrail();
	}
	
	public virtual void startTrail() {
		currentTrail = trailEffect.Spawn(transform.position);
		currentTrail.StartTrail(this);
	}
	
	public virtual void endTrail() {
		currentTrail.DieTrail();
		currentTrail=null;
	}
	
	public virtual IEnumerator Shoot(Vector3 velocity, float distance)
	{
		var start = transform.position;
		var speed = velocity.magnitude;
		Collider hitWall = null;
		var hitPoint = Vector3.zero;
		while (distance > 0)
		{
			RaycastHit hit;
			var pos = transform.position;
			pos.y = 0;
			
			if (Physics.CheckSphere(pos, radius, characterMask))
			{
				Collider collider = null;
				var dist = float.MaxValue;
				
				foreach (var overlap in Physics.OverlapSphere(pos, radius, characterMask))
				{
					var d = Vector3.Distance(overlap.GetComponent<Collider>().transform.position, start);
					if (d < dist && overlap.GetComponent<Collider>() != this.gameObject.GetComponent<Collider>())
					{
						dist = d;
						collider = overlap.GetComponent<Collider>();
					}
				}
				
				
				if(collider) {
					if(transform.DealDamageKnockback(collider.transform,damage,velocity.normalized*force,forceDuration,Owner)) {
						hitDir = velocity.normalized;
						Hit(hitSound,true);
						distance = 0;
					}
				}
			}
			else if (Physics.Raycast(transform.position, velocity.normalized, out hit, 2f, WallMask))
			{
				hitWall = hit.collider;
				hitPoint = hit.point;
			}
			else if (hitWall != null)
			{
				hitDir = velocity.normalized;
				transform.position = hitPoint;
				Hit(hitSound);
				break;
			}
			
			distance -= speed * Time.deltaTime;
			transform.localPosition += velocity * Time.deltaTime;
			
			yield return 0;
		}
		Hit();
	}
	
	public virtual void Hit(string Sound, bool shake=false) {
		if(Sound!="")
			SoundManager.instance.Play(Sound);
			
		if(hitEffect) {
			Transform hitFX = hitEffect.Spawn(transform.position);
			hitFX.rotation=Quaternion.LookRotation(hitDir);
		}
		
		if(shake && hitCamShake>0&&CamManager.instance.shaker)
			CamManager.instance.shaker.shake(hitCamShake);
		
		Hit();
	}
	
	public virtual void Hit() {
		StopAllCoroutines();
			
		if(currentTrail)
			endTrail();
			
		this.Recycle();
	}
}
