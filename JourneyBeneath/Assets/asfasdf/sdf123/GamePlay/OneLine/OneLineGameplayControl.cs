using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class OneLineGameplayControl : MonoBehaviour
{
	
	public List<AFasdcvxc> listBlocks;	
	public GameObject blockPrefap;	
	public AFasdcvxc currentBlock;
	public static OneLineGameplayControl instance;
	public bool isDraging = false;
	public GameObject finger;

	public List<GameElementColorType> listBlockColorType;
	
	public GameElementColorType currentBlockColorType;
	[Space (10)]
	[Header ("Use to create new levels")]
	
	[Tooltip ("Set true để bật chế độ tạo level cho level đang chọn")]
	public bool isLevelEditor = false;
	
	[Header ("Board size in LevelEditor Mode")]
	[Range (4, 10)]
	public int boardW;
	[Range (4, 10)]
	public int boardH;

	void Awake ()
	{

		GameObject blockPools = new GameObject ();	
		SimplePool.Preload (blockPrefap, 100, blockPools.transform);
		instance = this;

		hintList = new List<AFasdcvxc> ();
	
		listBlockColorType = new List<GameElementColorType> ();
		listBlockColorType.Add (GameElementColorType.BEGINNER_COLOR);
		listBlockColorType.Add (GameElementColorType.MEDIUM_COLOR);
		listBlockColorType.Add (GameElementColorType.EXPERT_COLOR);
		listBlockColorType.Add (GameElementColorType.MASTER_COLOR);
	}

	void OnDisable ()
	{
	
		if (finger != null && finger.activeInHierarchy) {
			HideFinger ();
		}

		if (listBlocks != null) {
			foreach (AFasdcvxc bl in listBlocks) {
				if (bl != null) {
					
					SimplePool.Despawn (bl.gameObject);
				}
			}
			listBlocks.Clear ();
		}
		CancelInvoke ();
	}

	List<AFasdcvxc> hintList;

	public void StartNewGame ()
	{

		listBlockColorType.Shuffle ();
		currentBlockColorType = listBlockColorType [0];

		finger.SetActive (false);
		hintList.Clear ();
		hintCount = 0;
		isDraging = false;
	
		if (listBlocks != null) {
			foreach (AFasdcvxc bl in listBlocks) {

				SimplePool.Despawn (bl.gameObject);
			}
			listBlocks.Clear ();
		} else {
			listBlocks = new List<AFasdcvxc> ();
		}

		string levelData = GameManager.LoadLevel (GameManager.currentPackName, GameManager.currentLevel);
		if (levelData == "") {
			GW_GAME_PLAY.instance.Invoke ("SelectPack", 0.2F);
		}
		string[] dataChars = levelData.Split (new char[]{ ',' });

		int w = int.Parse (dataChars [0]);

		int h = int.Parse (dataChars [1]);
	
		for (int x = 0; x < h; x++) {
			for (int y = 0; y < w; y++) {
				Vector3 pos = new Vector3 (-(float)w / 2 + 0.5F + y, (float)h / 2 - 0.5F - x);

				GameObject blObj = SimplePool.Spawn (blockPrefap, pos, Quaternion.identity);
                AFasdcvxc bl = blObj.GetComponent<AFasdcvxc> ();
				bl.id = x * w + y;
				bl.name = "block_" + x + "_" + y;
				listBlocks.Add (bl);
				bl.isWall = true;
				bl.isFiled = false;
				bl.nextBlock = null;
			}
		}
		for (int i = 2; i < dataChars.Length; i++) {

            AFasdcvxc bl = listBlocks [int.Parse (dataChars [i])];
			bl.isWall = false;
			hintList.Add (bl);
			if (i == 2) {
				bl.isFiled = true;
				currentBlock = bl;
				bl.startPosShowAni.tween.Restart (true);
				bl.startBlockDot.SetActive (true);
			}
		}

		CalcCamera (w, h);

		GW_GAME_PLAY.instance.InitInfo ();
 
		if (GameManager.currentLevel == 1 && GameManager.currentPackType == PackType.Beginner) {
			ShowFinger ();
		}
		GameManager.gameState = GameState.PLAYING;
	}

	void CalcCamera (int col, int row)
	{

		if (col >= row) {

			Camera.main.orthographicSize = col * Camera.main.pixelHeight / (Camera.main.pixelWidth * 2) + 1F;
		} else {

			Camera.main.orthographicSize = row;
		}
	}
	public 	void CreateLevelEditorBoard (int w, int h)
	{
		if (listBlocks != null) {
			foreach (AFasdcvxc bl in listBlocks) {

				SimplePool.Despawn (bl.gameObject);
			}
			listBlocks.Clear ();
		} else {
			listBlocks = new List<AFasdcvxc> ();
		}
		listBlockColorType.Shuffle ();
		currentBlockColorType = listBlockColorType [0];
		if (levelEditListBlockID != null) {
			levelEditListBlockID.Clear ();
		} else {
			levelEditListBlockID = new List<int> ();
		}
		boardW = w;
		boardH = h;
		for (int x = 0; x < h; x++) {
			for (int y = 0; y < w; y++) {
				Vector3 pos = new Vector3 (-(float)w / 2 + 0.5F + y, (float)h / 2 - 0.5F - x);
				GameObject blObj = SimplePool.Spawn (blockPrefap, pos, Quaternion.identity);
                AFasdcvxc bl = blObj.GetComponent<AFasdcvxc> ();
				bl.nextBlock = null;
				bl.isFiled = false;
				bl.id = x * w + y;
				bl.name = "block_" + x + "_" + y;
				listBlocks.Add (bl);
				bl.isWall = false;
			}
		}
		CalcCamera (boardW, boardH);

	}

	public void BuildLevel (string text)
	{
	
		#if UNITY_EDITOR
		string fileName = "Level_" + GameManager.currentLevel + ".txt";
		string directory = "/Resources/Data/" + GameManager.currentGameName + "/" + GameManager.currentPackName + "/";
		if (!AssetDatabase.IsValidFolder ("Assets/Resources/LevelData")) {
			AssetDatabase.CreateFolder ("Assets/Resources", "LevelData");
			File.WriteAllText (Application.dataPath + directory + fileName, text);
			AssetDatabase.Refresh ();
		} else {
			File.WriteAllText (Application.dataPath + directory + fileName, text);
			AssetDatabase.Refresh ();
		}
		#endif
	}

	void OnEnable ()
	{		
		if (isLevelEditor) {	
	
			CreateLevelEditorBoard (boardW, boardH);
			GameManager.gameState = GameState.PLAYING;
		} else {

			StartNewGame ();
		}
	}
	public List<int> levelEditListBlockID;

	void Update ()
	{
		if (Input.GetMouseButtonUp (0)) {
			isDraging = false;
			if (isLevelEditor) {
				if (levelEditListBlockID.Count > 0) {
					string t = boardW + "," + boardH;
					for (int i = 0; i < levelEditListBlockID.Count; i++) {
						t += "," + levelEditListBlockID [i];
					}
					BuildLevel (t);
				}
			}

		}
	}

	public void CheckWin ()
	{
	
		for (int i = 0; i < listBlocks.Count; i++) {
			if (listBlocks [i].isWall == false && listBlocks [i].isFiled == false) {
				return;
			}
		}
		if (finger.activeInHierarchy) {
			HideFinger ();
		}
		Debug.Log ("Chien Thang");
		GameManager.gameState = GameState.WIN;
	
		float time = 0;
		for (int i = 0; i < listBlocks.Count; i++) {
			if (listBlocks [i].isWall) {
				listBlocks [i].ani.tween.SetDelay (0.01F * i);
				listBlocks [i].ani.tween.Restart (true);
				time += 0.05F * i;
			}

		}

		Invoke ("ShowWinPopup", 1);

	}

	void ShowWinPopup ()
	{
		GW_GAME_PLAY.instance.popupWin.gameObject.SetActive (true);
	}

	int hintCount;
	[Space (10)]
	[Tooltip ("Số ô được fill sau 1 lần bấm hint")]
	public int hintStep = 3;

	public void Hint ()
	{
	
		for (int i = 0; i < listBlocks.Count; i++) {
			if (listBlocks [i].isWall == false) {
				listBlocks [i].isFiled = false;
				listBlocks [i].nextBlock = null;
			}
		}
		hintList [0].isFiled = true;
 
		for (int i = 0; i < (hintCount + 1) * hintStep; i++) {
			if (i < hintList.Count - 1) {
				hintList [i].nextBlock = hintList [i + 1];
				hintList [i + 1].isFiled = true;
				currentBlock = hintList [i + 1];
			}
		}
		hintCount++;
		CheckWin ();
	}

	void HideFinger ()
	{
		finger.transform.DOKill ();
		finger.SetActive (false);
	}

	void ShowFinger ()
	{

		List<Vector3> path = new List<Vector3> ();
		for (int i = 0; i < hintList.Count; i++) {
			path.Add (hintList [i].transform.position);
		}

		finger.SetActive (true);
		finger.transform.position = path [0];
		finger.transform.DOKill ();
		finger.transform.DOPath (path.ToArray (), 4, PathType.Linear, PathMode.Full3D).SetEase (Ease.Linear).SetDelay (0.5F).SetLoops (-1, LoopType.Restart);
	}
}
