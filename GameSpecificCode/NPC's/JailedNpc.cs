using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JailedNpc : MonoBehaviour {

    //Npc that is jailed 
    private int npcId;

    private Transform _canvas;
    public GameObject okPanel;
    public Button okBtn;
    public Text content;

    public bool playerHasKey = false;

    public SpawnManager spawnManager;

	// Use this for initialization
	void Start ()
    {
        PlayerInteract playerInteract = GameObject.Find("PickUpRange").GetComponent<PlayerInteract>();
        bool[] villagers = playerInteract.GetUnlockedVillagers();
        while (true)
        {
            int generateVillagerID = Random.Range(0, villagers.Length);
            if (villagers[generateVillagerID] == false)
            {
                npcId = generateVillagerID;
                break;
            }
        }

        _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        okPanel = _canvas.Find("OkPanel").gameObject;
        okBtn = okPanel.transform.Find("Panel").Find("Ok_Btn").GetComponent<Button>();
        content = okPanel.transform.Find("Text").GetComponent<Text>();
        okPanel.SetActive(false);
    }

    public void PromptPlayer()
    {
        BarController barController = GameObject.Find("Captain").GetComponent<BarController>();

        if (barController.foundKey == true)
        {
            //UNLOCK THE NPC IN THE VILLAGE AND NOTIFY THE PLAYER OF THAT FACT
            Debug.Log("You have unlocked a new villager");
            PlayerInteract playerInteract = GameObject.Find("PickUpRange").GetComponent<PlayerInteract>();
            playerInteract.UnlockVillager(npcId);
            barController.foundKey = false;
            barController.challengeCounters[6]++;
            barController.InvokeSound(playerInteract.levelCompleteAudio);
            Destroy(this.gameObject);
        }
        else
        {
            okPanel.SetActive(true);
            content.text = "HEYYY YOUUU HELP ME. YOU CAN RELEASE ME IF YOU FIND THE KEY... MEET ME BACK HERE WHEN YOU DO";
            okBtn.onClick.AddListener(delegate { Ok(); });
        }
    }

    public void Ok()
    {
        okBtn.onClick.RemoveAllListeners();
        okPanel.SetActive(false);
    }
}
