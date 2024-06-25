using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	//������ �����͸� ������ �迭 ����
    public Transform[] spawnPoint;
	public SpawnData[] spawnDatas;

	int level;

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

		//FloorToInt : �Ҽ��� �Ʒ��� ������ int ������ �ٲٴ� �Լ�,
		//CeilToInt : �Ҽ��� �Ʒ��� �ø��� int ������ �ٲٴ� �Լ�
		//10�ʰ� ����������, ������ 1�� �����Ѵ�.
		level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnDatas.Length - 1);

		//�ð��� 0.2�ʰ� ������
		if (timer > spawnDatas[level].spawnTime) 
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
		GameObject enemy =  GameManager.instance.pool.Get(0);

		//�� ��, ���͸� ������ ��ġ�� �����Ѵ�.
		//��ġ�� spawnPoint�� ����� �� �������� �ϳ��� �����ϰ� �����ؼ� �����´�.
		//�ڱ��ڽ��� 0�� ������ֱ� ������, Range�� 1���� ���۵ȴ�.
		enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

		enemy.GetComponent<Enemy>().Init(spawnDatas[level]);
	}
}

//����ȭ(Serialization) : ��ü�� ���� Ȥ�� �����ϱ� ���� ��ȯ
//unity������ �ν����� â������ ������ �� �ְ� �ϱ� ���� ����ȭ�� �����Ѵ�.
[System.Serializable]
public class SpawnData
{
	public int spriteType;
	public float spawnTime;
	public int health;
	public float speed;
}