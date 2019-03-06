using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour {

    //LevelManger
    public GameObject levelManager;
    public GameObject[] _Pets = new GameObject[11];
    public Transform petSpawnPoint;

    //COUNTER STUFF
    public int enemyCount { get; set; }
    public int bossCount { get; set; }
    public int treeCount { get; set; }
    public int rockCount { get; set; }

    //ROOM STUFF
    private int roomType;

    public bool[] isRoomExsist = new bool[8];

    public int RoomType
    {
        get
        {
            GenerateRoomType();
            return roomType;
        }
    }

    void Start()
    {
        GameObject findLevelManager = GameObject.Find("_LevelManager(Clone)");
        if (findLevelManager == null)
        {
            GameObject newLevelManger = Instantiate(levelManager) as GameObject;
            LevelManager newLevel = newLevelManger.GetComponent<LevelManager>();
            newLevel.InitData();
        }
        StartCoroutine("SetUpTime", 0.1f);
    }

    public IEnumerator SetUpTime(int time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Find("BossHealthBar").SetActive(false);
        SpawnPet();
    }

    void GenerateRoomType()
    {
        float enemyRoom = Random.value;

        if (enemyRoom >= 0.6) //Check if its going to be an enemy room (repeatable)
        {
            float bossRoom = Random.value;

            if (bossRoom <= 0.15f)
            {
                roomType = 1; //Boss Room
            }
            else
            {
                roomType = 0; //Enemy Room
            }
        }
        else //Check if its gonna be every other room (non repeatable)
        {
            int generateRoom = Random.Range(0, isRoomExsist.Length);

            while (isRoomExsist[generateRoom] == true){
                generateRoom = Random.Range(0, isRoomExsist.Length);
            }

            isRoomExsist[generateRoom] = true;
            roomType = generateRoom;
        }
    }

    public void SpawnPet(){
        if(CloudVariables.Values[8] == 0)
        {
            if (CloudVariables.Values[9] == 1){
                GameObject petObject = Instantiate(_Pets[CloudVariables.Values[8]], petSpawnPoint.position, petSpawnPoint.rotation) as GameObject;
            }
        }
        else{
            GameObject petObject = Instantiate(_Pets[CloudVariables.Values[8]], petSpawnPoint.position, petSpawnPoint.rotation) as GameObject;
        }
    }

    public void LoadMainMenu(){
        Destroy(GameObject.Find("_LevelManager(Clone)"));
        SceneManager.UnloadScene(1);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(0);
    }

    public void PlayAgain(){
        Destroy(GameObject.Find("_LevelManager(Clone)"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
