using System.Collections;
using UnityEngine;

public class ScreenFadeService : IScreenFadeService
{
    [SerializeField]
    private float fadeDuration = 2.5f;

    public IEnumerator Fade(CanvasGroup canvasGroup, float startAlpha, float targetAlpha)
    {
        canvasGroup.alpha = startAlpha;

        var elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
