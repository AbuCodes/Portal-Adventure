using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResourceScript : MonoBehaviour {

    //Resource Stuff
    public int health;
    public int original_health;
    public int damage = 1;
    public AudioClip damage_sound;
    public AudioClip collect;
    public AudioSource audioS;
    public DOTweenAnimation shakeAnim;
    public GameObject particleBits;
    public float reset_time;
    public float original_reset_time;

    // Use this for initialization
    void Start () {

        original_health = health;
        shakeAnim = this.gameObject.GetComponent<DOTweenAnimation>();
        audioS = GameObject.Find("SFX").GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {

        //Check if dead
        if (health <= 0)
        {
            Dead();
        }

    }

    void Damage() 
	{
        //On damage do particle effects and play sound do animation
        BarController barcontroller = GameObject.Find ("Captain").GetComponent<BarController>();
        if(barcontroller.GetPick() == true){
            damage = 2;
        }
		health -= damage;
        damage = 1;
        shakeAnim.DOPlayById("SHAKE");
        audioS.PlayOneShot(damage_sound, 1f);
        Instantiate(particleBits, this.gameObject.transform.position, Quaternion.Euler(270, 45, 0));
    }

	void Dead()
	{
		//Give player resource and prepare for reset
		//health = original_health;
		//reset_time = original_reset_time;
        audioS.PlayOneShot(collect, 1f);
		BarController barcontroller = GameObject.Find ("Captain").GetComponent<BarController>();
        int resourceAmount = 2 + Random.Range(0, CloudVariables.Values[7]+2);
        barcontroller.resource_points += resourceAmount;
        barcontroller.SetScore(barcontroller.GetScore() + Random.Range(2, 7));
        if(barcontroller.GetHorseShoe() == true){
            resourceAmount += 2;
            barcontroller.resource_points += 2;
        }
        barcontroller.challengeCounters[3] += resourceAmount;
        this.gameObject.SetActive(false);
    }

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "HitBox") 
		{
			Damage();
			col.gameObject.GetComponent<BoxCollider> ().enabled = false;
		}
	}
}
