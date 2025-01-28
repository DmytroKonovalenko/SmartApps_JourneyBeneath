using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPackItem : MonoBehaviour
{
	public TextMeshProUGUI tPackName, tPercentCompleted;
	public Image iProgress;
	LevelPackModule _levelPack;
    public Button button;
    public int id;

	public LevelPackModule levelPack {
		get { return _levelPack; }
		set { 
			_levelPack = value;
			tPackName.text = value.packType.ToString ();
			int levelPassed = 0;
			switch (value.packType) {
			case PackType.Beginner:
				levelPassed = GameManager.dataSaveDict [GameManager.currentGameName].beginner.Count;
				break;
			case PackType.Medium:
				levelPassed = GameManager.dataSaveDict [GameManager.currentGameName].medium.Count;
				break;
			case PackType.Expert:
				levelPassed = GameManager.dataSaveDict [GameManager.currentGameName].expert.Count;
				break;
			case PackType.Master:
				levelPassed = GameManager.dataSaveDict [GameManager.currentGameName].master.Count;
				break;
			}
			float progress = (float)levelPassed / value.LevelsCount;
			iProgress.fillAmount = progress;
			tPercentCompleted.text = Mathf.RoundToInt (progress * 100).ToString () + "%";
		}
	}

    void Awake()
    {
        if (button == null)
        {
            button = GetComponentInChildren<Button>();
        }
    }
    void OnEnable ()
	{
		
	}

	public void GotoSelectLevel ()
	{
		GameManager.currentPackType = levelPack.packType;
		GameManager.currentPackName = tPackName.text;
		GameManager.currentPackModule = levelPack;
		GameManager.gameState = GameState.SELECT_LEVEL;
		WindowManager.OpenWindow (WindowName.GW_LEVEL_SELECT);
	}


}
