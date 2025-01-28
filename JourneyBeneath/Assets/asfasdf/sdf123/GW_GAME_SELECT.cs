using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GW_GAME_SELECT : MonoBehaviour
{
	public static GW_GAME_SELECT instance;

	void Awake()
	{
		instance = this;
	}


	public List<GameSelectButton> listGameSelectButtons;

	int maxID = 6;

	public void GoToLevelPacksSelect()
	{
		GameManager.LoadGameData(GameManager.currentGameName);
		GameManager.gameState = GameState.SELECT_PACK;
		WindowManager.OpenWindow(WindowName.GW_LEVEL_PACK);
	}

	void Start()
	{

		maxID = GameDefine.instance.gameTypeConfig.listGames.Count;

		for (int i = 0; i < listGameSelectButtons.Count; i++)
		{

			if (i - 1 >= 0)
			{
				listGameSelectButtons[i].id = i - 1;
			}
			else
			{
				listGameSelectButtons[i].id = maxID - 1;
			}

		}

	}

	//
	
	Vector3 mousePos;
	Vector2 mousePos2D;


	Vector2 posTemp;


	Vector2 lastPosTemp;
	int idTemp;
	Vector2 initMovePos;
	MoveDir mvDir;
	public bool canClickButtonGameType = true;

	public void OpenSetting()
	{
		WindowManager.OpenWindow(WindowName.GW_GAME_SETTING);
	}
	public void OpenMyCharacter()
	{
		WindowManager.OpenWindow(WindowName.GW_GAME_CHARACTER);
	}




	enum MoveDir
	{
		Left,
		Right
	}
}