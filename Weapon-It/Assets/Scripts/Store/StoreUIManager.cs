using UnityEngine;
using UnityEngine.UI;

public class StoreUIManager : MonoBehaviour
{
    #region Main UI Variables

    public GameObject weaponsWindow;
    public GameObject charactersWindow;
    public GameObject basesWindow;

    public GameObject mainStoreUI, weaponsUpgradesUI, baseUpgradesUI;

    public GameObject weaponUpgradesButton;
    public GameObject baseUpgradeButton;

    public GameObject buyOrEquipItemButton;

    public Text coinsIndiactor;

    #endregion

    #region Weapon Upgrades Editor Variables

    [Header("Weapon Upgrades Varaibles")]
    public ParameterBar damageBar;
    public ParameterBar fireRateBar;
    public ParameterBar rangeBar;
    public Text weaponUpgradesCostIndicator;

    #endregion

    #region Base Upgrades Editor Variables

    [Header("Base Upgrades Varaibles")]
    public ParameterBar healthBar;
    public Text baseUpgradesCostIndicator;

    #endregion

    [HideInInspector]
    public Transform currentWindow;

    [HideInInspector]
    public int currentItemNum;

    [HideInInspector]
    public StoreManager sManager;

    public void Init()
    {
        OpenItemWindow("Character");

        GetCurrentItemNum();

        damageBar.sManager = sManager;
        fireRateBar.sManager = sManager;
        rangeBar.sManager = sManager;
        healthBar.sManager = sManager;

        coinsIndiactor.text = sManager.coins.ToString();
    }

    public void OpenItemWindow(string windowTypeName)
    {
        // Open wanted window - need to check for each item type.
        if (windowTypeName == StoreManager.ItemTypes.Weapon.ToString())
        {
            currentWindow = weaponsWindow.transform;
            weaponsWindow.SetActive(true);
            charactersWindow.SetActive(false);
            basesWindow.SetActive(false);

            GetCurrentItemNum();
        }
        else if (windowTypeName == StoreManager.ItemTypes.Character.ToString())
        {
            currentWindow = charactersWindow.transform;
            charactersWindow.SetActive(true);
            weaponsWindow.SetActive(false);
            basesWindow.SetActive(false);

            GetCurrentItemNum();
        }
        else if (windowTypeName == StoreManager.ItemTypes.Base.ToString())
        {
            currentWindow = basesWindow.transform;
            basesWindow.SetActive(true);
            charactersWindow.SetActive(false);
            weaponsWindow.SetActive(false);

            GetCurrentItemNum();
        }
        else
            Debug.LogWarning("Misspelled the type name! check button string.");

        // Update Current item
        sManager.currentItem =
            currentWindow.transform.GetChild(currentItemNum).GetComponent<StoreItem>();
        UpdateBuyOrEquipButton(sManager.currentItem.equipped, sManager.currentItem.bought);

        UpdateMainUI(sManager.currentItem);
    }

    private void GetCurrentItemNum()
    {
        if (currentWindow.childCount == 0)
        {
            Debug.LogWarning("No items in window");
            return;
        }

        bool once = false; // used to check if there is more than 1 active object
        for (int i = 0; i < currentWindow.childCount; i++)
        {
            if (currentWindow.GetChild(i).gameObject.activeSelf)
            {
                if (once)
                {
                    Debug.LogWarning("More than 1 active item - check your window!");
                    return;
                }

                currentItemNum = i;
                once = true;
            }
        }
    }

    public void UpdateMainUI(StoreItem currentItem)
    {
        UpdateWeaponUpgradeButton(currentItem);
        UpdateBaseUpgradeButton(currentItem);
        UpdateBuyOrEquipButton(currentItem.equipped, currentItem.bought);
    }

    private void UpdateWeaponUpgradeButton(StoreItem currentItem)
    {
        if (currentItem.type != StoreManager.ItemTypes.Weapon)
            weaponUpgradesButton.SetActive(false);
        else
        {
            if (currentItem.bought)
                weaponUpgradesButton.SetActive(true);
            else
                weaponUpgradesButton.SetActive(false);
        }
    }

    private void UpdateBaseUpgradeButton(StoreItem currentItem)
    {
        if (currentItem.type != StoreManager.ItemTypes.Base)
            baseUpgradeButton.SetActive(false);
        else
        {
            if (currentItem.bought)
                baseUpgradeButton.SetActive(true);
            else
                baseUpgradeButton.SetActive(false);
        }
    }

