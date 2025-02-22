using UnityEngine;

public enum PowerUpType { Dash, Transparency, DoubleJump }

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "Game/Power Up")]
public class PowerUp : ScriptableObject
{
	public PowerUpType powerUpType;
	public Sprite powerUpIcon; // Ícone do power-up
	public float duration; // Quanto tempo o efeito dura
}