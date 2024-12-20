using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private EventInstance ambience;
	private EventInstance music;
	private readonly List<EventInstance> eventInstances = new();
	private readonly List<StudioEventEmitter> eventEmitters = new();

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

	public void InitializeMusic(EventReference eventReference, int musicEventIndex)
	{
		music.getPlaybackState(out PLAYBACK_STATE state);

		if (state == PLAYBACK_STATE.STOPPED)
		{
			music = CreateEventInstance(eventReference);
			music.start();
		}

		music.setParameterByName("MusicArea", musicEventIndex);
	}

	public void InitializeAmbience(EventReference eventReference)
	{
		ambience = CreateEventInstance(eventReference);
		ambience.start();
	}

	public void SetAmbienceParameters(string paramName, float paramValue)
	{
		ambience.setParameterByName(paramName, paramValue);
	}

	public void PlayOneShot(EventReference eventReference, Vector3 worldPos)
	{
		RuntimeManager.PlayOneShot(eventReference, worldPos);
	}

	public EventInstance CreateEventInstance(EventReference eventReference)
	{
		var instance = RuntimeManager.CreateInstance(eventReference);
		eventInstances.Add(instance);
		return instance;
	}

	public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterObject)
	{
		var emitter = emitterObject.GetComponent<StudioEventEmitter>();
		emitter.EventReference = eventReference;
		eventEmitters.Add(emitter);
		return emitter;
	}

	private void CleanUp()
	{
		foreach (var instance in eventInstances)
		{
			instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			instance.release();
		}

		foreach (var emitter in eventEmitters)
		{
			emitter.Stop();
		}
	}

	private void OnDestroy()
	{
		CleanUp();
	}

	public void DisableAllSounds()
	{
		CleanUp();
	}
}
