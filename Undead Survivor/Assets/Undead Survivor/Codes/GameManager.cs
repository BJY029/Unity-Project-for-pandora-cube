using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	//���� �������� ���� ���� ����
	public float gameTime;
	public float maxGameTime = 2 * 10f;

    public Player player;
	public PoolManager pool;

	private void Awake()
	{
		instance = this;
	}

	//�ð��� �����ϴ� �Լ�
	private void Update()
	{
		gameTime += Time.deltaTime;

		//gameTime�� maxGameTime�� �Ѿ�� �ʵ��� ����
		if(gameTime > maxGameTime)
			gameTime = maxGameTime;
	}
}
