using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class Word : MonoBehaviour
{
	private StudioEventEmitter eventEmitter;

	void Start()
	{
		eventEmitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.Instance.Heartbeat, this.gameObject);
		eventEmitter.Play();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			gameObject.SetActive(false);
			eventEmitter.Stop();
		}
	}
}