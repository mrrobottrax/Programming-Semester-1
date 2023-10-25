using UnityEngine;

public class PlayerAim : MonoBehaviour
{
	[SerializeField] Transform playerModel;
	[SerializeField] Transform cameraPivot;

	[SerializeField] float sensitivity = 1;

	[SerializeField] float minPitch = -89;
	[SerializeField] float maxPitch = 89;

	public float Yaw { get; private set; }
	public float Pitch { get; private set; }

	private void Start()
	{
		// update rotation on mouse move
		InputManager.Controls.DefaultMap.Look.performed += ctx => MouseDelta(ctx.ReadValue<Vector2>());

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void MouseDelta(Vector2 delta)
	{
		Yaw += delta.x * sensitivity;
		Pitch -= delta.y * sensitivity;

		Yaw %= 360;
		Pitch = Mathf.Clamp(Pitch, minPitch, maxPitch);
	}

	private void Update()
	{
		// update model rotation
		{
			Vector3 euler = playerModel.localEulerAngles;
			playerModel.localEulerAngles = new Vector3(euler.x, Yaw, euler.z);
		}

		// update camera rotation
		{
			Vector3 euler = cameraPivot.localEulerAngles;
			cameraPivot.localEulerAngles = new Vector3(Pitch, euler.y, euler.z);
		}
	}
}