using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosiveBarrel : MonoBehaviour
{

    bool canExplode = true;

    public ParticleSystem ps1;
    public ParticleSystem ps2;

    public GameObject barrel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Explode(){
        if(!canExplode){
            return;
        }
        canExplode = false;

        ps1.Play();
        ps2.Play();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f, 1 << LayerMask.NameToLayer("Player"));
        Debug.Log(hitColliders.Length);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<CarController>().Kill();
            //hitCollider.kill? End the barrels life >:D
        }

        barrel.SetActive(false);
        GetComponent<CapsuleCollider>().enabled = false;

    }

    
    IEnumerator WaitNext(){
        yield return new WaitForSeconds(1);
        canExplode = true;
    }
}
