using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerAim))]
public class PlayerShoot : MonoBehaviour
{
	[SerializeField] float bulletSpeed = 15;
	[SerializeField] Transform bulletSpawn;
	[SerializeField] GameObject bulletPrefab;

	PlayerAim playerAim;

	private void Awake()
	{
		playerAim = GetComponent<PlayerAim>();
	}

	private void Start()
	{
		InputManager.Controls.DefaultMap.Fire1.performed += ctx => Fire();
	}

	public void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab);
		bullet.transform.position = bulletSpawn.position;
		bullet.GetComponent<Rigidbody>().velocity = playerAim.Rotate3D(new Vector3(0, 0, bulletSpeed));
	}
}
