using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    public CarController carController;

    // Update is called once per frame
    void Update()
    {

    }

    public void Forward(float value){
        carController.forwardInput = value;
    }

    public void Backwards(float value){
        carController.reverseInput = value;
    }

    public void Turn(Vector2 value){
        carController.turnInput = value.x;
    }

    public void Break(float value){
        carController.breakInput = value;
    }

    
}
