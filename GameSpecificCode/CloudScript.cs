using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {

    [SerializeField]
    private float cloudTime;
    [SerializeField]
    private float cloudTimeReset;

    public Transform player;

    public float distance;

    public float Cloud_Speed = 1f;

    public Transform resetPos;

    void Start()
    {
        cloudTimeReset = cloudTime;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	void Update () {

        transform.Translate(Vector3.back * Time.deltaTime * Cloud_Speed);

        cloudTime -= Time.deltaTime;

        distance = Vector3.Distance(transform.position, player.position);

        if (cloudTime < 0 && distance >= 33)
        {
            cloudTime = cloudTimeReset + Random.Range(0, 6);
            this.transform.position = new Vector3(resetPos.position.x, resetPos.position.y, resetPos.position.z + Random.Range(-10, 11)); //resetPos.position;
        }
    }
}
