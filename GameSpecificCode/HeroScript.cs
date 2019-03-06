using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroScript : MonoBehaviour {

	//LEVELS
	public int hp_lvl;
	public int damage_lvl;
	public int defence_lvl;
	public int speed_lvl;
	public int goldGain_lvl;
	public int resourceGain_lvl;

	//COST
	public int goldCost;
	public int gemCost;

	//UI REFERENCES
	public Text lvlText;
	public Text statDescText;
	public Image statIcon;
	public Sprite[] statIconSprites = new Sprite[6];

	public string[] statDesc = new string[] {
		"INCREASE YOUR MAXIMUM HEALTH",
		"INCREASE ATTACK DAMAGE",
		"INCREASE YOUR DEFENCE",
		"INCREASE SPEED",
		"INCREASE GOLD GAIN",
		"INCREASE RESOURCE GAIN"
	};

	// Use this for initialization
	void Start () {
		
	}

	public void GetAllStats(){

	}

	public int GenerateGoldCost(int level){
		return 0;
	}

	public int GenerateGemCost(int level){
		return 0;
	}
}

