using UnityEngine;
using System.Collections;

public class ScaleUpOnSpawn : MonoBehaviour {
	public float Duration=0.5f;
	public float WaitBeforeScaling=0.15f;
	// Use this for initialization
	void Start () {
		StartCoroutine(ScaleUpOnStart());
	}
	
	public virtual IEnumerator ScaleUpOnStart() {
		//float decreaseRate = 1 + shakeIntensity;
		yield return new WaitForSeconds(WaitBeforeScaling);
		StartCoroutine(transform.ScaleFrom(Vector3.zero,Duration,Ease.QuadIn));
	}
}
