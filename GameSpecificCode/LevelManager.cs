using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public int levelCounter = 0;
    public int bossLevelCounter = 0;
    public float playerHealth;
    public int playerCoins;
    public int playerResources;
    public int playerScore;
    public int playerCraftingCost;
    public bool playerRevived;
    public int playerUpgradedAttack;
    public int playerUpgradedDefence;
    public int playerUpgradedHealth;
    public int playerUpgradedSpeed;
    public int[] playerChallengeCounter = new int [7];
    public bool[] villageState = new bool[5];
    public int[] playerInventory = new int[9];
    public int[] slotID = new int[9];
	public int[] slotValue = new int[9];
	public string[] slotName = new string[9];
	public string [] slotDesc = new string[9];
	public Sprite[] slotSprite = new Sprite[9];
	public int[] slotType = new int[9];
	public int[] slotStack = new int[9];
	public Image[] slotIcon = new Image[9];

    public bool playerKey;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void GetData()
    {
        Debug.Log("Saving player data");
        levelCounter++;

        //Save player health
        BarController player = GameObject.Find("Captain").GetComponent<BarController>();
        playerHealth = player.GetHealth();

        //Get Coins
        playerCoins = player.GetCoins();

        //Get Resources
        playerResources = player.GetResources();

        //Get Score
        playerScore = player.GetScore();

        //Get Crafting Cost
        playerCraftingCost = player.craftingCost;

        //Gey Key
        playerKey = player.foundKey;

        //Get Player Revived State
        playerRevived = player.GetPlayerRevived();

        //Get Extra Stats
        playerUpgradedAttack = player.upgradedDamage;
        playerUpgradedDefence = player.upgradedDefence;
        playerUpgradedHealth = player.upgradedHealth;
        playerUpgradedSpeed = player.upgradedSpeed;

        //Get Challenge counter
        playerChallengeCounter = player.challengeCounters;

        //Get Inventory
        PlayerInventory inventoryScript = GameObject.Find("Captain").GetComponent<PlayerInventory>();
        playerInventory = inventoryScript.slotID;
        this.slotID = inventoryScript.slotID;
        this.slotValue = inventoryScript.slotValue;
        this.slotName = inventoryScript.slotName;
        this.slotDesc = inventoryScript.slotDesc;
        this.slotSprite = inventoryScript.slotSprite;
        this.slotType = inventoryScript.slotType;
        this.slotStack = inventoryScript.slotStack;
        this.slotIcon = inventoryScript.slotIcon;

        //Get Villager
        villageState = GameObject.Find("PickUpRange").GetComponent<PlayerInteract>().UnlockedVillagers;
    }

    public void SetData()
    {
        Debug.Log("Set player data");
        //Set player health
        BarController player = GameObject.Find("Captain").GetComponent<BarController>();
        player.SetHealth(playerHealth);

        //Set Coins
        player.SetCoins(playerCoins);

        //Set Resources
        player.SetResources(playerResources);

        //Set Score
        player.SetScore(playerScore);

        //Set Crafting Cost
        player.craftingCost = playerCraftingCost;

        //Set key
        player.foundKey = playerKey;

        //Set Player Revived State
        player.SetPlayerRevived(playerRevived);

        //Set Extra Stats
        player.upgradedDamage = playerUpgradedAttack;
        player.upgradedDefence = playerUpgradedDefence;
        player.upgradedHealth = playerUpgradedHealth;
        player.upgradedSpeed = playerUpgradedSpeed;

        //Set challenge counter
        player.challengeCounters = playerChallengeCounter;

        //Set Inventory
        PlayerInventory inventoryScript = GameObject.Find("Captain").GetComponent<PlayerInventory>();
        inventoryScript.slotID = this.slotID;
        inventoryScript.slotValue = this.slotValue;
        inventoryScript.slotName = this.slotName;
        inventoryScript.slotDesc = this.slotDesc;
        inventoryScript.slotSprite = this.slotSprite;
        inventoryScript.slotType = this.slotType;
        inventoryScript.slotStack = this.slotStack;
        //inventoryScript.slotIcon = this.slotIcon;
        for (int i = 0; i < playerInventory.Length; i++)
        {
            inventoryScript.slotIcon[i].sprite = inventoryScript.slotSprite[i];
            if(inventoryScript.slotIcon[i].sprite == null){
                inventoryScript.slotIcon[i].sprite = inventoryScript.emptySlotSprite;
            }
            inventoryScript.EnableItem(playerInventory[i]);
        }

        //Set Villagers
        GameObject.Find("PickUpRange").GetComponent<PlayerInteract>().UnlockedVillagers = villageState;

        //Set Key
    }

    public void InitData()
    {

    }
}
