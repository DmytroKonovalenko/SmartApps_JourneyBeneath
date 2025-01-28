using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AFasdcvxc : MonoBehaviour
{
	
	public GameObject startBlockDot;
	
	public DOTweenAnimation ani, startPosShowAni;
	
	public SpriteRenderer wallRender;
	bool _isWall;

	public int id;

	public GameObject fillObj;
	
	public LineRenderer surfaceLine, bodyLine, shadowLine, whiteLine;
 
    AFasdcvxc _nextBlock;
	
	Vector3 surfaceLineKC = new Vector3 (0, 0.02F, 0);
	Vector3 shadowLineKC = new Vector3 (0, -0.05F, 0);
	Vector3 bodyLineKC = new Vector3 (0, -0.02F, 0);

	public bool isDraging = false;

	[Space (10)]
	[Header ("Color")]
	
	public SpriteRenderer cellSurface;
	public SpriteRenderer cellDark;
	public SpriteRenderer blockSurface, blockDark, blockShadow;


	public AFasdcvxc nextBlock {
		get { return _nextBlock; }
		set {
			_nextBlock = value;
	
			surfaceLine.SetPosition (0, transform.position + surfaceLineKC);
			bodyLine.SetPosition (0, transform.position + bodyLineKC);
			shadowLine.SetPosition (0, transform.position + shadowLineKC);
			whiteLine.SetPosition (0, transform.position);
		
			if (value != null) {
				surfaceLine.enabled = true;
				bodyLine.enabled = true;
				shadowLine.enabled = true;
				whiteLine.enabled = true;
				surfaceLine.SetPosition (1, value.transform.position + surfaceLineKC);
				bodyLine.SetPosition (1, value.transform.position + bodyLineKC);
				shadowLine.SetPosition (1, value.transform.position + shadowLineKC);
				whiteLine.SetPosition (1, value.transform.position);
			} else {
				surfaceLine.enabled = false;
				bodyLine.enabled = false;
				shadowLine.enabled = false;
				whiteLine.enabled = false;
			}
		}
	}
	public bool isWall {
		get{ return _isWall; }
		set {
			_isWall = value;
			if (value == true) {
				wallRender.enabled = true;
			} else {
				wallRender.enabled = false;
			}
		}
	}

	bool _isFilled;
	public bool isFiled {
		get { 
			return _isFilled;
		}
		set { 
			_isFilled = value;
			if (value) {
				fillObj.SetActive (true);

			} else {
				fillObj.SetActive (false);
			}
		}
	}

	void Awake ()
	{
		startBlockDot.SetActive (false);
		isFiled = false;
		nextBlock = null;
		GameColorManager.instance.onColorChange += new System.Action (this.ChangeCellColor);
	}

	void Start ()
	{
		SetBlockColor ();
		ChangeCellColor ();
	}

	void OnMouseDown ()
	{
		if (GameManager.gameState != GameState.PLAYING) {
			return;
		}
		if (OneLineGameplayControl.instance.isLevelEditor) {
		
			OneLineGameplayControl.instance.currentBlock = this;
			OneLineGameplayControl.instance.isDraging = true;
			isFiled = true;
			OneLineGameplayControl.instance.levelEditListBlockID.Add (id);

		} else {
		
			if (OneLineGameplayControl.instance.currentBlock == this) {
				OneLineGameplayControl.instance.isDraging = true;

			}
		}
	}

	void OnDestroy ()
	{
		GameColorManager.instance.onColorChange -= this.ChangeCellColor;
	}

	void OnMouseEnter ()
	{
		if (GameManager.gameState != GameState.PLAYING) {
			return;
		}
		if (OneLineGameplayControl.instance.isDraging) {
			if (isFiled == false && isWall == false) {
				
				if (Vector3.Distance (transform.position, OneLineGameplayControl.instance.currentBlock.transform.position) <= 1) {
					OneLineGameplayControl.instance.currentBlock.nextBlock = this;
					OneLineGameplayControl.instance.currentBlock = this;

					isFiled = true;

					
					if (OneLineGameplayControl.instance.isLevelEditor) {
						OneLineGameplayControl.instance.levelEditListBlockID.Add (id);
					} else {
						
						OneLineGameplayControl.instance.CheckWin ();
					}
				}

			} else {
				
				if (nextBlock == OneLineGameplayControl.instance.currentBlock) {
					OneLineGameplayControl.instance.currentBlock.isFiled = false;
					if (OneLineGameplayControl.instance.isLevelEditor) {
						OneLineGameplayControl.instance.levelEditListBlockID.Remove (nextBlock.id);
					}
					nextBlock = null;
					OneLineGameplayControl.instance.currentBlock = this;
					

				}
			}
		}
	}

	void ChangeCellColor ()
	{
		Color col = GameColorManager.GetColor (GameElementColorType.GP_CELL);
		cellDark.color = GameManager.GetDarkColor (col);
		ChangeShadowColor ();
	}

	void ChangeShadowColor ()
	{
		
		Color bgColor = GameColorManager.GetColor (GameElementColorType.BACKGROUND);
		blockShadow.color = GameManager.GetDarkColor (bgColor);
		shadowLine.startColor = blockShadow.color;
		shadowLine.endColor = blockShadow.color;
	}

	void SetBlockColor ()
	{
		Color col = GameColorManager.GetColor (OneLineGameplayControl.instance.currentBlockColorType);
		Color bgColor = GameColorManager.GetColor (GameElementColorType.BACKGROUND);
		blockSurface.color = col;
		blockDark.color = GameManager.GetDarkColor (col);
		blockShadow.color = GameManager.GetDarkColor (bgColor);

		surfaceLine.startColor = col;
		surfaceLine.endColor = col;

		bodyLine.startColor = blockDark.color;
		bodyLine.endColor = blockDark.color;

		shadowLine.startColor = blockShadow.color;
		shadowLine.endColor = blockShadow.color;
	}
}

[System.Serializable]
public class CustomColor
{
	public Color surfaceColor;
	public Color darkToneColor;
	public Color shadowColor;
}