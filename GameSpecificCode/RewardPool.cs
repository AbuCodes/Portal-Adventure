using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPool : MonoBehaviour {

    public GameObject[] rewardPrefabs;
    public int amountToPool;
    List<GameObject> rewardObjectsPool;

    public Transform spawnPoint;
    public GameObject parentObject;
    public string[] rewardTag = new string[3];
    public int[] rewardObjectManager = new int[3];
    public string[] previousNames = new string[2];

    // Use this for initialization
    void Start ()
    {
        #region Pool Generation Algroithm;
        rewardTag[0] = "LowReward";
        rewardTag[1] = "MidReward";
        rewardTag[2] = "HighReward";

        rewardObjectsPool = new List<GameObject>();
        for (int i = 0; i < rewardPrefabs.Length; i++)
        {
            for (int k = 0; k < amountToPool; k++)
            {
                GameObject newObject = (GameObject)Instantiate(rewardPrefabs[i]);
                newObject.SetActive(false);
                newObject.transform.position = spawnPoint.transform.position;
                newObject.transform.parent = parentObject.transform;
                rewardObjectsPool.Add(newObject);
            } 
        }

        int objectCount = 0;
        for (int i = 0; i < rewardTag.Length; i++)
        {
            for (int j = 0; j < rewardObjectManager[i] * amountToPool; j++)
            {
                rewardObjectsPool[objectCount].tag = rewardTag[i];
                objectCount++;
            } 
        }
        #endregion
    }

    public GameObject GetPooledObject(int rewardID)
    {
        for (int i = 0; i < rewardObjectsPool.Count; i++)
        {
            if (!rewardObjectsPool[i].activeInHierarchy && rewardObjectsPool[i].tag == rewardTag[rewardID] && rewardObjectsPool[i].name != previousNames[0] && rewardObjectsPool[i].name != previousNames[1])
            {
                if (rewardTag[rewardID] == rewardTag[1])
                {
                    previousNames[0] = previousNames[1];
                    previousNames[1] = rewardObjectsPool[i].name; 
                }
                return rewardObjectsPool[i];
            }
        }
        int genNum = CalculateObjectToGenerate(rewardID);
        GameObject newObject = (GameObject)Instantiate(rewardPrefabs[genNum]);
        newObject.SetActive(false);
        newObject.tag = rewardTag[rewardID];
        newObject.transform.position = spawnPoint.transform.position;
        newObject.transform.parent = parentObject.transform;
        rewardObjectsPool.Add(newObject);

        return newObject; 

    }

    int CalculateObjectToGenerate (int id)
    {
        int x;

        if (id == 0) 
        {
            x = Random.Range(0, rewardObjectManager[0]);
            return x;
        }
        else if (id == 1)
        {
            x = Random.Range(rewardObjectManager[0], rewardObjectManager[1] + rewardObjectManager[0]);
            return x;
        }
        else if (id == 2)
        {
            x = Random.Range(rewardObjectManager[1] + rewardObjectManager[0], rewardObjectManager[2] + rewardObjectManager[1] + rewardObjectManager[0]);
            return x;
        }

        x = Random.Range(0, rewardPrefabs.Length);
        return x;
       
     }

}