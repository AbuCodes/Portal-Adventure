using UnityEngine;
using System.Collections;

public class DespawnOnDeath : MonoBehaviour {

	public float TimeToDespawn=1f;
	// Use this for initialization

	void OnDeath() {
		StartCoroutine(Despawn());
	}
	
	IEnumerator Despawn()
    {
    	yield return new WaitForSeconds(TimeToDespawn);
        this.transform.Recycle();
    }
}
