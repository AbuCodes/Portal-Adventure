using UnityEngine;
using System.Collections;

public class GoalDoor : MonoBehaviour {

	public string LevelToGo = "Adventure";
	
	void OnTriggerEnter(Collider other) {
        if(other.tag=="Player")
        	Application.LoadLevel(LevelToGo);
    }
}
