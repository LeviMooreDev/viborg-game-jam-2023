using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelColliderCheck : MonoBehaviour
{
    public CarController carController;

    private void OnTriggerEnter(Collider other)
    {
        if (carController.dead)
        {
            return;
        }

        if (other.tag == "ExplosiveBarrel")
        {
            Destroy(this);
            other.GetComponent<ExplosiveBarrel>().Explode();
            AudioManager.I.Play(AudioManager.A.explosion1);
        }
    }
}
