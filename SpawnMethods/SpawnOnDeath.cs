using UnityEngine;
using System.Collections;

public class SpawnOnDeath : MonoBehaviour {

	public Transform[] SpawnObjects;
	public int SpawnRandomQuantityMin=0;
	public int SpawnRandomQuantityMax=1;
	public bool SpawnRandom=true;
	public float PositionVariationFactor=0;

	public virtual void Start() {
		for (int i = 0; i < SpawnObjects.Length; i++)
			SpawnObjects[i].CreatePool();
	}
	
	public virtual void OnDeath() {
		Vector3 spawnposition = transform.position;
		if(SpawnRandom) {
			float SpawnRandomQuantity=Random.Range(SpawnRandomQuantityMin,SpawnRandomQuantityMax);
			for (int i = 0; i < SpawnRandomQuantity; i++){
				spawnposition=new Vector3(transform.position.x+(Random.Range(-1.5f,1.5f)*PositionVariationFactor),transform.position.y,transform.position.z+(Random.Range(-1.5f,1.5f)*PositionVariationFactor));
			    var Obj = SpawnObjects[Random.Range(0,SpawnObjects.Length)].Spawn(spawnposition);
			    GameManager.instance.ToOrganizer(Obj);
			 }
		} else {
			for (int i = 0; i < SpawnObjects.Length; i++){
			     var Obj = SpawnObjects[i].Spawn(spawnposition);
			     GameManager.instance.ToOrganizer(Obj);
			}
		}
	}

}
