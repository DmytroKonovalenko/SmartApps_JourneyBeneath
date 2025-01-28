using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectButton : MonoBehaviour
{
	public Vector2 offset;
	public RectTransform rectTrans;
	[SerializeField]
	int _id;

	public int id {
		get { return _id; }
		set {
			_id = value;
			gameType = GameDefine.instance.gameTypeConfig.listGames [value];
		}
	}

	public Image iGameLogo;
	public Text tGameName;
	public Image iProgress;
	public Text tLevelPassed;

	int totalLevel;
	DataSaveModule dataSave;

	GameTypeModule _gameType;

	public GameTypeModule gameType {
		get { return _gameType; }
		set {
			_gameType = value;
			iGameLogo.sprite = value.gameLogo;
			tGameName.text = value.gameNameShow;
			dataSave = GameManager.dataSaveDict [value.gameName];
			int totalLevelPassed = dataSave.beginner.Count + dataSave.expert.Count + dataSave.medium.Count + dataSave.master.Count;
			totalLevel = 0;
			for (int i = 0; i < value.levelPacks.Length; i++) {
				totalLevel += value.levelPacks [i].LevelsCount;
			}

			iProgress.fillAmount = (float)totalLevelPassed / totalLevel;
			tLevelPassed.text = totalLevelPassed + "/" + totalLevel;
		}
	}

	void Awake ()
	{
		rectTrans = GetComponent<RectTransform> ();
	}

	void OnEnable ()
	{
		gameType = GameDefine.instance.gameTypeConfig.listGames [id];
		
	}

	public void OnClick ()
	{
		if (GW_GAME_SELECT.instance.canClickButtonGameType) {
			GameManager.currentGameType = gameType;
			GameManager.currentGameName = gameType.gameName;
			GameManager.gameState = GameState.SELECT_PACK;
			GameManager.LoadGameData (GameManager.currentGameName);

			WindowManager.OpenWindow (WindowName.GW_LEVEL_PACK);
		}
	}
}
