using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerDmgBuff 
{
    public float Multiplier;
    public float Duration;

    public void DamageBuff(float multiplier, float duration)
    {
        Multiplier = multiplier;
        Duration = duration;
    }
}
