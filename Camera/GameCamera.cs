using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	private Transform target;

	private Vector3 cameraTarget;

	public float posX = 0;
	public float posZ = 0;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {

		cameraTarget = new Vector3 (target.position.x + posX, transform.position.y, target.position.z + posZ);
		transform.position = Vector3.Lerp (transform.position, cameraTarget, Time.deltaTime * 8);
    }
}
