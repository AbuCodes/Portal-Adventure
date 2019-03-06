using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour {

    //Node References
    public Transform spawnPoint;
    public GameObject cube;
    public GameObject cube2;

    [SerializeField]
    private float turnChancePercent;

    public int cubeSpawnAmount;

    public Vector2[] storedPoints;

    public List<Vector2> storedObjects = new List<Vector2>();

	// Use this for initialization
	void Start ()
    {
        storedPoints = new Vector2[cubeSpawnAmount];

        turnChancePercent = Random.value;

        GenerateWall(cubeSpawnAmount, turnChancePercent);

        GenerateTurrets();

        this.gameObject.transform.rotation = Quaternion.Euler(0, -45, 0);
	}

    void GenerateWall(int amount, float chance)
    {
        int x_axis = 0; int y_axis = 0;

        for (int i = 0; i < amount; i++)
        {
            if (chance >= 0.5)
            {
                x_axis += RandomizeDirection(x_axis);
            }
            else
            {
                y_axis += RandomizeDirection(y_axis);
            }

            GameObject newObject = Instantiate(cube, spawnPoint.position, spawnPoint.rotation) as GameObject;
            newObject.transform.parent = spawnPoint.transform;
            newObject.transform.position = new Vector3(spawnPoint.position.x + x_axis, 1, spawnPoint.position.z + y_axis);

            storedPoints[i].x = x_axis;
            storedPoints[i].y = y_axis;

            chance = Random.value;
        }
    }

    int RandomizeDirection (int number)
    {
        int[] unit = new int[2];
        unit[0] += 1;
        unit[1] += -1;

        int chooseUnit = Random.Range(0, 1);

        number = unit[chooseUnit];

        return number;
    }

    void GenerateTurrets()
    {

        int[] pointsToCheck = new int[4] { -1, 1, -1, 1 };

        for (int i = 0; i < storedPoints.Length; i++)
        {

            for (int j = 0; j < pointsToCheck.Length; j++)
            {

                int cubeCounter = 0;
                Vector2 newPoint;

                if (j <= 1)
                {
                    newPoint.x = storedPoints[i].x + pointsToCheck[j];
                    newPoint.y = storedPoints[i].y; 
                }
                else
                {
                    newPoint.x = storedPoints[i].x;
                    newPoint.y = storedPoints[i].y + pointsToCheck[j];
                }

                bool isNotEmpty = CheckArray(0, (int)newPoint.x, (int)newPoint.y);

                if (isNotEmpty == false)
                {
                    for (int k = 0; k < pointsToCheck.Length; k++)
                    {
                        Vector2 newEmptyPoint;

                        if (k <= 1)
                        {
                            newEmptyPoint.x = newPoint.x + pointsToCheck[k];
                            newEmptyPoint.y = newPoint.y;
                        }
                        else
                        {
                            newEmptyPoint.x = newPoint.x;
                            newEmptyPoint.y = newPoint.y + pointsToCheck[k];
                        }

                        isNotEmpty = CheckArray(0, (int)newEmptyPoint.x, (int)newEmptyPoint.y);

                        if (isNotEmpty == true)
                        {
                
                            cubeCounter++;

                            if (cubeCounter >= 2)
                            {

                                if (storedObjects.Count <= 0)
                                {
                                    storedObjects.Add(new Vector2(newPoint.x, newPoint.y));
                                    GameObject newObject = Instantiate(cube2, spawnPoint.position, spawnPoint.rotation) as GameObject;
                                    newObject.transform.parent = spawnPoint.transform;
                                    newObject.transform.position = new Vector3(spawnPoint.position.x + (int)newPoint.x, 0.5f, spawnPoint.position.z + (int)newPoint.y);
                                }

                                else
                                {
                                    isNotEmpty = CheckArray(1, (int)newPoint.x, (int)newPoint.y);

                                    if (isNotEmpty == false)
                                    {
                                        storedObjects.Add(new Vector2(newPoint.x, newPoint.y));
                                        GameObject newObject = Instantiate(cube2, spawnPoint.position, spawnPoint.rotation) as GameObject;
                                        newObject.transform.parent = spawnPoint.transform;
                                        newObject.transform.position = new Vector3(spawnPoint.position.x + (int)newPoint.x, 0.5f, spawnPoint.position.z + (int)newPoint.y);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    bool CheckArray (int id, int x, int y)
    {
        Vector2 point;
        point.x = x;
        point.y = y;

        if (id == 0)
        {
            for (int i = 0; i < storedPoints.Length; i++)
            {
                if (point.x == storedPoints[i].x && point.y == storedPoints[i].y)
                {
                    return true;
                }
            } 
        }

        else if (id == 1)
        {
            foreach (Vector2 privatePoint in storedObjects.ToArray())
            {
                if (privatePoint.x == point.x && privatePoint.y == point.y)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
