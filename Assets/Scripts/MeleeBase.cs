using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBase : WeaponBase
{
	[SerializeField] float range = 2;

	protected override void Attack(Vector3 direction)
	{
		Debug.DrawRay(playerAim.transform.position, direction * range, Color.red, 2, false);
	}
}
