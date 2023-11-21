using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerAim), typeof(CapsuleCollider))]
public class PlayerMove : MonoBehaviour
{
	Rigidbody rb;
	CapsuleCollider capsule;

	const float jumpForce = 4;
	const float moveSpeed = 5;
	const float acceleration = 50;

	[SerializeField] LayerMask groundLayers = ~0;
	bool isGrounded = false;

	public Vector2 moveVector = Vector2.zero;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		capsule = GetComponent<CapsuleCollider>();
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
		rb.AddForce(acceleration * Time.fixedDeltaTime * new Vector3(moveVector.x, 0, moveVector.y), ForceMode.VelocityChange);

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

	public void Jump()
	{
		if (isGrounded)
		{
			rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
		}
	}
}
