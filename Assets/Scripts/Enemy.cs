using System;
using UnityEngine;

[RequireComponent(typeof(CharacterMove), typeof(Health))]
public class Enemy : MonoBehaviour
{
	[SerializeField] Transform player;
	[SerializeField] LayerMask visibilityMask;
	[SerializeField] MeshRenderer meshRenderer;
	[SerializeField] float runThreshold = 25;
	CharacterMove characterMove;
	Health health;

	[Flags]
	enum AiFlags
	{
		None = 0,
		Coward = 1 << 0,
		Brave = 1 << 1
	}
	[SerializeField] AiFlags flags;

	enum AiState
	{
		Wandering,
		Attacking,
		Retreating,
		Dead
	}
	AiState state;

	bool canSeePlayer = false;

	Vector2 wanderDir = Vector2.zero;
	float wanderTimer = 0;
	const float wanderTime = 2;

	private void Awake()
	{
		characterMove = GetComponent<CharacterMove>();
		health = GetComponent<Health>();
	}

	private void FixedUpdate()
	{
		GetData();
		GetState();
		PickMoveDir();
		SetColor();
	}

	void PickMoveDir()
	{
		switch (state)
		{
			case AiState.Wandering:
				{
					wanderTimer -= Time.fixedDeltaTime;
					if (wanderTimer <= 0)
					{
						wanderTimer = wanderTime;

						// random int to make movement a little less random
						wanderDir = new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)).normalized;
					}

					characterMove.moveVector = wanderDir;
				}
				break;
			case AiState.Attacking:
				{
					Vector3 dir3 = player.position - transform.position;
					Vector2 dir = new(dir3.x, dir3.z);

					characterMove.moveVector = dir.normalized;
				}
				break;
			case AiState.Retreating:
				{
					Vector3 dir3 = player.position - transform.position;
					Vector2 dir = new(dir3.x, dir3.z);

					characterMove.moveVector = -dir.normalized;
				}
				break;
			case AiState.Dead:
				characterMove.moveVector = Vector2.zero;
				break;
			default:
				break;
		}
	}

	void SetColor()
	{
		switch (state)
		{
			case AiState.Wandering:
				meshRenderer.material.color = Color.green;
				break;
			case AiState.Attacking:
				meshRenderer.material.color = Color.red;
				break;
			case AiState.Retreating:
				meshRenderer.material.color = Color.blue;
				break;
			case AiState.Dead:
				meshRenderer.material.color = Color.grey;
				break;
			default:
				break;
		}
	}

	void GetData()
	{
		// Raycast to player to see if we can see them
		Vector3 dir = player.position - transform.position;
		canSeePlayer = !Physics.Raycast(transform.position, dir, dir.magnitude, visibilityMask, QueryTriggerInteraction.Ignore);
	}

	void GetState()
	{
		if (health.GetHealth() <= 0)
		{
			state = AiState.Dead;
			return;
		}

		if (canSeePlayer)
		{
			if (flags.HasFlag(AiFlags.Coward))
			{
				state = AiState.Retreating;
				return;
			}
			else if (flags.HasFlag(AiFlags.Brave))
			{
				state = AiState.Attacking;
				return;
			}

			if (health.GetHealth() < runThreshold)
			{
				state = AiState.Retreating;
				return;
			}

			state = AiState.Attacking;
			return;
		}

		state = AiState.Wandering;
	}
}
