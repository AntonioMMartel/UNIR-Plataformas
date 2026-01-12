using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour, IItem
{
    public static event Action<int> OnCoinCollect;
    [SerializeField] int points = 5;
    public void Collect()
    {
        OnCoinCollect.Invoke(points); 
        Destroy(gameObject);
    }
}
