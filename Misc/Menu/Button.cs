using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public TextMesh text;
	
	public Animator anim;
	
	public int PresetButtonAction;
	
	public string ButtonHoverSound="Hover";
	public string ButtonClickSound="Click";
	
	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<TextMesh>();
	}
	
	void OnMouseEnter() {
		if(ButtonHoverSound!="")
			SoundManager.instance.Play(ButtonHoverSound);
		anim.SetBool("hover",true);
	}
	
	void OnMouseExit () {
		anim.SetBool("hover",false);
	}
	
	void OnMouseDown () {
		if(ButtonClickSound!="")
			SoundManager.instance.Play(ButtonClickSound);
		SendMessage("ButtonClick", PresetButtonAction, SendMessageOptions.DontRequireReceiver);
		StartCoroutine(transform.Shake(0.02f,0.1f));
	}
}
