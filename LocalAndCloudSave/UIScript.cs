using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	public static UIScript Instance {get; private set;}

	public Text[] ValuesTextArray;

	public GameObject _Manager;

	//HERO RELATED REFERENCES
	public int selectedStat;

	//HERO UI REFERENCES
	public Text lvlText;
	public Text goldCostText;
	public Text gemCostText;
	public Text statDescText;
	public Image statIcon;
	public Sprite[] statIconSprites = new Sprite[6];
	public GameObject MessageObject;

	//CHALLENGE UI REFERENCES
	public Text[] challengeText = new Text[3];
	public GameObject[] challengeObject = new GameObject[3]; 
	
	//AUDIO
	public AudioSource source;
	public AudioClip ErrorSound;
	public AudioClip LevelSound;

	//SETTINGS MENU
	public GameObject musicOn;
	public GameObject musicOff;
	public GameObject sfxOn;
	public GameObject sfxOff;

	//HIGHSCORE MENU
	public Text Highscore;
	public Text Level;

	public string[] statDesc = new string[] {
		"INCREASE YOUR STARTING HEALTH WITH COINS OR GEMS.",
		"INCREASE ATTACK DAMAGE WITH COINS OR GEMS.",
		"INCREASE YOUR DEFENCE WITH COINS OR GEMS.",
		"INCREASE SPEED WITH COINS OR GEMS.",
		"INCREASE GOLD GAIN WITH COINS OR GEMS.",
		"INCREASE RESOURCE GAIN WITH COINS OR GEMS."
	};

	// Use this for initialization
	void Start () {
		Instance = this;
		StartCoroutine(InitManager());
	}

	IEnumerator InitManager(){
		yield return new WaitForEndOfFrame();
		if(GameObject.Find("Manager(Clone)") == null){
			Instantiate(_Manager);
		}
		StartCoroutine(InitCoroutine());
	}

	IEnumerator InitCoroutine(){
		yield return new WaitForSeconds(1f);
		UpdateText();
		SetChallengeObjects();
		Highscore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
		Level.text = PlayerPrefs.GetInt("Level", 0).ToString();
	}

	public void UpdateText(){
		//Update Gold and Gems
		ValuesTextArray[0].text = "x" + CloudVariables.Values[0].ToString(); //GOLD
		ValuesTextArray[1].text = "x" + CloudVariables.Values[1].ToString(); //GEMS
	}

	public void UpdateHeroPanel(int id){
		selectedStat = id;
		lvlText.text = CloudVariables.Values[id+2].ToString();
		statDescText.text = statDesc[id];
		statIcon.sprite = statIconSprites[id];
		goldCostText.text = "-" + GetGoldCost(CloudVariables.Values[id+2]).ToString();
		gemCostText.text = "-" + GetGemCost(CloudVariables.Values[id+2]).ToString();
	}

	public void UpgradeStatGold(){
		if(CloudVariables.Values[0] >= GetGoldCost(CloudVariables.Values[selectedStat+2])){
			CloudVariables.Values[0] -= GetGoldCost(CloudVariables.Values[selectedStat+2]);
			CloudVariables.Values[selectedStat+2]++;
			InvokeSound(LevelSound);
				StartCoroutine(InvokeMessage("LEVEL UP! +1", 3f));
		}
		else {
			//NotEnoughFunds.SetActive(true);
			StartCoroutine(InvokeMessage("NOT ENOUGH GOLD!", 1f));
			InvokeSound(ErrorSound);
		}
		Save();
		UpdateText();
		UpdateHeroPanel(selectedStat);
	}

	public void UpgradeStatGems(){
		if(CloudVariables.Values[1] >= GetGemCost(CloudVariables.Values[selectedStat+2])){
			CloudVariables.Values[1] -= GetGemCost(CloudVariables.Values[selectedStat+2]);
			CloudVariables.Values[selectedStat+2]++;
			InvokeSound(LevelSound);
			StartCoroutine(InvokeMessage("LEVEL UP! +1", 3f));
		}
		else {
			//NotEnoughFunds.SetActive(true);
			StartCoroutine(InvokeMessage("NOT ENOUGH GEMS!", 1f));
			InvokeSound(ErrorSound);
		}
		Save();
		UpdateText();
		UpdateHeroPanel(selectedStat);
	}

	public int GetGoldCost(int level){
		int m = 75;
		int x = level;
		int b = 100;
		return (m*x) + b;
	}

	public int GetGemCost(int level){
		int m = 17;
		int x = level;
		int b = 10;
		return (m*x) + b;
	}

	public void SetChallengeObjects(){

		ChallengeManager challengeManager = GameObject.Find("Manager(Clone)").GetComponent<ChallengeManager>();

		for (int i = 0; i < challengeObject.Length; i++){
			if (challengeManager.completedChallenges[i] == true){
				challengeObject[i].SetActive(false);
			}
			challengeText[i].text = challengeManager.challengeDesc[challengeManager.challengeID[i]];
		}
	}

	public void Save(){
		PlayGameScript.Instance.SaveData();
	}

	#region Google Play Functions
	public void ShowAchievements(){
		PlayGameScript.ShowAchievementsUI();
	}

	public void ShowLeaderboard(){
		PlayGameScript.ShowLeaderboardUI();
	}
	#endregion

	#region TEST CODE
	public void AddGold(){
		Debug.Log("Previous Gold: " + CloudVariables.Values[0]);
		CloudVariables.Values[0] += 100;
		Debug.Log("Current Gold: " + CloudVariables.Values[0]);
		UpdateText();
		Save();
	}

	public void AddGems(){
		Debug.Log("Previous Gold: " + CloudVariables.Values[1]);
		CloudVariables.Values[1] += 100;
		Debug.Log("Current Gold: " + CloudVariables.Values[1]);
		UpdateText();
		Save();
	}
	#endregion

	#region UI Invoke Methods
	public IEnumerator InvokeMessage(string message, float time){
        MessageObject.SetActive(true);
        MessageObject.GetComponent<Text>().text = message;
        yield return new WaitForSeconds(time);
        MessageObject.SetActive(false);
    }

    public void InvokeSound(AudioClip sound){
        source.PlayOneShot(sound, 1f);
    }
	#endregion

	#region SETTIGNS MENU FUNCTIONS
	public void SoundOnOff (AudioSource source){
		//MUSIC
		if (source.mute == false && source.name == "MenuMusic"){
			source.mute = true;
			musicOn.SetActive(false);
			musicOff.SetActive(true);
		}
		else if (source.mute == true && source.name == "MenuMusic"){
			source.mute = false;
			musicOn.SetActive(true);
			musicOff.SetActive(false);
		}

		//SFX
		if (source.mute == false && source.name == "SFX"){
			source.mute = true;
			sfxOn.SetActive(false);
			sfxOff.SetActive(true);
		}
		else if (source.mute == true && source.name == "SFX"){
			source.mute = false;
			sfxOn.SetActive(true);
			sfxOff.SetActive(false);
		}
	}
	#endregion

	#region IAP
	public void GrantGold(int amount){
		CloudVariables.Values[0] += amount;
		Save();
		UpdateText();
	}

	public void GrantGems(int amount){
		CloudVariables.Values[1] += amount;
		Save();
		UpdateText();
	}

	public void DisableAds(){
		PlayerPrefs.SetInt("NoAds", 1);
	}
	#endregion
}
