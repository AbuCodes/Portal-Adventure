using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    // Instantiate a prefab with an attached Missile script
    public GameObject Object;
    public float period;
    public float period_reset;
    

	void Start()
	{
		// Instantiate the missile at the position and rotation of this object's transform

	}
    void Update()
    {
        if(period > 0)
        {
            period -= Time.deltaTime;
        }
        else if(period <= 0)
        {
            period = period_reset;
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        Instantiate(Object, transform.position, transform.rotation);
    }
}