using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWeaponManager : MonoBehaviour {
	[HideInInspector]
	public PlayerController pC;
	
	public Weapon defaultWeapon;
	Weapon spawnedDefaultWeapon;
	public Weapon currentWeapon;
	public List<Weapon> Inventory = new List<Weapon>();
	public Transform WeaponHoldingSpot;
	public int MaximumInventory=3;
	
	void Awake () {
		pC=GetComponent<PlayerController>();
		if(defaultWeapon) {
			defaultWeapon.CreatePool();
			EquipWeapon(defaultWeapon,true,true);
		}
		if(MaximumInventory<2)
			MaximumInventory=2;
	}
	
	public bool EquipWeapon(Weapon newWeapon, bool Spawn, bool Default=false) {
		if(currentWeapon!=spawnedDefaultWeapon && MaximumInventory < Inventory.Count+1)
			DropWeapon();
		else {
			if(Inventory.Count>1 && Inventory.IndexOf(currentWeapon)==0) {
				//Debug.Log("Trying to drop default weapon, change weapon and drop the other one.");
				ChangeWeapon(1);
				DropWeapon();
			} else {
				if(currentWeapon)
					currentWeapon.gameObject.SetActive(false);
				if(!Default)
					spawnedDefaultWeapon.gameObject.SetActive(false);
			}
		}

		if(Spawn)
			currentWeapon = newWeapon.Spawn(WeaponHoldingSpot.position);
		else {
			currentWeapon = newWeapon;
			currentWeapon.gameObject.SetActive(true);
		}
				
		if(Default)
			spawnedDefaultWeapon=currentWeapon;
		
		if(!Inventory.Contains(currentWeapon))
			Inventory.Add(currentWeapon);
		
		foreach (Weapon w in Inventory)
    		w.gameObject.SetActive(false);
    	
		currentWeapon.gameObject.SetActive(true);
		currentWeapon.name=newWeapon.name;
		currentWeapon.Owner=pC;
		currentWeapon.transform.parent=WeaponHoldingSpot;
		currentWeapon.transform.localPosition=Vector3.zero;
		currentWeapon.transform.rotation=Quaternion.Euler(0, 0, 0);
		currentWeapon.transform.localRotation=Quaternion.Euler(0, 0, 0);
		
		if(!Default)
			GameManager.instance.playerInfo[pC.pID].currentWeapon=currentWeapon.gameObject.name;
		
		if(!GameManager.instance.playerInfo[pC.pID].Inventory.Contains(currentWeapon.name))
			GameManager.instance.playerInfo[pC.pID].Inventory.Add(currentWeapon.name);
		
		if(!GameManager.instance.playerInfo[pC.pID].Init)
			GameManager.instance.playerInfo[pC.pID].Init=true;
		
		currentWeapon.OnEquip();
		
		return true;
	}
	
	public void EquipWeapon (string weaponName) {
		if(!Resources.Load("Weapons/"+weaponName))
			return;
		GameObject weaponSpawned = (GameObject) Instantiate(Resources.Load("Weapons/"+weaponName));
		if(weaponSpawned) {
			weaponSpawned.name=weaponName;
			StartCoroutine(WaitAndEquipeWeapon(weaponSpawned.GetComponent<Weapon>()));
		}
	}
	
	IEnumerator WaitAndEquipeWeapon(Weapon weapon) {
		yield return new WaitForSeconds(0.1f);
		var pickUp = weapon.GetComponent<PickupWeapon>();
		if(pickUp)
			pickUp.RemovePickupStuff();
		EquipWeapon(weapon,false);
	}
	
	public void EquipDefaultWeapon () {
		spawnedDefaultWeapon.gameObject.SetActive(true);
		spawnedDefaultWeapon.Owner=pC;
		currentWeapon = spawnedDefaultWeapon;
		GameManager.instance.playerInfo[pC.pID].currentWeapon=null;
		currentWeapon.OnEquip();
	}
	
	public void TrySwapWeapon() {
		if(Inventory.Count==1)
			return;
		
		int cIndex = Inventory.IndexOf(currentWeapon);
		if(cIndex>=Inventory.Count-1)
			ChangeWeapon(0);
		else
			ChangeWeapon(cIndex+1);
	}
	
	public void ChangeWeapon(int i) {
		foreach (Weapon w in Inventory) {
    		w.gameObject.SetActive(false);
    	}
		Inventory[i].gameObject.SetActive(true);
		Inventory[i].Owner=pC;
		currentWeapon = Inventory[i];
		
		if(i==0)
			GameManager.instance.playerInfo[pC.pID].currentWeapon=null;
			
		currentWeapon.OnEquip();
	}
	
	public void DropWeapon() {
		if(currentWeapon==spawnedDefaultWeapon||!currentWeapon)
			return;
		
		if(Inventory.Contains(currentWeapon))
			Inventory.Remove(currentWeapon);
			
		if(GameManager.instance.playerInfo[pC.pID].Inventory.Contains(currentWeapon.name))
			GameManager.instance.playerInfo[pC.pID].Inventory.Remove(currentWeapon.name);
		
		currentWeapon.gameObject.SetActive(true);
		currentWeapon.Owner=null;
		currentWeapon.transform.parent=null;
		currentWeapon.transform.position=transform.position;
		currentWeapon.transform.rotation=Quaternion.Euler(0, 0, 0);
		currentWeapon.transform.localRotation=Quaternion.Euler(0, 0, 0);
		currentWeapon.InterruptCoroutines();
		currentWeapon.OnUnequip();
		
		
		var pW = currentWeapon.gameObject.AddComponent<PickupWeapon>();
		pW.CantPickUpOnSpawn=1.5f;
		
		currentWeapon=null;

		ChangeWeapon(Inventory.Count-1);
	}
}
