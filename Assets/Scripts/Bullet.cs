using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] float timeOut = 10;
	[SerializeField] bool destroyOnImpact;
	public WeaponBase weapon;
	public Vector3 direction;

	private void Start()
	{
		Destroy(gameObject, timeOut);
	}

	private void OnCollisionEnter(Collision collision)
	{
		// check if we can deal damage
		if (collision.gameObject.TryGetComponent(out Health health))
		{
			health.Damage(weapon.GetBaseDamage());
			if (collision.rigidbody)
			{
				collision.rigidbody.AddForce(direction * weapon.GetKnockback(), ForceMode.VelocityChange);
			}
		}

		if (destroyOnImpact)
		{
			Destroy(gameObject);
		}
	}
}
