using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerAim), typeof(CapsuleCollider))]
public class PlayerMove : MonoBehaviour
{
	Rigidbody rb;
	PlayerAim playerAim;
	CapsuleCollider capsule;

	const float jumpForce = 4;
	const float moveSpeed = 5;
	const float acceleration = 50;

	[SerializeField] LayerMask groundLayers = ~0;
	bool isGrounded = false;

	private void Awake()
	{
		InputManager.Awake();

		rb = GetComponent<Rigidbody>();
		playerAim = GetComponent<PlayerAim>();
		capsule = GetComponent<CapsuleCollider>();
	}

	private void Start()
	{
		InputManager.Controls.DefaultMap.Jump.performed += ctx => Jump();
	}

	private void OnDestroy()
	{
		InputManager.Destroy();
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
		const float groundCheckEpsilon = 0.01f;

		Vector3 bottom = transform.position + transform.rotation * capsule.center;
		bottom -= transform.up * (capsule.height / 2.0f - capsule.radius + groundCheckEpsilon);

		isGrounded = Physics.CheckSphere(bottom, capsule.radius, groundLayers, QueryTriggerInteraction.Ignore);
	}

	void Move()
	{
		Vector2 moveDir = RotateMoveDir(InputManager.Controls.DefaultMap.Move.ReadValue<Vector2>());
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

	Vector2 RotateMoveDir(Vector2 dir)
	{
		Vector2 rot = dir;

		rot.x = dir.x * Mathf.Cos(Mathf.Deg2Rad * playerAim.Yaw) + dir.y * Mathf.Sin(Mathf.Deg2Rad * playerAim.Yaw);
		rot.y = dir.x * Mathf.Sin(Mathf.Deg2Rad * -playerAim.Yaw) + dir.y * Mathf.Cos(Mathf.Deg2Rad * -playerAim.Yaw);

		return rot;
	}

	void Jump()
	{
		if (isGrounded)
		{
			rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
		}
	}
}
