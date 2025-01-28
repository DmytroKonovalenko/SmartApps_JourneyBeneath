using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSetting : MonoBehaviour
{
    public void OpenMenu()
    {
        WindowManager.OpenWindow(WindowName.GW_GAME_SELECT);
    }
}
