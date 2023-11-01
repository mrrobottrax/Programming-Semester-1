using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGunBase : ProjectileGun
{
    [Header("Burst Stats")]
    [SerializeField] int shotsPerBurst;
    [SerializeField] float timeBetweenShots;

	WaitForSeconds waitForNextShot;

	protected override void Awake()
	{
		base.Awake();
		waitForNextShot = new WaitForSeconds(timeBetweenShots);
	}

	protected override void Attack(Vector3 direction)
	{
		StartCoroutine(BurstLoop(0, direction));
	}

	IEnumerator BurstLoop(int bulletNumber, Vector3 dir)
	{
		base.Attack(dir);

		yield return waitForNextShot;

		int newBulletNumber = bulletNumber + 1;
		if (newBulletNumber < shotsPerBurst)
			StartCoroutine(BurstLoop(newBulletNumber, dir));
	}
}
