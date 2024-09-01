using Assets._Scripts.BaseInfos;
using UnityEngine;

public class AmbienceTrigger : MonoBehaviour
{
	[SerializeField]
	[Range(0, 1)]
	private float intensity = 0f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			switch (gameObject.tag)
			{
				case Constants.Tags.rain:
					AudioManager.Instance.InitializeAmbience(FMODEvents.Instance.ForestRain);
					break;

				case Constants.Tags.rainIntensity:
					AudioManager.Instance.SetAmbienceParameters(Constants.Tags.rainIntensity, intensity);
					break;

				default:
					break;
			}
		}
	}
}
