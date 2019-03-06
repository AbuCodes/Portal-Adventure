using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour {

    public int spawnID;
    public int getRoom;

    public Transform spawnPoint;

    //Enemy releated refrences
    public GameObject[] _Enemies = new GameObject[30];
    public GameObject[] _WallGenerator = new GameObject[5];

    //Boss releated refrences
    public GameObject[] _Bosses = new GameObject[17];
    public GameObject _BossPlatform;

    //Neutral room related refrences
    public GameObject _Mineral;
    public GameObject _Tree;
    public GameObject[] _NPC = new GameObject[30];
    public GameObject _CreepyOldMan;

    public GameObject[] _Puzzle = new GameObject[3];
    public GameObject _Event;
    public GameObject _Village;
    public GameObject _Shop;
    public GameObject _Craft;
    public GameObject _PetShop;
    public GameObject _JailedNpc;
    public GameObject _Portal;
    public GameObject _Vegetation;
    public GameObject[] _Object = new GameObject[2];

    //TEST CODE VARS
    public BlockGenerator blockGenerator;
    public SpawnManager spawnManager;

    // Use this for initialization
    void Start()
    {
        spawnPoint = this.gameObject.transform;

        blockGenerator = GameObject.Find("BasePlatform").GetComponent<BlockGenerator>(); //Same with this one
        spawnManager = GameObject.Find("_Manager").GetComponent<SpawnManager>(); //Change the reference to this object later

        if (blockGenerator.LastBlock == spawnID)
        {
            //SET UP PORTAL ROOM HERE
            GameObject newPortalObject = Instantiate(_Portal, spawnPoint.position, spawnPoint.rotation) as GameObject;
            newPortalObject.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
            newPortalObject.transform.rotation = Quaternion.Euler(0, 135, 0);
            GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
            newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
            newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);
            Debug.Log("Setting up portal here: " + blockGenerator.LastBlock);
        }
        else //enemy = 0, boss = 1, village = 2, resource = 3, event = 4, shop = 5, jailednpc = 6, puzzle = 7, pet room = 8, empty = 9, farm = 10 
        {
            //EVERYTHING ELSE HERE
            getRoom = spawnManager.RoomType;

            PlayerInteract playerInteract = GameObject.Find("PickUpRange").GetComponent<PlayerInteract>();

            while (getRoom == 6)
            {
                if (playerInteract.AllVillagersUnlocked() == true)
                {
                    getRoom = spawnManager.RoomType;
                }
                else
                {
                    break;
                }
            }

            if (getRoom == 0) //ENEMY ROOM
            {
                int amountOfEnemies = Random.Range(3, 5);

                GameObject.Find("Captain").GetComponent<BarController>().enemyCount += amountOfEnemies;

                int chooseEnemy = Random.Range(0, _Enemies.Length);

                for (int i = 0; i < amountOfEnemies; i++)
                {
                    GameObject newObject = Instantiate(_Enemies[chooseEnemy], spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newObject.transform.position = new Vector3(spawnPoint.position.x + Random.Range(-10, 11), 1, spawnPoint.position.z + Random.Range(-10, 11));
                    spawnManager.enemyCount++;
                }
 
                GameObject newWallObject = Instantiate(_WallGenerator[Random.Range(0, 5)], spawnPoint.position, spawnPoint.rotation) as GameObject;
                newWallObject.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);
            }
            else if (getRoom == 1) //BOSS ROOM
            {
                int chooseBoss = Random.Range(0, _Bosses.Length);

                GameObject newBossObject = Instantiate(_Bosses[chooseBoss], spawnPoint.position, spawnPoint.rotation) as GameObject;
                newBossObject.transform.position = new Vector3(spawnPoint.position.x, 2, spawnPoint.position.z-0.5f);
                spawnManager.bossCount++;

                GameObject newPlatformObject = Instantiate(_BossPlatform, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newPlatformObject.transform.position = new Vector3(spawnPoint.position.x, 0.5f, spawnPoint.position.z);

                int amountOfEnemies = Random.Range(3, 5);

                GameObject.Find("Captain").GetComponent<BarController>().enemyCount += amountOfEnemies;

                int chooseEnemy = Random.Range(0, _Enemies.Length);

                for (int i = 0; i < amountOfEnemies; i++)
                {
                    GameObject newObject = Instantiate(_Enemies[chooseEnemy], spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newObject.transform.position = new Vector3(spawnPoint.position.x + Random.Range(-10, 11), 1, spawnPoint.position.z + Random.Range(-10, 11));
                    spawnManager.enemyCount++;
                }
            }
            else if (getRoom == 2) //VILLAGE ROOM
            {
                GameObject newVillageObject = Instantiate(_Village, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVillageObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
            }
            else if (getRoom == 3) //RESOURCE ROOM
            {
                int mineralsAmount = Random.Range(5, 10);
                int treesAmount = Random.Range(5, 10);

                //Here we want to spawn the minerals
                for (int i = 0; i < mineralsAmount; i++)
                {
                    int posX = GiveMePosition(1, 4);
                    int posY = GiveMePosition(1, 4);

                    GameObject newMineralObject = Instantiate(_Mineral, spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newMineralObject.transform.position = new Vector3(spawnPoint.position.x + posX, 1, spawnPoint.position.z + posY);
                    spawnManager.rockCount++;
                }

                //Here spawn the trees (they will extend more than the minerals)
                for (int i = 0; i < treesAmount; i++)
                {
                    int posX = GiveMePosition(5, 10);
                    int posY = GiveMePosition(5, 10);

                    GameObject newTreeObject = Instantiate(_Tree, spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newTreeObject.transform.position = new Vector3(spawnPoint.position.x + posX, 1, spawnPoint.position.z + posY);
                    spawnManager.treeCount++;
                }

                //Spawn Vegetation
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);

                //Spawn Mysterious Old Man NPC
                GameObject newNpcObject = Instantiate(_CreepyOldMan, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newNpcObject.transform.position = new Vector3(spawnPoint.position.x, 1, spawnPoint.position.z);

                GameObject newNpcObject2 = Instantiate(_NPC[Random.Range(0, _NPC.Length)], spawnPoint.position, spawnPoint.rotation) as GameObject;
                newNpcObject2.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
            }
            else if (getRoom == 4) //EVENT ROOM
            {
                GameObject newEventObject = Instantiate(_Event, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newEventObject.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);
            }
            else if (getRoom == 5) //SHOP ROOM
            {
                GameObject newShopObject = Instantiate(_Shop, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newShopObject.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);
            }
            else if (getRoom == 6) //CRAFT ROOM
            {
                GameObject newCraftObject = Instantiate(_Craft, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newCraftObject.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);
            }
            else if (getRoom == 7) //JAILED NPC
            {
                GameObject newJailedNpc = Instantiate(_JailedNpc, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newJailedNpc.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);
            }
            else if (getRoom == 8) //PUZZLE ROOM
            {
                int choosePuzzle = Random.Range(0, 3);

                GameObject newPuzzleObject = Instantiate(_Puzzle[choosePuzzle], spawnPoint.position, spawnPoint.rotation) as GameObject;
                newPuzzleObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
            }
            else if (getRoom == 9) //EMPTY ROOM
            {
                //GameObject newPetObject = Instantiate(_PetShop, spawnPoint.position, spawnPoint.rotation) as GameObject;
                //newPetObject.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);

                float randomChance = Random.value;
                if(randomChance >= 0.5f){
                    GameObject newObject = Instantiate(_Object[Random.Range(0, _Object.Length)], spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newObject.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
                }

            }
            else if (getRoom == 10) // EMPTY ROOM + ENEMIES
            {
                int amountOfEnemies = Random.Range(3, 5);

                int chooseEnemy = Random.Range(0, _Enemies.Length);

                GameObject.Find("Captain").GetComponent<BarController>().enemyCount += amountOfEnemies;

                for (int i = 0; i < amountOfEnemies; i++)
                {
                    GameObject newObject = Instantiate(_Enemies[chooseEnemy], spawnPoint.position, spawnPoint.rotation) as GameObject;
                    newObject.transform.position = new Vector3(spawnPoint.position.x + Random.Range(-10, 11), 1, spawnPoint.position.z + Random.Range(-10, 11));
                    spawnManager.enemyCount++;
                }
 
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);
            }
            else if (getRoom == 11) //NPC ROOM
            {
                GameObject newNpcObject = Instantiate(_NPC[Random.Range(0, _NPC.Length)], spawnPoint.position, spawnPoint.rotation) as GameObject;
                newNpcObject.transform.position = new Vector3(spawnPoint.position.x + 0, 1, spawnPoint.position.z + 0);
                
                GameObject newVegetationObject = Instantiate(_Vegetation, spawnPoint.position, spawnPoint.rotation) as GameObject;
                newVegetationObject.transform.position = new Vector3(spawnPoint.position.x + 0, 0.5f, spawnPoint.position.z + 0);
                newVegetationObject.transform.rotation = Quaternion.Euler(0, -45, 0);
            }
        }
    }

    int GiveMePosition(int min, int max)
    {
        int number = Random.Range(min, max);

        float changeState = Random.value;

        if (changeState >= 0.5f)
        {
            number *= -1;
        }

        return number;
    }
}
