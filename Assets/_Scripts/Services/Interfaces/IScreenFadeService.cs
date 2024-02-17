using System.Collections;
using UnityEngine;

public interface IScreenFadeService : IGameService
{
    IEnumerator Fade(CanvasGroup canvasGroup, float startAlpha, float targetAlpha);
}