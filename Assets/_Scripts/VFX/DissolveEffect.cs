using Assets._Scripts.BaseInfos;
using System.Collections;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
	private Material material;

	[SerializeField]
	private float fadeDuration = 3f;

	void Start()
	{
		material = gameObject.GetComponent<Renderer>().material;
		material.SetFloat("_FadeAmount", 2);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constants.Tags.Player))
		{
			StartCoroutine(PlayDissolve());
		}
	}

	public IEnumerator PlayDissolve()
	{
		var elapsedTime = 0f;
		while (elapsedTime < fadeDuration)
		{
			material.SetFloat("_FadeAmount", Mathf.Lerp(1, -1, elapsedTime / fadeDuration));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}
}
