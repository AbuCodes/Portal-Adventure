using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	[System.Serializable]
	public class playerDetection {
		public float detectionRange;
		public float detectionInterval;
		[HideInInspector]
		public bool detectionOnlyIfPlayerInSight=false;
	}
	
	[System.Serializable]
	public class Attack {
		public EnemyAttack enemyAttack;
		public float attackAtAngle=50f;
		public float attackAtMinRange=4f;
		public float attackAtMaxRange=4f;
		public float attackInterval=0.8f;
		public bool rotateDuringAttack=false;
		public bool moveDuringAttack=false;
	}
	
	[System.Serializable]
	public class Evade {
		public bool canEvade;
		public float evadeRange;
		public float evadeOnHealth;
		public float evadeInterval;
	}
	
	[System.Serializable]
	public class Hurt {
		public bool stopOnHurt=true;
		public float recoverDuration=0.3f;
	}
	
	[System.Serializable]
	public class Stun {
		public bool canBeStunned;
		public float recoverDuration;
	}

public class EnemyAI : MonoBehaviour {

	public enum State { Roam, Idle, Chase, Evade, Attack, Hurt, Stun, Dead }
	
	public playerDetection PlayerDetectionSettings;
	public Attack AttackSettings;
	public Evade EvadeSettings;
	public Hurt HurtSettings;
	public Stun StunSettings;
	
	public float ChoiceMakingInterval;
	
	public bool canRoamAround=true;
	public bool Log=true;
	
	[HideInInspector]
	public PlayerController playerTarget;
	
	[HideInInspector]
	public characterMove cM;
	[HideInInspector]
	public characterHealth cH;
	
	float TimeOfHurt, TimeOfRandomAngleChange,TimeOfActionChoice,choiceInterval,TimeOfAttack,TimeOfPlayerDetect;
	
	[HideInInspector]
	public bool attacking=false;
	
	float h;
	float v;
	
	State state = State.Idle;
	
	// Use this for initialization
	public virtual void Start () {
		if(AttackSettings.enemyAttack)
			AttackSettings.enemyAttack.eAI = this;
		cM=GetComponent<characterMove>();
		cH=GetComponent<characterHealth>();
		choiceInterval=ChoiceMakingInterval;
	}
	
	public virtual void OnEnable() {
		if(!cM)
			cM=GetComponent<characterMove>();
		else if(cM.anim)
			cM.anim.SetBool("Dead",false);
		TimeOfRandomAngleChange = Time.time;
		TimeOfActionChoice = Time.time;
		TimeOfAttack = Time.time;
		TimeOfPlayerDetect = Time.time;
		TimeOfHurt = Time.time;
	}	
	
	void OnGUI () {
		if (!Log)
			return;
        GUI.Label (new Rect (0,0,400,50), "State "+state);
    }
	
	void FixedUpdate () {	
		var newState = UpdateState();
		
		if (newState != state)
			SetState(newState);
	}
	
	public virtual void MakeChoice() {
		if(TimeSince(TimeOfActionChoice,choiceInterval)) {
			
		}
	}
	
	public virtual void SetState(State newState)
	{
		state = newState;
		
		switch (state)
		{
			case State.Roam:
				SetRoam();
				break;
	
			case State.Idle:
				SetIdle();
				break;
	
			case State.Chase:
				SetChase();
				break;
			
			case State.Evade:
				SetEvade();
				break;
			
			case State.Attack:
				SetAttack();
				break;
				
			case State.Hurt:
				SetHurt();
				break;
				
			case State.Stun:
				SetStun();
				break;
				
			case State.Dead:
				SetDead();
				break;
		}
	}

	public virtual void SetRoam() {
		dLog("State mudou para Roam");
	}
	
	public virtual void SetIdle() {
		dLog("State mudou para Idle.");
	}
	
	public virtual void SetChase() {
		dLog("State mudou para Chase");
	}
	
	public virtual void SetEvade() {
		dLog("State mudou para Evade");
	}
	
	public virtual void SetAttack() {
		dLog("State mudou para Attack");
	}
	
	public virtual void SetHurt() {
		dLog("State mudou para Hurt");
		if(cM.anim)
			cM.anim.Play("Hurt",0);
	}
	
	public virtual void SetStun() {
		dLog("State mudou para Stun");
	}
	
	public virtual void SetDead() {
		dLog("State mudou para Dead");
			cM.anim.SetBool("Dead",true);
	}
	


