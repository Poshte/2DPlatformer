using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static List<Transform> GetAllChildren(this Transform tran)
    {
        var children = new List<Transform>();
        for (int i = 0; i < tran.childCount; i++)
        {
            children.Add(tran.GetChild(i));
        }
        return children;
    }
}
