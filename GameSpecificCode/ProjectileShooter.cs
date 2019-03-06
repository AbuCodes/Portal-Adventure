using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour {

    Transform player;
    public GameObject Object;
    public GameObject destroyEffect;
    public Transform[] points = new Transform[3];
    public float fireRate = 1f;
    public float fireRate_reset;
    public bool _turret;
    public bool _wall;
    public AudioSource audioS;
    public AudioClip launchSound;
    public AudioClip destroySound;

    float minDist = 1f;

    void Start () {
        fireRate_reset = fireRate;
        player = GameObject.Find("Captain").GetComponent<Transform>();
        audioS = GameObject.Find("Rocket").GetComponent<AudioSource>();
        
        if (_turret){
            minDist = 1f;
        }
        else{
            minDist = 5f;
        }
    }

    void Update()
    {

        RaycastHit hit;
        Vector3 fwd = this.transform.TransformDirection(Vector3.forward);
        float dist = Vector3.Distance(player.position, transform.position);
        
        if (dist <= 15f && dist >= minDist)
        {
            if (fireRate > 0)
            {
                fireRate -= Time.deltaTime;
            }
            else if (fireRate <= 0)
            {
                fireRate = fireRate_reset;
                //SpawnObject();
                if (Physics.Raycast(this.transform.position, fwd, out hit, 1) && hit.transform.tag == "Wall")
                {

                }
                else
                {
                    SpawnObject();
                }
            } 
        }

        //This detects if player has stepped on turret
        if(_turret) {
            if(dist <= 1){
                if(destroyEffect != null){
                    player.GetComponent<BarController>().InvokeSound(destroySound);
                    Instantiate(destroyEffect, this.gameObject.transform.position, Quaternion.Euler(270, 45, 0));
                }
                this.transform.parent.parent.gameObject.SetActive(false);
            }
        }
    }

    void SpawnObject()
    {
        audioS.PlayOneShot(launchSound, 1f);

        if(_turret) {
            Instantiate(Object, points[0].transform.position, transform.rotation);
        } 
        else {
            Instantiate(Object, points[0].transform.position, transform.rotation);
            Instantiate(Object, points[1].transform.position, points[1].transform.rotation);
            Instantiate(Object, points[2].transform.position, points[2].transform.rotation);

            if(_wall == true){
                Instantiate(Object, points[3].transform.position, points[3].transform.rotation);
            }  
        }
    }
}
