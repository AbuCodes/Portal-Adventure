using UnityEngine;
using System.Collections;

	[System.Serializable]
	public class LootAttributes {
		public Transform Prefab;
		public int Weight;
		public float SpawnY=0;
	}

public class WeightedLootSpawner : MonoBehaviour {

	public LootAttributes[] items;
	
	public bool spawnOnDeath=true;
	public bool spawnOnStart=false;
	public int SpawnRandomQuantityMin=1;
	public int SpawnRandomQuantityMax=1;
	public float PositionVariationFactor=0;
	
	public virtual void Start() {
		for (int i = 0; i < items.Length; i++)
			items[i].Prefab.CreatePool();
			
		if(spawnOnStart)
			SpawnRandomItem();
	}
	
	public int RandomItem() {
	    int range = 0;
	    for (int i = 0; i < items.Length; i++)
	        range += items[i].Weight;
	 
	    var rand = Random.Range(0, range);
	    int top = 0;
	 
	    for (int i = 0; i < items.Length; i++) {
	        top += items[i].Weight;
	        if (rand < top)
	            return i;
	    }
	    
	    return 0;
	}
	
	public void  SpawnRandomItem(Vector3 pos) {
		float SpawnRandomQuantity=Random.Range(SpawnRandomQuantityMin,SpawnRandomQuantityMax);
		for (int i = 0; i < SpawnRandomQuantity; i++){
	   		int ItemToSpawn = RandomItem();
	   		pos.x+=Random.Range(-1.5f,1.5f)*PositionVariationFactor;
	   		pos.z+=Random.Range(-1.5f,1.5f)*PositionVariationFactor;
			pos.y=transform.position.y+items[ItemToSpawn].SpawnY;
			var Obj = items[ItemToSpawn].Prefab.Spawn(pos);
		    GameManager.instance.ToOrganizer(Obj);
		 }
	}
	
	public void SpawnRandomItem() {
		SpawnRandomItem(transform.position);
	}
	
	void OnDeath() {
		if(spawnOnDeath) 
			SpawnRandomItem();
	}
}