using UnityEngine;
using System.Collections;

public class BlockGenerator : MonoBehaviour {

    //Array dimensions
    public static int Row = 3; //0,1,2
    public static int Column = 3; //0,1,2
 
    //Actual arrays
    private int[,] path = new int[Row, Column];
    public GameObject[] block = new GameObject[9]; //0-8
    private int count = -1;
    private int isLastBlock;

    //Get the last node
    public int LastBlock
    {
        get
        {
            return isLastBlock;
        }
    }

	// Use this for initialization
	void Awake () {

        path[0, 1] = 1;

        //Generate the path
        for (int row = 0; row < Row; row++)
        {
            for (int column = 0; column < Column; column++)
            {
                if (path[0, column] == 0)
                {
                    path[0, column] = Random.Range(0, 2);
                }
                else if (row > 0)
                {
                    if (path[row, column] == 0)
                    {
                        if (path[row-1, column] == 1)
                        {
                            path[row, column] = Random.Range(0, 2);
                        }
                    }
                }
            }
        }

        //These two lines make sure that the node connecting to the starting node is never broken
        path[0, 1] = 1;
        block[1].SetActive(true);

        //Find each position in the array that is a one and enable object of reference
        for (int row = 0; row < Row; row++)
        {
            for (int column = 0; column < Column; column++)
            {
                count++;

                if(path[row, column] == 1)
                {
                    block[count].SetActive(true);
                    isLastBlock = count;
                }
                else
                {
                    block[count].SetActive(false);
                }
            }
        }
    }
}
