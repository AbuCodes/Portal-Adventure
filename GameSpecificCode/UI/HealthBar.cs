using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	
	public GameObject ParentBar;
	
	// Update is called once per frame
	public void UpdateHealth (float health) {
		transform.localScale = new Vector3(health,1,1);
	}
	
	public void HideBar() {
		ParentBar.SetActive(false);
		gameObject.SetActive(false);
	}
}