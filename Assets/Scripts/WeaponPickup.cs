using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
	[SerializeField] WeaponBase weapon;
	[SerializeField] float respawnTime;
	[SerializeField] int ammo;
	GameObject mesh;

	bool active = true;

	private void Start()
	{
		mesh = Instantiate(weapon.modelPrefab, transform);
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (!active) return;

		if (collision.gameObject.CompareTag("Player"))
		{
			if (collision.gameObject.TryGetComponent(out PlayerShoot shoot))
			{
				shoot.AddWeapon(weapon);
				shoot.AddAmmo(ammo);
				StartCoroutine(IRespawnTimer());
			}
		}
	}

	IEnumerator IRespawnTimer()
	{
		active = false;
		mesh.SetActive(false);

		yield return new WaitForSeconds(respawnTime);

		active = true;
		mesh.SetActive(true);
	}
}
