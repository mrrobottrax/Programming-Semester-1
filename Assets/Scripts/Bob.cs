using UnityEngine;

public class Bob : MonoBehaviour
{
	[SerializeField]
	float rotationSpeed = 40;
	[SerializeField]
	float bobSpeed = 2;
	[SerializeField]
	float bobAmt = 0.1f;

	float startY;

	private void Awake()
	{
		startY = transform.position.y;
	}

	private void Update()
	{
		transform.eulerAngles = transform.rotation.eulerAngles + rotationSpeed * Time.deltaTime * Vector3.up;
		transform.position = new Vector3(
			transform.position.x,
			startY + Mathf.Sin(Time.time * bobSpeed) * bobAmt,
			transform.position.z
		);
	}
}
