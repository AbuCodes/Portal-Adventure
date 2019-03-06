using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattle : MonoBehaviour {

	//Object References
	public GameObject player;
	public GameObject boss;
	public GameObject BossBattleUI;
	public GameObject teleportObject;
	public SpawnSystem spawnObjects;

	//Spawn points
	public Transform playerSpawnPoint;
	public Transform bossSpawnPoint;
	public Transform enemySpawnPoint1;
	public Transform enemySpawnPoint2;
	public AudioClip battleAudio;
	public AudioClip previousAudio;

	public bool bossBatlleStarted = false;


	// Use this for initialization
	void Start () {
        int chooseBoss = Random.Range(0, spawnObjects._Bosses.Length);
        GameObject newBossObject = Instantiate(spawnObjects._Bosses[chooseBoss], bossSpawnPoint.position, bossSpawnPoint.rotation) as GameObject;
    	newBossObject.transform.position = new Vector3(bossSpawnPoint.position.x, 2, bossSpawnPoint.position.z-0.5f);
		boss = newBossObject;
		player = GameObject.Find("Captain");
		StartCoroutine(InitLevel());
	}

	int timer = 5;
	int counter = 3;
	void StartBattle () {

		if (bossBatlleStarted == false){
			BossBattleUI.SetActive(false);
			//Invoke Message "Begin" for 3 Seconds
			StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Begin", 3));
			GameObject newEnemyObject = Instantiate(spawnObjects._Enemies[Random.Range(0, spawnObjects._Enemies.Length)], enemySpawnPoint1.position, enemySpawnPoint1.rotation) as GameObject;
			GameObject newEnemyObject2 = Instantiate(spawnObjects._Enemies[Random.Range(0, spawnObjects._Enemies.Length)], enemySpawnPoint2.position, enemySpawnPoint2.rotation) as GameObject;
			//Fade into boss battle music
			bossBatlleStarted = true;
		}

		if (boss != null){
			//Spawn Enemies
			if (timer <= 0 && counter >= 0){
				GameObject newEnemyObject = Instantiate(spawnObjects._Enemies[Random.Range(0, spawnObjects._Enemies.Length)], enemySpawnPoint1.position, enemySpawnPoint1.rotation) as GameObject;
				GameObject newEnemyObject2 = Instantiate(spawnObjects._Enemies[Random.Range(0, spawnObjects._Enemies.Length)], enemySpawnPoint2.position, enemySpawnPoint2.rotation) as GameObject;
				timer = 5;
				counter--;
			}
			else {
				timer--;
			}
		}
		else {
			//Cancel invoke
			CancelInvoke("StartBattle");
			teleportObject.SetActive(true);
			player.GetComponent<BarController>().challengeCounters[4]++;
			StartCoroutine(FadeOut(GameObject.Find("BG Music").GetComponent<AudioSource>(), 2f));
			StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Battle Complete +ITEM", 5));
		}
	}

	
    public IEnumerator InitLevel()
    {
        yield return new WaitForEndOfFrame();
        LevelManager level = GameObject.Find("_LevelManager(Clone)").GetComponent<LevelManager>();
		if ((level.levelCounter % 10) == 0 && level.levelCounter != 0){
			//TP PLAYEER
			BossBattleUI.SetActive(true);
			player.transform.position = playerSpawnPoint.position;
			//START BATTLE
			AudioSource audio = GameObject.Find("BG Music").GetComponent<AudioSource>();
			previousAudio = audio.clip;
			audio.clip = battleAudio;
			audio.Play();
			InvokeRepeating("StartBattle", 3, 1);
		}
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
