using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	//������ �����͸� ������ �迭 ����
    public Transform[] spawnPoint;

	//���� Ÿ�̹��� ���ϱ� ���� ����
    float timer;

	private void Awake()
	{
		//GetComponetsInChildren�� �ش� ������Ʈ�� �ڽĵ��� �����ϰ� �ִ�.
		//�ڱ� �ڽ��� 0��° �ε����� ����ȴ�.
		//Spanwer �ڱ� �ڽŰ� �ڽĵ��� �迭�� ����ȴ�.
		spawnPoint = GetComponentsInChildren<Transform>();
	}


	void Update()
    {
		//�� �������� �ð��� ���Ѵ�.
		timer += Time.deltaTime;

		//�ð��� 0.2�ʰ� ������
		if(timer > 0.2f)
		{
			//Ÿ�̸Ӹ� 0���� �ʱ�ȭ�Ѵ�.
			timer = 0f;
			//�Լ��� ȣ���Ͽ� ���� �����Ѵ�.
			Spawn();
		}
    }

	//���� �����ϴ� �Լ�
	void Spawn()
	{
		//�켱 ������ ���͸� ���Ѵ�.
		//Ǯ�� ����� ���͵��� �����ϰ� �����´�.
		GameObject enemy =  GameManager.instance.pool.Get(Random.Range(0,2));

		//�� ��, ���͸� ������ ��ġ�� �����Ѵ�.
		//��ġ�� spawnPoint�� ����� �� �������� �ϳ��� �����ϰ� �����ؼ� �����´�.
		//�ڱ��ڽ��� 0�� ������ֱ� ������, Range�� 1���� ���۵ȴ�.
		enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
	}
}
