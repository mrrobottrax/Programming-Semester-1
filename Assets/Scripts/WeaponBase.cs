using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
	[Header("Viewmodel")]
	public GameObject modelPrefab;

	[Header("Weapon Base Stats")]
	[SerializeField] float cooldown;
	[SerializeField] bool isFullAuto;

	[Header("Ammo Stats")]
	[SerializeField] bool usesAmmo;
	[SerializeField] int clipSize;
	[SerializeField] int reserveSize;

	int clip = 0;
	int reserve = 0;

	[HideInInspector]
	public PlayerAim playerAim;
	[HideInInspector]
	public TMP_Text ammoText;

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
		UpdateAmmoDisplay();
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

	protected virtual void DecrementAmmo()
	{
		if (clip <= 0)
			return;

		--clip;
		if (clip < 0)
			clip = 0;

		UpdateAmmoDisplay();
	}

	protected virtual void TryAttack()
	{
		if (!CanAttack())
			return;

		if (usesAmmo)
			DecrementAmmo();

		Attack(playerAim.Rotate3D(Vector3.forward));
		StartCoroutine(AttackCooldown());
	}

	protected abstract void Attack(Vector3 direction);

	public int GetClip()
	{
		return clip;
	}

	public int GetReserve()
	{
		return reserve;
	}

	public bool IsFull()
	{
		return reserve >= reserveSize;
	}

	public virtual void Reload()
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

		UpdateAmmoDisplay();
	}

	public virtual void AddAmmo(int amount)
	{
		reserve += amount;
		if (reserve > reserveSize)
			reserve = reserveSize;

		UpdateAmmoDisplay();
	}

	protected void UpdateAmmoDisplay()
	{
		if (usesAmmo)
		{
			ammoText.enabled = true;

			ammoText.text = $"{clip} / {reserve}";
		}
		else
		{
			ammoText.enabled = false;
		}
	}

	protected virtual bool CanAttack()
	{
		return usesAmmo && clip > 0;
	}
}
