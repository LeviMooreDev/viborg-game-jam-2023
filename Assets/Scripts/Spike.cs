using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public Transform spikeParent;

    public float moveSpeed = 1;
    public float waitTime = 2;

    public float upY;
    public float downY;

    public bool isUp = true;
    bool move = false;

   

    // Start is called before the first frame update
    void Start()
    {
      Setup();
    }

    public void Setup(){
        spikeParent.localPosition = new Vector3(0, downY, 0);
         StartCoroutine(WaitNext()); 
    }

    public void Play(){
        move = true;
    }

    public void Stop(){
        move = false;
    }

    void Update()
    {
        if(move){
            float yVal = isUp ? downY : upY;
            spikeParent.localPosition = new Vector3(0, Mathf.MoveTowards(spikeParent.localPosition.y, yVal, moveSpeed*Time.deltaTime),0);;
            if((!isUp && spikeParent.localPosition.y == upY) || (isUp && spikeParent.localPosition.y == downY)){
                StartCoroutine(WaitNext()); 
            }
        }
    }

    IEnumerator WaitNext()
    {
        Stop();
        yield return new WaitForSeconds(waitTime);
        
        isUp = !isUp;
        Play();
    }

    internal bool canKill()
    {
        return spikeParent.localPosition.y > -0.25f;
    }
}
