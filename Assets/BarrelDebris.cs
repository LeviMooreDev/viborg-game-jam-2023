using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDebris : MonoBehaviour
{

    public Rigidbody[] rbs;

    public float force;

    public ParticleSystem dust;

    // Start is called before the first frame update
    void Start()
    {
       // StartCoroutine(WaitTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode(Vector3 barrelForce){
        foreach(Rigidbody rb in rbs){
            rb.isKinematic = false;


            Vector3 dir = rb.gameObject.transform.localPosition;
            dir.Normalize();

            rb.AddForce(barrelForce+(dir+Vector3.up*2)*force, ForceMode.Impulse);

            dust.Play();
        }

        StartCoroutine(NOCLiider());
    }

    IEnumerator WaitTime(){
        yield return new WaitForSeconds(2);
        Explode(Vector3.zero);
    }

    IEnumerator NOCLiider()
    {
        yield return new WaitForSeconds(5);
        foreach (Rigidbody rb in rbs)
        {
            Destroy(rb.gameObject.GetComponent<MeshCollider>());
            Destroy(rb);
        }
    }
}
