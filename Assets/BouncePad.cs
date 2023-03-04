using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BouncePad : MonoBehaviour
{

    bool ready =  true;

    public GameObject top;
    public Transform springPivot;

    public Vector3 downScale;
    public Vector3 upScale;

    public Vector3 downPos;
    public Vector3 upPos;

    bool goUp = true;
    bool move = false;

    public float movePercentage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.isPressed && ready){
            Spring();
        }


        if(move){
            Vector3 targetPos = goUp ? upPos : downPos;
            top.transform.localPosition = Vector3.MoveTowards(top.transform.localPosition, targetPos, (movePercentage * (upPos.y - downPos.y)) * Time.deltaTime);

            Vector3 targetScale = goUp ? upScale : downScale;
            springPivot.localScale = Vector3.MoveTowards(springPivot.localScale, targetScale, (movePercentage * (upScale.y - downScale.y)) * Time.deltaTime);
            if(springPivot.localScale == targetScale){
                if(goUp){
                    goUp = false;
                }
                else{
                   move = false;
                   Reset();
                }
            }
        }
    }

    void Reset(){
        springPivot.localScale = downScale;
        top.transform.localPosition = downPos;
    }

    public void Spring(){
        ready = false;
        Reset();
        goUp = true;
        move = true;
        //StartCoroutine(WaitNext());
    }

    IEnumerator WaitNext(){
        yield return new WaitForSeconds(1);
        ready = true;
    }
}
