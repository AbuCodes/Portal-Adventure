using UnityEngine;
using System.Collections;

public class CheatingRoom : MonoBehaviour {

	public RangedWeapon wp;
	public characterMove cM;
	public CamManager cF;
	
	public CheatingRoomInput ci;
	
	public Transform RangedEnemy;
	public Transform MeleeEnemy;
	
	public GUIStyle myStyle;
	

	public Font MyFont;
	
	bool ShowCheats=true;
	
	bool Init=false;
	
	void Start() {
		StartCoroutine(LateStart());
		ci=GetComponent<CheatingRoomInput>();
	}
	void Update () {
		if(!ShowCheats||!Init)
			return;
			
		if(ci.height.Length>0)
			cF.height = int.Parse(ci.height);
		if(ci.distance.Length>0)
			cF.distance = int.Parse(ci.distance);
		
		if(ci.acceleration.Length>0)
			cM.acceleration = float.Parse(ci.acceleration);
		if(ci.deceleration.Length>0)
			cM.deceleration = float.Parse(ci.deceleration);
		if(ci.moveSpeed.Length>0)
			cM.currentMoveSpeed = float.Parse(ci.moveSpeed);
		if(ci.jumpForce.Length>0)
			cM.jumpForce = float.Parse(ci.jumpForce);
		if(ci.gravity.Length>0)
			cM.gravity = float.Parse(ci.gravity);
		
		if(ci.projectileSpeed.Length>0)	
			wp.projectileSettings.projectileSpeed = float.Parse(ci.projectileSpeed);
		if(ci.projectileAngleVariation.Length>0)
			wp.projectileSettings.projectileAngleVariation = float.Parse(ci.projectileAngleVariation);
		if(ci.bulletsPerShot.Length>0)
			wp.ShootSettings.bulletsPerShot = int.Parse(ci.bulletsPerShot);
		if(ci.weaponKick.Length>0)
			wp.ShootSettings.weaponKick = float.Parse(ci.weaponKick);
		if(ci.weaponKickDuration.Length>0)
			wp.ShootSettings.weaponKickDuration = float.Parse(ci.weaponKickDuration);
		if(ci.ReloadInterval.Length>0)
			wp.AmmoSettings.ReloadInterval = float.Parse(ci.ReloadInterval);
		
		if(ci.AttackCooldown.Length>0)	
			wp.AttackCooldown = float.Parse(ci.AttackCooldown);
	}
	
	IEnumerator LateStart() {
		yield return new WaitForSeconds(0.1f);
		
		wp=(RangedWeapon)cM.GetComponent<PlayerController>().weaponManager.currentWeapon;
		
		ci.height=cF.height.ToString();
		ci.distance=cF.distance.ToString();
		
		ci.acceleration = cM.acceleration.ToString();
		ci.deceleration = cM.deceleration.ToString();
		ci.moveSpeed = cM.currentMoveSpeed.ToString();
		ci.jumpForce = cM.jumpForce.ToString();
		ci.gravity = cM.gravity.ToString();
		
		ci.projectileSpeed = wp.projectileSettings.projectileSpeed.ToString();
		ci.projectileAngleVariation = wp.projectileSettings.projectileAngleVariation.ToString();
		ci.bulletsPerShot = wp.ShootSettings.bulletsPerShot.ToString();
		ci.weaponKick = wp.ShootSettings.weaponKick.ToString();
		ci.weaponKickDuration = wp.ShootSettings.weaponKickDuration.ToString();
		ci.ReloadInterval = wp.AmmoSettings.ReloadInterval.ToString();
		ci.AttackCooldown = wp.AttackCooldown.ToString();
		
		wp.AmmoSettings.MaxAmmo=500;
		wp.AmmoSettings.CurrentAmmo=500;
		
		Init=true;
		yield return 0;
	}
	
	void OnGUI() {
		if(!Init)
			return;
			
		GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize=13;
		GUIStyle myStyleInput = new GUIStyle();
		myStyleInput.fontSize=13;
		GUIStyle myStyleH = new GUIStyle();
		myStyleH.fontSize=22;
		GUIStyle myStyleV = new GUIStyle();
		myStyleV.fontSize=16;
		myStyleV.normal.textColor=Color.white;
	    GUI.skin.font = MyFont;
	    
	    if(DrawButtonWithShadow(new Rect(10,Screen.height - 50, 100, 50),new GUIContent("Main Menu"),myStyleV,0.5f,new Vector2(3,3)))
	            Application.LoadLevel("MainMenu");
	    
	    if(!ShowCheats) {
			if(DrawButtonWithShadow(new Rect(Screen.width - 125,Screen.height - 50, 100, 50),new GUIContent("Show Cheats"),myStyleV,0.5f,new Vector2(3,3)))
	            ShowCheats=!ShowCheats;
	        return;
		}

		int yspace = 10;
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("Camera"),myStyleH,Color.white,Color.black,new Vector2(2,2));
	    
