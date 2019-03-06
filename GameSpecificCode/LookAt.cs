using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    public Transform player;
    public float speed = 5;

    void Start ()
    {
        player = GameObject.Find("Captain").GetComponent<Transform>();
    }

	void Update ()
    {
        Vector3 direction = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(direction);
	}
}
