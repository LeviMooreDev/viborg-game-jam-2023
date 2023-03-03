using Sirenix.OdinInspector;
using UnityEngine;

public class CarVisual : MonoBehaviour
{
    public CarController car;

    public Transform tz;
    public Transform tx;


    [LabelText("velocity to angle")]
    public AnimationCurve zTiltVelocityToAngle;
    [LabelText("time")]
    public float zTiltTime = .05f;

    private float x;
    private float z;

    private void Update()
    {
        var target = -zTiltVelocityToAngle.Evaluate(Mathf.Abs(car.SideVelocity)) * (car.SideVelocity > 0 ? -1 : 1);
        z = Mathf.LerpAngle(z, target, Time.deltaTime / zTiltTime);

        float circumference = 2 * Mathf.PI * .5f;
        float rotationPerSecond = car.Velocity / circumference;
        x += rotationPerSecond;

        tx.transform.localEulerAngles = new Vector3(x, 0, 0);
        tz.transform.localEulerAngles = new Vector3(0, 0, z);
    }
}
