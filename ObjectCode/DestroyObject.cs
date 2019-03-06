using UnityEngine;
using System.Collections;

public class DestroyParticles : MonoBehaviour
{
    public int Duration;

    private IEnumerator Start()
    {
        //yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }

}