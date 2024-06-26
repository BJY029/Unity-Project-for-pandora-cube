//������ �ڵ�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //���ο� ���̺귯���� �߰��Ǿ���.

public class Player : MonoBehaviour
{
	public Vector2 inputVec;
	public float speed;
	public Scanner scanner;
	public Hand[] hands;

	Rigidbody2D rigid;
	SpriteRenderer spriter;
	Animator anim;


	//�ʱ�ȭ ����
	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		scanner = GetComponent<Scanner>();
		//GetComponentsInChildren<Hand>(true)���� true�� ��Ȱ��ȭ�� ������Ʈ�� �����ϰ� �ȴ�.
		hands = GetComponentsInChildren<Hand>(true);
	}

	private void FixedUpdate()
	{
		//normalized�� �׼� â���� �����߱� ������ �����Ѵ�.
		Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
		rigid.MovePosition(rigid.position + nextVec);
	}

	//InputSystem�� OnMove�Լ��� ���.
	//�̵� �̺�Ʈ(����, ������, ����, �Ʒ��� ��)�� �߻����� �� ȣ��˴ϴ�.
	void OnMove(InputValue value)
	{
		inputVec = value.Get<Vector2>(); //ȣ��� ���� vector2������ ��ȯ �� inputVec�� �����Ѵ�
	}

	//�������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�
	void LateUpdate()
	{
		//�ִϸ����Ϳ��� ������ �Ķ���Ϳ� ������ Ÿ������ �ۼ�(speed �Ķ���ʹ� float)
		//SetFloat("�Ķ���� �̸�", ��);
		//inputVec.magnitude; ������ ������ ũ�Ⱚ�� ��ȯ
		anim.SetFloat("Speed", inputVec.magnitude);

		//x���� 0�� �ƴҶ�, ���� x���� 0���� ������ flipX�� true, 0���� ũ�� flase ���� ���� �ȴ�.
		if (inputVec.x != 0) spriter.flipX = inputVec.x < 0;
	}
}