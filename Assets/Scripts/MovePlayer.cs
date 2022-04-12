using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Class] MovePlayer
 * 플레이어의 움직임을 제어합니다.
 */
public class MovePlayer : MonoBehaviour
{
	private GameManager gameManager;

	private SpriteRenderer spriteRenderer;

	private float maxX = 9f;

	private void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			if (Input.GetKeyDown(KeyCode.A))
				MoveLeft();
			if (Input.GetKeyDown(KeyCode.D))
				MoveRight();
		}
	}

	/*
	 * [Method] MoveLeft()
	 * 플레이어를 좌측으로 이동시킵니다.
	 */
	public void MoveLeft()
	{
		if (!gameManager.isGameActive) return;

		if (transform.position.x > -maxX)
		{
			spriteRenderer.flipX = false;
			transform.Translate(-1f, 0f, 0f);
		}
		else
		{
			transform.position = new Vector3(-maxX, -3f, -2f);
		}
	}

	/*
	 * [Method] MoveRight()
	 * 플레이어를 우측으로 이동시킵니다.
	 */
	public void MoveRight()
	{
		if (!gameManager.isGameActive) return;

		if (transform.position.x < maxX)
		{
			spriteRenderer.flipX = true;
			transform.Translate(1f, 0f, 0f);
		}
		else
		{
			transform.position = new Vector3(maxX, -3f, -2f);
		}
	}
}
