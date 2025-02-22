using System.Collections;
using UnityEngine;

public class DivineLightAI2D : MonoBehaviour
{
	[Header("Detection Settings")]
	[SerializeField] private float detectionRadius = 5f; // Raio de detecção
	[SerializeField] private float searchTime = 3f; // Tempo que o inimigo continua procurando após perder o jogador
	[SerializeField] private LayerMask playerLayer; // Camada do jogador

	[Header("Movement Settings")]
	[SerializeField] private float patrolSpeed = 1.5f;
	[SerializeField] private float chaseSpeed = 3.5f;
	[SerializeField] private Transform[] patrolPoints;

	private Rigidbody2D rb;
	private Transform player;
	private int currentPatrolIndex = 0;
	private bool isChasing = false;
	private bool isSearching = false;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player")?.transform;

		if (player == null)
		{
			Debug.LogError("⚠️ Jogador não encontrado! Certifique-se de que o Player tem a tag 'Player'.");
		}

		if (patrolPoints.Length > 0)
		{
			StartCoroutine(PatrolRoutine());
		}
	}

	private void Update()
	{
		if (isChasing)
		{
			ChasePlayer();
		}
		else if (!isSearching)
		{
			MoveToPatrolPoint();
		}

		DetectPlayer();
	}

	void MoveToPatrolPoint()
	{
		if (patrolPoints.Length == 0) return;

		Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];
		Vector2 direction = (targetPatrolPoint.position - transform.position).normalized;

		rb.linearVelocity = direction * patrolSpeed;

		if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 0.3f)
		{
			StartCoroutine(WaitAtPatrolPoint());
		}
	}

	IEnumerator WaitAtPatrolPoint()
	{
		rb.linearVelocity = Vector2.zero;
		yield return new WaitForSeconds(2f);
		currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
	}

	void ChasePlayer()
	{
		if (player == null) return;

		Vector2 direction = (player.position - transform.position).normalized;
		rb.linearVelocity = direction * chaseSpeed;
	}

	void DetectPlayer()
	{
		Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

		if (detectedPlayer != null)
		{
			if (!isChasing)
			{
				Debug.Log("🚨 Jogador detectado! Inimigo iniciando perseguição.");
			}

			isChasing = true;
			isSearching = false;
			StopCoroutine(SearchForPlayer());
		}
		else if (isChasing && !isSearching)
		{
			Debug.Log("👀 Perdemos o jogador... procurando por " + searchTime + " segundos.");
			StartCoroutine(SearchForPlayer());
		}
	}

	IEnumerator SearchForPlayer()
	{
		isSearching = true;
		yield return new WaitForSeconds(searchTime);
		isChasing = false;
		isSearching = false;
		Debug.Log("❌ O jogador não foi encontrado. Voltando à patrulha.");
	}

	private IEnumerator PatrolRoutine()
	{
		while (!isChasing)
		{
			MoveToPatrolPoint();
			yield return null;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}
}


