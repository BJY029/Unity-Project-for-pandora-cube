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

    //해당 오브젝트의 크기를 1로 초기화 하는 함수
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
		AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
	}
    
    //해당 오브젝트의 크기를 0으로 초기화 하는 함수
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
	}
    
    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        //1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false); 
        }


        //2. 그 중에서 랜덤 3개 아이템 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
			ran[1] = Random.Range(0, items.Length);
			ran[2] = Random.Range(0, items.Length);

            //랜덤으로 받은 인덱스 중 중복이 없으면 해당 반복문을 빠져나간다.
			if (ran[0] != ran[1] && ran[1] != ran[2] && ran[2] != ran[0])
                break;
        }

        //반복문을 통해 각 아이템을 활성화 시킨다.
        for(int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

			//만랩 아이템의 경우는 소비 아이템으로 대체
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