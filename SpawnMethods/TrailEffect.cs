using UnityEngine;
using System.Collections;

public class TrailEffect : MonoBehaviour {

	public Transform FollowTransform;
	ParticleSystem part;
	
	void Awake() {
		part=gameObject.GetComponent<ParticleSystem>();
	}
	
	void OnEnable () {
		if(part) {
			part.Clear();
			part.enableEmission=false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(FollowTransform&&FollowTransform.gameObject.activeInHierarchy)
			transform.position=FollowTransform.position;
		else {
			if(part.enableEmission)
				DieTrail();
		}
	}
	
	public void StartTrail(projectile proj) {
		FollowTransform=proj.transform;
		transform.position=proj.transform.position;
		if(part) {
			part.Clear();
			part.Play();
			//part.enableEmission=true;
			StartCoroutine(StartEmitting());
		}
	}
	
	IEnumerator StartEmitting() {
		yield return 0;
		part.enableEmission=true;
	}
	
	public void DieTrail() {
		FollowTransform=null;
		if(part)
			part.Stop();
			
		if (gameObject.activeInHierarchy)
			StartCoroutine(Despawn());
	}
	
	IEnumerator Despawn()
    {
    	yield return new WaitForSeconds(0.2f);
    	if(part) {
    		part.Stop();
    		part.Clear();
    		part.enableEmission=false;
    	}
        this.Recycle();
    }
    
    void OnDisable() {
    	if(part) {
    		part.Stop();
    		part.Clear();
    		part.enableEmission=false;
    	}
    }
}
