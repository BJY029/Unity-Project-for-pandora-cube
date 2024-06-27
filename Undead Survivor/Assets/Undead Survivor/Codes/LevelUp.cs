using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    //�ش� ������Ʈ�� ũ�⸦ 1�� �ʱ�ȭ �ϴ� �Լ�
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }
    
    //�ش� ������Ʈ�� ũ�⸦ 0���� �ʱ�ȭ �ϴ� �Լ�
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }
    
    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        //1. ��� ������ ��Ȱ��ȭ
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false); 
        }


        //2. �� �߿��� ���� 3�� ������ Ȱ��ȭ
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
			ran[1] = Random.Range(0, items.Length);
			ran[2] = Random.Range(0, items.Length);

            //�������� ���� �ε��� �� �ߺ��� ������ �ش� �ݺ����� ����������.
			if (ran[0] != ran[1] && ran[1] != ran[2] && ran[2] != ran[0])
                break;
        }

        //�ݺ����� ���� �� �������� Ȱ��ȭ ��Ų��.
        for(int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

			//���� �������� ���� �Һ� ���������� ��ü
            if(ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
				ranItem.gameObject.SetActive(true);
			}
        }
    }
}