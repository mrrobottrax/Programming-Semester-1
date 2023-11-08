using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

[Serializable]
public struct InventoryEntry
{
	public WeaponBase weapon;
	public GameObject model;
}

[RequireComponent(typeof(PlayerAim))]
public class PlayerShoot : MonoBehaviour
{
	[Header("References")]
	[SerializeField] TMP_Text ammoText;

	[SerializeField] List<InventoryEntry> inventory = new();
	[SerializeField] Transform gunPivot;

	int activeWeaponIndex = -1;
	PlayerAim playerAim;

	private void Awake()
	{
		playerAim = GetComponent<PlayerAim>();

		for (int i = 0; i < inventory.Count; i++)
		{
			inventory[i].weapon.playerAim = playerAim;
			inventory[i].weapon.ammoText = ammoText;

			if (inventory[i].model == null && inventory[i].weapon.modelPrefab != null)
			{
				InventoryEntry entry = inventory[i];
				entry.model = Instantiate(inventory[i].weapon.modelPrefab, gunPivot);
				inventory[i] = entry;
			}
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

		if (activeWeaponIndex >= 0 && inventory[activeWeaponIndex].model)
			inventory[activeWeaponIndex].model.SetActive(false);
		if (inventory[index].model)
			inventory[index].model.SetActive(true);

		activeWeaponIndex = index;
		GetActiveWeapon().OnSetActive();
	}

	WeaponBase GetActiveWeapon()
	{
		return inventory[activeWeaponIndex].weapon;
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
		int find = inventory.FindIndex((InventoryEntry entry) => { return entry.weapon == weapon; });
		if (find >= 0)
		{
			SetGunActive(find);
			return;
		}

		InventoryEntry entry = new InventoryEntry
		{
			weapon = weapon,
			model = Instantiate(weapon.modelPrefab, gunPivot)
		};

		inventory.Add(entry);

		int index = inventory.Count - 1;
		inventory[index].weapon.playerAim = playerAim;
		inventory[index].weapon.ammoText = ammoText;
		SetGunActive(index);
	}
}
