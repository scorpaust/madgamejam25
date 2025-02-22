using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	[Header("Target")]
	[SerializeField] private Transform target; // O jogador

	[Header("Camera Settings")]
	[SerializeField] private float followSpeed = 5f; // Velocidade de suaviza��o
	[SerializeField] private Vector2 offset = new Vector2(2f, 0f); // Offset da c�mera em rela��o ao jogador
	[SerializeField] private bool useBounds = false; // Limitar a c�mera dentro de um espa�o?

	[Header("Camera Bounds (Optional)")]
	[SerializeField] private Vector2 minBounds;
	[SerializeField] private Vector2 maxBounds;

	private Vector3 targetPosition;

	void LateUpdate()
	{
		if (target == null) return; // Se n�o h� jogador, n�o faz nada

		// Calcula a posi��o alvo da c�mera baseada no offset
		float targetX = target.position.x + (target.localScale.x > 0 ? offset.x : -offset.x);
		float targetY = target.position.y + offset.y;

		targetPosition = new Vector3(targetX, targetY, transform.position.z);

		// Se os limites est�o ativados, restringe a posi��o
		if (useBounds)
		{
			targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
			targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
		}

		// Suaviza a movimenta��o da c�mera
		transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
	}
}


