using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	//스폰할 포인터를 저장할 배열 선언
    public Transform[] spawnPoint;
	public SpawnData[] spawnDatas;
	public float levelTime;

	int level;

	//스폰 타이밍을 정하기 위해 선언
    float timer;

	private void Awake()
	{
		//GetComponetsInChildren은 해당 오브젝트의 자식들을 포함하고 있다.
		//자기 자신이 0번째 인덱스로 저장된다.
		//Spanwer 자기 자신과 자식들이 배열에 저장된다.
		spawnPoint = GetComponentsInChildren<Transform>();
		
		//최대 시간에 몬스터 데이터 크기로 나누어 자동으로 구간 시간 계산
		levelTime = GameManager.instance.maxGameTime / spawnDatas.Length;
	}


	void Update()
    {
		if (!GameManager.instance.isLive)
			return;

		//매 프레임의 시간을 더한다.
		timer += Time.deltaTime;

		//FloorToInt : 소수점 아래는 버리고 int 형으로 바꾸는 함수,
		//CeilToInt : 소수점 아래를 올리고 int 형으로 바꾸는 함수
		//10초가 지날때마다, 레벨이 1씩 증가한다.
		level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnDatas.Length - 1);

		//시간이 0.2초가 지나면
		if (timer > spawnDatas[level].spawnTime) 
		{
			//타이머를 0으로 초기화한다.
			timer = 0f;
			//함수를 호출하여 적을 생성한다.
			Spawn();
		}
    }

	//적을 생성하는 함수
	void Spawn()
	{
		//우선 생성할 몬스터를 정한다.
		//풀에 저장된 몬스터들을 랜덤하게 가져온다.
		GameObject enemy =  GameManager.instance.pool.Get(0);

		//그 후, 몬스터를 랜덤한 위치에 생성한다.
		//위치는 spawnPoint에 저장된 각 포인터중 하나를 랜덤하게 선출해서 가져온다.
		//자기자신이 0에 저장되있기 때문에, Range는 1부터 시작된다.
		enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

		enemy.GetComponent<Enemy>().Init(spawnDatas[level]);
	}
}

//직렬화(Serialization) : 개체를 저장 혹은 전송하기 위해 변환
//unity에서의 인스펙터 창에서도 관리할 수 있게 하기 위해 직렬화를 진행한다.
[System.Serializable]
public class SpawnData
{
	public int spriteType;
	public float spawnTime;
	public int health;
	public float speed;
}