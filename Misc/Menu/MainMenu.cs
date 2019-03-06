using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public int clicked=1;

	public float GoToAngleY=0;

	public string StartLevelName = "Adventure";
	public string SecondOptionLevelName = "MakeALevel";
	public string ThirdOptionLevelName = "CheatingArea";

	void ButtonClick(int option) {
		if(option == 1||option==2||option==3) {
			StartCoroutine(
				Camera.main.transform.RotateTo(
					Quaternion.Euler(Camera.main.transform.localRotation.x+12, GoToAngleY, Camera.main.transform.localRotation.z),
					1f,
					Ease.QuadIn
				)
			);
			clicked++;	
		}
		if(option == 4) {
			StartCoroutine(
				Camera.main.transform.RotateTo(
					Quaternion.Euler(Camera.main.transform.localRotation.x+12, Camera.main.transform.localRotation.y-(90*clicked), Camera.main.transform.localRotation.z),
					1f,
					Ease.QuadIn
				)
			);
		}
		if(option == 9) {
			if(GameManager.instance.GameMode==0)
				Application.LoadLevel(StartLevelName);
			if(GameManager.instance.GameMode==1)
				Application.LoadLevel(SecondOptionLevelName);
		}
		if(option==2) {
			GameManager.instance.GameMode=0;
		}
		if(option==3) {
			GameManager.instance.GameMode=1;
		}
		if(option==5) {
			Application.LoadLevel("MainMenu");
		}
		if(option==6) {
			Application.LoadLevel(ThirdOptionLevelName);
		}
	}
}
