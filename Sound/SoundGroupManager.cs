using UnityEngine;
using System.Collections;

public class SoundGroupManager : MonoBehaviour {

	public AudioClip[] Sounds;
	public bool Music=false;
	public bool RandomPitch=false;
	public float RandomPitchMin,RandomPitchMax;
	private float startingvolume;
	private int played=0;
	
	void Awake() {
		startingvolume = GetComponent<AudioSource>().volume;
	}
	
	void OnEnable () {
		if(Sounds.Length==0) {
			Debug.Log("There is no sound on "+gameObject.name+" SoundGroupManager, ABORT");
			this.Recycle();
			return;
		}
		if(!GetComponent<AudioSource>())
			gameObject.AddComponent<AudioSource>();
		PickAudio();
		if(RandomPitch)
			GetComponent<AudioSource>().pitch = Random.Range(RandomPitchMin,RandomPitchMax);
		SetVolume();
		GetComponent<AudioSource>().Play();
		StartCoroutine(WaitForStopPlaying());
	}
	
	public void SetVolume() {
		if(Music)
			GetComponent<AudioSource>().volume=SoundManager.instance.MusicVolume*startingvolume;
		else
			GetComponent<AudioSource>().volume=SoundManager.instance.SFXVolume*startingvolume;
	}
	
	void PickAudio() {
		if(Music) {
			GetComponent<AudioSource>().clip = Sounds[played];
			if(played<Sounds.Length-1)
				played++;
			else
				played=0;
		} else {
			GetComponent<AudioSource>().clip = Sounds[Random.Range(0, Sounds.Length)];
		}
	}
	
	public void ForceStop() {
		StopAllCoroutines();
		this.Recycle();
	}
	
	void OnApplicationFocus(bool focus) {
	   if(!focus) {
			StopAllCoroutines();
	    	AudioListener.pause = true;
	   }
	   else {
			AudioListener.pause = false;
			StartCoroutine(WaitForStopPlaying());
	   }
	}
	
	IEnumerator WaitForStopPlaying()
    {
        while (GetComponent<AudioSource>().isPlaying)
        {
            yield return 0;
        }
        
        if(SoundManager.instance.currentMusic==this&&this.enabled)
        	OnEnable();
        else
        	this.Recycle();
    }
}
