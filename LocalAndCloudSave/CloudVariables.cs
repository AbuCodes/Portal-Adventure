using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudVariables : MonoBehaviour {

	[SerializeField]
	public static int[] Values {get; set;}

	private void Awake(){
		Values = new int[20];
	}
}
