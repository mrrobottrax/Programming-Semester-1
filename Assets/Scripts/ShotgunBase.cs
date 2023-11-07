using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBase : ProjectileGun
{
	[Header("Shotgun Stats")]
	[SerializeField] int bulletsPerShot;

	protected override void Attack(Vector3 direction)
	{
		for (int i = 0; i < bulletsPerShot; i++)
			FireBullet(GetSpreadDir(direction));
	}
}
