using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int playerIndex;

    public int state = 0; //0 = roll, 1 = build

    public CarInput carInput;

    //public Level

    public void OnForwardTrigger (InputValue value){
       float input = (float)(value.Get()??0f);
       if(state == 0){
        carInput.Forward(input);
       }
        
    }

     public void OnBackwardsTrigger (InputValue value){
       float input = (float)(value.Get()??0f);
       if(state == 0){
            carInput.Backwards(input);
       }
        
       
    }

    public void OnJoystick(InputValue value){
        Vector2 input = value.Get<Vector2>();
        if(input == null){
            input = Vector2.zero;
        }
        if(state == 0){
            carInput.Turn(input);
        }
        
    }

    public void OnBreak(InputValue value){
        float input = (float)(value.Get()??0f);
        if(state == 0){
            carInput.Break(input);
        }
    }


    public void OnButtonA(){
        Debug.Log("Pressed A");
    }
}
