using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();
    [SerializeField] private Image equippedItemImage1;
    [SerializeField] private GameObject equippedItemContainer1;
    [SerializeField] private Image equippedItemImage2;
    [SerializeField] private GameObject equippedItemContainer2;
    [SerializeField] private Image equippedItemImage3;
    [SerializeField] private GameObject equippedItemContainer3;

    private ShopItem equippedItem;

    private void Start()
    {
        CoinsController.Instance.OnCoinsChanged += UpdateShopButtons;
        LoadShopData();
        UpdateShopButtons();
    }
    public void GoToMyCharacter()
    {
        WindowManager.OpenWindow(WindowName.GW_GAME_CHARACTER);
    }
    private void OnDestroy()
    {
        if (CoinsController.Instance != null)
        {
            CoinsController.Instance.OnCoinsChanged -= UpdateShopButtons;
        }
    }

    private void UpdateShopButtons()
    {
        foreach (var item in shopItems)
        {
            item.itemButton.interactable = item.isPurchased || CoinsController.Instance.GetCoins() >= item.price;
            item.priceObject.SetActive(!item.isPurchased);
            item.equippedObject.SetActive(item == equippedItem);
        }
    }

    public void OnItemButtonClicked(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= shopItems.Count)
        {
            Debug.LogError("Invalid item index.");
            return;
        }

        ShopItem item = shopItems[itemIndex];

        if (!item.isPurchased)
        {
            if (CoinsController.Instance.SpendCoins(item.price))
            {
                item.isPurchased = true;
                SaveShopData();
                UpdateShopButtons();
            }
        }
        else
        {
            EquipItem(item);
        }
    }

    private void EquipItem(ShopItem item)
    {
        if (equippedItem != null)
        {
            equippedItem.equippedObject.SetActive(false);
        }

        equippedItem = item;
        equippedItem.equippedObject.SetActive(true);

        equippedItemImage1.sprite = item.itemSprite;
        equippedItemImage2.sprite = item.itemSprite;
        equippedItemImage3.sprite = item.itemSprite;

        // Активуємо контейнери екіпіровки
        equippedItemContainer1.SetActive(true);
        equippedItemContainer2.SetActive(true);
        equippedItemContainer3.SetActive(true);

        SaveShopData();
    }

    public void UnequipItem()
    {
        if (equippedItem != null)
        {
            equippedItem.equippedObject.SetActive(false);
            equippedItem = null;

          
            equippedItemContainer1.SetActive(false);
            equippedItemContainer2.SetActive(false);
            equippedItemContainer3.SetActive(false);

            equippedItemImage1.sprite = null;
            equippedItemImage2.sprite = null;
            equippedItemImage3.sprite = null;

            SaveShopData();
        }
    }

    private void SaveShopData()
    {
        foreach (var item in shopItems)
        {
            PlayerPrefs.SetInt($"ShopItem_{item.itemButton.name}_Purchased", item.isPurchased ? 1 : 0);
        }

        PlayerPrefs.SetString("EquippedItem", equippedItem != null ? equippedItem.itemButton.name : "");
        PlayerPrefs.SetInt("EquippedContainersActive", equippedItem != null ? 1 : 0); 
        PlayerPrefs.Save();
    }

    public void LoadShopData()
    {
        foreach (var item in shopItems)
        {
            item.isPurchased = PlayerPrefs.GetInt($"ShopItem_{item.itemButton.name}_Purchased", 0) == 1;
        }

        string equippedItemName = PlayerPrefs.GetString("EquippedItem", "");
        equippedItem = shopItems.Find(i => i.itemButton.name == equippedItemName);

        bool containersActive = PlayerPrefs.GetInt("EquippedContainersActive", 0) == 1;
        equippedItemContainer1.SetActive(containersActive);
        equippedItemContainer2.SetActive(containersActive);
        equippedItemContainer3.SetActive(containersActive);

        if (equippedItem != null)
        {
            equippedItemImage1.sprite = equippedItem.itemSprite;
            equippedItemImage2.sprite = equippedItem.itemSprite;
            equippedItemImage3.sprite = equippedItem.itemSprite;
            equippedItem.equippedObject.SetActive(true);
        }
    }
}
