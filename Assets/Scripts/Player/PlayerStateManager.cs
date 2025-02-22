using System;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
	public static PlayerStateManager Instance { get; private set; }

	public enum PlayerState
	{
		Normal,
		Dead,
		Transparent,
		PowerUpActive,
		InLight,
		InShadow
	}

	private PlayerState currentState = PlayerState.Normal;

	// Eventos para notificar mudanças de estado
	public event Action<PlayerState> OnStateChanged;

	private void Awake()
	{
		// Garante que só existe uma instância dessa classe
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public PlayerState GetCurrentState()
	{
		return currentState;
	}

	public void SetState(PlayerState newState)
	{
		if (currentState != newState)
		{
			currentState = newState;
			OnStateChanged?.Invoke(currentState);
		}
	}

	public bool IsState(PlayerState state)
	{
		return currentState == state;
	}

	public void ResetState()
	{
		currentState = PlayerState.Normal;
		OnStateChanged?.Invoke(currentState);
	}
}

