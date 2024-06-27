using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    //�ش� �������� ������ �ٸ� ��ũ��Ʈ���� �ҷ�����
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
        //������ �̹����� �ش� ������Ʈ�� �ڽĿ� ��ġ�� �ִ�.
        //�ش� �Լ��� �ڱ� �ڽ��� �����ϰ� �ֱ� ������, �ι�° ���� �����´�.
        icon = GetComponentsInChildren<Image>()[1];
        //�ش� �������� ��������Ʈ��, ��ũ����Ʈ ������Ʈȭ �ߴ� ���� ������ ���������� ���� �����´�.
        icon.sprite = data.itemIcon;

        //GetComponents�� ������ ���������� ������ ���󰣴�.
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];                             
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
	}

    //���� ���� Ȱ��ȭ �Ǿ��� �� ȣ��ȴ�.
	private void OnEnable()
	{
        //���� �ؽ�Ʈ �ʱ�ȭ
		textLevel.text = "Lv." + (level + 1);

        //�� ������ Ÿ���� ����, ���� �Ű����� ���� �ٸ��� ������
        //�����ϱ� ���� switch �� ���
        switch (data.itemType)
        {
            case ItemData.ItemType.Melle:
            case ItemData.ItemType.Range:
                //���� ������ �ΰ��� �Ű������� �ʿ�
                //������ �Ű������� ������� ��Ÿ���� ������ 100�� ���Ѵ�.
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
			case ItemData.ItemType.Glove:
			case ItemData.ItemType.Shoe:
                //��� ������ �ϳ��� �Ű������� �ʿ�
				textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
				break;
            default:
                //ü�� ȸ���� �Ű������� �ʿ䰡 ����.
				textDesc.text = string.Format(data.itemDesc);
				break;

		}
        
	}

    //��ư�� Ŭ���� ��� ����
    public void OnClick()
    {
        //�� Ÿ�Ը��� Ŭ�� ������ �ٸ��� �ϱ� ���� �̸� ����
        switch(data.itemType)
        {
            case ItemData.ItemType.Melle:
			case ItemData.ItemType.Range:
                if(level == 0)
                {
                    //���ο� ���� ������Ʈ�� �ڵ�� ����
                    GameObject newWeapon = new GameObject();
                    //�ش� ������Ʈ�� �ʱ�ȭ �ϰ�, weapon�� ����
                    weapon = newWeapon.AddComponent<Weapon>();
                    //�ش� ������Ʈ�� �ʱ�ȭ
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
				//���� ����
				level++;
				break;
			case ItemData.ItemType.Glove:
			case ItemData.ItemType.Shoe:
				if (level == 0)
				{
					//���ο� ���� ������Ʈ�� �ڵ�� ����
					GameObject newGear = new GameObject();
					//�ش� ������Ʈ�� �ʱ�ȭ �ϰ�, weapon�� ����
					gear = newGear.AddComponent<Gear>();
					//�ش� ������Ʈ�� �ʱ�ȭ
					gear.Init(data);
				}
				else
				{
					float nextRate = data.damages[level];
					gear.LevelUp(nextRate);
				}
				//���� ����
				level++;
				break;
			case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
				break;
		}

        

        //���� ������ �ִ뿡 �����ϸ�
        //�Ʊ� ��ũ��Ʈ�� ������Ʈȭ �ߴ� ������, damage�� �ε��� ���� �� ������ ���� �ȴ�.
        if(level == data.damages.Length)
        {
            //�ش� ��ư�� ��Ȱ��ȭ �Ѵ�.
            GetComponent<Button>().interactable = false;
        }
        
    }
}