	public virtual State UpdateState()
	{
		switch (state)
		{
			
			case State.Roam:
				var RoamStateResult = RoamState();
				if(RoamStateResult!=state)
					return RoamStateResult;
				break;
			
			case State.Idle:
				var IdleStateResult = IdleState();
				if(IdleStateResult!=state)
					return IdleStateResult;
				break;
			
			case State.Chase:
				var ChaseStateResult = ChaseState();
				if(ChaseStateResult!=state)
					return ChaseStateResult;
				break;
				
			case State.Evade:
				var EvadeStateResult = EvadeState();
				if(EvadeStateResult!=state)
					return EvadeStateResult;
				break;
				
			case State.Attack:
				var AttackStateResult = AttackState();
				if(AttackStateResult!=state)
					return AttackStateResult;
				break;
			
			case State.Hurt:
				var HurtStateResult = HurtState();
				if(HurtStateResult!=state)
					return HurtStateResult;
				break;
				
			case State.Stun:
				var StunStateResult = StunState();
				if(StunStateResult!=state)
					return StunStateResult;
				break;
				
			case State.Dead:
				var DeadStateResult = DeadState();
				if(DeadStateResult!=state)
					return DeadStateResult;
				break;
		}
		
		return state;
	}
	
	public virtual State RoamState() {
		if(TimeSince(TimeOfRandomAngleChange,2f)) {
			StartCoroutine(MoveFloatH(Random.Range(-1.5f,1.5f),2f));
			StartCoroutine(MoveFloatV(Random.Range(-1.5f,1.5f),2f));
			
			TimeOfRandomAngleChange=Time.time;
		}
		
		if(TimeSince(TimeOfPlayerDetect,PlayerDetectionSettings.detectionInterval)) {
			var pTarget = GetClosestPlayer(PlayerDetectionSettings.detectionRange , PlayerDetectionSettings.detectionOnlyIfPlayerInSight);
			if(pTarget) {
				playerTarget = pTarget;
				return State.Chase;
			}
		}
		
		if(TimeSince(TimeOfActionChoice,ChoiceMakingInterval)) {
			if(Random.value<0.3f&&canRoamAround)
				return State.Idle;
		}
		
		cM.TryMovement(h,v,true);
		
		return state;
	}
	
	public virtual State IdleState() {
	
		if(TimeSince(TimeOfActionChoice,ChoiceMakingInterval)) {
			TimeOfActionChoice=Time.time;
			if(Random.value<0.3f&&canRoamAround)
				return State.Roam;
		}
		
		if(TimeSince(TimeOfPlayerDetect,PlayerDetectionSettings.detectionInterval)) {
			TimeOfPlayerDetect=Time.time;
			var pTarget = GetClosestPlayer(PlayerDetectionSettings.detectionRange , PlayerDetectionSettings.detectionOnlyIfPlayerInSight);
			if(pTarget) {
				playerTarget = pTarget;
				return State.Chase;
			}
		}
	
		return state;
	}
  
	public virtual State ChaseState() {
		//Vixe sumiu o Player quero outro
		if(!playerTarget||playerTarget.cH.Dead) {
			var pTarget = GetClosestPlayer(PlayerDetectionSettings.detectionRange*3);
			//Se achei outro seta, senão vira idle
			if(pTarget)
				playerTarget = pTarget;
			else
				return State.Idle;
		}
		
		if(TimeSince(TimeOfPlayerDetect,PlayerDetectionSettings.detectionInterval)) {
			TimeOfPlayerDetect=Time.time;
			if(transform.DistanceTo(playerTarget.transform)<AttackSettings.attackAtMinRange && cM.body.transform.isInFront(playerTarget.transform,AttackSettings.attackAtAngle))
				return State.Attack;
		}
		
		Vector3 dir = transform.DirectionTo(playerTarget.transform);
		cM.TryMovement(dir.x,dir.z,true);
		
		return state;
	}
	
	public virtual State EvadeState() {
	
		return state;
	}
	
	public virtual State AttackState() {
		if(attacking && AttackSettings.rotateDuringAttack)
			RotateToPlayer();
			
		if(attacking && AttackSettings.moveDuringAttack)
			MoveToPlayer();
			
		if(attacking)
			return state;
			
		if(!playerTarget||playerTarget.cH.Dead)
			return State.Chase;
		
		if(TimeSince(TimeOfPlayerDetect,PlayerDetectionSettings.detectionInterval)) {
			TimeOfPlayerDetect=Time.time;
			if(transform.DistanceTo(playerTarget.transform)>AttackSettings.attackAtMaxRange || !cM.body.transform.isInFront(playerTarget.transform,AttackSettings.attackAtAngle))
				return State.Chase;
		}
		
		if(!AttackSettings.enemyAttack)
			return State.Chase;
			
		if(TimeSince(TimeOfAttack,AttackSettings.attackInterval)) {
			TimeOfAttack=Time.time;
			AttackSettings.enemyAttack.Attack(transform.DirectionTo(playerTarget.transform));
		}
		
		RotateToPlayer();
		MoveToPlayer();
		
		return state;
	}
	
