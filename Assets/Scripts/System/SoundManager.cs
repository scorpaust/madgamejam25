using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance { get; private set; }

	[Header("Audio Sources")]
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	[Header("Audio Settings")]
	[Range(0f, 1f)] public float musicVolume = 1f;
	[Range(0f, 1f)] public float sfxVolume = 1f;

	[Header("Audio Clips")]
	public AudioClip[] musicTracks; // Lista de músicas
	private int currentMusicIndex = 0;
	private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

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

	private void Start()
	{
		if (musicTracks.Length > 0)
		{
			PlayMusic(musicTracks[currentMusicIndex]);
		}
	}

	public void PlayMusic(AudioClip clip)
	{
		if (clip != null)
		{
			StartCoroutine(FadeMusic(clip));
		}
	}

	private IEnumerator FadeMusic(AudioClip newClip)
	{
		float fadeTime = 1f;
		float startVolume = musicSource.volume;

		// Fade Out
		for (float t = 0; t < fadeTime; t += Time.deltaTime)
		{
			musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
			yield return null;
		}

		musicSource.clip = newClip;
		musicSource.Play();

		// Fade In
		for (float t = 0; t < fadeTime; t += Time.deltaTime)
		{
			musicSource.volume = Mathf.Lerp(0, musicVolume, t / fadeTime);
			yield return null;
		}

		musicSource.volume = musicVolume;
	}

	public void PlayNextMusic()
	{
		currentMusicIndex = (currentMusicIndex + 1) % musicTracks.Length;
		PlayMusic(musicTracks[currentMusicIndex]);
	}

	public void PlaySFX(string sfxName)
	{
		if (sfxDictionary.ContainsKey(sfxName))
		{
			sfxSource.PlayOneShot(sfxDictionary[sfxName], sfxVolume);
		}
		else
		{
			Debug.LogWarning($"SFX {sfxName} não encontrado!");
		}
	}

	public void AddSFX(string key, AudioClip clip)
	{
		if (!sfxDictionary.ContainsKey(key))
		{
			sfxDictionary.Add(key, clip);
		}
	}

	public void SetMusicVolume(float volume)
	{
		musicVolume = volume;
		musicSource.volume = volume;
	}

	public void SetSFXVolume(float volume)
	{
		sfxVolume = volume;
	}
}
