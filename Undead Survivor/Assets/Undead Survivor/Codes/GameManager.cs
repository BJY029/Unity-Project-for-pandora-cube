using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	//���� �������� ���� ���� ����
	//Header : �ν������� �Ӽ����� �̻ڰ� ���н����ִ� Ÿ��Ʋ
	[Header("# Game Control")]
	public float gameTime;
	public float maxGameTime = 2 * 10f;
	public bool isLive;


	//����, ų��, ����ġ ���� ����
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
	public GameObject enemyCleaner;

	private void Awake()
	{
		instance = this;
	}

	public void GameStart(int id)
	{
		playerId = id;

		health = maxHealth;
		
		player.gameObject.SetActive(true);

		uiLevelUp.Select(id % 2);	

		Resume();

		//����� ȣ��
		AudioManager.instance.PlayBgm(true);

		//ȿ���� ȣ��
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
	}

	//���� ���� �Լ�
	public void GameOver()
	{
		//�ڷ�ƾ ȣ��
		StartCoroutine(GameOverRoutine());
	}

	IEnumerator GameOverRoutine()
	{
		//�ٸ� ��ũ��Ʈ ������ �����.
		isLive = false;

		//0.5���� �����̸� ��ȯ�Ѵ�.
		yield return new WaitForSeconds(0.5f);

		//����� ���� UI�� ȭ�鿡 ǥ���Ѵ�.
		uiResult.gameObject.SetActive(true);
		uiResult.Lose();

		//Stop �Լ��� ȣ���Ͽ� �ð��� �����.
		Stop();

		
		AudioManager.instance.PlayBgm(false);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
	}

	public void GameVictory()
	{
		//�ڷ�ƾ ȣ��
		StartCoroutine(GameVictoryRoutine());
	}

	IEnumerator GameVictoryRoutine()
	{
		//�ٸ� ��ũ��Ʈ ������ �����.
		isLive = false;

		//enemyCleaner�� Ȱ��ȭ ���Ѽ� ���� �����Ѵ�.
		enemyCleaner.SetActive(true);

		//0.5���� �����̸� ��ȯ�Ѵ�.
		yield return new WaitForSeconds(0.5f);

		//����� ���� UI�� ȭ�鿡 ǥ���Ѵ�.
		uiResult.gameObject.SetActive(true);
		uiResult.Win();

		//Stop �Լ��� ȣ���Ͽ� �ð��� �����.
		Stop();

		AudioManager.instance.PlayBgm(false);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
	}

	public void GameRetry()
	{
		SceneManager.LoadScene(0);
	}

	//�ð��� �����ϴ� �Լ�
	private void Update()
	{
		if (!isLive)
			return;

		gameTime += Time.deltaTime;

		//gameTime�� maxGameTime�� �Ѿ�� �ʵ��� ����
		if (gameTime > maxGameTime)
		{
			gameTime = maxGameTime;
			GameVictory();
		}
	}

	//����ġ �߰� �Լ�
	public void GetExp()
	{
		if (!isLive)
			return;

		//����ġ�� �߰��Ѵ�.
		exp++;

		//���� ����ġ�� �� ������ ��ŭ�� ����ġ�� ������
		//�ִ� ������ ���� �ʵ���, ���� ������, �ִ� ���� �� �ּڰ��� ��ȯ�ϴ� 
		//Mathf.Min�� ����Ѵ�. �̷��� �Ǹ�, �ִ� ������ �Ѿ��, �� �̻��� ������ �ݿ����� �ʴ´�.
		if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
		{
			//�������� �Ѵ�.
			level++;
			//����� ����ġ�� �ʱ�ȭ�Ѵ�.
			exp = 0;

			uiLevelUp.Show();

		}
	}

	public void Stop()
	{
		isLive = false;

		//����Ƽ�� �ð� �ӵ�(����)
		Time.timeScale = 0;
	}

	public void Resume()
	{
		isLive = true;

		//����Ƽ�� �ð� �ӵ�(����)
		Time.timeScale = 1;
	}
}
