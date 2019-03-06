using UnityEngine;
using System.Collections;

public class SpawnOnStart : SpawnOnDeath {

	// Use this for initialization
	public override void Start () {
		base.Start();
		OnEnable();
	}
	
	void OnEnable() {
		OnDeath();
	}
	
}
