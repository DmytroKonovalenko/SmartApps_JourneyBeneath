using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMyCharacter : MonoBehaviour
{
    public void OpenMenu()
    {
        WindowManager.OpenWindow(WindowName.GW_GAME_SELECT);
    }

    public void OpenShop()
    {
        WindowManager.OpenWindow(WindowName.GW_GAME_SHOP);
    }
}
