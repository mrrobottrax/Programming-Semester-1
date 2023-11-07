using UnityEngine;

public class PlayerAim : MonoBehaviour
{
	[SerializeField] Transform playerModel;
	[SerializeField] Transform cameraPivot;

	[SerializeField] float sensitivity = 1;

	[SerializeField] float minPitch = -89;
	[SerializeField] float maxPitch = 89;

	public Transform projectileSpawn;

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

	public Vector2 Rotate2D(Vector2 dir)
	{
		Vector2 rot = dir;

		rot.x = dir.x * Mathf.Cos(Mathf.Deg2Rad * Yaw) + dir.y * Mathf.Sin(Mathf.Deg2Rad * Yaw);
		rot.y = dir.x * Mathf.Sin(Mathf.Deg2Rad * -Yaw) + dir.y * Mathf.Cos(Mathf.Deg2Rad * -Yaw);

		return rot;
	}

	public Vector3 RotateYaw(Vector3 dir)
	{
		Vector3 rot = dir;

		rot.x = dir.x * Mathf.Cos(Mathf.Deg2Rad * Yaw) + dir.z * Mathf.Sin(Mathf.Deg2Rad * Yaw);
		rot.z = dir.x * Mathf.Sin(Mathf.Deg2Rad * -Yaw) + dir.z * Mathf.Cos(Mathf.Deg2Rad * -Yaw);

		return rot;
	}

	public Vector3 RotatePitch(Vector3 dir)
	{
		Vector3 rot = dir;

		rot.y = dir.z * Mathf.Sin(Mathf.Deg2Rad * -Pitch) + dir.y * Mathf.Cos(Mathf.Deg2Rad * -Pitch);
		rot.z = dir.z * Mathf.Cos(Mathf.Deg2Rad * Pitch) + dir.y * Mathf.Sin(Mathf.Deg2Rad * Pitch);

		return rot;
	}

	public Vector3 Rotate3D(Vector3 dir)
	{
		return RotateYaw(RotatePitch(dir));
	}
}