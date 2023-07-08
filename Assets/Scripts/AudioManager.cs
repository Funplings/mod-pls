using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] private int sfxChannelCount = 2;
    [SerializeField] private List<SFX> sfxList;
    [SerializeField] private List<Music> musicList;
    private AudioSource musicSource = null;
    private AudioSource loopSource = null;
    private Music playingSong = null;

    private Dictionary<string, SFX> sfxMap = new Dictionary<string, SFX>();
    private Dictionary<string, Music> musicMap = new Dictionary<string, Music>();

    #region SFX
    private int sfxChannelIndex = 0;
    private AudioSource[] sfxChannels;
    #endregion

    #region Options
    private float musicVolume = 1;
    private float sfxVolume = 1;
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (SFX sfx in sfxList)
        {
            sfxMap[sfx.name] = sfx;
        }

        foreach (Music music in musicList)
        {
            musicMap[music.name] = music;
        }

        SetupSFXChannels();
    }

    void Start()
    {
        PlayMusic("biden");
    }

    private void SetupSFXChannels()
    {
        sfxChannels = new AudioSource[sfxChannelCount];
        for (int i = 0; i < sfxChannelCount; i++)
        {
            sfxChannels[i] = gameObject.AddComponent<AudioSource>();
            sfxChannels[i].bypassEffects = true;
            sfxChannels[i].bypassListenerEffects = true;
            sfxChannels[i].bypassReverbZones = true;
        }
    }

    public bool CurrentlyPlaying(string musicName)
    {
        return playingSong != null && musicName.Equals(playingSong.name);
    }


    public void PlayMusic(string musicName, float fadeOutTime = .5f, float fadeInTime = 0f)
    {
        if (musicMap.ContainsKey(musicName) && !CurrentlyPlaying(musicName))
        {
            playingSong = musicMap[musicName];
            StartCoroutine(FadeAndDestroyOld(musicMap[musicName], fadeOutTime, fadeInTime));
        }
    }

    public void PlaySFX(string sfxName)
    {
        if (sfxMap.ContainsKey(sfxName))
        {
            SFX sfx = sfxMap[sfxName];
            AudioSource sfxChannel = sfxChannels[sfxChannelIndex];
            sfxChannel.clip = sfx.clip;
            sfxChannel.volume = sfx.volume * sfxVolume;
            sfxChannel.Play();
            sfxChannelIndex = (sfxChannelIndex + 1) % sfxChannelCount;
        }
    }

    public void SetMusicVolume(float ratio)
    {
        musicVolume = ratio;

        // live edit
        if (playingSong != null)
        {
            if (musicSource != null) musicSource.volume = playingSong.volume * musicVolume;
            if (loopSource != null) loopSource.volume = playingSong.volume * musicVolume;
        }
    }

    public void SetSFXVolume(float ratio)
    {
        sfxVolume = ratio;
    }

    IEnumerator FadeAndDestroyOld(Music newMusic, float fadeOutTime, float fadeInTime)
    {

        AudioSource oldSource = musicSource;
        AudioSource oldLoop = loopSource;
        AudioSource currentSource = gameObject.AddComponent<AudioSource>();
        AudioSource newLoop = newMusic.firstPass == null ? null : gameObject.AddComponent<AudioSource>();
        loopSource = newLoop;
        musicSource = currentSource;

        // Fade out
        if (oldSource != null)
        {

            if (fadeOutTime > 0)
            {

                float originalVolume = oldSource.volume; // already option-adjusted
                float currentTime = 0;
                while (fadeOutTime > currentTime)
                {
                    oldSource.volume = Mathf.Lerp(originalVolume, 0, currentTime / fadeOutTime);
                    if (oldLoop) oldLoop.volume = oldSource.volume;
                    currentTime += Time.deltaTime;
                    yield return null;
                }
                oldSource.volume = 0;
            }
            Destroy(oldSource);
            if (oldLoop) Destroy(oldLoop);
        }

        // Loop setup
        if (newMusic.firstPass == null)
        {
            currentSource.clip = newMusic.loopPass;
            currentSource.Play();
            currentSource.loop = newMusic.loop;
        }
        else
        {
            currentSource.clip = newMusic.firstPass;
            currentSource.Play();
            newLoop.clip = newMusic.loopPass;
            newLoop.volume = newMusic.volume * musicVolume;
            newLoop.PlayScheduled(newMusic.firstPass.length + AudioSettings.dspTime);
            newLoop.loop = newMusic.loop;
        }

        // Fade In
        float targetVolume = newMusic.volume * musicVolume;
        float cTime = 0;
        currentSource.volume = 0;
        while (fadeInTime > cTime)
        {
            if (currentSource == null || currentSource != musicSource) yield break;
            currentSource.volume = Mathf.Lerp(0, targetVolume, cTime / fadeInTime);
            cTime += Time.deltaTime;
            yield return null;
        }
        currentSource.volume = targetVolume;

    }


    public void JumpTime(float time)
    {
        this.musicSource.time = time;
    }
}

[System.Serializable]
public class SFX
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class Music
{
    public string name;
    public AudioClip firstPass;
    public AudioClip loopPass;
    public bool loop = true;

    [Range(0, 1)]
    public float volume = 1;
}
