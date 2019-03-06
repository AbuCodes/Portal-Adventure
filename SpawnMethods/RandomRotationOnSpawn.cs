using UnityEngine;
using System.Collections;

public class RandomRotationOnSpawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, Random.Range (0, 360),transform.rotation.eulerAngles.z);
	}
}
