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

	//WaitForFixedUpdate 변수 선언 및 초기화
	//코루틴에서 사용할 것으로, 매번 new를 통해 선언을 하면
	//프로그램에 좋지 않은 영향을 끼치기에 변수를 선언한다.
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
		//GetCurrentAnimatorStateInfo() : 현재 상태 정보를 가져오는 함수, 애니메이터의 레이어를 인자로 가진다
		//현재 애니메이션의 레이어는 BaseLayer 하나밖에 없기 때문에 0을 인자로 넣어준다.
		if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

		//몬스터가 살아있고, 총알에 맞지 않은 경우에만 실행


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

	//collider와 충돌한 경우 
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Bullet") || !isLive)
			return;

		//만약 충돌한 collider가 Bulllet인 경우

		//체력을 총알의 데미지만큼 깎는다.
		//Bullet 스크립트의 damamge 변수를 통해 값을 불러온다.
		health -=  collision.GetComponent<Bullet>().damage;


		//코르틴을 호출할 땐 해당 함수를 먼져 사용해야 함.
		StartCoroutine(KnockBack()); //StartCoroutine("KnockBack");도 가능



		if (health > 0)
		{
			//애니메이션에서 Hit 트리거를 활성화 한다.
			anim.SetTrigger("Hit");
		}
		else
		{
			//체력이 0보다 작은 경우(죽은경우)

			//isLive 을 false로 초기화
			isLive = false;

			//component의 비활성화는 .enabled = false
			coll.enabled = false;

			//rigidbody의 물리적 비활성화는 .simulated = false
			//물리를 시뮬 돌리지 않겠다는 뜻
			rigid.simulated = false;

			//죽은 시체가 다른 Enemy를 가리지 않도록,
			//Enemy레이어(2) 보다 하나 작게 설정
			spriter.sortingLayerID = 1;

			//애니메이션에서 Dead가 ture 된것을 활성화
			anim.SetBool("Dead", true);

			GameManager.instance.kill++;
			GameManager.instance.GetExp();
			
		}
	}

	//코루틴(Coroutine) : 생명 주기와 비동기처럼 실행되는 함수
	//IEnumerator : 코루틴만의 반환형 인터페이스
	IEnumerator KnockBack()
	{
		//yield : 코루틴의 반환 키워드
		//yield return null; //1프레임 쉬기
		//yield return new WaitForSeconds(2f);//2초 쉬기, 매번 new를 사용하는 것은 좋지 않다.
		yield return wait; //다음 하나의 물리 프레임 딜레이

		//플레이어의 위치를 PlayerPos에 저장
		Vector3 PlayerPos = GameManager.instance.player.transform.position;	
		//내 방향에서 플레이어 위치를 빼서 플레이어의 반대 방향 구하기
		Vector3 dirVec = transform.position - PlayerPos;

		//나에게 플레이어의 반대 방향으로 힘 적용하기
		//순간적인 힘이므로 ForceMode2D.Impulse 속성 추가
		rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
	}

	void Dead()
	{
		gameObject.SetActive(false);
	}
}
