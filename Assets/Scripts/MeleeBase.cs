using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBase : WeaponBase
{
	[SerializeField] float range = 2;
	[SerializeField] LayerMask layerMask;

	protected override void Attack(Vector3 direction)
	{
		Debug.DrawRay(playerAim.transform.position, direction * range, Color.red, 2, false);
		if (Physics.Raycast(playerAim.transform.position, direction, out RaycastHit hit, range, layerMask))
		{
			if (hit.collider.gameObject.TryGetComponent(out Health health))
			{
				health.Damage(baseDamage);
				
				if (hit.rigidbody)
				{
					hit.rigidbody.AddForce(direction * knockback, ForceMode.VelocityChange);
				}
			}
		}
	}
}
