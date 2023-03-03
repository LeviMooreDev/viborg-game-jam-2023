using UnityEngine;

public class CarInput : MonoBehaviour
{
    public InputManager inputManager;
    public CarController carController;

    // Update is called once per frame
    void Update()
    {
        carController.forwardInput = inputManager.Forward;
        carController.reverseInput = inputManager.Reverse;
        carController.breakInput = inputManager.Break;
        carController.turnInput = inputManager.Turn;
    }
}
