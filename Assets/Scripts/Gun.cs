using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Gun : WeaponBase
{
	[Header("Gun Stats")]
	[SerializeField] int clipSize;
	[SerializeField] int reserveSize;
	[SerializeField] float spread;

	[HideInInspector]
	public TMP_Text ammoText;

	int clip = 0;
	int reserve = 0;

	protected override void Attack(Vector3 direction)
	{
		DecrementAmmo();

		FireBullet(GetSpreadDir(direction));

		UpdateAmmoDisplay();
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

	public override void OnSetActive()
	{
		base.OnSetActive();
		UpdateAmmoDisplay();
	}

	public abstract void FireBullet(Vector3 direction);

	public int GetClip()
	{
		return clip;
	}

	public int GetReserve()
	{
		return reserve;
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
		ammoText.text = $"{clip} / {reserve}";
	}

	protected override bool CanAttack()
	{
		return base.CanAttack() && clip > 0;
	}
}
