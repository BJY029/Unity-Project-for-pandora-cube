using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    //��ü ����
    Rigidbody2D rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>(); 
	}

	public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        //������ -1(����)���� ū �Ϳ� ���ؼ��� �ӵ��� �����Ѵ�.
        //��, �Ѿ��� ���
        if(per > -1)
        {
            //�Ѿ��� �ӵ��� dir ������ �ʱ�ȭ�Ѵ�.
            rigid.velocity = dir * 15f;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        //���� ��Ұ�, ������� -1�� �ƴ� ��� ����
        //������� -1 �Ѵ�.
        per--;

        //���� ������� -1�� �Ǹ�
        if(per == -1)
        {
            //�ش� ��ü�� �ӵ��� 0���� �ʱ�ȭ �ϰ�
            rigid.velocity = Vector2.zero;
            //�ش� ��ü�� ��Ȱ��ȭ �Ѵ�.
            gameObject.SetActive(false);
        }
	}
}
