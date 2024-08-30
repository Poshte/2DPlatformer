using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerController : MonoBehaviour
{
	[SerializeField]
	[Range(0, 1)]
	private float rainIntensity = 0f; 

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (gameObject.CompareTag("AreaRain"))
			{
				AudioManager.Instance.InitializeAmbience(FMODEvents.Instance.ForestRain);
			}
			else if (gameObject.CompareTag("RainIntensity"))
			{
				AudioManager.Instance.SetAmbienceParameters(nameof(rainIntensity), rainIntensity);
			}
		}
	}
}
