using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PersistentData
{
    public Player.Data player = new();
    public ItemStack.Data[] stacks = Array.Empty<ItemStack.Data>();
    public Dictionary<string, bool> bools = new();

    public static Vector2Int ArrayToVector2Int(int[] arr)
    {
        return new Vector2Int(arr[0], arr[1]);
    }

    public static int[] Vector2IntToArr(Vector2Int vector)
    {
        return new[] { vector.x, vector.y, };
    }

    public static Vector3 ArrayToVector3(float[] arr)
    {
        return new Vector3(arr[0], arr[1], arr[2]);
    }

    public static float[] Vector3ToArr(Vector3 vector)
    {
        return new[] { vector.x, vector.y, vector.z, };
    }
}