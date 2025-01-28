using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public Button itemButton;           // Кнопка для купівлі/екіпіровки товару
    public GameObject priceObject;       // Об'єкт з ціною
    public GameObject equippedObject;    // Об'єкт, що показує, що товар екіпірований
    public int price;                    // Ціна товару
    public Sprite itemSprite;            // Спрайт товару
    public bool isPurchased = false;     // Чи куплено товар
}