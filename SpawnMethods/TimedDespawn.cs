using UnityEngine;
using System.Collections;

public class TimedDespawn : MonoBehaviour {

	public float TimeToDespawn=1f;
	// Use this for initialization

	void OnEnable() {
		StartCoroutine(Despawn());
	}
	
	IEnumerator Despawn()
    {
    	yield return new WaitForSeconds(TimeToDespawn);
        this.transform.Recycle();
    }
}
