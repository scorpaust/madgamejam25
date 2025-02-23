using UnityEngine;
using UnityEngine.SceneManagement;
using static FadeScript;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void QuitGame()
	{
		Debug.Log("Game is quitting...");
		Application.Quit();
	}
}
