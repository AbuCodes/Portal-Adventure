using UnityEngine;
using System.Collections;

public class PickupHeal : PickupObject {

	public int HealAmount=5;
	public float OptionalHealPercentage;
	public bool ShouldHealPercentage=false;
	
	public override void PickupEffect(PlayerController pC) {
		if(ShouldHealPercentage)
        	pC.cH.HealDamagePct(OptionalHealPercentage);
        else
        	pC.cH.HealDamage(HealAmount);
                
        this.transform.Recycle();
    }
}
