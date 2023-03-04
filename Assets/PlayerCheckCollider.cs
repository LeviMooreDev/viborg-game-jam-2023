using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckCollider : MonoBehaviour
{
    public bool on;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            on = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            on = false;
        }
    }
}
