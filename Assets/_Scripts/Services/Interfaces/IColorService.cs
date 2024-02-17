using System.Collections;
using UnityEngine;

public interface IColorService : IGameService
{
    IEnumerator ChangeColor(Renderer renderer, Color startColor, Color endColor, float colorChangeSpeed);
}