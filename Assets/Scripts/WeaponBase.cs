using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
	[Header("Weapon Base Stats")]
	[SerializeField] float cooldown;
	[SerializeField] bool isFullAuto;

	[HideInInspector]
	public PlayerAim playerAim;

	WaitForSeconds cooldownTimer;
	WaitUntil waitForCooldown;
	bool onCooldown = false;

	Coroutine attackLoop;

	protected virtual void Awake()
	{
		cooldownTimer = new WaitForSeconds(cooldown);
		waitForCooldown = new WaitUntil(() => !onCooldown);
	}

	public virtual void OnSetActive()
	{

	}

	public virtual void StartAttack()
	{
		attackLoop = StartCoroutine(AttackLoop());
	}

	public virtual void StopAttack()
	{
		StopCoroutine(attackLoop);
	}

	IEnumerator AttackLoop()
	{
		yield return waitForCooldown;

		TryAttack();

		if (isFullAuto)
			attackLoop = StartCoroutine(AttackLoop());
	}

	IEnumerator AttackCooldown()
	{
		onCooldown = true;
		yield return cooldownTimer;
		onCooldown = false;
	}

	protected virtual void TryAttack()
	{
		if (!CanAttack())
			return;

		Attack(playerAim.Rotate3D(Vector3.forward));
		StartCoroutine(AttackCooldown());
	}

	protected virtual bool CanAttack()
	{
		return true;
	}

	protected abstract void Attack(Vector3 direction);
}
