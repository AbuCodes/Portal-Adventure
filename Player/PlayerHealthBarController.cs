using UnityEngine;
using System.Collections;

public class PlayerHealthBarController : MonoBehaviour {

	PlayerController pC;
	HealthBarManager hbManager;
	
	void Start () {
		pC = GetComponent<PlayerController>();
		if(!HealthBarManager.instance)
			Destroy(this);
	}
	
	void OnDamage () {
		HealthBarManager.instance.UpdateBar(pC.pID,pC.cH.GetHealthPct());
	}
	
	void OnHeal () {
		HealthBarManager.instance.UpdateBar(pC.pID,pC.cH.GetHealthPct());
	}
}
