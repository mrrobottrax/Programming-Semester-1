using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] float timeOut = 10;
	[SerializeField] bool destroyOnImpact;

	private void Start()
	{
		Destroy(gameObject, timeOut);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (destroyOnImpact)
		{
			Destroy(gameObject);
		}
	}
}