    private void UpdateBuyOrEquipButton(bool equipedd,bool bought)
    {
        if (equipedd)
        {
            buyOrEquipItemButton.transform.GetChild(0).gameObject.SetActive(false);
            buyOrEquipItemButton.transform.GetChild(1).gameObject.SetActive(false);
            buyOrEquipItemButton.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (bought)
        {
            buyOrEquipItemButton.transform.GetChild(0).gameObject.SetActive(false);
            buyOrEquipItemButton.transform.GetChild(1).gameObject.SetActive(true);
            buyOrEquipItemButton.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            buyOrEquipItemButton.transform.GetChild(0).gameObject.SetActive(true);
            buyOrEquipItemButton.transform.GetChild(1).gameObject.SetActive(false);
            buyOrEquipItemButton.transform.GetChild(2).gameObject.SetActive(false);

            buyOrEquipItemButton.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = 
                sManager.currentItem.cost.ToString();
        }
    }

    void UpdateBar(ParameterBar bar, float minValue, float maxValue, float currentValue)
    {
        bar.minValue = minValue;
        bar.maxValue = maxValue;
        bar.value = currentValue;

        bar.UpdateBar();
    }


    // Used by the Parametere bars to update costs and appearance
    public void UpdateUpgradeCostAndAppearance()
    {
        if (sManager.currentItem.type == StoreManager.ItemTypes.Weapon)
            UpdateWeaponUpgradeCostsAndAppearance();

        else if (sManager.currentItem.type == StoreManager.ItemTypes.Base)
            UpdateBaseUpgradeCostsAndAppearance();
    }

    #region Weapon Upgrades Methods

    public void OpenWeaponUpgrades()
    {
        weaponsUpgradesUI.SetActive(true);
        mainStoreUI.SetActive(false);

        SetWeaponsBarsBaseParameters();

        UpdateWeaponUpgradeMenu();
    }

    public void CloseWeaponUpgrades()
    {
        weaponsUpgradesUI.SetActive(false);
        mainStoreUI.SetActive(true);

        sManager.currentItem.GetComponent<Upgrades>().
            UpdateUpgradesAppearance(sManager.currentItem.GetComponent<Gun>());
    }

    public void UpdateWeaponUpgradeMenu()
    {
        Gun currentGun = sManager.currentItem.GetComponent<Gun>();

        UpdateBar(damageBar, currentGun.MinDamage, currentGun.MaxDamage, currentGun.damage);
        UpdateBar(fireRateBar, currentGun.MinFireRate, currentGun.MaxFireRate, currentGun.fireRate);
        UpdateBar(rangeBar, currentGun.MinRange, currentGun.MaxRange, currentGun.range);

        coinsIndiactor.text = sManager.coins.ToString();

        UpdateWeaponUpgradeCostsAndAppearance();
    }

    void UpdateWeaponUpgradeCostsAndAppearance()
    {
        float upgradesCost = sManager.CalculateWeaponUpgradeCosts();

        // Determine if has enough money and show that on the cost indicator
        weaponUpgradesCostIndicator.text = upgradesCost.ToString();

        if (sManager.coins > upgradesCost) // Has enough money
            weaponUpgradesCostIndicator.color = Color.black;
        else
            weaponUpgradesCostIndicator.color = Color.red;

        sManager.currentItem.GetComponent<Upgrades>().UpdateUpgradesAppearance(this);
    }

    public void SetWeaponsBarsBaseParameters()
    {
        Gun currentGun = sManager.currentItem.GetComponent<Gun>();

        damageBar.baseValue = currentGun.damage;
        damageBar.logMultilpier = currentGun.damageLogMultilpier;
        damageBar.upgradeCount = currentGun.damageUpgradeCount;


        rangeBar.baseValue = currentGun.range;
        rangeBar.logMultilpier = currentGun.rangeLogMultilpier;
        rangeBar.upgradeCount = currentGun.rangeUpgradeCount;

        fireRateBar.baseValue = currentGun.fireRate;
        fireRateBar.logMultilpier = currentGun.fireRateLogMultilpier;
        fireRateBar.upgradeCount = currentGun.fireRateUpgradeCount;
    }

    #endregion

    #region Base Upgrades Methods

    public void OpenBaseUpgrades()
    {
        baseUpgradesUI.SetActive(true);
        mainStoreUI.SetActive(false);

        SetBaseBarsBaseParameters();

        UpdateBaseUpgradesMenu();
    }

    public void CloseBaseUpgrades()
    {
        baseUpgradesUI.SetActive(false);
        mainStoreUI.SetActive(true);
    }

    public void UpdateBaseUpgradesMenu()
    {
        Base currentBase = sManager.currentItem.GetComponent<Base>();

        UpdateBar(healthBar, currentBase.MinHealth, currentBase.MaxHealth, currentBase.health);

        coinsIndiactor.text = sManager.coins.ToString();

        UpdateBaseUpgradeCostsAndAppearance();
    }

    void UpdateBaseUpgradeCostsAndAppearance()
    {
        float upgradesCost = sManager.CalculateBaseUpgradeCosts();

        // Determine if has enough money and show that on the cost indicator
        baseUpgradesCostIndicator.text = upgradesCost.ToString();

        if (sManager.coins > upgradesCost) // Has enough money
            baseUpgradesCostIndicator.color = Color.black;
        else
            baseUpgradesCostIndicator.color = Color.red;
    }

    public void SetBaseBarsBaseParameters()
    {
        Base currentBase = sManager.currentItem.GetComponent<Base>();

        healthBar.baseValue = currentBase.health;
        healthBar.logMultilpier = currentBase.healthLogMultiplier;
        healthBar.upgradeCount = currentBase.healthUpgradeCount;

        Debug.Log(currentBase.healthUpgradeCount);
    }
    #endregion
}
