using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] CharacterMove playerMove;
	[SerializeField] PlayerAim playerAim;

	private void Awake()
	{
		InputManager.Awake();
	}

	private void Start()
	{
		InputManager.Controls.DefaultMap.Jump.performed += ctx => playerMove.Jump();
	}

	private void OnDestroy()
	{
		InputManager.Destroy();
	}

	private void Update()
	{
		playerMove.moveVector = playerAim.Rotate2D(InputManager.Controls.DefaultMap.Move.ReadValue<Vector2>());
	}
}
