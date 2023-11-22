using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Gun
{
	[Header("Projectile Stats")]
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] float velocity;

	public override void FireBullet(Vector3 direction)
	{
		GameObject bullet = Instantiate(projectilePrefab);
		bullet.transform.SetPositionAndRotation(playerAim.projectileSpawn.position, Quaternion.LookRotation(direction));
		bullet.GetComponent<Rigidbody>().velocity = direction * velocity;

		Bullet bulletScript = bullet.GetComponent<Bullet>();
		bulletScript.weapon = this;
		bulletScript.direction = direction;
	}
}
