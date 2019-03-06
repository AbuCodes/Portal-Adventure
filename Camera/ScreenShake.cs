using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

    public float shakeTimer, shakeIntensity;
    bool shakeIsOn = true;

    public float x;
    public float y;


	// Update is called once per frame
	void Update () {
        if(shakeIsOn)
        {
            if(shakeTimer > 0) {
                shakeTimer -= Time.deltaTime;
                transform.position = Random.insideUnitCircle * shakeIntensity;
            }
            else
            {
                transform.position = Vector3.zero;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shake(x, y);
        }
        	
	}

    public void Shake(float timer, float intensity)
    {
        shakeTimer = timer;
        shakeIntensity = intensity;
    }
}
