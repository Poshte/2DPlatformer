using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RainCollisionManager : MonoBehaviour
{
	[Header("Lighting Setup")]
	public GameObject lightPrefab;
	public int maxLights = 20;
	private ParticleSystem rainParticleSystem;
	private List<ParticleCollisionEvent> collisionEvents;
	private List<GameObject> lightPool;
	private int currentLightIndex = 0;

	void Start()
	{
		rainParticleSystem = GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
		lightPool = new List<GameObject>();

		for (int i = 0; i < maxLights; i++)
		{
			var newLight = Instantiate(lightPrefab, transform);
			newLight.SetActive(false);
			lightPool.Add(newLight);
		}
	}

	void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = rainParticleSystem.GetCollisionEvents(other, collisionEvents);

		for (int i = 0; i < numCollisionEvents; i++)
		{
			var hitPosition = collisionEvents[i].intersection;
			TriggerLightSplash(hitPosition);
		}
	}

	void TriggerLightSplash(Vector3 position)
	{
		var activeLight = lightPool[currentLightIndex++];
		activeLight.transform.position = position;
		activeLight.SetActive(true);

		if (currentLightIndex >= maxLights)
		{
			currentLightIndex = 0;
		}
	}
}