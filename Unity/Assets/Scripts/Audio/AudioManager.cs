using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Sistemas de sons
    [SerializeField] private AudioSource _bgMusicSrc;
    [SerializeField] private AudioSource _sfxSrc;
    public AudioSource _carSrc;
    public AudioSource _carMovingSrc;

    // Listas de sons
    [SerializeField] private AudioClip[] _bgMusics;
    [SerializeField] private AudioClip[] _sfxClips;

    // Player
    [Header("Audio")]
    [SerializeField] private AudioClip[] carCrashes;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GetVolumeBg();
        
    }

    #region Scene Management
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeMusicForScene(scene.name);
    }

    public void ChangeMusicForScene(string sceneName)
    {
        // Map scene names to specific audio clips
        AudioClip clipToPlay = null;

        switch (sceneName)
        {
            case "MainMenu":
                clipToPlay = _bgMusics[0];
                break;
            default:
                clipToPlay = _bgMusics[1]; 
                break;
        }

        if (clipToPlay != null && _bgMusicSrc.clip != clipToPlay)
        {
            _bgMusicSrc.clip = clipToPlay;
            _bgMusicSrc.Play();
        }
        else if (clipToPlay == null)
        {
            _bgMusicSrc.Stop(); // Stop if no music is assigned
        }
    }
    #endregion

    #region Play Sound

    public void PlayMusic(int idBgMusic)
    {
        AudioClip clip = _bgMusics[idBgMusic];
        _bgMusicSrc.Stop();
        _bgMusicSrc.clip = clip;
        _bgMusicSrc.loop = true;
        _bgMusicSrc.Play();

    }

    public void PlaySoundEffect(int idSfx)
    {
        AudioClip clip = _sfxClips[idSfx];
        _sfxSrc.PlayOneShot(clip);
    }
    
    public void PlayCarCrash()
    {
        AudioClip clip = carCrashes[Random.Range(1, carCrashes.Length)];
        _carSrc.PlayOneShot(clip);
    }

    public void PlayCarBigCrash()
    {
        AudioClip clip = carCrashes[0];
        _carSrc.PlayOneShot(clip);
    }

    public void PlayCarMoving()
    {
        if (_carMovingSrc.isPlaying)
        {
            _carMovingSrc.Pause();
        }
        else
        {
            _carMovingSrc.Play();
        }
    }

    #endregion

    #region Volume changer

    public void MuteSFX(bool option)
    {
        _sfxSrc.mute = option;
    }
   
    public void MuteBG(bool option)
    {
        _bgMusicSrc.mute = option;
    }
    
    public void UpdateVolumeSfx(float valor)
    {
        float newVolume = valor == 0 ? -80 : 0 - (30f - (30f * valor));
        audioMixer.SetFloat("VolumeSFX", newVolume);
    }
    
    public void UpdateVolumeBg(float valor)
    {
        float newVolume = valor == 0 ? -80 : 0 - (30f - (30f * valor));
        audioMixer.SetFloat("VolumeBGM", newVolume);
    }

    public void UpdateVolumeMaster(float valor)
    {
        float newVolume = valor == 0 ? -80 : 0 - (30f - (30f * valor));
        audioMixer.SetFloat("VolumeMaster", newVolume);
    }

    #endregion

    #region Get and Set

    public void SaveVolumeBg()
    {
        float volumeMusic = _bgMusicSrc.volume;
        PlayerPrefs.SetFloat("VolumeBG", volumeMusic);

    }
    
    public void SaveVolumeSfx()
    {
        float volumeMusic = _sfxSrc.volume;
        PlayerPrefs.SetFloat("VolumeSFX", volumeMusic);
    }
    
    public float GetVolumeBg()
    {
        float volume = PlayerPrefs.GetFloat("VolumeBG", 0.5f);
        _bgMusicSrc.volume = volume;
        return volume;
    }
    
    public float GetVolumeSfx()
    {
        float volume = PlayerPrefs.GetFloat("VolumeSFX", 0.5f);
        _sfxSrc.volume = volume;
        return volume;
    }

    #endregion
}
