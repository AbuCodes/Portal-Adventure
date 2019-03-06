using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public characterMove cM;
	[HideInInspector]
	public PlayerInput pI;
	[HideInInspector]
	public characterHealth cH;
	[HideInInspector]
	public PlayerWeaponManager weaponManager;
	
	[HideInInspector]
	public int pID;
	
	void Awake () {
		cM=GetComponent<characterMove>();
		pI=GetComponent<PlayerInput>();
		cH=GetComponent<characterHealth>();
		weaponManager=GetComponent<PlayerWeaponManager>();
	}
	
	public virtual void OnDamage() {		
		if(cM.anim)
			cM.anim.Play("Hurt",2);
	}
	
	public void OnEnable() {
		if(CamManager.instance)
			CamManager.instance.Players.Add(this);
	}
	
	public void OnDisable() {
		if(CamManager.instance)
			CamManager.instance.Players.Remove(this);
	}
}
