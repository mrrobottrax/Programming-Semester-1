using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private void Awake()
	{
		InputManager.Awake();
	}

	private void OnDestroy()
	{
		InputManager.Destroy();
	}
}
