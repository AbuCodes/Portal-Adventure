using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMaker : MonoBehaviour {

    public Transform spawnPoint;

    public GameObject player;

    public Vector3 playerPosition;

    public GameObject[] EventPrefabs = new GameObject[3];

	// Use this for initialization
	void Start ()
    {
        //Get refrerances
        player = GameObject.Find("Captain");
        spawnPoint = GameObject.FindGameObjectWithTag("EventSpawnPoint").transform;
	}

    //Creates the event and teleports the player to the proper event spawn point
    public void MakeEvent(int eventValue, Vector3 position)
    {
        //Create the event
        GameObject newObject = Instantiate(EventPrefabs[eventValue], spawnPoint.position, spawnPoint.rotation) as GameObject;
        newObject.transform.position = new Vector3(spawnPoint.position.x, 0.5f, spawnPoint.position.z);

        //Teleport player and save there previous position in the world
        playerPosition = position;
        player.transform.position = new Vector3(-37.70658f, spawnPoint.position.y, 39.68368f);
        Debug.Log("Player Position is:" + playerPosition);
    }

    public void EventComplete ()
    {
        //Teleport player back and reward
        Debug.Log("EVENT COMPLETE ");
        player.transform.position = new Vector3(playerPosition.x, 1, playerPosition.z);
        player.GetComponent<BarController>().coinCount += 10;
        player.GetComponent<BarController>().InvokeSound(player.GetComponent<BarController>().powerUpSound);
    }
}
