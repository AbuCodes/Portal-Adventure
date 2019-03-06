using UnityEngine;
using System.Collections;

public class LockScript : MonoBehaviour {

    /*We want a trigger that when entered will lock the player in
     *Untill the condition is met for exit
     *We will need to store reference to the colliders and to the spawnerscript
     */

    public bool lockFlag = false;

    //References
    public GameObject lockObject;
    public SpawnScript spawnScript;
    public GameObject CloudMaster;
    public GameObject spawnPoint;

    // Use this for initialization
    void Start ()
    {
        if (spawnScript.spawn_npc)
        {
            lockFlag = true;
            lockObject.SetActive(false);
        }
	}

    void EnemySubtract()
    {
        if(lockFlag == true)
        {
            spawnScript.enemyCount--;
            if(spawnScript.enemyCount <= 0)
            {
                lockObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(lockFlag == false)
            {
                lockFlag = true;
                lockObject.SetActive(true);
            }

            CloudMaster.transform.position = new Vector3(spawnPoint.transform.position.x, CloudMaster.transform.position.y, spawnPoint.transform.position.z);
        }
    }
}
