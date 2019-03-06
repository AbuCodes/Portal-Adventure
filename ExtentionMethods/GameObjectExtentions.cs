using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtentions
{
    public static GameObject GetMyChild (this GameObject inside, string wanted, bool recursive = false)
    {
        foreach (Transform child in inside.transform)
        {
            if (child.tag == wanted) return child.gameObject;
            if (recursive)
            {
                var within = GetMyChild(child.gameObject, wanted, true);
                if (within) return within;
            }
        }
        return null;
    }
}
