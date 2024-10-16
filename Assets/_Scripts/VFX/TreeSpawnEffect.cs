using Assets._Scripts.BaseInfos;
using System.Collections;
using UnityEngine;

public class TreeSpawnEffect : MonoBehaviour
{
	private Material material;
	[SerializeField]
	private float fadeDuration = 5f;
	private float defaultFadeAmount = 10f;

	void Start()
	{
		material = gameObject.GetComponent<Renderer>().material;

		material.SetFloat("_FadeAmount", -defaultFadeAmount);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constants.Tags.Player))
		{
			StartCoroutine(SpawnTree());
		}
	}

	public IEnumerator SpawnTree()
	{
		var elapsedTime = 0f;

		while (elapsedTime < fadeDuration)
		{
			material.SetFloat("_FadeAmount", Mathf.Lerp(-defaultFadeAmount, defaultFadeAmount, elapsedTime / fadeDuration));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}
}
