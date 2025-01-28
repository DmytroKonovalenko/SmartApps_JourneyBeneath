using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupHint : MonoBehaviour
{
	public Text tHintAmount;

    public GameObject popupRewardVideo;

	void OnEnable ()
	{
		tHintAmount.text = GameDefine.hintCount.ToString ();
		GameManager.gameState = GameState.PAUSING;
	}

	void Start ()
	{
		GameDefine.instance.OnHintCountChange += this.OnHintCountChange;
	}

	void OnHintCountChange ()
	{
		tHintAmount.text = GameDefine.hintCount.ToString ();
	}

	public void Cancel ()
	{
		gameObject.SetActive (false);
		GameManager.gameState = GameState.PLAYING;
	}

	public void UseHint ()
	{
		if (GameDefine.hintCount > 0) {
			GameDefine.hintCount--;
			GW_GAME_PLAY.instance.Hint ();
			gameObject.SetActive (false);
			GameManager.gameState = GameState.PLAYING;
		}
        else
        {
            gameObject.SetActive(false);
            popupRewardVideo.gameObject.SetActive(true);
        }
	}
}
