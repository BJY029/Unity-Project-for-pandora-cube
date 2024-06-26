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
		//�ش� ����� �̸��� �÷��̾��� �ڽ����� ��ġ ����
		name = "Gear " + data.name;
		transform.parent = GameManager.instance.player.transform;
		transform.localPosition = Vector3.zero;

		//..Property Set
		//�ش� ������ Ÿ�԰� �Ӽ� ������ �ʱ�ȭ
		type = data.itemType;
		rate = data.damages[0];
		//�� ������ �����ϴ� �Լ� ȣ��
		ApplyGear();
	}

	//��ư�� ������ ������ �Լ��� ȣ��Ǹ�
	public void LevelUp(float rate)
	{
		//rate���� �ֽ�ȭ�ϰ�
		this.rate = rate;
		//�ش� rate���� ������� �� �� �°� ������ ����
		ApplyGear();
	}

	void ApplyGear()
	{
		//�� ���� Ÿ�Կ� ���� ����Ʈ�� �ݿ��ϴ� ������ �ٸ���
		switch (type)
		{
			//�ش� �������� �尩�̸�
			case ItemData.ItemType.Glove:
				//����� ����
				RateUp();
				break;
			//�ش� �������� ��ȭ��
			case ItemData.ItemType.Shoe:
				//�̵� �ӵ� ����
				SpeedUp(); 
				break;
		}
	}

	//�尩�� ����� ������� ������Ű�� �Լ�
	void RateUp()
	{
		//weeapons �迭��, ���� parents �ڽĿ� �Ҵ�Ǿ��ִ� ������� �������� ��� ����
		Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

		//�ݺ������� �� ���� �Ӽ� Ȯ��
		foreach(Weapon weapon in weapons)
		{
			switch(weapon.id)
			{
				//������ id�� 0�̸�(���� ����)
				case 0:
					//�ش� ������ ȸ�� �ӵ� ����
					weapon.speed = 150 + (150 * rate);
					break;
				//������ ���̵� 0�� �ƴϸ�(1, ���Ÿ� ����)
				default:
					//�ش� ������ ����� ����
					weapon.speed = 0.5f * (1f - rate);
					break;
			}
		}
	}

	//��ȭ�� ����� �̵��ӵ� ������Ű�� �Լ�
	void SpeedUp()
	{
		//�⺻ �̵��ӵ� �ʱ�ȭ
		float speed = 5;

		//�÷��̾��� �̵��ӵ� ����
		GameManager.instance.player.speed = speed + speed * rate;
	}
}
