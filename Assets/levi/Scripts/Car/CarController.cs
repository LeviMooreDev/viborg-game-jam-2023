using UnityEngine;
using UnityEngine.UIElements;

public class CarController : MonoBehaviour
{
    //input
    [HideInInspector]
    public float forwardInput;
    [HideInInspector]
    public float reverseInput;
    [HideInInspector]
    public float turnInput;
    [HideInInspector]
    public float breakInput;

    public bool debugVelocity;

    [Header("Moter")]
    public AnimationCurve acceleration;
    public float deceleration = 50;
    public float dragDeceleration = 20;
    public float breakingDeceleration = 150;
    public float breakingDecelerationTurnMultiplier = .5f;
    public bool counterGravity;

    [Header("Wheel")]
    public AnimationCurve turnCurve;
    public float AvailableTurnAngle { get; private set; }
    public float TurnAngle { get; private set; }

    [Header("Drag")]
    public float groundDrag = 4;
    public float airDrag = 1;

    [Header("Limits")]
    public float maxForwardVelocity = 60f;
    public float minForwardVelocity = 0f;
    public float maxReverseVelocity = 20f;
    public float minReverseVelocity = 0f;
    public float maxForwardVelocityTargetAhead = 10;

    [Header("Others")]
    public Rigidbody motor;
    public new Rigidbody collider;
    public LayerMask groundLayer;
    public float groundCheckLength = 2f;
    public float alignToGroundTime = 20;

    //gear
    public int Gear { get; private set; }

    //forces
    public Vector3 LocalVelocity { get; private set; }
    public float SideVelocity => LocalVelocity.x < -.01f || LocalVelocity.x > .01f ? LocalVelocity.x : 0;
    public float SideVelocityAbs => Mathf.Abs(SideVelocity);
    public float Velocity => LocalVelocity.z < -.01f || LocalVelocity.z > .01f ? LocalVelocity.z : 0;
    public float VelocityAbs => Mathf.Abs(Velocity);
    public float TargetVelocity { get; private set; }
    public float Force { get; private set; }
    public float AvailableAcceleration { get; private set; }
    //ground
    private bool isCarGrounded;
    private RaycastHit groundHit;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        motor.name = $"{name}: {motor.name}";
        motor.transform.parent = null;
        if (collider != null)
        {
            collider.name = $"{name}: {collider.name}";
            collider.transform.parent = null;
        }
    }

    private void Update()
    {
        UpdateValues();
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);
        MotorUpdate();
        WheelUpdate();
        VisualUpdate();

        if (debugVelocity)
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * .5f, Color.green);
            Debug.DrawLine(transform.position, transform.position + motor.velocity.normalized, Color.red);
        }
    }
    private void UpdateValues()
    {
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out groundHit, groundCheckLength, groundLayer);
        motor.drag = isCarGrounded ? groundDrag : airDrag;
        if (collider != null)
        {
            collider.freezeRotation = isCarGrounded;
        }
        LocalVelocity = transform.InverseTransformDirection(motor.velocity);
        AvailableAcceleration = acceleration.Evaluate(Mathf.Abs(TargetVelocity));
    }

    public float rampForce = 25;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ramp")
        {
            motor.velocity = motor.transform.forward * rampForce;
        }
    }

    private void MotorUpdate()
    {
        //break
        if (isCarGrounded)
        {
            if (breakInput > 0)
            {
                MotorBreak(breakInput);
                TryGearChange(0, 0);
            }

            //forward
            else if (forwardInput > 0)
            {
                if (Gear == 1)
                {
                    MotorAccelerate(forwardInput, maxForwardVelocity, minForwardVelocity);
                }
                else
                {
                    MotorDecelerate(forwardInput);
                    TryGearChange(1, 0);
                }
            }
            //reverse
            else if (reverseInput > 0)
            {
                if (Gear == -1)
                {
                    MotorAccelerate(reverseInput, maxReverseVelocity, minReverseVelocity);
                }
                else
                {
                    MotorDecelerate(reverseInput);
                    TryGearChange(-1, 0);
                }
            }
            //deceleration
            else
            {
                MotorDrag();
            }
        }
    }
    private void MotorAccelerate(float amount = 1, float? maxVelocity = null, float? minVelocity = null)
    {
        amount = Mathf.Clamp01(amount);
        TargetVelocity += Time.deltaTime * AvailableAcceleration * amount;
        if (maxVelocity != null)
        {
            TargetVelocity = Mathf.Min(maxVelocity ?? 0, TargetVelocity);
        }
        if (minVelocity != null)
        {
            TargetVelocity = Mathf.Max(minVelocity ?? 0, TargetVelocity);
        }
    }
    private void MotorDecelerate(float amount = 1)
    {
        amount = Mathf.Clamp01(amount);
        TargetVelocity -= Time.deltaTime * deceleration * amount;
        TargetVelocity = Mathf.Max(0, TargetVelocity);
    }
    private void MotorBreak(float amount = 1)
    {
        amount = Mathf.Clamp01(amount);
        var decelerationAmount = breakingDeceleration;
        if (SideVelocity != 0)
        {
            decelerationAmount *= breakingDecelerationTurnMultiplier;
            //multiply turn amount
        }
        TargetVelocity -= Time.deltaTime * decelerationAmount * amount;
        TargetVelocity = Mathf.Max(0, TargetVelocity);
    }
    private void MotorDrag()
    {
        if (Velocity < 5)
        {
            TargetVelocity = 0;
        }
        TargetVelocity -= Time.deltaTime * dragDeceleration;
        TargetVelocity = Mathf.Max(0, TargetVelocity);
    }

    private void SetGear(int newGear)
    {
        Gear = newGear;
    }
    private void TryGearChange(int newGear, float? maxVelocity)
    {
        if (maxVelocity != null && VelocityAbs > maxVelocity)
        {
            return;
        }

        SetGear(newGear);
    }

    private void WheelUpdate()
    {
        AvailableTurnAngle = (int)turnCurve.Evaluate(Mathf.Abs(Velocity));
        if (VelocityAbs < 1) AvailableTurnAngle = 0;

        TurnAngle = turnInput * AvailableTurnAngle;
        if (Gear < 0) TurnAngle *= -1;

        transform.Rotate(0, Time.deltaTime * TurnAngle, 0, Space.World);
    }
    private void VisualUpdate()
    {
        transform.position = motor.transform.position;
    }
    private void FixedUpdate()
    {
        if (isCarGrounded)
        {
            Vector3 speed = motor.transform.forward * TargetVelocity;
            if (Gear < 0) speed *= -1;

            Vector3 force = speed.normalized * motor.mass * TargetVelocity;
            force = motor.drag * force / (1f - 0.02f * motor.drag);
            Force = force.magnitude;

            motor.AddForce(force);
            if (counterGravity)
            {
                motor.AddForce(-Physics.gravity, ForceMode.Acceleration);
            }
        }
        else
        {
            motor.AddForce(transform.up * -200f);
        }

        if (collider != null)
        {
            collider.MoveRotation(transform.rotation);
        }
    }

    public void DestroyMotor()
    {
        Destroy(motor.gameObject);
    }
}