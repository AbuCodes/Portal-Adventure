using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBattle : MonoBehaviour {

	public int waveCounter = 0;
	public bool waveStarted = false;
	public GameObject player;
	public GameObject teleportObject;
	public GameObject turretObject;
	public SpawnSystem spawnObjects;
	
	public Transform playerSpawnPoint;
	public Transform[] enemySpawnPoints = new Transform[2];
	public List<GameObject> enemyList = new List<GameObject>();
	public AudioClip battleAudio;
	public AudioClip previousAudio;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Captain");
		StartCoroutine(InitLevel());
	}

	int timer = 5;
	bool waveComplete = false;
	void StartBattle () {

		//Invoke Message "Begin" for 3 Seconds

		if(waveCounter == 1 && waveStarted == false){
			SpawnEnemies(2);
			StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Wave 1", 3));
			waveStarted = true;
		}

		if(waveCounter == 2 && waveStarted == false){
			SpawnEnemies(4);
			StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Wave 2", 3));
			waveStarted = true;
		}

		if(waveCounter == 3 && waveStarted == false){
			SpawnEnemies(6);
			StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Wave 3", 3));
			waveStarted = true;
		}

		if(waveCounter == 4 && waveStarted == false){
			player.GetComponent<BarController>().coinCount += 100;
			StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Wave Encounter Complete +100G", 3));
			teleportObject.SetActive(true);
			turretObject.SetActive(false);
			StartCoroutine(FadeOut(GameObject.Find("BG Music").GetComponent<AudioSource>(), 2f));
			player.GetComponent<BarController>().challengeCounters[5]++;
			CancelInvoke("StartBattle");
		}


		if(waveStarted == true && AllEnemiesKilled() == true){
			if(waveComplete == false){
				StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Wave Complete", 3));
				waveComplete = true;
			}

			if(timer <= 0){
				waveCounter++;
				waveStarted = false;
				waveComplete = false;
				timer = 5;
			}
			else {
				timer--;
			}
		}


		//CancelInvoke("StartBattle");
		//Spawn Teleporter
		//Fade out of boss battle music
		//Invoke Message "Battle Complete +ITEM"
		//teleportObject.SetActive(true);
		//StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Wave 1", 5));
	}

	public IEnumerator InitLevel()
    {
        yield return new WaitForEndOfFrame();
        LevelManager level = GameObject.Find("_LevelManager(Clone)").GetComponent<LevelManager>();
		if ((level.levelCounter % 10) != 0 && level.levelCounter != 0 && (level.levelCounter % 5) == 0){
			//TP PLAYEER
			player.transform.position = playerSpawnPoint.position;
			//START BATTLE
			waveCounter++;
			StartCoroutine(player.GetComponent<BarController>().InvokeMessage("WAVE ENCOUNTER SURVIVE!", 5));
			AudioSource audio = GameObject.Find("BG Music").GetComponent<AudioSource>();
			previousAudio = audio.clip;
			audio.clip = battleAudio;
			audio.Play();
			InvokeRepeating("StartBattle", 3, 1);
		}
    }

	private void SpawnEnemies(int amount){
		
		int flag = 0;

		for (int i = 0; i < amount; i++){
			GameObject newEnemyObject = Instantiate(spawnObjects._Enemies[Random.Range(0, spawnObjects._Enemies.Length)], enemySpawnPoints[flag].position, enemySpawnPoints[flag].rotation) as GameObject;
			enemyList.Add(newEnemyObject);
			if(flag == 0){
				flag = 1;
			}
			else{
				flag = 0;
			}
		}
	}

	private bool AllEnemiesKilled(){
		for (int i = 0; i < enemyList.Count; i++){
			if(enemyList[i] != null){
				return false;
			}
		}
		return true;
	}

	IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
		audioSource.clip = previousAudio;
		audioSource.Play();
    }
}
