using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;
    private Vector3 movement;
    private Animator animator;

    private bool lockRotation = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        movement = new Vector3(h, 0f, v).normalized;

        if (movement.magnitude > 0f)
        {
            lockRotation = false;
        }

        if (animator != null)
        {
            bool isMoving = movement.magnitude > 0;
            animator.SetBool("IsMoving", isMoving);
        }
    }

    void FixedUpdate()
    {
        Vector3 velocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);
        rb.linearVelocity = velocity;

        if (!lockRotation && movement != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(movement.x, 0f, movement.z));
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rot, 10f * Time.fixedDeltaTime));
        }
    }

    public void LockRotationToShootDirection(Vector3 shootDirection)
    {
        if (shootDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(shootDirection);
            rb.MoveRotation(rot);
            lockRotation = true;
        }
    }

    public bool IsMoving()
    {
        return movement.magnitude > 0f;
    }
}
