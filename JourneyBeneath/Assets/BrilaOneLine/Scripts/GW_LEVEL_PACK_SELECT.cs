using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GW_LEVEL_PACK_SELECT : MonoBehaviour
{
	public List<LevelPackItem> listLevelPackItem;


    void OnEnable()
    {
        for (int i = 0; i < listLevelPackItem.Count; i++)
        {
            LevelPackModule lvPack = GameManager.GetGameConfigByName(GameManager.currentGameName).levelPacks[i];
            listLevelPackItem[i].id = i;
            listLevelPackItem[i].levelPack = lvPack;

           
            if (i == 0)
            {
                listLevelPackItem[i].button.interactable = true;
            }
            else
            {
                
                float prevPackProgress = GetPackProgress(listLevelPackItem[i - 1].levelPack);
                listLevelPackItem[i].button.interactable = prevPackProgress >= 1.0f; 
            }
        }
    }
    float GetPackProgress(LevelPackModule levelPack)
    {
        int levelPassed = 0;
        switch (levelPack.packType)
        {
            case PackType.Beginner:
                levelPassed = GameManager.dataSaveDict[GameManager.currentGameName].beginner.Count;
                break;
            case PackType.Medium:
                levelPassed = GameManager.dataSaveDict[GameManager.currentGameName].medium.Count;
                break;
            case PackType.Expert:
                levelPassed = GameManager.dataSaveDict[GameManager.currentGameName].expert.Count;
                break;
            case PackType.Master:
                levelPassed = GameManager.dataSaveDict[GameManager.currentGameName].master.Count;
                break;
        }

        return (float)levelPassed / levelPack.LevelsCount; 
    }

    public void BackToGameSelect ()
	{
		GameManager.gameState = GameState.SELECT_GAME;
		WindowManager.OpenWindow (WindowName.GW_GAME_SELECT);
	}
}
