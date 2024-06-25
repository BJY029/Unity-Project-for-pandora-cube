using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	//레벨 디자인을 위한 변수 선언
	public float gameTime;
	public float maxGameTime = 2 * 10f;

    public Player player;
	public PoolManager pool;

	private void Awake()
	{
		instance = this;
	}

	//시간을 관리하는 함수
	private void Update()
	{
		gameTime += Time.deltaTime;

		//gameTime이 maxGameTime을 넘어가지 않도록 설정
		if(gameTime > maxGameTime)
			gameTime = maxGameTime;
	}
}
