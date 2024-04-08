using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    // Use FindObjectOfType<AudioManager>().Play("bundleName", "audioName"); to play audio

    [Serializable]
    public class AudioBundle
	{
        public string bundleName;
        public Sound[] bundleSounds;
    }
    [SerializeField] AudioBundle[] audioBundles;

    public static AudioManager itsMe;
    public float MasterVolume = 1;
    
    void Awake()
    {
        if (itsMe == null)
        {
            itsMe = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Will stay from scene to scene
        DontDestroyOnLoad(gameObject);

        foreach (AudioBundle ab in audioBundles)
		{
            LoadArray(ab.bundleSounds);
		}
    }

    private void Start()
    {
        //Play("bgmTitle"); 

    }

	private void FixedUpdate()
	{
    }

    public void Play(string bundleName, string audioName)
    {
        /// Plays a specific AudioClip based on which bundle name it's in and what it's audio name is
        foreach (AudioBundle ab in audioBundles)
        {
            if (ab.bundleName != bundleName)
                continue;

            foreach (Sound s in ab.bundleSounds)
            {
                if (s.name != audioName && s.clip.name != audioName)
                    continue;

                s.source.Play();
                return;
            }
        }

        Debug.LogError("AudioManager.Play() - Didn't find shit");
    }

    public void PlayRandom(string bundleName)
    {
        /// Plays a random AudioClip based on which bundle name it's in
        foreach (AudioBundle ab in audioBundles)
        {
            if (ab.bundleName != bundleName)
                continue;

            ab.bundleSounds[Random.Range(0, ab.bundleSounds.Length)].source.Play();
            return;
        }

        Debug.LogError("AudioManager.PlayRandom() - Didn't find shit");
    }

    /*public void PlaySFX(string name)
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);

        foreach (Sound s in sfxSounds)
        {
            if (s.name == name)
            {
                s.source.Play();
                //Debug.LogError("hello???");
                return;
            }
        }
        Debug.LogError("Didn't find shit");
    }

    public void PlayUI(string name)
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);

        foreach (Sound s in sfxUI)
        {
            if (s.name == name)
            {
                s.source.Play();
                //Debug.LogError("hello???");
                return;
            }
        }
        Debug.LogError("Didn't find shit");
    }


    #region UI Sounds
    public void PlayHoverUI()
    {
        //Sound s = sfxUI[;
        PlayUI("uiShift" + Random.Range(0, 8));
        //Debug.LogWarning(s.name);
    }

    public void PlayClickUI()
    {
        //Sound s = sfxUI[Random.Range(9, sfxUI.Length)];

        PlayUI("uiClick" + Random.Range(0, 2));
        //Debug.LogWarning(s.name);
    }

    #endregion*/

    public void StopBundle(string bundleName)
    {
        /// Stop every sound inside a particular audio bundle
        foreach (AudioBundle ab in audioBundles)
		{
            if (ab.bundleName != bundleName)
                continue;

            foreach (Sound s in ab.bundleSounds)
			{
                if (s.source.isPlaying) s.source.Stop();
			}
            return;
		}

        Debug.LogError("AudioManager.StopBundle - Didn't find shit");
    }

    public void StopSpecific(string bundleName, string audioName)
    {
        /// Stop a specific AudioClip inside a particular audio bundle
        foreach (AudioBundle ab in audioBundles)
        {
            if (ab.bundleName != bundleName)
                continue;

            foreach (Sound s in ab.bundleSounds)
            {
                if ((s.name == audioName || s.clip.name != audioName) && s.source.isPlaying)
                {
                    s.source.Stop();
                    return;
                }
            }
        }

        Debug.LogError("AudioManager.StopSpecific() - Didn't find shit");
    }

    public void StopAll()
    {
        AudioSource[] allSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach (AudioSource s in allSources)
        {
            if (s.isPlaying)
            {
                s.Stop();
            }
        }
    }

    public void SetPitch(string bundleName, float pitchValue)
    {
        /// Sets the pitch of an audio bundle's AudioClips
        foreach (AudioBundle ab in audioBundles)
        {
            if (ab.bundleName != bundleName)
                continue;

            foreach (Sound s in ab.bundleSounds)
            {
                s.source.pitch = pitchValue;
                return;
            }
        }
    }

    /*public void Pause(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Pause();
                break;
            }
        }
    }

    public void Unpause(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.UnPause();
                break;
            }
        }
    }*/

    public void LoadArray(Sound[] sA)
	{
        foreach (Sound s in sA)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume * MasterVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void LoadSingle(Sound s)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume * MasterVolume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }
}
