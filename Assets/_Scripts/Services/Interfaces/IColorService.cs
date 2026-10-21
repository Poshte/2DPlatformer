using System.Collections;
using UnityEngine;

public interface IColorService : IGameService
{
    IEnumerator ChangeColor(SpriteRenderer renderer, Color startColor, Color endColor, float colorChangeSpeed);
}