using UnityEngine;
using System.Collections;

public class CheatingRoomInput : MonoBehaviour {
	public string height="",distance="",acceleration="",deceleration="",moveSpeed="",jumpForce="",gravity="",projectileSpeed="";
	public string projectileAngleVariation="",bulletsPerShot="",weaponKick="",weaponKickDuration="",ReloadInterval="",AttackCooldown="";
	
	public int[] column;
	public int[] row;
	
	void OnGUI() {
		height = GUI.TextField(new Rect(column[0], row[0], 40, 25),height, 4);
	    distance = GUI.TextField(new Rect(column[1], row[1], 40, 25),distance, 4);
	    
	    acceleration = GUI.TextField(new Rect(column[2], row[2], 40, 25),acceleration, 4);
	    deceleration = GUI.TextField(new Rect(column[3], row[3], 40, 25),deceleration, 4);
	    
	    moveSpeed = GUI.TextField(new Rect(column[4], row[4], 40, 25),moveSpeed, 4);
	    jumpForce = GUI.TextField(new Rect(column[5], row[5], 40, 25),jumpForce, 4);
	    gravity = GUI.TextField(new Rect(column[6], row[6], 40, 25),gravity, 4);
	    
	    projectileSpeed = GUI.TextField(new Rect(column[7], row[7], 40, 25),projectileSpeed, 4);
	    projectileAngleVariation = GUI.TextField(new Rect(column[8], row[8], 40, 25),projectileAngleVariation, 4);
	    
	    bulletsPerShot = GUI.TextField(new Rect(column[9], row[9], 40, 25),bulletsPerShot, 4);
	    ReloadInterval = GUI.TextField(new Rect(column[10], row[10], 40, 25),ReloadInterval, 4);
	    
	    weaponKick = GUI.TextField(new Rect(column[11], row[11], 40, 25),weaponKick, 4);
	    weaponKickDuration = GUI.TextField(new Rect(column[12], row[12], 47, 25),weaponKickDuration, 4);
	    
	    AttackCooldown = GUI.TextField(new Rect(column[13], row[13], 47, 25),AttackCooldown, 4);
	}
}
