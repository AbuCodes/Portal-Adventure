using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	[System.Serializable]
	public class PlayerInfo {
		public PlayerInput.InputMethod pInput = PlayerInput.InputMethod.SingleplayerKeyboard;
		public PlayerController pC=null;
		public int score=0;
		public int currentbullets=0;
		public string currentWeapon=null;
		public List<string> Inventory = new List<string>();
		public bool Dead=false;
		public bool Init=false;
	}

public class GameManager : MonoBehaviour 
{
    private static GameManager _instance;
	
	public PlayerInfo[] playerInfo = new PlayerInfo[4];
	
	public int numberOfPlayers=1;
	
	public int GameMode=0;
	
	public bool useSeed=false;
	public int MAP_WIDTH,MAP_HEIGHT,WALL_PERCENTAGE,TreasureObjSpawnerDistance,EnemySpawnerDistance,BreakableObjSpawnerDistance,DecorationSpawnerDistance,seed;

	//[HideInInspector]
	public bool Loading=false;
	
	public Transform TrashOrganizer;
	
	public static GameManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
 
				if(!_instance) {
					GameObject singleton = new GameObject();
					singleton.name="GameManager";
					_instance = singleton.AddComponent<GameManager>();
				}
                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(_instance.gameObject);
            }
 
            return _instance;
        }
    }

    void Awake() 
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(this != _instance)
                Destroy(this.gameObject);
        }
        
		playerInfo = new PlayerInfo[4];
		
		for (int i = 0; i < playerInfo.Length; i++)
		    playerInfo[i] = new PlayerInfo();
        
		SoundManager.instance.Init();
    }
    
    void OnLevelWasLoaded ()
    {
    	if(Loading)
    		StartSeededLevel();
    	if(Application.loadedLevelName=="MainMenu"||Application.loadedLevelName=="CheatingArea")
    		return;
    	
    	var SpawnPointObj = GameObject.FindObjectOfType<SpawnPoint>();
    	if(SpawnPointObj)
    		return;
    	
		var PreventSpawn = GameObject.FindObjectOfType<PreventAutoSpawn>();
		if(PreventSpawn)
			return;

        spawnPlayers();
    }
    
    public void spawnPlayers() {
    	var Player = GameObject.FindObjectOfType<PlayerController>();
        if(Player)
        	Destroy(Player.gameObject);
        	
    	StartCoroutine(ExecuteSpawnPlayers());
    }
    
    IEnumerator ExecuteSpawnPlayers()
    {
    	var spawnPoint = GameObject.FindObjectOfType<SpawnPoint>();
    	yield return 0;

    	for (int i = 0; i < numberOfPlayers; i++) {			    	
	    	Vector3 SpawnPosition = new Vector3(Random.Range(-10,10),2, Random.Range(-10,10));
	    	
	    	if(spawnPoint) {
		    	if(SpawnPoint.instance.isRandomSpawn)
		    		SpawnPosition=SpawnPoint.instance.SpawnPoints[Random.Range(0,SpawnPoint.instance.SpawnPoints.Length)].position;
		    	else
		    		SpawnPosition=SpawnPoint.instance.SpawnPoints[i].position;
	    	}
	    	
			GameObject myPlayer = (GameObject) Instantiate(Resources.Load("Player"), SpawnPosition, Quaternion.identity);
			
			playerInfo[i].pC = myPlayer.GetComponent<PlayerController>();
			
			playerInfo[i].pC.pID = i;
			
			playerInfo[i].pC.pI.SetInput(playerInfo[i].pInput);
			
			myPlayer.name = "Player "+i;
			
			GameObject myPlayerPointer = (GameObject) Instantiate(Resources.Load("PlayerPointer"));
			
			myPlayerPointer.transform.parent = playerInfo[i].pC.cM.body;
			myPlayerPointer.transform.localPosition = new Vector3(0,0.15f,0);
			if(i==0)
				myPlayerPointer.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.65f, 0.4f, 0.02f, 0.7f));
			else if(i==1)
				myPlayerPointer.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.086f, 0.45f, 0.43f, 0.7f));
			else if(i==2)
				myPlayerPointer.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.42f, 0.04f, 0.55f, 0.7f));
			else if(i==3)
				myPlayerPointer.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.42f, 0.05f, 0.05f,0.7f));
			
			yield return 0;
			
			if(playerInfo[i].Init) {
				if(playerInfo[i].Inventory.Count>0) {
					foreach (string s in playerInfo[i].Inventory) {
						if(s!=playerInfo[i].pC.weaponManager.defaultWeapon.name)
							playerInfo[i].pC.weaponManager.EquipWeapon(s);
							
						yield return 0;
					}
				}
			} else {
				playerInfo[i].Init=true;
			}
	    }
    }
    
    public void ResetPlayers() {
    	for (int i = 0; i < playerInfo.Length; i++) {
    		playerInfo[i].pC=null;
			playerInfo[i].score=0;
			playerInfo[i].currentbullets=0;
			playerInfo[i].currentWeapon=null;
			playerInfo[i].Dead=false;
			playerInfo[i].Init=false;
    	}
    }
    
    public void PlayerDied(int ID) {
    	playerInfo[ID].Dead=true;
    	
    	int DeadAmmount=0;
    	int CurrentPlayerAmmount=0;
    	for (int i = 0; i < playerInfo.Length; i++) {
    		if(playerInfo[i].Dead)
    			DeadAmmount++;
    		if(playerInfo[i].pC!=null)
    			CurrentPlayerAmmount++;
    	}
    	
    	if(DeadAmmount>=CurrentPlayerAmmount)
    		DoGameOver();
    }
    
    public void PlayerRevived(int ID) {
    	playerInfo[ID].Dead=false;
    }
    
    public IEnumerator PlaySeededLevel() {
    	useSeed=true;
    	Loading=true;
    	Application.LoadLevel("Adventure");
    	yield return 0;
    }
    
    void StartSeededLevel() {
    	var DG = GameObject.FindObjectOfType<ProceduralLevelGenerator>();
    	Debug.Log("DG is "+DG);
    	if(DG) {
    		DG.MAP_WIDTH=MAP_WIDTH;
    		Debug.Log("MAP_WIDTH"+DG.MAP_WIDTH);
    		Debug.Log("LevelName"+Application.loadedLevelName);
			DG.MAP_HEIGHT=MAP_HEIGHT;
			DG.Seed=seed;
			DG.useSeed=true;
			DG.WALL_PERCENTAGE=WALL_PERCENTAGE;
			DG.TreasureObjSpawnerDistance=TreasureObjSpawnerDistance;
			DG.EnemySpawnerDistance=EnemySpawnerDistance;
			DG.BreakableObjSpawnerDistance=BreakableObjSpawnerDistance;
			DG.DecorationSpawnerDistance=DecorationSpawnerDistance;
			DG.DoGenerate();
    	}
    	Loading=false;
    	useSeed=false;
    	//useSeed=false;
    }
    
    void SpawnOrganizer() {
    	GameObject org = new GameObject();
    	org.name = "Trash Organizer";
    	TrashOrganizer=org.transform;
    }
    
    public void ToOrganizer(Transform trash) {
    	if(!TrashOrganizer)
    		SpawnOrganizer();
    		
    	if(TrashOrganizer)
    		trash.parent = TrashOrganizer;
    }
    
    public void DoGameOver() {
    	GameObject myGO = (GameObject) Instantiate(Resources.Load("GameOver"), Camera.main.transform.position, Quaternion.identity);
    	myGO.transform.parent = Camera.main.transform;
    	myGO.transform.localPosition = new Vector3(0,1.56f,7.48f);
    	myGO.transform.localRotation = Quaternion.Euler(0,0,0);
    }
}