using System.Collections;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
	private PlayerCharacter player; // Referência ao script do jogador

	private void Start()
	{
		player = FindObjectOfType<PlayerCharacter>(); // Encontra o PlayerCharacter no objeto
	}

	public void ApplyPowerUp(PowerUp powerUp)
	{
		StartCoroutine(ActivatePowerUp(powerUp));
	}

	private IEnumerator ActivatePowerUp(PowerUp powerUp)
	{
		switch (powerUp.powerUpType)
		{
			case PowerUpType.Dash:
				player.UnlockAbility("DashUnlocker");
				break;

			case PowerUpType.Transparency:
				player.UnlockAbility("TransparencyUnlocker");
				break;

			case PowerUpType.DoubleJump:
				player.UnlockAbility("DoubleJumpUnlocker");
				break;
		}

		Debug.Log($"🛡️ Power-Up {powerUp.powerUpType} ativado por {powerUp.duration} segundos!");

		yield return new WaitForSeconds(powerUp.duration);

		// Desativar após o tempo
		switch (powerUp.powerUpType)
		{
			case PowerUpType.Dash:
				player.UnlockAbility("DashUnlocker");
				break;

			case PowerUpType.Transparency:
				player.UnlockAbility("TransparencyUnlocker");
				break;

			case PowerUpType.DoubleJump:
				player.UnlockAbility("DoubleJumpUnlocker");
				break;
		}

		Debug.Log($"❌ Power-Up {powerUp.powerUpType} expirou!");
	}
}
