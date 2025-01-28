using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public Button itemButton;           // ������ ��� �����/�������� ������
    public GameObject priceObject;       // ��'��� � �����
    public GameObject equippedObject;    // ��'���, �� ������, �� ����� ����������
    public int price;                    // ֳ�� ������
    public Sprite itemSprite;            // ������ ������
    public bool isPurchased = false;     // �� ������� �����
}