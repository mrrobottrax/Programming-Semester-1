using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerAim))]
public class PlayerShoot : MonoBehaviour
{
	[Header("References")]
	[SerializeField] TMP_Text ammoText;
	[SerializeField] List<WeaponBase> inventory = new();
	[SerializeField] List<GameObject> weaponModels = new();
	[SerializeField] Transform gunPivot;

	int activeWeaponIndex = -1;
	PlayerAim playerAim;

	private void Awake()
	{
		playerAim = GetComponent<PlayerAim>();

		for (int i = 0; i < inventory.Count; i++)
		{
			inventory[i].playerAim = playerAim;
			inventory[i].ammoText = ammoText;
		}

		SetGunActive(0);
	}

	void SetGunActive(int index)
	{
		if (index >= inventory.Count)
		{
			ammoText.enabled = false;
			return;
		}

		if (activeWeaponIndex >= 0)
			weaponModels[activeWeaponIndex].SetActive(false);
		weaponModels[index].SetActive(true);

		activeWeaponIndex = index;
		GetActiveWeapon().OnSetActive();
	}

	WeaponBase GetActiveWeapon()
	{
		return inventory[activeWeaponIndex];
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
		if (activeWeaponIndex < 0)
			return;

		GetActiveWeapon().StartAttack();
	}

	void StopFire()
	{
		if (activeWeaponIndex < 0)
			return;

		GetActiveWeapon().StopAttack();
	}

	void Reload()
	{
		if (activeWeaponIndex < 0)
			return;

		GetActiveWeapon().Reload();
	}

	public void AddAmmo(int amount)
	{
		if (activeWeaponIndex < 0)
			return;

		GetActiveWeapon().AddAmmo(amount);
	}

	public bool IsFull()
	{
		if (activeWeaponIndex < 0)
			return true;

		return GetActiveWeapon().IsFull();
	}

	public void AddWeapon(WeaponBase weapon)
	{
		int find = inventory.IndexOf(weapon);
		if (find >= 0)
		{
			SetGunActive(find);
			return;
		}

		inventory.Add(weapon);
		weaponModels.Add(Instantiate(weapon.modelPrefab, gunPivot));

		int index = inventory.Count - 1;
		inventory[index].playerAim = playerAim;
		inventory[index].ammoText = ammoText;
		SetGunActive(index);
	}
}
