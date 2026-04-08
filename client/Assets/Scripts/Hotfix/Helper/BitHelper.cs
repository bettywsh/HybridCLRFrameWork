using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BitHelper
{
   
    //bitPosition 0开始
    public static bool CheckState(int currentState, int bitPosition)
    {
        return (currentState & (1 << (bitPosition))) != 0;
    }

    //bitPosition 1开始
    public static int SetState(int RewardIndex, int bitPosition)
    {
        return RewardIndex | (1 << (bitPosition - 1));
    }

    public static int SetStateZero(int RewardIndex, int bitPosition)
    {
        return RewardIndex & ~(1 << (bitPosition - 1));
    }
}
