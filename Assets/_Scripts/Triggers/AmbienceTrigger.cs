using Assets._Scripts.BaseInfos;
using UnityEngine;

public class AmbienceTrigger : MonoBehaviour
{
	[SerializeField]
	[Range(0, 1)]
	private float intensity = 0f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constants.Tags.Player))
		{
			switch (gameObject.tag)
			{
				case Constants.Tags.Rain:
					AudioManager.Instance.InitializeAmbience(FMODEvents.Instance.ForestRain);
					break;

				case Constants.Tags.RainIntensity:
					AudioManager.Instance.SetAmbienceParameters(Constants.Tags.RainIntensity, intensity);
					break;

				default:
					break;
			}
		}
	}
}
