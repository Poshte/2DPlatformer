using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
	[field: Header("Interact SFX")]
	[field: SerializeField] public EventReference InteractSound { get; private set; }
	[field: SerializeField] public EventReference PortalTeleport { get; private set; }

	[field: Header("Player SFX")]
	[field: SerializeField] public EventReference PlayerFootstepsSound { get; private set; }

	[field: Header("Word SFX")]
	[field: SerializeField] public EventReference Heartbeat { get; private set; }

	[field: Header("Ambience")]
	[field: SerializeField] public EventReference ForestRain { get; private set; }

	[field: Header("Music")]
	[field: SerializeField] public EventReference TestMusic { get; private set; }

	[field: Header("Trigger SFX")]
	[field: SerializeField] public EventReference DiaryAddedSound { get; private set; }

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
