using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour {

    /* This object will be spawned when the village is created.
     * It will spawn the proper unlocked npc's from values stored
     * in the player object.
     */

    public bool[] unlockedVillagers = new bool[5];
    public GameObject[] villagers = new GameObject[5];

    public GameObject ShopNpc;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("SetUpTimer", 1);
	}

    public IEnumerator SetUpTimer(int time)
    {
        yield return new WaitForSeconds(time);
        SetUpVillage();
    }

    //Set up village by getting values from player interact
    public void SetUpVillage()
    {
        PlayerInteract playerInteract = GameObject.Find("PickUpRange").GetComponent<PlayerInteract>();
        unlockedVillagers = playerInteract.UnlockedVillagers;

        for (int i = 0; i < villagers.Length; i++)
        {
            if (unlockedVillagers[i] == true)
            {
                villagers[i].SetActive(true);
            }
        }

        //ShopNpc
        GameObject shop = Instantiate(GameObject.Find("ShopNPC"));
        villagers[4].GetComponent<ItemStore>().itemName = shop.GetComponent<ItemStore>().itemName;
        villagers[4].GetComponent<ItemStore>().itemDesc = shop.GetComponent<ItemStore>().itemDesc;
        villagers[4].GetComponent<ItemStore>().itemPrice = shop.GetComponent<ItemStore>().itemPrice;
        villagers[4].GetComponent<ItemStore>().itemSprite = shop.GetComponent<ItemStore>().itemSprite;
        villagers[4].GetComponent<ItemStore>().StoreMenu = shop.GetComponent<ItemStore>().StoreMenu;
    }
}
