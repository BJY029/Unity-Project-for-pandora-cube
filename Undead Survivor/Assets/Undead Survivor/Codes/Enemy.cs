using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
	public float health;
	public float maxHealth;
	public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
	Animator anim;
    SpriteRenderer spriter;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		//���Ͱ� ����ִ� ��쿡 ����
		if (!isLive) return;

		//��ġ ���� = Ÿ�� ��ġ(�÷��̾�) - ���� ��ġ
		//�� ����Ʈ�� ���� �ٸ� ����Ʈ�� ������ ����
		//�� ������Ʈ���� �ٸ� ������Ʈ�� ����Ű�� ���Ͱ��� �ȴ�.
		//���ʹ� Ÿ�� ������Ʈ�� ������ ����Ű��, ũ��� �� ������ ������ �Ÿ��� ����.
		Vector2 dirVec = target.position - rigid.position;

		//���� = ��ġ ������ ����ȭ(Normalized)
		//���� ��ġ = ���� * �ӵ� * ������ �ӵ�
		Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

		//�÷��̾��� Ű �Է� ���� ���� �̵� = ������ ���� ���� ���� �̵�
		//���� �̵� = �� ��ġ + ���Ⱚ
		rigid.MovePosition(rigid.position + nextVec);
		//���� �ӵ��� �̵��� ������ ���� �ʵ��� �ӵ��� �����Ѵ�.
		rigid.velocity = Vector2.zero;
	}

	private void LateUpdate()
	{
		//���Ͱ� ����ִ� ��쿡 ����
		if (!isLive) return;

		//�÷��̾��� x��ǥ ��ġ�� ���� x��ǥ ��ġ ���� ������
		//��, ���Ͱ� �÷��̾��� �����ʿ� ��ġ�� ������
		//���Ͱ� �ٶ󺸴� ���� �ٲٱ�(flipX = true)
		spriter.flipX = target.position.x < rigid.position.x;
	}

	//��ũ��Ʈ�� Ȱ��ȭ �� ��, ȣ��Ǵ� �̺�Ʈ �Լ�
	private void OnEnable()
	{
		target = GameManager.instance.player.GetComponent<Rigidbody2D>();
		isLive = true;
		health = maxHealth;
	}

	public void Init(SpawnData data)
	{
		anim.runtimeAnimatorController = animCon[data.spriteType];
		speed = data.speed;
		maxHealth = data.health;
		health = data.health;
	}
}
