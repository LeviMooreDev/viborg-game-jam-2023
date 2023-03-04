using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{

    public Transform pivot;

    public float minZ = 0f;  
    public float maxZ = 180f;


    public float maxSpeed = 10f;
    
    public float speed = 1f;
    float startSpeed;
    
    public float accelSpeed = 0.05f;

    bool move = false;
    bool isMax = false;

    public float waitTime = 2;

    void Start()
    {
      startSpeed = speed;
      Setup();
    }

    public void Setup(){
        pivot.rotation = Quaternion.Euler(0,0,0);
         StartCoroutine(WaitNext()); 
    }

    public void Play(){
        speed = startSpeed;
        move = true;
    }

    public void Stop(){
        move = false;
    }

    void Update()
    {
        if(move){
            float zVal = isMax ? minZ : maxZ;
            speed *= (1+(accelSpeed * Time.deltaTime));
            if(speed > maxSpeed) speed = maxSpeed;
            pivot.rotation = Quaternion.Euler(0,0, Mathf.MoveTowards(pivot.localEulerAngles.z, zVal, speed*Time.deltaTime));
            if((!isMax && pivot.localEulerAngles.z == maxZ) || (isMax && pivot.localEulerAngles.z == minZ)){
                StartCoroutine(WaitNext()); 
            }
        }
    }

    IEnumerator WaitNext(){
        Stop();
        yield return new WaitForSeconds(waitTime);
        isMax = !isMax;
        Play();
    }
}
