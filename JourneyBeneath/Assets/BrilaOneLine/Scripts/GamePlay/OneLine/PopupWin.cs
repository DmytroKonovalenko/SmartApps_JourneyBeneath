﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : MonoBehaviour
{
	
	public CanvasGroup canvasGroup;

	public Button bShowBoard, bHideBoard, bNext;

	public Image iProgress;
	bool _isShowBoard;

	public Button  bMenu, bReplay, bHint;

	
	public bool isShowBoard {
		get { return _isShowBoard; }
		set { 
			_isShowBoard = value;
			if (value) {
				bShowBoard.gameObject.SetActive (false);
				canvasGroup.alpha = 1;
				bMenu.gameObject.SetActive (true);
			} else {
				canvasGroup.alpha = 0;
				bShowBoard.gameObject.SetActive (true);
				bMenu.gameObject.SetActive (false);
			}
		}
	}

	void OnEnable ()
	{
		
		isShowBoard = true;
		bReplay.gameObject.SetActive (false);
		bHint.gameObject.SetActive (false);
		
		switch (GameManager.currentPackType) {
		case PackType.Beginner:
			if (!GameManager.dataSaveDict [GameManager.currentGameName].beginner.Contains (GameManager.currentLevel)) {
				GameManager.dataSaveDict [GameManager.currentGameName].beginner.Add (GameManager.currentLevel);
			}
			break;
		case PackType.Medium:
			if (!GameManager.dataSaveDict [GameManager.currentGameName].medium.Contains (GameManager.currentLevel)) {
				GameManager.dataSaveDict [GameManager.currentGameName].medium.Add (GameManager.currentLevel);
			}
			break;
		case PackType.Expert:
			if (!GameManager.dataSaveDict [GameManager.currentGameName].expert.Contains (GameManager.currentLevel)) {
				GameManager.dataSaveDict [GameManager.currentGameName].expert.Add (GameManager.currentLevel);
			}
			break;
		case PackType.Master:
			if (!GameManager.dataSaveDict [GameManager.currentGameName].master.Contains (GameManager.currentLevel)) {
				GameManager.dataSaveDict [GameManager.currentGameName].master.Add (GameManager.currentLevel);
			}
			break;
		}
        CoinsController.Instance.AddCoins(10);
        GameManager.SaveData ();
        StartCoroutine(ShowAds());
	}

    IEnumerator ShowAds()
    {
        yield return new WaitForSeconds(0.5f);

    }

	void OnDisable ()
	{
		bReplay.gameObject.SetActive (true);
		bHint.gameObject.SetActive (true);

	}

	public void HideBoard ()
	{
		isShowBoard = false;
	}

	public void ShowBoard ()
	{
		isShowBoard = true;
	}

	public void Next ()
	{
		GameManager.currentLevel++;
        OneLineGameplayControl.instance.StartNewGame ();
		gameObject.SetActive (false);
        
    }

	public void SelectLevel ()
	{
		
	}
    

}
