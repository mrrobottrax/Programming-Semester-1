using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeOut = 10;

	private void Start()
	{
		Destroy(gameObject, timeOut);
	}
}
