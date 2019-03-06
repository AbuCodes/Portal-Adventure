using UnityEngine;
using System.Collections;

public class HealthBarManager : MonoBehaviour {
	public HealthBar[] healthbar;
	public HealthBar[] ammobar;
	public TextMesh[] CoinText;
	private static HealthBarManager _instance;
	
	public static HealthBarManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<HealthBarManager>();

				if(_instance==null)
					return null;
                DontDestroyOnLoad(_instance.gameObject);
            }
 
            return _instance;
        }
    }
 
    void Awake() 
    {
        if(_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
	
	void Start() {
		for (int i = GameManager.instance.numberOfPlayers; i < healthbar.Length; i++)
		    healthbar[i].HideBar();
	}
	
	// Update is called once per frame
	public void UpdateBar (int player, float health) {
		if(healthbar.Length>0 && healthbar[player]!=null)
			healthbar[player].UpdateHealth(health);
	}
	
	// Update is called once per frame
	public void UpdateAmmoBar (int player, float health) {
		if(ammobar.Length>0 && ammobar[player]!=null)
			ammobar[player].UpdateHealth(health);
	}
	
	// Update is called once per frame
	public void UpdateScore (int player) {
		if(CoinText.Length>0 && CoinText[player]!=null)
			CoinText[player].text="X "+GameManager.instance.playerInfo[player].score;
	}
}
