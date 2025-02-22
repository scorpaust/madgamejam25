using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float acceleration = 10f;
	[SerializeField] private float deceleration = 15f;
	[SerializeField] private float maxSpeed = 10f;
	private float currentSpeed;
	[SerializeField] private float dashSpeed = 10f;
	private Rigidbody2D rb;
	[SerializeField] private float jumpForce = 8f;
	private bool isGrounded;
	private bool canDoubleJump;

	[Header("Gravity")]
	[SerializeField] private float gravityScale = 2f;
	[SerializeField] private float fallingGravityScale = 3f;

	[Header("Light and Shadow")]
	[SerializeField] private float lightDamage = 10f;
	[SerializeField] private float shadowRecovery = 5f;
	[SerializeField] private Slider energyBar;
	[SerializeField] private float maxEnergy = 100f;
	private float currentEnergy;
	private bool isInLight;

	[Header("State and Feedbacks")]
	[SerializeField] private Color normalColor = Color.black;
	[SerializeField] private Color dangerColor = Color.white;
	private SpriteRenderer sr;

	[Header("Ground Detection")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckDistance = 0.1f;
	[SerializeField] private LayerMask groundLayer;

	[Header("Abilities")]
	[SerializeField] private bool hasDash = false;
	[SerializeField] private bool hasDoubleJump = false;
	private bool isDashing = false;

	void Start()
	{
		rb = GetComponentInChildren<Rigidbody2D>();
		sr = GetComponentInChildren<SpriteRenderer>();
		currentEnergy = maxEnergy;
	}

	void Update()
	{
		CheckForDamage();
		Jump();
	}

	void FixedUpdate()
	{
		Move();
	}

	void Move()
	{
		float moveInput = Input.GetAxisRaw("Horizontal");

		if (!isDashing)
		{
			currentSpeed = Mathf.Lerp(currentSpeed, moveInput * maxSpeed, acceleration * Time.fixedDeltaTime);
			rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
		}
	}

	void Jump()
	{
		isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

		// Reset double jump when touching the ground
		if (isGrounded)
		{
			canDoubleJump = false;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isGrounded)
			{
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
			}
			else if (hasDoubleJump && canDoubleJump)
			{
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
				canDoubleJump = false; // Disable double jump until grounded
			}
		}

		// Apply different gravity scales
		rb.gravityScale = rb.linearVelocity.y < 0 ? fallingGravityScale : gravityScale;
	}

	void CheckForDamage()
	{
		if (isInLight)
		{
			TakeDamage(lightDamage * Time.deltaTime);
		}
		else
		{
			RecoverEnergy(shadowRecovery * Time.deltaTime);
		}

		sr.color = isInLight ? dangerColor : normalColor;
	}

	void TakeDamage(float amount)
	{
		currentEnergy -= amount;
		currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
		UpdateEnergyBar();

		if (currentEnergy <= 0)
		{
			PurifyAndRespawn();
		}
	}

	void RecoverEnergy(float amount)
	{
		currentEnergy += amount;
		currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
		UpdateEnergyBar();
	}

	void UpdateEnergyBar()
	{
		if (energyBar != null)
		{
			energyBar.value = currentEnergy / maxEnergy;
		}
	}

	void PurifyAndRespawn()
	{
		Debug.Log("The dark creature was purified by light!");
		StartCoroutine(ReloadScene());
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("LightZone"))
		{
			isInLight = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("LightZone"))
		{
			isInLight = false;
		}
	}

	private IEnumerator ReloadScene()
	{
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
