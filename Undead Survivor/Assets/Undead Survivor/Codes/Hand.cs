using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	//�ش� ������Ʈ�� �޼��̸� True, �������̸� False�� ������ �ȴ�.
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

	//�� ��Ȳ�� �´� ��ġ�� ����
	Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
	Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
	Quaternion leftRot = Quaternion.Euler(0, 0, -35);
	Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

	private void Awake()
	{
		//player�� �ش� ��ũ��Ʈ�� �θ��� �÷��̾�� �ʱ�ȭ
		player = GetComponentsInParent<SpriteRenderer>()[1];
	}

	private void LateUpdate()
	{
		//player�� �����Ǿ��ִ��� Ȯ��
		bool isReverse = player.flipX;
		
		//�޼��̸�
		if(isLeft) //��������
		{
			//���� �÷��̾ ������ ���¸�, ������ ��ġ�� ����, �ƴϸ� �״��
			transform.localRotation = isReverse ? leftRotReverse : leftRot;
			//�޼��� player ���� ���ο� �°� ����
			spriter.flipY = isReverse;
			//������ �Ǹ� ���̾ 4��, �ƴϸ� 6���� ����
			spriter.sortingOrder = isReverse ? 4 : 6;
		}
		//�������̸�
		else //���Ÿ� ����
		{
			//���� �÷��̾ ������ ���¸�, ������ ��ġ�� ����, �ƴϸ� �״��
			transform.localPosition = isReverse ? rightPosReverse : rightPos;
			//�������� player ���� ���ο� �°� ����
			spriter.flipX = isReverse;
			//������ �Ǹ� ���̾ 6����, �ƴϸ� 4�� ����
			spriter.sortingOrder = isReverse ? 6 : 4;
		}
	}
}
