using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{

    public Sound[] soundsEffects;
    public Sound[] musics;

    public AudioMixer audioMix;

    public float masterVolume, musicVolume, sfxVolume;

    protected override void Awake()
    {

        //audioMix.GetFloat("MasterVolume",out masterVolume);
        //audioMix.GetFloat("MusicsVolume", out musicVolume);
        //audioMix.GetFloat("SFXVolume", out sfxVolume);

        base.Awake();

        foreach (Sound s in soundsEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;

            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            if (s.playOnAwake)
            {
                Play(s.name);
            }
            s.source.outputAudioMixerGroup = s.output;
        }
        foreach (Sound s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;

            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            if (s.playOnAwake)
            {
                PlayMusic(s.name);
            }
            s.source.outputAudioMixerGroup = s.output;
        }

    }

    public void Play(string name)
    {
        Sound s = Array.Find(soundsEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound name not find : " + name);
            return;
        }
        s.source.Play();
    }

    public void PlayOnAwake(string name, bool playOnAwake)
    {
        Sound s = Array.Find(soundsEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound name not find : " + name);
            return;
        }
        s.source.playOnAwake = playOnAwake;
    }
    public void PauseSound(string name)
    {
        Sound s = Array.Find(soundsEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("musics name not find : " + name);
            return;
        }
        s.source.Pause();
    }
    public void UnpauseSound(string name)
    {
        Sound s = Array.Find(soundsEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("musics name not find : " + name);
            return;
        }
        s.source.UnPause();
    }
    public void StopSound(string name)
    {
        Sound s = Array.Find(soundsEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("musics name not find : " + name);
            return;
        }
        s.source.Stop();
    }

    public void ModifyVolume(string name, float volume)
    {
        Sound s = Array.Find(soundsEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound name not find : " + name);
            return;
        }
        s.source.volume = volume;
    }

    public void ModifyAllVolume(float i)
    {
        audioMix.SetFloat("MasterVolume", Mathf.Log10(i) * 20);
        masterVolume = i;
    }

    public void ModifyMusicVolume(float i)
    {
        audioMix.SetFloat("MusicsVolume", Mathf.Log10(i) * 20);
        musicVolume = i;
    }

    public void ModifySFXVolume(float i)
    {
        audioMix.SetFloat("SFXVolume", Mathf.Log10(i) * 20);
        sfxVolume = i;
    }

    public void ModifyPitch(string name, float pitch)
    {
        Sound s = Array.Find(soundsEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound name not find : " + name);
            return;
        }
        s.source.pitch = pitch;
    }
    public float PlayTime(string name)
    {
        Sound s = Array.Find(soundsEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound name not find : " + name);
            return 0;
        }
        s.source.Play();
        return s.clip.length;
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("musics name not find : " + name);
            return;
        }
        s.source.Play();
    }
    public void PauseMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("musics name not find : " + name);
            return;
        }
        s.source.Pause();
    }
    public void UnpauseMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("musics name not find : " + name);
            return;
        }
        s.source.UnPause();
    }
    public void StopMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("musics name not find : " + name);
            return;
        }
        s.source.Stop();
    }

    public void ModifyMusicVolume(string name, float volume)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound name not find : " + name);
            return;
        }
        s.source.volume = volume;
    }

    public void StopAllSoud()
    {
        foreach (Sound s in soundsEffects)
        {
            s.source.Stop();
        }
        foreach (Sound s in musics)
        {
            s.source.Stop();
        }
    }

}