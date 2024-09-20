using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
  [Header("Audio")]
	[SerializeField] private string winLevelSound = "level_win";
	[SerializeField] private string defeatLevelSound = "level_lose";

  #region Public Methods
  public static void Win()
	{
		AudioManager.Instance.StopTrack(1); // stop music
		AudioManager.Instance.StopTrack(6); // stop turn sfx
		AudioManager.Instance.PlaySoundOneShot(Instance.winLevelSound, 7);

		Instance.isGameOverWin = true;
		Instance.isGameOverDefeat = false;

		// CurrentLevel = SceneManager.GetActiveScene().buildIndex - FirstLevelBuildIndex + 1;

		// OnGameWin?.Invoke();
		// OnGameOver?.Invoke(true);
	}

	public static void Defeat()
	{
		AudioManager.Instance.StopTrack(1); // stop music
		AudioManager.Instance.StopTrack(6); // stop turn sfx
		AudioManager.Instance.PlaySoundOneShot(Instance.defeatLevelSound, 7);

		Instance.isGameOverWin = false;
		Instance.isGameOverDefeat = true;

		// OnGameDefeat?.Invoke();
		// OnGameOver?.Invoke(false);
	}

  public static void GetMuteSettingsFromSave()
	{
		Instance.SetMusicMute(AudioManager.Instance.IsMusicMuted);
		Instance.SetSfxMuted(AudioManager.Instance.IsSfxMuted);
	}

  public static void ToggleMusic()
	{
		AudioManager.Instance.IsMusicMuted = !AudioManager.Instance.IsMusicMuted;
		Instance.SetMusicMute(AudioManager.Instance.IsMusicMuted);
	}

	public static void ToggleSfx()
	{
		AudioManager.Instance.IsSfxMuted = !AudioManager.Instance.IsSfxMuted;
		Instance.SetSfxMuted(AudioManager.Instance.IsSfxMuted);
	}
  #endregion

  #region Private Methods
  private void SetMusicMute(bool isMuted)
	{
		if (isMuted)
		{
			AudioManager.Instance.ChangeTrackVolume(1, 0f);
			AudioManager.Instance.ChangeTrackVolume(2, 0f);
		}
		else
		{
			// hard coded volume because this is just for muting,
			// the actual volume is setted up on AudioMixer asset
			AudioManager.Instance.ChangeTrackVolume(1, .5f);
			AudioManager.Instance.ChangeTrackVolume(2, .5f);
		}

		AudioManager.Instance.IsMusicMuted = isMuted;
	}

	private void SetSfxMuted(bool isMuted)
	{
		if (isMuted)
		{
			for (int i = 1; i < AudioManager.Instance.Tracks.Length + 1; i++)
			{
				if (i <= 2) continue;
				AudioManager.Instance.ChangeTrackVolume(i, 0f);
			}
		}
		else
		{
			for (int i = 1; i < AudioManager.Instance.Tracks.Length + 1; i++)
			{
				if (i <= 2) continue;
				AudioManager.Instance.ChangeTrackVolume(i, .5f);
			}
		}

		AudioManager.Instance.IsSfxMuted = isMuted;
	}
  #endregion

}