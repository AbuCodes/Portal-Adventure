using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventObject : MonoBehaviour {

    private int chooseEvent; //we want this to be random at the start

    private Transform _canvas;

    private GameObject _Event;

    private Transform player;

    public Text EventName;

    public Text EventDescription;

    public Button Accept_Btn;

    private bool _isDone = false;

    public string[,] eventInfo = new string[,] 
    {
        {
            "Challenge Cube",
            "Hello i am the shiny cube if you complete my challenge i shall reward you with gold and glory, however the outcome of failing is certain death. Do you accept my challenge?"
        },
        {
            "Challenge Cube",
            "Hello i am the shiny cube if you complete my challenge i shall reward you with gold and glory, however the outcome of failing is certain death. Do you accept my challenge?"
        },
        {
            "Challenge Cube",
            "Hello i am the shiny cube if you complete my challenge i shall reward you with gold and glory, however the outcome of failing is certain death. Do you accept my challenge?"
        },
    };

    public AudioClip battleAudio;
	public AudioClip previousAudio;

    void Start()
    {
        chooseEvent = Random.Range(0, 3);
        _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
    }

    public void PromptPlayer()
    {
        if (_isDone == false)
        {
            if (!_Event)
            {
                _Event = (GameObject)Instantiate(Resources.Load("EventPanel"));
                _Event.transform.SetParent(_canvas, false);
                EventName = _Event.transform.Find("EventName_Text").GetComponent<Text>();
                EventDescription = _Event.transform.Find("EventDescription_Text").GetComponent<Text>();
                Accept_Btn = _Event.transform.Find("EventAccept_Btn").GetComponent<Button>();
                EventName.text = eventInfo[chooseEvent, 0]; // event choosen, name
                EventDescription.text = eventInfo[chooseEvent, 1]; // event choosen, description
                Accept_Btn.onClick.AddListener(delegate { PlayerAccepted(); });
            }
            else
            {
                _Event.SetActive(true);
            } 
        }
        else
        {
            //You cant do this event anymore
        }
    }

    //Handles the player acceptance of the event
    public void PlayerAccepted()
    {
        _isDone = true;

        player = GameObject.Find("Captain").transform;

        Transform eventSpawnPoint = GameObject.FindGameObjectWithTag("EventSpawnPoint").transform;

        AudioSource audio = GameObject.Find("BG Music").GetComponent<AudioSource>();
		previousAudio = audio.clip;
		audio.clip = battleAudio;
		audio.Play();

        _Event.GetComponent<EventMaker>().MakeEvent(chooseEvent, player.transform.position);
        //player.transform.position = new Vector3(eventSpawnPoint.position.x, eventSpawnPoint.position.y, eventSpawnPoint.position.z);
        _Event.SetActive(false);
    }

    public void PlayerDecline()
    {

    }

    public void Complete(){
        StartCoroutine(player.GetComponent<BarController>().InvokeMessage("Challenge Complete +10G", 2f));
        _Event.GetComponent<EventMaker>().EventComplete();
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
