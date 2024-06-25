using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //���� ID. ������ ID. ������, ����, �ӵ� ���� ����
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
                //���� ȸ�� ����, �ð�������� ������ ������ ����,
                //foward(0, 0, 1)�� �ƴ�, back(0, 0, -1)�� ���
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
				break;
			default:

				break;
		}

        // .. Level Up TEST ..
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("������ �Լ��� ȣ���մϴ�.");
            LevelUp(20, 5);
        }
	}

    public void LevelUp(float damage, int cnt)
    {
        this.damage = damage;
        this.count += cnt;   

        if(id == 0) Batch();
    }

    //�ʱ�ȭ �Լ�
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

    //���⸦ ��ġ�ϴ� �Լ�
    void Batch()
    {
        for(int i = 0; i < count; i++)
        {
            //�ش� �������� ��ġ�� bullet ������ ���� ��,
            Transform bullet;
            
            //���� �� �ڽ�(���� ����)������ �ε���(count)���� ũ��, 
            //�� �ڽ��� Ȱ���ϰ�
            if(i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            //�� �ڽ�(���� ����)������ �ε���(count)���� ������(������ �ؼ� count�� ������ ���)
            //pool���� ���⸦ �����´�.
            else 
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
				bullet.parent = transform;
			}
            
            //��ġ, ȸ�� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            //���� count�� 3�̸�
            //ù��° ���� : 0
            //�ι�° ���� : 120
            //����° ���� : 240
            Vector3 rotVec = Vector3.forward * 360 * i / count;
            //�� ������ ȸ�� ���� Rotate�� ����
            bullet.Rotate(rotVec);
            //ȸ�� ��ġ�� bullet.up(����) * 1.5f�� ����,
            //�̹� �ռ� �ڵ忡�� ���÷� ���� �־��� ������ ������ World�� �ش�.
            bullet.Translate(bullet.up * 1.5f, Space.World);

            //�ҷ� �� ������ �Ӽ� ����
            //Bullet ��ũ��Ʈ�� Init() �Լ��� ȣ���ϸ�, ���ڸ� ���� Weapon ������ damage�� -1�� �Ѱ��ش�.
            bullet.GetComponent<Bullet>().Init(damage, -1); //-1�� Infinity Per.
        }
    }
}
