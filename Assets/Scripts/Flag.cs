using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour, IItem
{
    public static event Action OnFlagCollect;
    public void Collect()
    {
        OnFlagCollect.Invoke();
    }
}
