using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {

    /* Store objects, npcs, enemies, bosses, special objects.
     * Store points of each object spawned.
     */
    
    //Control Values
    public int spawnValue = 10;
    public Transform spawnPoint;

    //Stored Prefabs
    public GameObject[] objects = new GameObject[5]; //OBJECTS: 0:Bronze Event 1:Silver Event 2:Gold Event || 3 CRYSTAL
    public GameObject[] npcs = new GameObject[1];
    public GameObject[] enemies = new GameObject[3];
    public GameObject[] bosses = new GameObject[2];
    public GameObject[] special_objects = new GameObject[4];
    public GameObject[] setups = new GameObject[6]; //Village: 5, Village: 4,
    public GameObject[] wallSetups = new GameObject[5];
    public GameObject turret;

    //Some flags 
    public bool spawn_npc = false;
    public int enemyCount;

	// Use this for initialization
	void Start () {

        spawnPoint = this.gameObject.transform;

        float pickRoom = Random.value;
        if (pickRoom <= 0.5f) //BAD ROOM 50% Chance 
        {
            //ENEMY ROOM - this is where our basic enemies will be spawned
            float pickRoomType = Random.value; //ENEMY ROOM
            if (pickRoomType <= 0.5)
            {
                int chooseEnemy = Random.Range(0, enemies.Length);
                int enemyAmount = Random.Range(3, 5);
                for (int i = 0; i < enemyAmount; i++)
                {
                    GameObject newObject = Instantiate(enemies[chooseEnemy], spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newObject.transform.position = new Vector3(spawnPoint.position.x + Random.Range(-10, 11), 1, spawnPoint.position.z + Random.Range(-10, 11));
                }

                //Here is where we will generate our walls - note this process might be randomly genereated in the future rather than 
                int pickWallSetup = Random.Range(0, 5);
                GameObject newWallSetup = Instantiate(wallSetups[pickWallSetup], spawnPoint.position, spawnPoint.rotation) as GameObject;

            }
            else if (pickRoomType >= 0.5f && pickRoomType <= 0.75f) //BOSS ROOM
            {
                int chooseBoss = Random.Range(0, bosses.Length);
                GameObject newBossObject = Instantiate(bosses[chooseBoss], spawnPoint.position, spawnPoint.rotation) as GameObject;
                special_objects[0].SetActive(true);

                int chooseEnemy = Random.Range(0, enemies.Length);
                int enemyAmount = Random.Range(3, 5);
                for (int i = 0; i < enemyAmount; i++)
                {
                    GameObject newEnemyObject = Instantiate(enemies[chooseEnemy], spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newEnemyObject.transform.position = new Vector3(spawnPoint.position.x + Random.Range(-10, 11), 1, spawnPoint.position.z + Random.Range(-10, 11));
                }

            }
            else if (pickRoomType >= 0.75f) //TURRET ROOM
            {
                GameObject newBossObject = Instantiate(turret, spawnPoint.position, spawnPoint.rotation) as GameObject;

                int chooseEnemy = Random.Range(0, enemies.Length);
                int enemyAmount = Random.Range(3, 5);
                for (int i = 0; i < enemyAmount; i++)
                {
                    GameObject newEnemyObject = Instantiate(enemies[chooseEnemy], spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newEnemyObject.transform.position = new Vector3(spawnPoint.position.x + Random.Range(-10, 11), 1, spawnPoint.position.z + Random.Range(-10, 11));
                }
            }
        }
        else if (pickRoom >= 0.5f && pickRoom <= 0.75f) //GOOD ROOM 25% Chance 
        {
            float pickRoomType = Random.value;
            if (pickRoomType <= 0.5) //EVENT ROOM
            {
                int pickEvent = Random.Range(0, 3);
                GameObject newEventObject = Instantiate(objects[pickEvent], spawnPoint.position, spawnPoint.rotation) as GameObject;
                //MAYBE WE WANT TO SPAWN OTHER STUFF HERE

            }
            else if (pickRoomType >= 0.5f && pickRoomType <= 0.75f) //NPC ROOM
            {
                int pickNpc = Random.Range(0, npcs.Length);
                GameObject newNpcObject = Instantiate(npcs[pickNpc], spawnPoint.position, spawnPoint.rotation) as GameObject;
            }
            else if (pickRoomType >= 0.75f) //VILLAGE
            {
                int pickVillage = Random.Range(setups.Length - 2, setups.Length);
                GameObject newVillageObject = Instantiate(setups[pickVillage], spawnPoint.position, spawnPoint.rotation) as GameObject;
            }
        }
        else if (pickRoom >=   0.75f) //NEUTRAL ROOM 25% Chance 
        {
            float pickRoomType = Random.value;
            if (pickRoomType <= 0.5)
            {
                //I think we want to spawn the 'speacial object' in the middle then we want to generate more objects around it
                GameObject newCrystalObject = Instantiate(special_objects[1], spawnPoint.position, spawnPoint.rotation) as GameObject;

                int crystalOreAmount = Random.Range(3, 5);
                for (int i = 0; i < crystalOreAmount; i++)
                {
                    GameObject newCrystalOreObject = Instantiate(objects[3], spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newCrystalOreObject.transform.position = new Vector3(spawnPoint.position.x + Random.Range(-10, 11), 1, spawnPoint.position.z + Random.Range(-10, 11));
                }

            }
            else if (pickRoomType > 0.5)
            {
                GameObject newChestObject = Instantiate(special_objects[2], spawnPoint.position, spawnPoint.rotation) as GameObject;
            }
        }

        //new Vector3(spawnPoint.position.x + Random.Range(-10, 11), 1, spawnPoint.position.z + Random.Range(-10, 11));
    }
}
