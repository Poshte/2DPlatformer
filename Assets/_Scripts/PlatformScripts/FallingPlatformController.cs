using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatformController : RaycastController
{
    [SerializeField]
    private LayerMask playerMask;

    //falling
    [SerializeField]
    private float fallDelay;
    [SerializeField]
    private float respawnDelay;

    //colors
    private Renderer platformRenderer;

    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;
    [SerializeField]
    private float colorChangeSpeed;

    public override void Start()
    {
        base.Start();

        platformRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        UpdateRaycastOrigin();
        DetectPlayer();
    }

    public void DetectPlayer()
    {
        for (int i = 0; i < verticalRayCount; i++)
        {
            var rayOrigin = raycastOrigin.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpace * i);
            var hit = Physics2D.Raycast(rayOrigin, Vector2.up, skinWidth, playerMask);
            if (hit)
            {
                StartCoroutine(ChangeColor());
                Invoke(nameof(Fall), fallDelay);
            }
        }
    }
    public void Fall()
    {
        gameObject.SetActive(false);
        platformRenderer.material.color = startColor;
        Invoke(nameof(Respawn), respawnDelay);
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
    }

    public IEnumerator ChangeColor()
    {
        var tick = 0f;
        while (platformRenderer.material.color != endColor)
        {
            tick += Time.deltaTime * colorChangeSpeed;
            platformRenderer.material.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
    }
}
