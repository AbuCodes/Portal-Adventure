using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flasher : MonoBehaviour {

	
	public struct MeshStruct {
		public Renderer mesh;
		public Material originalMat;
	}
	
	//FLASH
	List<MeshStruct> meshS = new List<MeshStruct>();
	List<Material> flashMaterial = new List<Material>();
	
	int fType;
	bool flash=false;
	
	void Awake () {
		foreach (Renderer m in gameObject.GetComponentsInChildren<Renderer>()){
			MeshStruct nS = new MeshStruct();
			nS.mesh = m;
			nS.originalMat = m.material;
			meshS.Add(nS);
		}
		
		//flashMaterial[0] = White
		flashMaterial.Add(Resources.Load<Material>("FlashMatWhite"));
		//flashMaterial[1] = Red
		flashMaterial.Add(Resources.Load<Material>("FlashMatRed"));
		//flashMaterial[2] = Yellow
		flashMaterial.Add(Resources.Load<Material>("FlashMatYellow"));
		//flashMaterial[3] = Green
		flashMaterial.Add(Resources.Load<Material>("FlashMatGreen"));
		//flashMaterial[4] = Blue
		flashMaterial.Add(Resources.Load<Material>("FlashMatBlue"));
	}
	
	void OnEnable() {
		endFlash();
	}
	
	public void Flash(int type, float duration) {
		StopCoroutine("FlashEffect");
		fType=type;
		if(fType>4)
			fType=0;
			
		endFlash();
		StartCoroutine("FlashEffect",duration);
	}
	
	IEnumerator FlashEffect(float duration) {
		float interval=0;
		float timeOfFlash=Time.time;
		while (duration > 0) {			
			if(flash)
				interval=0.04f;
			else
				interval=0.06f;
				
			if(timeOfFlash + interval < Time.time) {
				timeOfFlash=Time.time;
				flash=!flash;
				doFlash();
			}
			
			duration -= Time.deltaTime * 6f;
			
			if (duration <= 0) {
				endFlash();
			}
			yield return 0;
		}
	}
	
	void endFlash() {
		foreach (MeshStruct m in meshS){
			m.mesh.enabled=true;
        	m.mesh.material=m.originalMat;
	    }
	}
	
	void doFlash(){
		  foreach (MeshStruct m in meshS){
	          if(fType==-1)
				 m.mesh.enabled=!flash;
	          else
				if(flash)
					m.mesh.material=flashMaterial[fType];
				else
					m.mesh.material=m.originalMat;
	      }
	}
	
}