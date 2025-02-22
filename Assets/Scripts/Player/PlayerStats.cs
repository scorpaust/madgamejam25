using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
	[Header("Health & Energy")]
	public float maxHealth = 100f;
	public float maxEnergy = 100f;
	public float lightDamagePerSecond = 10f;
	public float shadowRecoveryPerSecond = 5f;

	[Header("Movement")]
	public float moveSpeed = 5f;
	public float acceleration = 10f;
	public float deceleration = 15f;
	public float maxSpeed = 10f;

	[Header("Jump & Gravity")]
	public float jumpForce = 20f;
	public float gravityScale = 2f;
	public float fallingGravityScale = 3f;
	public bool hasDoubleJump = false;

	[Header("Dash Ability")]
	public bool hasDash = false;
	public float dashSpeed = 20f;
	public float dashDuration = 0.2f;
	public float dashCooldown = 1f;

	[Header("Abilities (Unlockable)")]
	public bool hasTransparency = false;

	[Header("Debug")]
	public bool debugMode = false;
}