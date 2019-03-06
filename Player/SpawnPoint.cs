using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public Transform[] SpawnPoints;
	
    private static SpawnPoint _instance;
 
	public bool SpawnOnLevelLoad=true;
	
	public bool isRandomSpawn=false;
 
    public static SpawnPoint instance {
        get {
            if(_instance == null)
                _instance = GameObject.FindObjectOfType<SpawnPoint>();
 
            return _instance;
        }
    }
 
	void Awake() {
        if(_instance == null)
            _instance = this;
        else
            if(this != _instance)
                Destroy(this.gameObject);
                
        if(SpawnOnLevelLoad)
			GameManager.instance.spawnPlayers();
    }
}
