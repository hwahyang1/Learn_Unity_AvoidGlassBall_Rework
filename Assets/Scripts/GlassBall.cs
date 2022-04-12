using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Class] GlassBall
 * 유리공의 움직임과 충돌 판정을 처리합니다.
 */
public class GlassBall : MonoBehaviour
{
	private GameManager gameManager;

	private float maxY = 6f;

	private void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void Update()
	{
		if (transform.position.y <= -maxY)
		{
			Destroy(gameObject);
		}
	}

	private void FixedUpdate()
	{
		transform.Translate(new Vector3(0f, -gameManager.glassBallSpeed, 0f));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		gameManager.ChangeHeart();
		Destroy(gameObject);
	}
}
