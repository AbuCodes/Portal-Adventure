using UnityEngine;
using System.Collections;

public class EnemyAtkRanged : EnemyAttack {
public ProjectileAttributes projectileSettings;
	public ShotAttributes ShootSettings;
	public string ShootAnimationString="Attack";
	
	public override void Start () {
		if(projectileSettings.ProjectileObject)
			projectileSettings.ProjectileObject.CreatePool();
		if(ShootSettings.MuzzleEffect)
			ShootSettings.MuzzleEffect.CreatePool();
	}
	
	public override void Attack(Vector3 attackDirection) {
		eAI.attacking=true;
		if(!projectileSettings.ProjectileObject)
			return;
		StartCoroutine(GenerateShots(ShootSettings.bulletsPerShot,attackDirection));
	}
	
	
	public virtual IEnumerator GenerateShots(int BulletAmmount,Vector3 attackDirection) {
		//float decreaseRate = 1 + shakeIntensity;
		yield return new WaitForSeconds(WaitBeforeAttack);
		float timeOfShot=Time.time;
		
		eAI.cM.isAttacking(true);
		
		if(BulletAmmount<1)
			BulletAmmount=1;
		
		if(ShootSettings.bulletsPerShotInterval==0 && ShootSettings.ShotSoundEffect!="")
			SoundManager.instance.Play(ShootSettings.ShotSoundEffect);
		
		while (BulletAmmount > 0) {			
			if(timeOfShot + ShootSettings.bulletsPerShotInterval < Time.time) {
				timeOfShot=Time.time;
				
				if(ShootSettings.bulletsPerShotInterval!=0 && ShootSettings.ShotSoundEffect!="")
					SoundManager.instance.Play(ShootSettings.ShotSoundEffect);
				
				if(ShootSettings.weaponKick!=0)
					eAI.cM.TakeKnockback(ShootSettings.spawnPoint.DirectionTo(eAI.transform).normalized*ShootSettings.weaponKick,ShootSettings.weaponKickDuration);
				
				spawnProjectile(attackDirection);
				BulletAmmount--;
			}
			yield return 0;
		}
		yield return new WaitForSeconds(WaitAfterAttack);
		eAI.attacking=false;
		eAI.cM.isAttacking(false);
	}
	
	public virtual void spawnProjectile(Vector3 attackDirection) {
		if(ShakeIntensity>0&&CamManager.instance.shaker)
			CamManager.instance.shaker.shake(ShakeIntensity);
			
		if(projectileSettings.projectileAngleVariation>0)
			attackDirection = new Vector3(attackDirection.x+Random.Range(-projectileSettings.projectileAngleVariation/100,projectileSettings.projectileAngleVariation/100),0,attackDirection.z);

		attackDirection = attackDirection * projectileSettings.projectileSpeed;
		
		if(eAI.cM.anim)
			eAI.cM.anim.Play(ShootAnimationString,0);
		
		attackMethods.spawnProjectile(projectileSettings.ProjectileObject,ShootSettings.spawnPoint.position,attackDirection,projectileSettings.projectileRange,eAI.transform);
		
		if(ShootSettings.MuzzleEffect) {
			Transform muzzle = ShootSettings.MuzzleEffect.Spawn(ShootSettings.spawnPoint.position);
			muzzle.parent = ShootSettings.spawnPoint;
			muzzle.rotation=Quaternion.LookRotation(attackDirection);
		}
	}
}
