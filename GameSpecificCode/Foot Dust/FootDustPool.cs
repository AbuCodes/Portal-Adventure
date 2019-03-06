using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootDustPool : MonoBehaviour {

    public GameObject footDustPrefab;
    public int amountToPool;
    List<GameObject> footDustPool;

    public Transform spawnPoint;
    public GameObject parentObject;

	// Use this for initialization
	void Start ()
    {
        footDustPool = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject newObj = (GameObject)Instantiate(footDustPrefab);
            newObj.SetActive(false);
            newObj.transform.position = spawnPoint.transform.position;
            newObj.transform.parent = parentObject.transform;
            footDustPool.Add(newObj);
        }
	
	}
	
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < footDustPool.Count; i++)
        {
            if (!footDustPool[i].activeInHierarchy)
            {
                return footDustPool[i];
            }
        }

        GameObject newObj = (GameObject)Instantiate(footDustPrefab);
        newObj.SetActive(false);
        newObj.transform.position = spawnPoint.transform.position;
        newObj.transform.parent = parentObject.transform;
        return newObj;
    }
}
