using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	//레벨 디자인을 위한 변수 선언
	//Header : 인스펙터의 속성들을 이쁘게 구분시켜주는 타이틀
	[Header("# Game Control")]
	public float gameTime;
	public float maxGameTime = 2 * 10f;
	public bool isLive;


	//레벨, 킬수, 경험치 변수 선언
	[Header("# Player Info")]
	public int playerId;
	public float health;
	public float maxHealth = 100;
	public int level;
	public int kill;
	public int exp;
	public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

	[Header("# Game Object")]
	public Player player;
	public PoolManager pool;
	public LevelUp uiLevelUp;
	public Result uiResult;
	public Transform uiJoy;
	public GameObject enemyCleaner;

	private void Awake()
	{
		instance = this;
		//게임 실행시, 지정한 프레임 숫자로 설정하는 함수
		Application.targetFrameRate = 60;
	}

	public void GameStart(int id)
	{
		playerId = id;

		health = maxHealth;
		
		player.gameObject.SetActive(true);

		uiLevelUp.Select(id % 2);	

		Resume();

		//배경음 호출
		AudioManager.instance.PlayBgm(true);

		//효과음 호출
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
	}

	//게임 종료 함수
	public void GameOver()
	{
		//코루틴 호출
		StartCoroutine(GameOverRoutine());
	}

	IEnumerator GameOverRoutine()
	{
		//다른 스크립트 동작을 멈춘다.
		isLive = false;

		//0.5초의 딜레이를 반환한다.
		yield return new WaitForSeconds(0.5f);

		//재시작 여부 UI를 화면에 표시한다.
		uiResult.gameObject.SetActive(true);
		uiResult.Lose();

		//Stop 함수를 호출하여 시간을 멈춘다.
		Stop();

		
		AudioManager.instance.PlayBgm(false);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
	}

	public void GameVictory()
	{
		//코루틴 호출
		StartCoroutine(GameVictoryRoutine());
	}

	IEnumerator GameVictoryRoutine()
	{
		//다른 스크립트 동작을 멈춘다.
		isLive = false;

		//enemyCleaner를 활성화 시켜서 적을 제거한다.
		enemyCleaner.SetActive(true);

		//0.5초의 딜레이를 반환한다.
		yield return new WaitForSeconds(0.5f);

		//재시작 여부 UI를 화면에 표시한다.
		uiResult.gameObject.SetActive(true);
		uiResult.Win();

		//Stop 함수를 호출하여 시간을 멈춘다.
		Stop();

		AudioManager.instance.PlayBgm(false);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
	}

	public void GameQuit()
	{
		Application.Quit();
	}

	public void GameRetry()
	{
		SceneManager.LoadScene(0);
	}

	//시간을 관리하는 함수
	private void Update()
	{
		if (!isLive)
			return;

		gameTime += Time.deltaTime;

		//gameTime이 maxGameTime을 넘어가지 않도록 설정
		if (gameTime > maxGameTime)
		{
			gameTime = maxGameTime;
			GameVictory();
		}
	}

	//경험치 추가 함수
	public void GetExp()
	{
		if (!isLive)
			return;

		//경험치를 추가한다.
		exp++;

		//일정 경험치를 얻어서 레벨업 만큼의 경험치를 얻으면
		//최대 레벨을 넘지 않도록, 현재 레벨과, 최대 레벨 중 최솟값을 반환하는 
		//Mathf.Min을 사용한다. 이렇게 되면, 최대 레벨을 넘어가도, 그 이상의 레벨이 반영되지 않는다.
		if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
		{
			//레벨업을 한다.
			level++;
			//얻었던 경험치를 초기화한다.
			exp = 0;

			uiLevelUp.Show();

		}
	}

	public void Stop()
	{
		isLive = false;

		//유니티의 시간 속도(배율)
		Time.timeScale = 0;

		uiJoy.localScale = Vector3.zero;
	}

	public void Resume()
	{
		isLive = true;

		//유니티의 시간 속도(배율)
		Time.timeScale = 1;

		uiJoy.localScale = Vector3.one;
	}
}
