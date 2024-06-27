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
	Collider2D coll;
	Animator anim;
    SpriteRenderer spriter;

	//WaitForFixedUpdate ���� ���� �� �ʱ�ȭ
	//�ڷ�ƾ���� ����� ������, �Ź� new�� ���� ������ �ϸ�
	//���α׷��� ���� ���� ������ ��ġ�⿡ ������ �����Ѵ�.
	WaitForFixedUpdate wait;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collider2D>();
		anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
		wait = new WaitForFixedUpdate();
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		if (!GameManager.instance.isLive)
			return;

		//GetCurrentAnimatorStateInfo() : ���� ���� ������ �������� �Լ�, �ִϸ������� ���̾ ���ڷ� ������
		//���� �ִϸ��̼��� ���̾�� BaseLayer �ϳ��ۿ� ���� ������ 0�� ���ڷ� �־��ش�.
		if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

		//���Ͱ� ����ְ�, �Ѿ˿� ���� ���� ��쿡�� ����


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
		if (!GameManager.instance.isLive)
			return;

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
		coll.enabled = true;
		rigid.simulated = true;
		spriter.sortingLayerID = 2;
		anim.SetBool("Dead", false);
		health = maxHealth;
	}

	public void Init(SpawnData data)
	{
		anim.runtimeAnimatorController = animCon[data.spriteType];
		speed = data.speed;
		maxHealth = data.health;
		health = data.health;
	}

	//collider�� �浹�� ��� 
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Bullet") || !isLive)
			return;

		//���� �浹�� collider�� Bulllet�� ���

		//ü���� �Ѿ��� ��������ŭ ��´�.
		//Bullet ��ũ��Ʈ�� damamge ������ ���� ���� �ҷ��´�.
		health -=  collision.GetComponent<Bullet>().damage;


		//�ڸ�ƾ�� ȣ���� �� �ش� �Լ��� ���� ����ؾ� ��.
		StartCoroutine(KnockBack()); //StartCoroutine("KnockBack");�� ����



		if (health > 0)
		{
			//�ִϸ��̼ǿ��� Hit Ʈ���Ÿ� Ȱ��ȭ �Ѵ�.
			anim.SetTrigger("Hit");
		}
		else
		{
			//ü���� 0���� ���� ���(�������)

			//isLive �� false�� �ʱ�ȭ
			isLive = false;

			//component�� ��Ȱ��ȭ�� .enabled = false
			coll.enabled = false;

			//rigidbody�� ������ ��Ȱ��ȭ�� .simulated = false
			//������ �ù� ������ �ʰڴٴ� ��
			rigid.simulated = false;

			//���� ��ü�� �ٸ� Enemy�� ������ �ʵ���,
			//Enemy���̾�(2) ���� �ϳ� �۰� ����
			spriter.sortingOrder = 1;

			//�ִϸ��̼ǿ��� Dead�� ture �Ȱ��� Ȱ��ȭ
			anim.SetBool("Dead", true);

			GameManager.instance.kill++;
			GameManager.instance.GetExp();
			
		}
	}

	//�ڷ�ƾ(Coroutine) : ���� �ֱ�� �񵿱�ó�� ����Ǵ� �Լ�
	//IEnumerator : �ڷ�ƾ���� ��ȯ�� �������̽�
	IEnumerator KnockBack()
	{
		//yield : �ڷ�ƾ�� ��ȯ Ű����
		//yield return null; //1������ ����
		//yield return new WaitForSeconds(2f);//2�� ����, �Ź� new�� ����ϴ� ���� ���� �ʴ�.
		yield return wait; //���� �ϳ��� ���� ������ ������

		//�÷��̾��� ��ġ�� PlayerPos�� ����
		Vector3 PlayerPos = GameManager.instance.player.transform.position;	
		//�� ���⿡�� �÷��̾� ��ġ�� ���� �÷��̾��� �ݴ� ���� ���ϱ�
		Vector3 dirVec = transform.position - PlayerPos;

		//������ �÷��̾��� �ݴ� �������� �� �����ϱ�
		//�������� ���̹Ƿ� ForceMode2D.Impulse �Ӽ� �߰�
		rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
	}

	void Dead()
	{
		gameObject.SetActive(false);
	}
}
