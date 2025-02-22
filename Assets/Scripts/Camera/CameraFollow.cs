using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	[Header("Target")]
	[SerializeField] private Transform target; // O jogador

	[Header("Camera Settings")]
	[SerializeField] private float followSpeed = 5f; // Velocidade de suavização
	[SerializeField] private Vector2 offset = new Vector2(2f, 0f); // Offset da câmera em relação ao jogador
	[SerializeField] private bool useBounds = false; // Limitar a câmera dentro de um espaço?

	[Header("Camera Bounds (Optional)")]
	[SerializeField] private Vector2 minBounds;
	[SerializeField] private Vector2 maxBounds;

	private Vector3 targetPosition;

	void LateUpdate()
	{
		if (target == null) return; // Se não há jogador, não faz nada

		// Calcula a posição alvo da câmera baseada no offset
		float targetX = target.position.x + (target.localScale.x > 0 ? offset.x : -offset.x);
		float targetY = target.position.y + offset.y;

		targetPosition = new Vector3(targetX, targetY, transform.position.z);

		// Se os limites estão ativados, restringe a posição
		if (useBounds)
		{
			targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
			targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
		}

		// Suaviza a movimentação da câmera
		transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
	}
}


