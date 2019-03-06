using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {

	private bool pauseEnabled = false;
	private GameObject PauseScreenObj;
	
	void Start (){
        pauseEnabled = false;
        Time.timeScale = 1;
        AudioListener.volume = 1;
    }
    
    public void TogglePauseMenu() {
    		if(pauseEnabled == true){
                Unpause();
            } else {
                Pause();
            }
    }
    
    public void Unpause() {
    	if(PauseScreenObj)
    		PauseScreenObj.SetActive(false);
        pauseEnabled = false;
        Time.timeScale = 1;
        AudioListener.volume = 1;
    }
    
    public void Pause() {
    	if(PauseScreenObj)
    		PauseScreenObj.SetActive(true);
    	else
    		CreateScreen();
        pauseEnabled = true;
        AudioListener.volume = 0;
        Time.timeScale = 0;
    }
    
    public void CreateScreen() {
    	GameObject go = (GameObject) Instantiate(Resources.Load("PauseScreenPfb"), transform.position, Quaternion.identity);
		go.transform.parent=transform;
		go.transform.localPosition=Vector3.zero;
		go.transform.localRotation=Quaternion.Euler(0,0,0);
		PauseScreenObj = go;
    }
}