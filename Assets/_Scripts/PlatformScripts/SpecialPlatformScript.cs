using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPlatformScript : MovingPlatformController
{
    [SerializeField]
    private LayerMask playerMask;

    public bool isPlayerOnTop;


    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
    }


    private void DetectPlayer()
    {
        for (int i = 0; i < verticalRayCount; i++)
        {
            var rayOrigin = raycastOrigin.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpace * i);

            var hit = Physics2D.Raycast(rayOrigin, Vector2.up, skinWidth, playerMask);
            if (hit)
            {
                EnableMove();
            }
        }
    }
    private void EnableMove()
    {
        isPlayerOnTop = true;
    }


}
