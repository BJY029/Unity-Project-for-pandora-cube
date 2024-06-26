using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gear : MonoBehaviour
{
	public ItemData.ItemType type;
	public float rate;

	public void Init(ItemData data)
	{
		//..Basic Setting
		//해당 기어의 이름과 플레이어의 자식으로 위치 변경
		name = "Gear " + data.name;
		transform.parent = GameManager.instance.player.transform;
		transform.localPosition = Vector3.zero;

		//..Property Set
		//해당 무기의 타입과 속성 정보를 초기화
		type = data.itemType;
		rate = data.damages[0];
		//기어에 정보를 적용하는 함수 호출
		ApplyGear();
	}

	//버튼이 눌려서 레벨업 함수가 호출되면
	public void LevelUp(float rate)
	{
		//rate값을 최신화하고
		this.rate = rate;
		//해당 rate값을 기반으로 각 기어에 맞게 레벨업 적용
		ApplyGear();
	}

	void ApplyGear()
	{
		//각 무기 타입에 따라 레이트를 반영하는 정보가 다르다
		switch (type)
		{
			//해당 아이템이 장갑이면
			case ItemData.ItemType.Glove:
				//연사력 증가
				RateUp();
				break;
			//해당 아이템이 장화면
			case ItemData.ItemType.Shoe:
				//이동 속도 증가
				SpeedUp(); 
				break;
		}
	}

	//장갑의 기능인 연사력을 증가시키는 함수
	void RateUp()
	{
		//weeapons 배열에, 현재 parents 자식에 할당되어있는 무기들의 정보들을 모두 저장
		Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

		//반복문으로 각 무기 속성 확인
		foreach(Weapon weapon in weapons)
		{
			switch(weapon.id)
			{
				//무기의 id가 0이면(근접 무기)
				case 0:
					//해당 무기의 회전 속도 증가
					weapon.speed = 150 + (150 * rate);
					break;
				//무기의 아이디가 0이 아니면(1, 원거리 무기)
				default:
					//해당 무기의 연사력 증가
					weapon.speed = 0.5f * (1f - rate);
					break;
			}
		}
	}

	//장화의 기능인 이동속도 증가시키는 함수
	void SpeedUp()
	{
		//기본 이동속도 초기화
		float speed = 5;

		//플레이어의 이동속도 증가
		GameManager.instance.player.speed = speed + speed * rate;
	}
}