	    int regularspacing=30;    
	    
	    yspace += regularspacing;
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("Height"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    DrawShadow(new Rect(95, yspace, 150, 100), new GUIContent("Distance"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    
		yspace += regularspacing;
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("Player"),myStyleH,Color.white,Color.black,new Vector2(2,2));
	    yspace += regularspacing;
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("Accel"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    DrawShadow(new Rect(95, yspace, 150, 100), new GUIContent("Decel"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    
		yspace += regularspacing;
	    
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("Speed"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    DrawShadow(new Rect(95, yspace, 150, 100), new GUIContent("Jump"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    DrawShadow(new Rect(180, yspace, 150, 100), new GUIContent("Gravity"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    
		yspace += regularspacing;
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("Weapon"),myStyleH,Color.white,Color.black,new Vector2(2,2));
	    yspace += regularspacing;
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("ProjSpeed"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    DrawShadow(new Rect(140, yspace, 150, 100), new GUIContent("AngleVariation"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    yspace += regularspacing;


	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("BulletsP/Shot"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    DrawShadow(new Rect(140, yspace, 150, 100), new GUIContent("ReloadTime"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    yspace += regularspacing;
	
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("WeaponKick"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    DrawShadow(new Rect(140, yspace, 150, 100), new GUIContent("KickDuration"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    
		yspace += regularspacing;
	    DrawShadow(new Rect(10, yspace, 150, 100), new GUIContent("Attack Interval"),myStyle,Color.white,Color.black,new Vector2(2,2));
	    
	    
	    
	    	    
	    if(DrawButtonWithShadow(new Rect(Screen.width - 190, 10, 100, 50),new GUIContent("Spawn Ranged Enemy"),myStyleV,0.5f,new Vector2(3,3)))
            SpawnRanged();
            
        if(DrawButtonWithShadow(new Rect(Screen.width - 190, 60, 100, 50),new GUIContent("Spawn Melee Enemy"),myStyleV,0.5f,new Vector2(3,3)))
            SpawnMelee();
        
        if(DrawButtonWithShadow(new Rect(Screen.width - 125,Screen.height - 50, 100, 50),new GUIContent("Hide cheats"),myStyleV,0.5f,new Vector2(3,3)))
            ShowCheats=!ShowCheats;
    }
    
    void SpawnRanged() {
    	Vector3 SpawnPosition = cM.gameObject.transform.position + (Random.onUnitSphere*25);
    	//ctor3 SpawnPosition = new Vector3(cM.gameObject.transform.position.x+Random.Range(-1,1)*20,2, cM.gameObject.transform.position.z+Random.Range(-1,1)*20);
    	SpawnPosition.y=2;
    	
    	Instantiate(RangedEnemy, SpawnPosition, Quaternion.identity);
    }
    
    void SpawnMelee() {
    	Vector3 SpawnPosition = cM.gameObject.transform.position + (Random.onUnitSphere*25);
    	//ctor3 SpawnPosition = new Vector3(cM.gameObject.transform.position.x+Random.Range(-1,1)*20,2, cM.gameObject.transform.position.z+Random.Range(-1,1)*20);
    	SpawnPosition.y=2;
    	
    	Instantiate(MeleeEnemy, SpawnPosition, Quaternion.identity);
    }
    
    public bool DrawButtonWithShadow(Rect r, GUIContent content, GUIStyle style, float shadowAlpha, Vector2 direction)
        {
            GUIStyle letters = new GUIStyle(style);
            letters.normal.background = null;
            letters.hover.background = null;
            letters.active.background = null;
 
            bool result = GUI.Button(r, content, style);
 
            Color color = r.Contains(Event.current.mousePosition) ? letters.hover.textColor : letters.normal.textColor;
 
            DrawShadow(r, content, letters, color, new Color(0f, 0f, 0f, shadowAlpha), direction);
 
            return result;
        }
    
    public void DrawShadow(Rect rect, GUIContent content, GUIStyle style, Color txtColor, Color shadowColor,
                                        Vector2 direction)
        {
            GUIStyle backupStyle = style;
 
            style.normal.textColor = shadowColor;
            rect.x += direction.x;
            rect.y += direction.y;
            GUI.Label(rect, content, style);
 
            style.normal.textColor = txtColor;
            rect.x -= direction.x;
            rect.y -= direction.y;
            GUI.Label(rect, content, style);
 
            style = backupStyle;
        }
}
