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
		//몬스터가 살아있는 경우에 실행
		if (!isLive) return;

		//위치 차이 = 타깃 위치(플레이어) - 나의 위치
		//한 포인트의 값을 다른 포인트의 값에서 빼면
		//한 오브젝트에서 다른 오브젝트를 가리키는 벡터값이 된다.
		//벡터는 타깃 오브젝트의 방향을 가리키고, 크기는 두 포지션 사이의 거리와 같다.
		Vector2 dirVec = target.position - rigid.position;

		//방향 = 위치 차이의 정규화(Normalized)
		//다음 위치 = 방향 * 속도 * 프레임 속도
		Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

		//플레이어의 키 입력 값을 더한 이동 = 몬스터의 방향 값을 더한 이동
		//몬스터 이동 = 내 위치 + 방향값
		rigid.MovePosition(rigid.position + nextVec);
		//물리 속도가 이동에 영향을 주지 않도록 속도를 제거한다.
		rigid.velocity = Vector2.zero;
	}

	private void LateUpdate()
	{
		//몬스터가 살아있는 경우에 실행
		if (!isLive) return;

		//플레이어의 x좌표 위치가 몬스터 x좌표 위치 보다 작으면
		//즉, 몬스터가 플레이어의 오른쪽에 위치해 있으면
		//몬스터가 바라보는 방향 바꾸기(flipX = true)
		spriter.flipX = target.position.x < rigid.position.x;
	}

	//스크립트가 활성화 될 때, 호출되는 이벤트 함수
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

	//collider와 충돌한 경우 
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Bullet"))
			return;

		//만약 충돌한 collider가 Bulllet인 경우

		//체력을 총알의 데미지만큼 깎는다.
		//Bullet 스크립트의 damamge 변수를 통해 값을 불러온다.
		health -=  collision.GetComponent<Bullet>().damage;

		if(health > 0)
		{

		}
		else
		{
			Dead();
		}
	}

	void Dead()
	{
		gameObject.SetActive(false);
	}
}
