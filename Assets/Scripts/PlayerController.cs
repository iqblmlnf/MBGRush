using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Wheel Joint References")]
    public WheelJoint2D backWheelJoint;
    public WheelJoint2D frontWheelJoint;

    [Header("Motor & Speed Settings")]
    [Tooltip("Kecepatan maksimal putaran roda (derajat/detik)")]
    public float maxMotorSpeed = 1200f;
    [Tooltip("Torsi maksimal motor roda (daya dorong)")]
    public float maxMotorTorque = 1500f;
    [Tooltip("Kecepatan akselerasi gas (derajat/detik^2)")]
    public float accelerationRate = 2500f;
    [Tooltip("Kecepatan perlambatan/pengereman (derajat/detik^2)")]
    public float decelerationRate = 3500f;

    [Header("Stability Settings")]
    [Tooltip("Sudut kemiringan maksimal sebelum mobil dianggap terbalik (derajat)")]
    public float maxTiltAngle = 80f;
    [Tooltip("Kekuatan torsi penyeimbang agar bodi mobil stabil tegak")]
    public float uprightStiffness = 6f;

    [Header("Audio")]
    public AudioSource engineAudio;

    private Rigidbody2D rb;
    private float currentMotorSpeed = 0f;
    private bool isFlipped;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Cek kemiringan bodi mobil terhadap vertikal
        float tiltAngle = Vector2.Angle(transform.up, Vector2.up);
        isFlipped = tiltAngle > maxTiltAngle;

        float input = Input.GetAxisRaw("Horizontal");

        // Kelola suara mesin dengan kehalusan pitch
        if (engineAudio != null)
        {
            if (!isFlipped && input != 0)
            {
                if (!engineAudio.isPlaying) engineAudio.Play();
                float targetPitch = Mathf.Lerp(0.8f, 1.4f, Mathf.Abs(currentMotorSpeed) / maxMotorSpeed);
                engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, targetPitch, Time.deltaTime * 5f);
            }
            else
            {
                engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, 0.8f, Time.deltaTime * 5f);
                if (engineAudio.isPlaying && Mathf.Abs(currentMotorSpeed) < 50f)
                    engineAudio.Stop();
            }
        }
    }

    void FixedUpdate()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (isFlipped)
        {
            currentMotorSpeed = 0f;
            DisableMotors();
            return;
        }

        // Hitung target kecepatan motor roda dengan halus (Ramping)
        float targetSpeed = 0f;
        if (input != 0)
        {
            targetSpeed = -input * maxMotorSpeed;
            currentMotorSpeed = Mathf.MoveTowards(currentMotorSpeed, targetSpeed, accelerationRate * Time.fixedDeltaTime);
        }
        else
        {
            currentMotorSpeed = Mathf.MoveTowards(currentMotorSpeed, 0f, decelerationRate * Time.fixedDeltaTime);
        }

        // Terapkan motor roda jika berputar
        if (Mathf.Abs(currentMotorSpeed) > 10f)
        {
            SetWheelMotor(backWheelJoint, currentMotorSpeed, maxMotorTorque);
            SetWheelMotor(frontWheelJoint, currentMotorSpeed, maxMotorTorque);
        }
        else
        {
            DisableMotors();
        }

        // Penyeimbang otomatis kemiringan bodi
        if (!isFlipped && uprightStiffness > 0)
        {
            float zRotation = transform.eulerAngles.z;
            if (zRotation > 180f) zRotation -= 360f;
            rb.AddTorque(-zRotation * uprightStiffness);
        }
    }

    private void SetWheelMotor(WheelJoint2D joint, float speed, float torque)
    {
        if (joint == null) return;
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = speed;
        motor.maxMotorTorque = torque;
        joint.motor = motor;
    }

    private void DisableMotors()
    {
        if (backWheelJoint != null) backWheelJoint.useMotor = false;
        if (frontWheelJoint != null) frontWheelJoint.useMotor = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isFlipped ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, transform.up * 1.5f);
    }
}