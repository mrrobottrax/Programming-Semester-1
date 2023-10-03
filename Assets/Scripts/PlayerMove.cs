using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    InputActionAsset actionAsset;

    InputActionMap actionMap;

    InputAction moveAction;
    InputAction jumpAction;

    Rigidbody rb;

    const float jumpForce = 4;
    const float moveSpeed = 5;
    const float acceleration = 50;

    [SerializeField] LayerMask groundLayers = ~0;
    float groundCheckDist;
    bool isGrounded = false;

    private void Awake()
    {
        actionAsset.Enable();
        actionMap = actionAsset.FindActionMap("Default Map");
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");

        jumpAction.performed += ctx => Jump();

        rb = gameObject.GetComponent<Rigidbody>();

        groundCheckDist = (GetComponent<Collider>().bounds.size.y / 2.0f) + 0.01f;
    }

    void FixedUpdate()
	{
        CheckGrounded();

        if (isGrounded)
        {
            Move();
        }
	}

    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDist, groundLayers);
    }

    void Move()
    {
        Vector2 moveDir = moveAction.ReadValue<Vector2>();
        rb.AddForce(acceleration * Time.fixedDeltaTime * new Vector3(moveDir.x, 0, moveDir.y), ForceMode.VelocityChange);

        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        if (speed == 0)
        {
            return;
        }

        if (speed > moveSpeed)
        {
            rb.velocity *= moveSpeed / speed;
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
        }
    }
}
