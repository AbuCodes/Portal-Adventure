using UnityEngine;
using System.Collections;

public class FootDustScript : MonoBehaviour {

    public float speed = 1.0f;
    public float scaleValue;

    public Vector3 startScale;
    public Vector3 targetScale;
    public Vector3 currentScale;

    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            currentScale = transform.localScale;
            currentScale += new Vector3(scaleValue, scaleValue, scaleValue) * Time.deltaTime * speed;//1f * Time.deltaTime * speed;
            transform.localScale = currentScale;

            if (currentScale.x > targetScale.x)
            {
                ResetScale();
            }
        }
    }

    void ResetScale()
    {
        transform.localScale = startScale;
        this.gameObject.SetActive(false);
    }
}
