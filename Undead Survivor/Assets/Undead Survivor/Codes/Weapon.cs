using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //���� ID. ������ ID. ������, ����, �ӵ� ���� ����
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    //�Ѿ� �߻� �ֱ⸦ ���� timer ����
    float timer;
    Player player;

	private void Awake()
	{
        player = GameManager.instance.player;
	}

	void Update()
    {
		if (!GameManager.instance.isLive)
			return;

		switch (id)
		{
			case 0:
                //���� ȸ�� ����, �ð�������� ������ ������ ����,
                //foward(0, 0, 1)�� �ƴ�, back(0, 0, -1)�� ���
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
				break;
            case 1:

			default:
                timer += Time.deltaTime;

                //speed�� �Ѿ� ���� �ӵ��� �ǹ��Ѵ�.
                //���� timer�� �Ѿ� ���� �ӵ��� �Ѿ��
                //ex)timer�� 0.3���� �ʱ�ȭ �ϸ�, 0.3�� ���� �Ѿ��� �����ȴ�.
                if(timer > speed)
                {
                    //Ÿ�̸� �ʱ�ȭ ��
                    timer = 0f;

                    //Fire() �Լ��� ���� �߻��Ѵ�.
                    Fire();
                }
				break;
		}

        // .. Level Up TEST ..
        //if (Input.GetButtonDown("Jump"))
        //{
        //    Debug.Log("������ �Լ��� ȣ���մϴ�.");
        //    LevelUp(10, 1);
        //}
	}

    public void LevelUp(float damage, int cnt)
    {
        this.damage = damage * Character.Damage;
        this.count += cnt;   

        if(id == 0) Batch();

		//BroadcastMessage : Ư�� �Լ� ȣ���� ��� �ڽĵ鿡�� ����ϴ� �Լ�
		player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
	}

    //�ʱ�ȭ �Լ�
	public void Init(ItemData data)
    {
        //..Basic Setting
        //��ũ��Ʈ�� ������Ʈȭ �ߴ� ���� ���ڷ� �޾� ������ �ʱ�ȭ
        name = "Weapon" + data.itemId;
        //�ش� �������� ��ġ�� �÷��̾� �ڽ����� �Ҵ�
        transform.parent = player.transform;
        //�ش� ��ġ�� ���峻 ��ġ�� �ʱ�ȭ
        transform.localPosition = Vector3.zero;


        //..Property Setting
        //�ش� ���� �������� ���� �����͸� �ʱ�ȭ
        id = data.itemId; 
        damage =  data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;
        
        //�ݺ����� ���ؼ�, poolmanager�� ��ϵǾ� �ִ� ��������� ���ƺ���
        //���� �̸��� �߰��ϸ�, �ش� �ε����� ������id�� �ʱ�ȭ
        //�̷��� �ϸ�, ��ũ��Ʈ�� ������Ʈ�� Ǯ���� �������� ������ �� �ִ�.
        for(int i = 0; i < GameManager.instance.pool.prefaps.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefaps[i])
            {
                prefabId = i;
                break;
            }
            
        }

        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                //speed ���� ���� �ӵ��� �ǹ�
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        //Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);


        //BroadcastMessage : Ư�� �Լ� ȣ���� ��� �ڽĵ鿡�� ����ϴ� �Լ�
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); //-1�� Infinity Per�� ��������� ������� �����̴�.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        //���� ��ĳ�ʰ� ���� Ž���ϸ�

        //Ž���� ���� ��ġ�� targetPos�� ����
        Vector3 targetPos = player.scanner.nearestTarget.position;
        //ũ�Ⱑ ���Ե� ������ dir�� ����
        //���� : ��ǥ��ġ - ���� ��ġ
        Vector3 dir = targetPos - transform.position;
        //������ ũ�⸦ 1�� ����ȭ
        dir = dir.normalized;

        //�Ѿ� ����(Ǯ ���� �ҷ��´�)
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        //�Ѿ� ���� ��ġ�� �÷��̾� ��ġ�� ����
        bullet.position = transform.position;

        //FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�, ��� ������ �־��ش�.
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
