using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public ParticleSystem dust1;
    public ParticleSystem dust2;

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

    public bool safe;

    void Update()
    {
        if(move){
            float zVal = isMax ? minZ : maxZ;
            speed *= (1+(accelSpeed * Time.deltaTime));
            if(speed > maxSpeed) speed = maxSpeed;
            pivot.rotation = Quaternion.Euler(0,0, Mathf.MoveTowards(pivot.localEulerAngles.z, zVal, speed*Time.deltaTime));
            if((!isMax && pivot.localEulerAngles.z == maxZ) || (isMax && pivot.localEulerAngles.z == minZ)){
                StartCoroutine(WaitNext());

                bool playAudio = false;
                float v = 0;
                foreach (var item in GameObject.FindGameObjectsWithTag("CarController"))
                {
                    float d = Vector3.Distance(item.transform.position, transform.position);
                    if (d < 20)
                    {
                        playAudio = true;
                        v = .4f;
                    }
                    else if (d < 18)
                    {
                        playAudio = true;
                        v = .6f;
                    }
                    else if (d < 15)
                    {
                        playAudio = true;
                        v = .7f;
                    }
                    else if (d < 13)
                    {
                        playAudio = true;
                        v = .8f;
                    }
                    else if (d < 11)
                    {
                        playAudio = true;
                        v = .9f;
                    }
                    else if (d < 9)
                    {
                        playAudio = true;
                        v = 1;
                    }
                }
                if (playAudio)
                {
                    AudioManager.I.Play(AudioManager.A.slam1, v);
                }

                if (isMax)
                {
                    dust1.Play();
                }
                else
                {
                    dust2.Play();
                }
            }
        }
    }

    IEnumerator WaitNext()
    {
        safe = true;
        Stop();
        yield return new WaitForSeconds(waitTime);
        isMax = !isMax;
        Play();
        yield return new WaitForSeconds(.5f);
        safe = false;
    }
}
