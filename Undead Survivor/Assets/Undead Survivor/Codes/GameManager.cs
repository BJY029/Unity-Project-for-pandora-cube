using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	//���� �������� ���� ���� ����
	//Header : �ν������� �Ӽ����� �̻ڰ� ���н����ִ� Ÿ��Ʋ
	[Header("# Game Control")]
	public float gameTime;
	public float maxGameTime = 2 * 10f;


	//����, ų��, ����ġ ���� ����
	[Header("# Player Info")]
	public int health;
	public int maxHealth = 100;
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

	private void Start()
	{
		health = maxHealth;
	}

	//�ð��� �����ϴ� �Լ�
	private void Update()
	{
		gameTime += Time.deltaTime;

		//gameTime�� maxGameTime�� �Ѿ�� �ʵ��� ����
		if(gameTime > maxGameTime)
			gameTime = maxGameTime;
	}

	//����ġ �߰� �Լ�
	public void GetExp()
	{
		//����ġ�� �߰��Ѵ�.
		exp++;

		//���� ����ġ�� �� ������ ��ŭ�� ����ġ�� ������
		if(exp == nextExp[level])
		{
			//�������� �Ѵ�.
			level++;
			//����� ����ġ�� �ʱ�ȭ�Ѵ�.
			exp = 0;

		}
	}
}
