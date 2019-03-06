using UnityEngine;
using System.Collections;

	[System.Serializable]
	public class ProjectileAttributes {
		public projectile ProjectileObject;
		public float projectileSpeed = 70f;
		public float projectileRange=40f;
		public float projectileAngleVariation=10;
	}
	
	[System.Serializable]
	public class ShotAttributes {
		public Transform MuzzleEffect;
		public Transform spawnPoint;
		public string ShotSoundEffect;
		public int bulletsPerShot=1;
		public float bulletsPerShotInterval =0f;
		public float weaponKick;
		public float weaponKickDuration;
	}
	
	[System.Serializable]
	public class AmmoAttributes {
		public int CurrentAmmo;
		public int MaxAmmo=150;
		public float ReloadInterval=0.1f;
		public float WaitTimeBeforeReload=0.2f;
		public bool AutoReload=true;
		public bool UpdateHUDAmmoBar=true;
	}

public class RangedWeapon : Weapon {
	
	public ProjectileAttributes projectileSettings;
	public ShotAttributes ShootSettings;
	public AmmoAttributes AmmoSettings;
	[HideInInspector]
	float timeOfShot;
	int ShotsFired;
	
	public override void Start () {
		if(projectileSettings.ProjectileObject)
			projectileSettings.ProjectileObject.CreatePool();
		if(ShootSettings.MuzzleEffect)
			ShootSettings.MuzzleEffect.CreatePool();
		
		AmmoSettings.CurrentAmmo=AmmoSettings.MaxAmmo;

		timeOfAttack = Time.time;
	}
	
	public override void ExecuteAttack(Vector3 attackDirection) {
		if(!Owner)
			return;
		if(AmmoSettings.CurrentAmmo>0)
			StartCoroutine(GenerateShots(ShootSettings.bulletsPerShot,attackDirection));
		else
			Reload();
	}
	
	
	public virtual IEnumerator GenerateShots(int BulletAmmount,Vector3 attackDirection) {
		timeOfShot=Time.time;
		
		if(BulletAmmount<1)
			BulletAmmount=1;
		
		InterruptReload();
		
		Owner.cM.isAttacking(true);
		
		ShotsFired=0;
		
		if(ShootSettings.bulletsPerShotInterval==0 && ShootSettings.ShotSoundEffect!="")
			SoundManager.instance.Play(ShootSettings.ShotSoundEffect);
		
		while (BulletAmmount > 0) {
			if(AmmoSettings.CurrentAmmo>0) {
				if(timeOfShot + ShootSettings.bulletsPerShotInterval < Time.time) {
					timeOfShot=Time.time;
					
					if(ShootSettings.bulletsPerShotInterval!=0 && ShootSettings.ShotSoundEffect!="")
						SoundManager.instance.Play(ShootSettings.ShotSoundEffect);
					
					if(ShootSettings.weaponKick!=0&&Owner)
						Owner.cM.TakeKnockback(ShootSettings.spawnPoint.DirectionTo(Owner.transform).normalized*ShootSettings.weaponKick,ShootSettings.weaponKickDuration);
					
					StartCoroutine(Auto.ShakeToZero(transform,0.1f,0.1f));
					
					spawnProjectile(Owner.pI.attackDirection);
					BulletAmmount--;
				}
			} else {
				BulletAmmount=0;
			}
			yield return 0;
			if(AmmoSettings.AutoReload)
				Reload();
		}
		
		Owner.cM.isAttacking(false);
	}
	
	public virtual void Reload() {
		StopCoroutine("ReloadAmmo");
		StartCoroutine("ReloadAmmo");
	}
	
	public virtual void InterruptReload() {
		StopCoroutine("ReloadAmmo");
	}
	
	public virtual IEnumerator ReloadAmmo() {
		float timeOfLastReload=Time.time;
		while (AmmoSettings.CurrentAmmo != AmmoSettings.MaxAmmo) {
			if(Owner) {
				if(timeOfLastReload + AmmoSettings.ReloadInterval < Time.time && timeOfShot + 0.3f < Time.time ) {
					timeOfLastReload=Time.time;
					
					AmmoSettings.CurrentAmmo++;
				}
				
				if(AmmoSettings.UpdateHUDAmmoBar && HealthBarManager.instance)
					HealthBarManager.instance.UpdateAmmoBar(Owner.pID,AmmoSettings.CurrentAmmo*1.0f/AmmoSettings.MaxAmmo*1.0f);
			} else {
				InterruptReload();
			}
			yield return 0;
		}
	}
	
	public virtual void spawnProjectile(Vector3 attackDirection) {
		if(ShakeIntensity>0&&CamManager.instance.shaker)
			CamManager.instance.shaker.shake(ShakeIntensity);
		
		if(projectileSettings.projectileAngleVariation > 0 && ((ShootSettings.bulletsPerShot>1 && ShotsFired!=0) || (ShootSettings.bulletsPerShot==1) ))
			attackDirection = new Vector3(attackDirection.x+Random.Range(-projectileSettings.projectileAngleVariation/100,projectileSettings.projectileAngleVariation/100),0,attackDirection.z+Random.Range(-projectileSettings.projectileAngleVariation/100,projectileSettings.projectileAngleVariation/100));

		ShotsFired++;
		
		attackDirection = attackDirection * projectileSettings.projectileSpeed;
		
		if(Owner.cM.anim)
			Owner.cM.anim.Play("Shoot2H",1);
		
		attackMethods.spawnProjectile(projectileSettings.ProjectileObject,ShootSettings.spawnPoint.position,attackDirection,projectileSettings.projectileRange,Owner.transform);
		
		AmmoSettings.CurrentAmmo--;
				if(AmmoSettings.UpdateHUDAmmoBar)
		
		if(AmmoSettings.UpdateHUDAmmoBar)
			HealthBarManager.instance.UpdateAmmoBar(Owner.pID,AmmoSettings.CurrentAmmo*1.0f/AmmoSettings.MaxAmmo*1.0f);
			
		if(ShootSettings.MuzzleEffect) {
			Transform muzzle = ShootSettings.MuzzleEffect.Spawn(ShootSettings.spawnPoint.position);
			muzzle.parent = ShootSettings.spawnPoint;
			muzzle.rotation=Quaternion.LookRotation(attackDirection);
		}
	}
	
	public override void OnEquip() {
		if(AmmoSettings.UpdateHUDAmmoBar && HealthBarManager.instance)
			HealthBarManager.instance.UpdateAmmoBar(Owner.pID,AmmoSettings.CurrentAmmo*1.0f/AmmoSettings.MaxAmmo*1.0f);
			
		if(AmmoSettings.AutoReload)
			Reload();
	}
}