	public virtual State HurtState() {
		if(TimeSince(TimeOfHurt,HurtSettings.recoverDuration))
			return State.Chase;
	
		return state;
	}
	
	public virtual State StunState() {
	
		return state;
	}
	
	public virtual State DeadState() {
	
		return state;
	}
	
	public virtual void RotateToPlayer() {
		Vector3 dir = transform.DirectionTo(playerTarget.transform);
		cM.RotateTo(dir,false);
	}
	
	public virtual void MoveToPlayer() {
		Vector3 dir = transform.DirectionTo(playerTarget.transform);
		cM.TryMovement(dir.x,dir.z,true);
	}
	
	public virtual void OnDamage(Transform attacker) {
		if(!playerTarget)
			playerTarget=attacker.GetComponent<PlayerController>();
			
		if(playerTarget.transform!=attacker && Random.value<0.4f)
			playerTarget=attacker.GetComponent<PlayerController>();
		
		if(!HurtSettings.stopOnHurt)
			return;
		
		TimeOfHurt = Time.time;
		SetState(State.Hurt);
	}
	
	public virtual void OnDeath() {
		SetState(State.Dead);
	}
	
	public virtual PlayerController GetClosestPlayer(float range, bool LOS=false) {
		if(!CamManager.instance)
			return null;
		
		float distance=range+0.1f;
		PlayerController ClosestPlayer = null;
		
		foreach (PlayerController player in CamManager.instance.Players) {
			if(player.cH.Dead)
				continue;
			if(LOS) {
				if(!HaveLineOfSight(player.transform))
					continue;
			}
			float myDistance = transform.DistanceTo(player.transform);
			if(myDistance<distance){
				distance=myDistance;
				ClosestPlayer=player;
			}
    	}
    	
    	return ClosestPlayer;
	}
	
	public bool HaveLineOfSight (Transform Other) {
		RaycastHit hit;
		var rayDirection = Other.position - transform.position;
		
		int layerToIgnore = 1 << gameObject.layer;
        layerToIgnore = ~layerToIgnore;
 
		if (Physics.Raycast (transform.position, rayDirection, out hit, PlayerDetectionSettings.detectionRange, layerToIgnore)) {
	        if (hit.transform == Other) {
	            return true;
	        } else {
	            return false;
	        }
	    }
	    return false;
	}
	
	public virtual List<PlayerController> GetPlayersInRange(float range) {
		if(!CamManager.instance)
			return null;
		
		List<PlayerController> Players = new List<PlayerController>();
		
		foreach (PlayerController player in CamManager.instance.Players) {
			if(player.cH.Dead)
				continue;
			float myDistance = transform.DistanceTo(player.transform);
			if(myDistance<range){
				Players.Add(player);
			}
    	}
    	
    	return Players;
	}
	
	IEnumerator MoveFloatH(float target, float duration)
	{
		float elapsed = 0;
		var start = h;
		var range = target - start;
		//Debug.Log("duration "+duration+" elapsed "+elapsed );
		while (elapsed < duration)
		{
			//Debug.Log("duration "+duration+" elapsed "+elapsed );
			elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
			h = start + range *  Ease.ExpoIn(elapsed / duration);
			yield return 0;
		}
		h = target;
	}
	
	IEnumerator MoveFloatV(float target, float duration)
	{
		float elapsed = 0;
		var start = v;
		var range = target - start;
		//Debug.Log("duration "+duration+" elapsed "+elapsed );
		while (elapsed < duration)
		{
			//Debug.Log("duration "+duration+" elapsed "+elapsed );
			elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
			v = start + range * Ease.ExpoIn(elapsed / duration);
			yield return 0;
		}
		v = target;
	}
	
	bool TimeSince(float thisTime, float interval) {
		if(thisTime + interval > Time.time) {
			return false;
		} else {
			return true;
		}
	}
	
	public void dLog(string log) {
		if(!Log)
			return;
			
		Debug.Log(log);
	}
}
