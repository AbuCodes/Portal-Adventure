using UnityEngine;
using System.Collections;

public class EnemyAtkSelfDestruct : EnemyAttack {

	public override void Attack(Vector3 attackDirection) {
		if(!eAI)
			return;
		eAI.cH.Die();
	}
}
