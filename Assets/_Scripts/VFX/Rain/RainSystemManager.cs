using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class RainSystemManager : MonoBehaviour
{
	[SerializeField] private Camera mainCamera;
	private ShapeModule shapeModule;
	private EmissionModule emissionModule;
	private Vector3 scale = new(0f, 1f, 1f);
	private Vector3 pos = new(0f, 0f, 0f);

	private const float widthOffset = 1.5f;
	private const float heightOffset = 4f;
	
	[SerializeField] [Range (0f, 2f)] private float rainDensity;

	private void Start()
	{
		var particleSys = GetComponent<ParticleSystem>();
		shapeModule = particleSys.shape;
		emissionModule = particleSys.emission;
	}

	void Update()
	{
		var camHeight = mainCamera.orthographicSize;
		var camWidth = camHeight * mainCamera.aspect;

		var targetWidth = (camWidth * 2f) * widthOffset;
		scale.x = targetWidth;
		shapeModule.scale = scale;

		emissionModule.rateOverTime = targetWidth * rainDensity;

		pos.x = mainCamera.transform.position.x;
		pos.y = mainCamera.transform.position.y + (camHeight * 1.5f) + heightOffset;
		transform.position = pos;
	}
}