using UnityEngine;
using System.Collections;

public class SpecialBridge : MonoBehaviour {

    void Start()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 bck = transform.TransformDirection(Vector3.back);
        Debug.DrawRay(transform.position, Vector3.forward);

        if ((Physics.Raycast(transform.position, fwd, 10)) && (Physics.Raycast(transform.position, bck, 10)))
        {
            //this.transform.parent.gameObject.SetActive(true);
            foreach (Transform t in this.transform.parent)
            {
                if(t.name == "Walls"){
                    t.transform.gameObject.SetActive(false);
                }
                if(t.name == "Bridge"){
                    t.transform.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            //this.transform.parent.gameObject.SetActive(false);
            foreach (Transform t in this.transform.parent)
            {
                if(t.name == "Walls"){
                    t.transform.gameObject.SetActive(true);
                }
                if(t.name == "Bridge"){
                    t.transform.gameObject.SetActive(false);
                }
            }
        }
    }
}
