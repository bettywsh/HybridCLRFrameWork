using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumHelper : MonoBehaviour
{
    public static bool CheckHasType(Enum flags, Enum type)
    {
        return CheckHasFlagsNum(Convert.ToInt32(flags), GetFlagsNum(type));
    }
    public static bool CheckHasFlags(Enum flagsA, Enum flagsB)
    {
        return CheckHasFlagsNum(Convert.ToInt32(flagsA), Convert.ToInt32(flagsB));
    }
    private static bool CheckHasFlagsNum(int flagsNumA, int flagsNumB)
    {
        return (flagsNumA & flagsNumB) == flagsNumB;
    }
    private static int GetFlagsNum(Enum type)
    {
        return 1 << Convert.ToInt32(type);
    }
}
