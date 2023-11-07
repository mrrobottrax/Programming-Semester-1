using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Gun : WeaponBase
{
	[Header("Gun Stats")]
	[SerializeField] float spread;

	protected override void Attack(Vector3 direction)
	{
		FireBullet(GetSpreadDir(direction));
	}

	protected Vector3 GetSpreadDir(Vector3 direction)
	{
		Vector3 randomDir = direction + new Vector3(
			Random.Range(-spread, spread),
			Random.Range(-spread, spread),
			Random.Range(-spread, spread)
			);
		randomDir.Normalize();
		return randomDir;
	}

	public abstract void FireBullet(Vector3 direction);
}
