using System;
using UnityEngine;

[RequireComponent(typeof(CharacterMove))]
public class Enemy : MonoBehaviour
{
	[SerializeField] Transform player;
	[SerializeField] LayerMask visibilityMask;
	CharacterMove characterMove;

	[Flags]
	enum AiFlags
	{
		None = 0,
		Scared = 1 << 0,
		AwareOfPlayer = 1 << 1,
		Tough = 1 << 2
	}
	AiFlags flags;

	enum AiState
	{
		Wandering,
		Attacking,
		Retreating
	}
	AiState state;

	private void Awake()
	{
		characterMove = GetComponent<CharacterMove>();
	}

	private void FixedUpdate()
	{
		SetFlags();

		if (flags.HasFlag(AiFlags.AwareOfPlayer))
		{
			Vector3 dir3 = player.position - transform.position;
			Vector2 dir = new(dir3.x, dir3.z);

			characterMove.moveVector = dir.normalized;
		}
		else
		{
			characterMove.moveVector = Vector2.zero;
		}
	}

	void SetFlags()
	{
		flags = AiFlags.None;

		// Raycast to player to see if we can see them
		Vector3 dir = player.position - transform.position;
		flags |= Physics.Raycast(transform.position, dir, dir.magnitude, visibilityMask, QueryTriggerInteraction.Ignore) ? AiFlags.None : AiFlags.AwareOfPlayer;
	}
}
