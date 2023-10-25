using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
	[SerializeField] int amount = 10;
	[SerializeField] float respawnTime = 16;
	[SerializeField] GameObject mesh;

	bool active = true;

	private void OnTriggerEnter(Collider collision)
	{
		if (!active) return;

		if (collision.gameObject.CompareTag("Player"))
		{
			if (collision.gameObject.TryGetComponent(out PlayerShoot shoot))
			{
				if (!shoot.IsFull())
				{
					shoot.AddAmmo(amount);
					StartCoroutine(IRespawnTimer());
				}
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
