using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class SplashLightEffect : MonoBehaviour
{
	public float lifetime = 0.15f;
	private Light2D splashLight;
	private float defaultIntensity;
	private float timer;

	void Awake()
	{
		splashLight = GetComponent<Light2D>();
		defaultIntensity = splashLight.intensity;
	}

	void OnEnable()
	{
		timer = lifetime;
		splashLight.intensity = defaultIntensity;
	}

	void Update()
	{
		timer -= Time.deltaTime;

		splashLight.intensity = Mathf.Lerp(0, defaultIntensity, timer / lifetime);

		if (timer <= 0)
		{
			gameObject.SetActive(false);
		}
	}
}