using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //��������� ������ ����
    //Ǯ ����� �ϴ� ����Ʈ��
    //�ϴ��� ���迩�� �Ѵ�.

    public GameObject[] prefaps; //������ ���� ���� ����
    List<GameObject>[] pools; //Ǯ ��� ����Ʈ

    //pools ����Ʈ �ʱ�ȭ
	private void Awake()
	{
		pools = new List<GameObject>[prefaps.Length];

        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
	}


    //�Լ� ����
    public GameObject Get(int index)
    {
        GameObject select = null;

        //������ Ǯ�� ��� �ִ�(��Ȱ��ȭ ��) ���� ������Ʈ ����
        foreach(GameObject item in pools[index])
        {
			//�߰��ϸ� select ������ �Ҵ�
			if (!item.activeSelf)
            {
				select = item;
                select.SetActive(true);
                break;
            }
        }

        //�� ã����
        if(select == null) {
			//���Ӱ� �����ϰ� seclect ������ �Ҵ�
			//Instantiate : ���� ������Ʈ�� �����Ͽ� ��鿡 �����ϴ� �Լ�
			select = Instantiate(prefaps[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
