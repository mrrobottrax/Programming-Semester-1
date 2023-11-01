using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerAim))]
public class PlayerShoot : MonoBehaviour
{
	[Header("References")]
	[SerializeField] TMP_Text ammoText;
	[SerializeField] Gun[] inventory = new Gun[3];

	Gun activeGun;

	private void Awake()
	{
		PlayerAim playerAim = GetComponent<PlayerAim>();

		for (int i = 0; i < inventory.Length; i++)
		{
			inventory[i].playerAim = playerAim;
			inventory[i].ammoText = ammoText;
		}

		SetGunActive(0);
	}

	void SetGunActive(int index)
	{
		activeGun = inventory[index];
		activeGun.OnSetActive();
	}

	private void Start()
	{
		InputManager.Controls.DefaultMap.Fire1.performed += ctx =>
		{
			bool isPressed = ctx.ReadValue<float>() > 0;
			if (isPressed)
				StartFire();
			else
				StopFire();
		};
		InputManager.Controls.DefaultMap.Reload.performed += ctx => Reload();

		InputManager.Controls.DefaultMap.Inventory0.performed += ctx => SetGunActive(0);
		InputManager.Controls.DefaultMap.Inventory1.performed += ctx => SetGunActive(1);
		InputManager.Controls.DefaultMap.Inventory2.performed += ctx => SetGunActive(2);
	}

	void StartFire()
	{
		activeGun.StartAttack();
	}

	void StopFire()
	{
		activeGun.StopAttack();
	}

	void Reload()
	{
		activeGun.Reload();
	}

	public void AddAmmo(int amount)
	{
		activeGun.AddAmmo(amount);
	}

	public bool IsFull()
	{
		return activeGun.IsFull();
	}
}
