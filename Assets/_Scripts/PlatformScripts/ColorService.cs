using System.Collections;
using UnityEngine;

public class ColorService
{
	public IEnumerator ChangeColor(Renderer renderer, Color startColor, Color endColor, float colorChangeSpeed)
	{
		var tick = 0f;
		while (renderer.material.color != endColor)
		{
			tick += Time.deltaTime * colorChangeSpeed;
			renderer.material.color = Color.Lerp(startColor, endColor, tick);
			yield return null;
		}
	}
}
