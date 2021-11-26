using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    public Sound[] sounds;

    private void Start()
    {
        _instance = this;
    }

    public static void Play(string soundName)
    {
        Sound loadedSound = new Sound();
        foreach(Sound _sound in _instance.sounds)
        {
            if(soundName == _sound.name)
            {
                loadedSound = _sound;
            }
        }

        if (string.IsNullOrEmpty(loadedSound.name))
        {
            Debug.Log("No sound with that name");
            return;
        }

        if(!loadedSound.playAfterGameOver && PlayerManagment.gameOver)
        {
            return;
        }

        AudioSource newAudioSource = ((GameObject)Instantiate(Resources.Load("SoundEmpty"),_instance.gameObject.transform)).GetComponent<AudioSource>();
        newAudioSource.transform.position = _instance.gameObject.transform.position;
        newAudioSource.clip = loadedSound.clip;
        newAudioSource.volume = loadedSound.volume;
        newAudioSource.pitch = loadedSound.pitch;
        newAudioSource.Play();
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume;
    public float pitch;
    public bool playAfterGameOver;
}
