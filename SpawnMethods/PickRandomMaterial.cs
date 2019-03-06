using UnityEngine;
using System.Collections;

public class PickRandomMaterial : MonoBehaviour {

	public Material[] PickFromMaterials;

	// Use this for initialization
	void OnEnable () {
		gameObject.GetComponent<Renderer>().material = PickFromMaterials [Random.Range (0, PickFromMaterials.Length)];
	}

}
