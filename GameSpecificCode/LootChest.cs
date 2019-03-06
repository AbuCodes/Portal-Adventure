using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LootChest : MonoBehaviour {

	private bool _chestOpened = false;
	public RewardPool rewardPool;
	public DOTweenAnimation openAnim;
	public AudioClip coin;
	public AudioSource audioS;
	
	void Start () {
		rewardPool = GameObject.Find("Reward Pool").GetComponent<RewardPool>();
		audioS = GameObject.Find("SFX").GetComponent<AudioSource>();
	}
    public IEnumerator OpenChest(int time)
    {
        for (int i = 0; i < time; i++)
        {
			GetCoin();
			audioS.PlayOneShot(coin, 1.0F);
            yield return new WaitForSeconds(0.5f);
        }
    }

	void GetCoin(){
		GameObject rewardObject = rewardPool.GetPooledObject(0);
		if(this.gameObject.transform.position.y <= 0.55f){
			rewardObject.transform.position = new Vector3(this.gameObject.transform.position.x + Random.Range(-2,3), 1.0F, this.gameObject.transform.position.z + Random.Range(-2,3));
		}
		else{
			rewardObject.transform.position = new Vector3(this.gameObject.transform.position.x + Random.Range(-2,3), this.gameObject.transform.position.y, this.gameObject.transform.position.z + Random.Range(-2,3));
		}
		rewardObject.SetActive(true);
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "HitBox")
        {
            if(_chestOpened == false){
				StartCoroutine("OpenChest", Random.Range(5,11));
				openAnim.DOPlayById("OPEN");
				_chestOpened = true;
			}
        }
	}
}
