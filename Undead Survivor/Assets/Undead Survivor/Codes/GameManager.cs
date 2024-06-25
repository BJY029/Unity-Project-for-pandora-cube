using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	//레벨 디자인을 위한 변수 선언
	//Header : 인스펙터의 속성들을 이쁘게 구분시켜주는 타이틀
	[Header("# Game Control")]
	public float gameTime;
	public float maxGameTime = 2 * 10f;


	//레벨, 킬수, 경험치 변수 선언
	[Header("# Player Info")]
	public int level;
	public int kill;
	public int exp;
	public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

	[Header("# Game Object")]
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

	//경험치 추가 함수
	public void GetExp()
	{
		//경험치를 추가한다.
		exp++;

		//일정 경험치를 얻어서 레벨업 만큼의 경험치를 얻으면
		if(exp == nextExp[level])
		{
			//레벨업을 한다.
			level++;
			//얻었던 경험치를 초기화한다.
			exp = 0;

		}
	}
}
