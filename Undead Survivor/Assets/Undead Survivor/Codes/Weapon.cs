using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //무기 ID. 프리펩 ID. 데미지, 개수, 속도 변수 선언
    public int id;
    public int prefabId;
    public float damage;
    public float count;
    public float speed;

	private void Start()
	{
		Init();
	}

	void Update()
    {
		switch (id)
		{
			case 0:
                //무기 회전 구현, 시계방향으로 돌려면 음수로 설정,
                //foward(0, 0, 1)이 아닌, back(0, 0, -1)을 사용
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
				break;
			default:

				break;
		}

        // .. Level Up TEST ..
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("레벨업 함수를 호출합니다.");
            LevelUp(20, 5);
        }
	}

    public void LevelUp(float damage, int cnt)
    {
        this.damage = damage;
        this.count += cnt;   

        if(id == 0) Batch();
    }

    //초기화 함수
	public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            default:

                break;
        }
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
            bullet.GetComponent<Bullet>().Init(damage, -1); //-1은 Infinity Per.
        }
    }
}
