using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public int cost;

    public StoreManager.ItemTypes type;

    public bool bought;
    public bool equipped;

    [Header("Gun Variables")]
    public float baseCost;
    public float costMultiplier;
}
