using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKillCheck : MonoBehaviour
{
    public CarController carController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hammer")
        {
            if (other.GetComponentInParent<Hammer>().safe)
            {
                carController.Kill();
            }
        }
    }
}
