using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    //해당 아이템의 정보를 다른 스크립트에서 불러오기
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

	private void Awake()
	{
        //아이콘 이미지는 해당 오브젝트의 자식에 위치해 있다.
        //해당 함수는 자기 자신을 포함하고 있기 때문에, 두번째 값을 가져온다.
        icon = GetComponentsInChildren<Image>()[1];
        //해당 아이콘의 스프라이트를, 스크립프트 오브젝트화 했던 것의 아이템 아이콘으로 부터 가져온다.
        icon.sprite = data.itemIcon;

        //GetComponents의 순서는 계층구조의 순서를 따라간다.
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];                             
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
	}

    //레벨 업이 활성화 되었을 때 호출된다.
	private void OnEnable()
	{
        //레벨 텍스트 초기화
		textLevel.text = "Lv." + (level + 1);

        //각 아이템 타입이 따라, 들어가는 매개변수 수가 다르기 때문에
        //구분하기 위해 switch 문 사용
        switch (data.itemType)
        {
            case ItemData.ItemType.Melle:
            case ItemData.ItemType.Range:
                //무기 설명에는 두개의 매개변수가 필요
                //데미지 매개변수는 백분율로 나타내기 때문에 100을 곱한다.
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
			case ItemData.ItemType.Glove:
			case ItemData.ItemType.Shoe:
                //장비 설명에는 하나의 매개변수가 필요
				textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
				break;
            default:
                //체력 회복은 매개변수가 필요가 없다.
				textDesc.text = string.Format(data.itemDesc);
				break;

		}
        
	}

    //버튼이 클린된 경우 실행
    public void OnClick()
    {
        //각 타입마다 클릭 반응를 다르게 하기 위해 미리 구분
        switch(data.itemType)
        {
            case ItemData.ItemType.Melle:
			case ItemData.ItemType.Range:
                if(level == 0)
                {
                    //새로운 게임 오브젝트를 코드로 생성
                    GameObject newWeapon = new GameObject();
                    //해당 오브젝트를 초기화 하고, weapon에 대입
                    weapon = newWeapon.AddComponent<Weapon>();
                    //해당 오브젝트를 초기화
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
				//레벨 증가
				level++;
				break;
			case ItemData.ItemType.Glove:
			case ItemData.ItemType.Shoe:
				if (level == 0)
				{
					//새로운 게임 오브젝트를 코드로 생성
					GameObject newGear = new GameObject();
					//해당 오브젝트를 초기화 하고, weapon에 대입
					gear = newGear.AddComponent<Gear>();
					//해당 오브젝트를 초기화
					gear.Init(data);
				}
				else
				{
					float nextRate = data.damages[level];
					gear.LevelUp(nextRate);
				}
				//레벨 증가
				level++;
				break;
			case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
				break;
		}

        

        //만약 레벨이 최대에 도달하면
        //아까 스크립트블 오브젝트화 했던 곳에서, damage의 인덱스 수가 곧 레벨의 수가 된다.
        if(level == data.damages.Length)
        {
            //해당 버튼을 비활성화 한다.
            GetComponent<Button>().interactable = false;
        }
        
    }
}
