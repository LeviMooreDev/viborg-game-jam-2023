using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int index;
    public Vector3 offset1;
    public Vector3 offset2;
    public Vector3 rotation1;
    public Vector3 rotation2;
    public Transform target;
    public float speed;

    private void Update()
    {
        Vector3 targetPos = target.position + offset1;
        transform.eulerAngles = rotation1;
        if (index == 1)
        {
            targetPos = target.position + offset2;
            transform.eulerAngles = rotation2;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }
}
