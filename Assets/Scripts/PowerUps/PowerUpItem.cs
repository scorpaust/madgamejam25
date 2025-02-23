using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
	[SerializeField] private PowerUp powerUp;

	[SerializeField] private GameObject vfx;

	private SpriteRenderer sr;

    void Awake()
	{
		sr = GetComponent<SpriteRenderer>();

		sr.sprite = powerUp.powerUpIcon;
	}
 
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			PowerUpEffect playerEffect = other.GetComponent<PowerUpEffect>();

			if (playerEffect != null)
			{
				playerEffect.ApplyPowerUp(powerUp);
				vfx.SetActive(true);
				vfx.GetComponent<ParticleSystem>().Play();
				Destroy(gameObject); // Remove o item após coleta
			}
		}
	}
}
