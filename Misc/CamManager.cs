using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamManager : MonoBehaviour {

	[HideInInspector]
	public List<PlayerController> Players = new List<PlayerController>();
	Vector3 LookVector;
	public int height=25;
	public int distance = 15;
	public float followSpeed=5;
	public float MaxZoomOut=10;
	public float rotateSpeed=2;
	Transform CameraManager;
	Transform Zoom;
	Vector3 midCenter,wantedPosition;
	[HideInInspector]
	public Shake shaker;

    private static CamManager _instance;
 
    public static CamManager instance {
        get {
            if(_instance == null)
                _instance = GameObject.FindObjectOfType<CamManager>();
 
            return _instance;
        }
    }
 
    void Awake() {
        if(_instance == null)
            _instance = this;
        else
            if(this != _instance)
                Destroy(this.gameObject);
    }
	
	void Start () {
		CameraManager = new GameObject().transform;
		CameraManager.name = "_Camera Manager";
		
		Zoom = new GameObject().transform;
		Zoom.name = "_Zoom";
		
		transform.parent = Zoom;
		Zoom.parent=CameraManager;
		
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		
		shaker=GetComponent<Shake>();
	}
	
	void Update () {
		if (Players.Count==0)
			return;
		if(Players.Count>1)
			calcCenter();
		else
			LookVector=Players[0].transform.position;
		Follow();
		RotateTowards();
		if (Input.GetKeyDown(KeyCode.PageDown)) {
            if(height==7) {
            	height=25;
            	distance=15;
            } else {
            	height=7;
            	distance=15;
            }
        }
	}
	
	void RotateTowards()
	{
		Vector3 LookToPosition;
		if(Players.Count>1)
			LookToPosition = midCenter;
		else
			LookToPosition = LookVector;
					
		Quaternion wantedRotation = Quaternion.LookRotation (LookToPosition - CameraManager.position);
		CameraManager.rotation = Quaternion.Slerp (CameraManager.rotation, wantedRotation, Time.deltaTime * rotateSpeed);
	}

	void Follow()
	{
		if(Players.Count>1)
			wantedPosition = midCenter+new Vector3(0, height, -distance);
		else
			wantedPosition = LookVector+new Vector3(0, height, -distance);
		
		CameraManager.position = Vector3.Lerp (CameraManager.position, wantedPosition, Time.deltaTime * followSpeed);
	}
	
	void calcCenter()
    {
    	List<float> xPositions = new List<float>();
    	List<float> zPositions = new List<float>();
    	
    	foreach (PlayerController player in Players) {
    		if(!player.cH.Dead) {
	    		xPositions.Add(player.gameObject.transform.position.x);
	    		zPositions.Add(player.gameObject.transform.position.z);
    		}
    	}
    	
    	float maxX = Mathf.Max(xPositions.ToArray());
    	float maxZ = Mathf.Max(zPositions.ToArray());
    	float minX = Mathf.Min(xPositions.ToArray());
    	float minZ = Mathf.Min(zPositions.ToArray());
    	
    	var minPosition = new Vector3(minX, 0, minZ);
    	var maxPosition = new Vector3(maxX, 0, maxZ);
    	
    	float distance = Vector3.Distance (minPosition,maxPosition);
    	distance = ((distance/3)-5);
    	distance = Mathf.Clamp(distance, 0, MaxZoomOut);
    	Zoom.transform.SetZ(-distance);
        
        midCenter = Vector3.Lerp(minPosition, maxPosition,0.5f);
    }
    
}
