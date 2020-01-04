using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [SerializeField]
    List<Upgrade> upgrades;
}

[System.Serializable]
class Upgrade
{
    public string name;

    public GameObject obj;

    [SerializeField]
    Requirement[] requirements;
}

[System.Serializable]
class Requirement
{
    public enum Type {FireRate, Range, Damage }
    public Type type;

    [Range(0.1f,50)]
    public float precentageValue;
}
