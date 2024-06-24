using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //프리펩들을 보관한 변수
    //풀 담당을 하는 리스트들
    //일대일 관계여야 한다.

    public GameObject[] prefaps; //프리펩 보관 변수 선언
    List<GameObject>[] pools; //풀 담당 리스트

    //pools 리스트 초기화
	private void Awake()
	{
		pools = new List<GameObject>[prefaps.Length];

        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
	}


    //함수 선언
    public GameObject Get(int index)
    {
        GameObject select = null;

        //선택한 풀의 놀고 있는(비활성화 된) 게임 오브젝트 접근
        foreach(GameObject item in pools[index])
        {
			//발견하면 select 변수에 할당
			if (!item.activeSelf)
            {
				select = item;
                select.SetActive(true);
                break;
            }
        }

        //못 찾으면
        if(select == null) {
			//새롭게 생성하고 seclect 변수에 할당
			//Instantiate : 원본 오브젝트를 복제하여 장면에 생성하는 함수
			select = Instantiate(prefaps[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
