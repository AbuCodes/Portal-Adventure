using UnityEngine;
using System.Collections;

public class PickupWeapon : PickupObject {

	Weapon myWeapon;
	Transform PickUpSign,PickUpSignPrefab;
	Rigidbody rb;
	SphereCollider m_Collider;
	ConstantRotation constR;
		
	void Awake() {
		myWeapon=GetComponent<Weapon>();
		AddPickupStuff();
	}

	public override void PickupEffect(PlayerController pC) {
        pC.weaponManager.EquipWeapon(myWeapon,false);
		RemovePickupStuff();
    }
    
    public void AddPickupStuff() {
		PickUpSignPrefab = Resources.Load<Transform>("WeaponPickupSign");
		PickUpSignPrefab.CreatePool();
		PickUpSign = PickUpSignPrefab.Spawn(transform.position);
		PickUpSign.localRotation = Quaternion.Euler(90,0,0);
		
		constR = gameObject.AddComponent<ConstantRotation>();
		constR.RotationsPerMinuteY=15;
		
		m_Collider = gameObject.AddComponent<SphereCollider>();
		m_Collider.isTrigger=true;
		m_Collider.radius=1.7f;
		
		rb = gameObject.AddComponent<Rigidbody>();
		if(rb) {
			rb.isKinematic=true;
			rb.useGravity=false;
		}
		
		TimeOfStart=Time.time;
    }
    
    public void RemovePickupStuff() {
    	var constR = GetComponent<ConstantRotation>();
        if(PickUpSign)
        	PickUpSign.Recycle();
        	
        if(constR)
        	Destroy(constR);
        	
        if(rb)
        	Destroy(rb);
        	
        if(m_Collider)
        	Destroy(m_Collider);
        	
        Destroy(this);
    }
}
