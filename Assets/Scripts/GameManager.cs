using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Class] GameManager
 * 게임의 전반적 실행, 설정을 관리합니다.
 */
public class GameManager : MonoBehaviour
{
	private static readonly string highScoreKey = "BestScore";

	private GameObject gameOverScreen;
	private bool _isGameActive;
	public bool isGameActive
	{
		get
		{
			return _isGameActive;
		}

		private set
		{
			_isGameActive = value;
		}
	}

	public Sprite heartFull;
	public Sprite heartEmpty;
	private GameObject heartParent;
	private int _heart = 3;
	public int heart
	{
		get
		{
			return _heart;
		}

		private set
		{
			if (value > 3) _heart = 3;
			else if (value < 0) _heart = 0;
			else _heart = value;
		}
	}

	public GameObject glassBall;
	private GameObject glassBallParent;
	[SerializeField] private float glassBallSpawnPeriod = 1f;
	public float glassBallSpeed = 0.15f;
	private float glassBallMaxX = 9.5f;

	public Text highScoreText;
	public Text currentScoreText;
	[SerializeField] private int defaultScore = 10;
	[SerializeField] private int bonusScore = 30;
	private int highScore;
	private int currentScore;
	private bool isNewBest;

	private float elapsedTime = 0f;


	private void Start()
	{
		Application.targetFrameRate = 60;
		InitGame();
	}

	private void Update()
	{
		if (!isGameActive) return;

		if (elapsedTime >= glassBallSpawnPeriod)
		{
			float randomX = Random.Range(-glassBallMaxX, glassBallMaxX);
			Instantiate(glassBall, new Vector3(randomX, 6f, -1f), Quaternion.identity, glassBallParent.transform);
			elapsedTime = 0f;
		}
		else
		{
			elapsedTime += Time.deltaTime;
		}
	}

	/*
	 * [Method] InitGame()
	 * 게임을 초기 실행 상태로 설정합니다.
	 */
	public void InitGame()
	{
		gameOverScreen = GameObject.Find("GameOver");
		heartParent = GameObject.Find("Hearts");
		glassBallParent = GameObject.Find("GlassBalls");

		gameOverScreen.SetActive(false);

		if (PlayerPrefs.HasKey(highScoreKey))
		{
			highScore = PlayerPrefs.GetInt(highScoreKey);
		}
		else
		{
			PlayerPrefs.SetInt(highScoreKey, 0);
			highScore = 0;
		}

		currentScore = 0;
		isNewBest = highScore == 0 ? true : false; // 첫 판은 보너스점수 X
		isGameActive = true;

		highScoreText.text = "최고 " + highScore + "점";
		currentScoreText.text = currentScore + "점";

		heart = 3;
		for (int i = 0; i < 3; i++)
		{
			heartParent.transform.GetChild(i).GetComponent<Image>().sprite = heartFull;
		}
	}

	/*
	 * [Method] AddScore()
	 * 플레이어의 현재 점수를 올리고 점수를 관리합니다.
	 */
	public void AddScore()
	{
		if (!isGameActive) return;

		currentScore += defaultScore;

		if (currentScore > highScore)
		{
			if (!isNewBest)
			{
				currentScore += bonusScore;
				isNewBest = true;
			}
			highScore = currentScore;
		}
		
		highScoreText.text = "최고 " + highScore + "점";
		currentScoreText.text = currentScore + "점";

		if (currentScore % 50 == 0)
		{
			if (glassBallSpawnPeriod > 0.35f)
			{
				glassBallSpawnPeriod -= 0.05f;
			}
			if (glassBallSpeed < 0.17f)
			{
				glassBallSpeed += 0.002f;
			}
		}
	}

	/*
	 * [Method] ResetScore()
	 * 플레이어의 최고 점수를 초기화 합니다.
	 */
	public void ResetScore()
	{
		currentScore = 0;
		highScore = 0;
		PlayerPrefs.SetInt(highScoreKey, 0);
		PlayerPrefs.Save();

		highScoreText.text = "최고 " + highScore + "점";
		currentScoreText.text = currentScore + "점";
	}

	/*
	 * [Method] ChangeHeart(bool isAdd=false)
	 * 체력을 1씩 변화시킵니다.
	 * 
	 * <bool isAdd=false>
	 * 체력을 증감을 결정합니다.
	 * true면 체력을 증가하고, false면 체력을 감소합니다.
	 */
	public void ChangeHeart(bool isAdd=false)
	{
		if (!isGameActive) return;

		if (isAdd)
		{
			heart += 1;
			heartParent.transform.GetChild(heart).GetComponent<Image>().sprite = heartFull;
		}
		else
		{
			heart -= 1;
			heartParent.transform.GetChild(heart).GetComponent<Image>().sprite = heartEmpty;

			if (heart == 0)
			{
				isGameActive = false;
				gameOverScreen.SetActive(true);
				if (highScore > PlayerPrefs.GetInt(highScoreKey))
				{
					PlayerPrefs.SetInt(highScoreKey, highScore);
					PlayerPrefs.Save();
				}
			}
		}
	}
}
