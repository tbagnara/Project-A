// Code is from Rize Education class GDM4 - C# programming

using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip backgroundMusic;
    public AudioClip levelMusic;
    public AudioClip jumpSound;
    public AudioClip walkSound;
    public AudioClip spikesSound;
    public AudioClip goalSound;

    public float MusicVolume 
    {
        get { return musicSource.volume; }
        set { musicSource.volume = Mathf.Clamp01(value); }
    }
    public float SFXVolume 
    {
        get { return sfxSource.volume; }
        set { sfxSource.volume = Mathf.Clamp01(value); }
    }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        MusicVolume = 0.1f;
        SFXVolume = 1;
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlaySoundEffect(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayWalkingSoundEffect()
    {
        if (sfxSource.isPlaying) return;
        sfxSource.PlayOneShot(walkSound);
    }

    

}
