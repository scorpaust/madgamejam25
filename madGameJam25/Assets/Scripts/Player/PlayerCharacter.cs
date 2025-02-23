using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerCharacter : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float acceleration = 10f;
	[SerializeField] private float deceleration = 15f;
	[SerializeField] private float maxSpeed = 10f;
	private float currentSpeed;
	private Rigidbody2D rb;

	[Header("Jump & Gravity")]
	[SerializeField] private float jumpForce = 20f;
	[SerializeField] private float gravityScale = 2f;
	[SerializeField] private float fallingGravityScale = 3f;
	private bool isGrounded;
	private bool canDoubleJump;

	[Header("Dash Ability")]
	[SerializeField] private float dashSpeed = 20f;
	[SerializeField] private float dashDuration = 0.2f;
	[SerializeField] private float dashCooldown = 1f;
	private bool isDashing = false;
	private bool canDash = true;

	[Header("Light and Shadow")]
	[SerializeField] private float lightDamage = 10f;
	[SerializeField] private float shadowRecovery = 5f;
	[SerializeField] private Slider energyBar;
	[SerializeField] private float maxEnergy = 100f;
	private float currentEnergy;
	private bool isInLight;

	[Header("State and Feedbacks")]
	[SerializeField] private Color normalColor = Color.white;
	[SerializeField] private Color dangerColor = Color.white;
	private SpriteRenderer sr;

	[Header("Ground Detection")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckDistance = 0.1f;
	[SerializeField] private LayerMask groundLayer;

	[Header("Interactions")]
	private Transform currentPlatform;

	[Header("Abilities (Unlockable)")]
	[SerializeField] private bool hasTransparency = false;
	[SerializeField] private bool hasDash = false;
	[SerializeField] private bool hasDoubleJump = false;
	private bool isTransparent = false;

    private Animator anim;  // Animator reference


    void Start()
	{
		rb = GetComponentInChildren<Rigidbody2D>();
		sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        currentEnergy = maxEnergy;
		PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
	}

	void Update()
	{
		CheckForDamage();
		Jump();
		if (hasDash) Dash();
		if (hasTransparency) ToggleTransparency();

    }

	void FixedUpdate()
	{
		Move();
	}

	void Move()
	{
		if (isDashing) return;

		float moveInput = Input.GetAxisRaw("Horizontal");
		currentSpeed = Mathf.Lerp(currentSpeed, moveInput * maxSpeed, acceleration * Time.fixedDeltaTime);
		rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        // Set Animation States
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

	void Jump()
	{
		isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

		if (isGrounded) canDoubleJump = true;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isGrounded)
			{
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetBool("isJumping", true); // Start Jump Animation
            }
			else if (hasDoubleJump && canDoubleJump)
			{
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
				canDoubleJump = false; // Prevents multiple double jumps
                anim.SetBool("isJumping", true); // Start Jump Animation
            }
		}

		rb.gravityScale = rb.linearVelocity.y < 0 ? fallingGravityScale : gravityScale;


        // Check if falling
        if (rb.linearVelocity.y < 0)
        {
            anim.SetBool("isJumping", false); // End Jump Animation
        }
    }

	void Dash()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
		{
			StartCoroutine(PerformDash());
		}
	}

	IEnumerator PerformDash()
	{
		isDashing = true;
		canDash = false;

		float originalGravity = rb.gravityScale;
		rb.gravityScale = 0;
		rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);

		yield return new WaitForSeconds(dashDuration);

		rb.gravityScale = originalGravity;
		isDashing = false;

		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}

	void ToggleTransparency()
	{
		if (Input.GetKeyDown(KeyCode.T) && hasTransparency)
		{
			StartCoroutine(ActivateTransparency());
		}
	}

	private IEnumerator ActivateTransparency()
	{
		isTransparent = true;
		PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Transparent);
		normalColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f); // Make semi-transparent
		gameObject.layer = LayerMask.NameToLayer("Ghost"); // Change layer to avoid detection

		yield return new WaitForSeconds(3f); // Duration of transparency

		isTransparent = false;
		PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
		normalColor = new Color(sr.color.r, sr.color.g, sr.color.b, 1f); // Restore visibility
		gameObject.layer = LayerMask.NameToLayer("Player"); // Restore original layer
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
		PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Dead);
		StartCoroutine(ReloadScene());
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		print("COllision detected: "+ other.gameObject.name);
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

	public void UnlockAbility(string abilityName)
	{
		switch (abilityName)
		{
			case "DashUnlocker":
				hasDash = true;
				Debug.Log("Dash Unlocked!");
				break;
			case "DoubleJumpUnlocker":
				hasDoubleJump = true;
				Debug.Log("Double Jump Unlocked!");
				break;
			case "TransparencyUnlocker":
				hasTransparency = true;
				Debug.Log("Transparency Unlocked!");
				break;
			default:
				Debug.Log("Unknown ability unlocked.");
				break;
		}
	}
}
