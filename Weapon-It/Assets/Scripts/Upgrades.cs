using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [SerializeField]
    List<Upgrade> upgrades;

    public void UpdateUpgradesAppearance(Gun gun)
    {
        // Iterate through all upgrades
        foreach (Upgrade upgrade in upgrades)
        {
            // Check if has requierments
            if (upgrade.requirements.Length != 0)
            {
                bool meetsRequierments = true;

                foreach (Requirement req in upgrade.requirements)
                {
                    if (!MeetsRequirment(req,gun))
                        meetsRequierments = false;
                }

                if(meetsRequierments)
                    EnableOnlyUpgradeObj(upgrade);
                else
                    upgrade.obj.SetActive(false);
            }
            else
            {
                EnableOnlyUpgradeObj(upgrade);
            }
        }
    }

    public void UpdateUpgradesAppearance(Base _base)
    {
        // Iterate through all upgrades
        foreach (Upgrade upgrade in upgrades)
        {
            // Check if has requierments
            if (upgrade.requirements.Length != 0)
            {
                bool meetsRequierments = true;

                foreach (Requirement req in upgrade.requirements)
                {
                    if (!MeetsRequirment(req, _base))
                        meetsRequierments = false;
                }

                if (meetsRequierments)
                    EnableOnlyUpgradeObj(upgrade);
                else
                    upgrade.obj.SetActive(false);
            }
            else
            {
                EnableOnlyUpgradeObj(upgrade);
            }
        }
    }

    public void UpdateUpgradesAppearance(StoreUIManager sUIMan)
    {
        // Iterate through all upgrades
        foreach (Upgrade upgrade in upgrades)
        {
            // Check if has requierments
            if (upgrade.requirements.Length != 0)
            {
                bool meetsRequierments = true;

                foreach (Requirement req in upgrade.requirements)
                {
                    if (!MeetsRequirment(req, sUIMan))
                        meetsRequierments = false;
                }

                if (meetsRequierments)
                    EnableOnlyUpgradeObj(upgrade);
                else
                    upgrade.obj.SetActive(false);
            }
            else
            {
                EnableOnlyUpgradeObj(upgrade);
            }
        }
    }

    private static void EnableOnlyUpgradeObj(Upgrade upgrade)
    {
        foreach (Transform obj in upgrade.obj.transform.parent)
        {
            if (obj.gameObject != upgrade.obj)
                obj.gameObject.SetActive(false);
            else
                obj.gameObject.SetActive(true);
        }
    }

    private bool MeetsRequirment(Requirement req, Gun gun)
    {
        switch (req.type)
        {
            case Requirement.Type.Damage:
                return Precentage(
                    gun.MinDamage,
                    gun.MaxDamage,
                    gun.damage) >= req.precentageValue ? true : false;
            case Requirement.Type.FireRate:
                return Precentage(
                    gun.MinFireRate,
                    gun.MaxFireRate,
                    gun.fireRate) >= req.precentageValue ? true : false;
            case Requirement.Type.Range:
                return Precentage(
                    gun.MinRange,
                    gun.MaxRange,
                    gun.range) >= req.precentageValue ? true : false;
        }

        Debug.LogWarning("Theres no requirment - check the requirment tab");
        return false;
    }
    private bool MeetsRequirment(Requirement req, Base _base)
    {
        switch (req.type)
        {
            case Requirement.Type.Health:
                return Precentage(
                    _base.MinHealth,
                    _base.MaxHealth,
                    _base.health) >= req.precentageValue ? true : false;
        }

        Debug.LogWarning("Theres no requirment - check the requirment tab");
        return false;
    }
    private bool MeetsRequirment(Requirement req, StoreUIManager storeUIMan)
    {
        switch(req.type)
        {
            case Requirement.Type.Damage:
                return Precentage(
                    storeUIMan.damageBar.minValue,
                    storeUIMan.damageBar.maxValue,
                    storeUIMan.damageBar.value) >= req.precentageValue ? true : false;
            case Requirement.Type.FireRate:
                return Precentage(
                    storeUIMan.fireRateBar.minValue,
                    storeUIMan.fireRateBar.maxValue,
                    storeUIMan.fireRateBar.value) >= req.precentageValue ? true : false;
            case Requirement.Type.Range:
                return Precentage(
                    storeUIMan.rangeBar.minValue,
                    storeUIMan.rangeBar.maxValue,
                    storeUIMan.rangeBar.value) >= req.precentageValue ? true : false;
        }

        Debug.LogWarning("Theres no requirment - check the requirment tab");
        return false;
    }

    private float Precentage(float min,float max,float value)
    {
        return ((value - min) / (max - min)) * 100;
    }
}

[System.Serializable]
class Upgrade
{
    public string name;

    public GameObject obj;

    public Requirement[] requirements;
}

[System.Serializable]
class Requirement
{
    public enum Type {FireRate, Range, Damage ,Health}
    public Type type;

    [Range(0.1f,100)]
    public float precentageValue;
}
