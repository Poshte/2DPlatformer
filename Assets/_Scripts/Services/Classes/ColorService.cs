using System.Collections;
using UnityEngine;

public class ColorService : IColorService
{
	public IEnumerator ChangeColor(SpriteRenderer renderer, Color startColor, Color endColor, float colorChangeSpeed)
	{
		var tick = 0f;
		while (renderer.color != endColor)
		{
			tick += Time.deltaTime * colorChangeSpeed;
			renderer.color = Color.Lerp(startColor, endColor, tick);
			yield return null;
		}
	}
}