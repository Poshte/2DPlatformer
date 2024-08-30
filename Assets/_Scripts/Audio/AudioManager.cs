using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("AudioManager is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("More than one instance of AudioManager exist");
        }
        else
        {
            _instance = this;
        }
    }

	public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference sound)
    {
        var instance = RuntimeManager.CreateInstance(sound);
		return instance;
    }
}
