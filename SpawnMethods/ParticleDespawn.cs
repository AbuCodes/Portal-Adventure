using UnityEngine;
using System.Collections;

public class ParticleDespawn : MonoBehaviour {

	ParticleSystem part;
	
	void Awake() {
		part=gameObject.GetComponent<ParticleSystem>();
		OnEnable();
	}

	private void OnEnable() {
		if(part) {
			part.Clear();
			part.Play();
		}
	}
	
	void OnDisable()
	{
		if(part) {
			part.Clear();
			part.Stop();
		}
	}
}
