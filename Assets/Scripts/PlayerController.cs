using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 25f;

    [Header("Jump")]
    public float jumpForce = 15f;
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Audio")]
    public AudioSource engineAudio;

    private Rigidbody2D rb;
    private float currentSpeed;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Cek apakah menyentuh tanah
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Grounded = " + isGrounded);
        }

        float input = Input.GetAxisRaw("Horizontal");

        // Gerakan
        if (input != 0)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                input * maxSpeed,
                acceleration * Time.deltaTime);

            if (!engineAudio.isPlaying)
                engineAudio.Play();
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0,
                deceleration * Time.deltaTime);

            if (engineAudio.isPlaying)
                engineAudio.Stop();
        }

        // Loncat
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        if (engineAudio != null)
        {
            engineAudio.pitch = Mathf.Lerp(
                0.8f,
                1.3f,
                Mathf.Abs(currentSpeed) / maxSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}