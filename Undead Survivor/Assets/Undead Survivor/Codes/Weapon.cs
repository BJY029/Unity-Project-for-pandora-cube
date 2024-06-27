using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //무기 ID. 프리펩 ID. 데미지, 개수, 속도 변수 선언
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    //총알 발사 주기를 위한 timer 선언
    float timer;
    Player player;

	private void Awake()
	{
        player = GameManager.instance.player;
	}

	void Update()
    {
		if (!GameManager.instance.isLive)
			return;

		switch (id)
		{
			case 0:
                //무기 회전 구현, 시계방향으로 돌려면 음수로 설정,
                //foward(0, 0, 1)이 아닌, back(0, 0, -1)을 사용
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
				break;
            case 1:

			default:
                timer += Time.deltaTime;

                //speed는 총알 연사 속도를 의미한다.
                //만약 timer가 총알 연사 속도를 넘어서면
                //ex)timer를 0.3으로 초기화 하면, 0.3초 마다 총알이 생성된다.
                if(timer > speed)
                {
                    //타이머 초기화 후
                    timer = 0f;

                    //Fire() 함수를 통해 발사한다.
                    Fire();
                }
				break;
		}

        // .. Level Up TEST ..
        //if (Input.GetButtonDown("Jump"))
        //{
        //    Debug.Log("레벨업 함수를 호출합니다.");
        //    LevelUp(10, 1);
        //}
	}

    public void LevelUp(float damage, int cnt)
    {
        this.damage = damage;
        this.count += cnt;   

        if(id == 0) Batch();

		//BroadcastMessage : 특정 함수 호출을 모든 자식들에게 방송하는 함수
		player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
	}

    //초기화 함수
	public void Init(ItemData data)
    {
        //..Basic Setting
        //스크립트블 오브젝트화 했던 것을 인자로 받아 데이터 초기화
        name = "Weapon" + data.itemId;
        //해당 데이터의 위치를 플레이어 자식으로 할당
        transform.parent = player.transform;
        //해당 위치의 월드내 위치를 초기화
        transform.localPosition = Vector3.zero;


        //..Property Setting
        //해당 무기 프로펩의 각종 데이터를 초기화
        id = data.itemId; 
        damage =  data.baseDamage;
        count = data.baseCount;
        
        //반복문을 통해서, poolmanager에 등록되어 있는 프리펩들을 돌아보고
        //같은 이름을 발견하면, 해당 인덱스를 프리펩id로 초기화
        //이렇게 하면, 스크립트블 오브젝트와 풀간의 독립성을 유지할 수 있다.
        for(int i = 0; i < GameManager.instance.pool.prefaps.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefaps[i])
            {
                prefabId = i;
                break;
            }
            
        }

        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                //speed 값은 연사 속도를 의미
                speed = 0.3f;
                break;
        }

        //Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);


        //BroadcastMessage : 특정 함수 호출을 모든 자식들에게 방송하는 함수
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //무기를 배치하는 함수
    void Batch()
    {
        for(int i = 0; i < count; i++)
        {
            //해당 프리펩의 위치를 bullet 변수에 저장 후,
            Transform bullet;
            
            //만약 내 자식(무기 갯수)갯수가 인덱스(count)보다 크면, 
            //내 자식을 활용하고
            if(i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            //내 자식(무기 갯수)갯수가 인덱스(count)보다 작으면(레벨업 해서 count가 증가한 경우)
            //pool에서 무기를 가져온다.
            else 
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
				bullet.parent = transform;
			}
            
            //위치, 회전 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            //만약 count이 3이면
            //첫번째 무기 : 0
            //두번째 무기 : 120
            //세번째 무기 : 240
            Vector3 rotVec = Vector3.forward * 360 * i / count;
            //각 무기의 회전 값을 Rotate에 대입
            bullet.Rotate(rotVec);
            //회전 위치를 bullet.up(로컬) * 1.5f로 진행,
            //이미 앞선 코드에서 로컬로 값을 주었기 때문에 진행은 World로 준다.
            bullet.Translate(bullet.up * 1.5f, Space.World);

            //불러 온 무기의 속성 설정
            //Bullet 스크립트의 Init() 함수를 호출하며, 인자를 현재 Weapon 변수인 damage와 -1을 넘겨준다.
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); //-1은 Infinity Per로 근접무기는 관통력이 무한이다.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        //만약 스캐너가 적을 탐지하면

        //탐지한 적의 위치를 targetPos에 저장
        Vector3 targetPos = player.scanner.nearestTarget.position;
        //크기가 포함된 방향을 dir에 저장
        //방향 : 목표위치 - 나의 위치
        Vector3 dir = targetPos - transform.position;
        //벡터의 크기를 1로 정규화
        dir = dir.normalized;

        //총알 생성(풀 에서 불러온다)
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        //총알 생성 위치는 플레이어 위치로 설정
        bullet.position = transform.position;

        //FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수, 축과 방향을 넣어준다.
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
