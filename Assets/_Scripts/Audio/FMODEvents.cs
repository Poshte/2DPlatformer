using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
	[field: Header("Interact SFX")]
	[field: SerializeField] public EventReference InteractSound { get; private set; }


	[field: Header("Player SFX")]
	[field: SerializeField] public EventReference PlayerFootstepsSound { get; private set; }


	private static FMODEvents _instance;
	public static FMODEvents Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError("FMODEvents is null");
			}

			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null)
		{
			Debug.LogError("More than one instance of FMODEvents exist");
		}
		else
		{
			_instance = this;
		}
	}
}
