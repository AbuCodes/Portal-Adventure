using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    public Transform pointA, pointB;
    public float speed;
 
    void FixedUpdate ()
    {
        speed = 1 *Time.deltaTime;
        transform.position = Vector3.MoveTowards(pointA.position, pointB.position, speed);

        if(this.gameObject.transform.position.y >= 1){
            Transform tempPoint;
            tempPoint = pointA;
            pointA = pointB;
            pointB = tempPoint;
        }
    }

    
}
