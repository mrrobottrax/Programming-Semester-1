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

    private void Awake()
    {
        actionAsset.Enable();
        actionMap = actionAsset.FindActionMap("Default Map");
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");

        jumpAction.performed += ctx => Jump();

        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
	{
        Vector2 moveDir = moveAction.ReadValue<Vector2>();
        rb.AddForce(new Vector3(moveDir.x, 0, moveDir.y), ForceMode.VelocityChange);
	}

    void Jump()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
    }
}
