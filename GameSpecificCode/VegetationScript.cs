using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationScript : MonoBehaviour {

	// Use this for initialization
	public GameObject[] objects;
	void Start () {
		GenerateObjects();
	}
	public void GenerateObjects() {
		for (int i = 0; i < objects.Length; i++){
			float randomFloat = Random.value;
			if(randomFloat >= 0.5f){
				objects[i].SetActive(true);
			}
		}
	}
}
