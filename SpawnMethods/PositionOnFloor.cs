using UnityEngine;
using System.Collections;

public class PositionOnFloor : MonoBehaviour {

	public float WantedFloorDistance=0.15f;
	public LayerMask FloorMask;
	
	// Use this for initialization
	void OnEnable () {
		RaycastHit hit;
		float dist;
		Vector3 dir;
		
		dist = 35;
    	dir = new Vector3(0,-1,0);
		if (Physics.Raycast(new Vector3 (transform.position.x,transform.position.y+10,transform.position.z), dir, out hit, dist, FloorMask)) {
			transform.position = new Vector3 (transform.position.x,hit.collider.gameObject.transform.position.y+WantedFloorDistance,transform.position.z);
		}
	}
}
