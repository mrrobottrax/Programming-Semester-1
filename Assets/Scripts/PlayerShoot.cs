using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(PlayerAim))]
public class PlayerShoot : MonoBehaviour
{
	[Header("Stats")]
	[SerializeField] float bulletSpeed = 15;

	[SerializeField] int clipSize = 5;
	[SerializeField] int maxReserve = 60;

	[SerializeField] int clip = 0;
	[SerializeField] int reserve = 0;

	[Header("References")]
	[SerializeField] Transform bulletSpawn;
	[SerializeField] GameObject bulletPrefab;
	[SerializeField] TMP_Text ammoText;

	PlayerAim playerAim;

	private void Awake()
	{
		playerAim = GetComponent<PlayerAim>();
	}

	private void Start()
	{
		InputManager.Controls.DefaultMap.Fire1.performed += ctx => Fire();
		InputManager.Controls.DefaultMap.Reload.performed += ctx => Reload();

		UpdateAmmo();
	}

	void Fire()
	{
		if (clip <= 0)
			return;

		GameObject bullet = Instantiate(bulletPrefab);
		bullet.transform.position = bulletSpawn.position;
		bullet.GetComponent<Rigidbody>().velocity = playerAim.Rotate3D(new Vector3(0, 0, bulletSpeed));

		--clip;
		if (clip < 0)
			clip = 0;

		UpdateAmmo();
	}

	void Reload()
	{
		if (reserve <= 0)
			return;

		int needed = clipSize - clip;

		reserve -= needed;

		clip = clipSize;

		// don't add bullets we don't have
		if (reserve < 0)
		{
			clip += reserve;
			reserve = 0;
		}

		UpdateAmmo();
	}

	public void AddAmmo(int amount)
	{
		reserve += amount;
		if (reserve > maxReserve)
			reserve = maxReserve;

		UpdateAmmo();
	}

	void UpdateAmmo()
	{
		ammoText.text = $"{clip} / {reserve}";
	}

	public bool IsFull()
	{
		return reserve >= maxReserve;
	}
}
