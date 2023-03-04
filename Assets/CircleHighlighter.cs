using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHighlighter : MonoBehaviour
{


    public float rotSpeed = 5;
    public Transform circle;

    // Update is called once per frame
    void Update()
    {
        circle.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}
