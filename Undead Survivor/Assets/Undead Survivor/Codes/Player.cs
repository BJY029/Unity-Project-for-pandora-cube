//수정된 코드
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem; //새로운 라이브러리가 추가되었다.

public class Player : MonoBehaviour
{
	public Vector2 inputVec;
	public float speed;
	public Scanner scanner;
	public Hand[] hands;

	Rigidbody2D rigid;
	SpriteRenderer spriter;
	Animator anim;


	//초기화 진행
	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		scanner = GetComponent<Scanner>();
		//GetComponentsInChildren<Hand>(true)에서 true는 비활성화된 오브젝트도 포함하게 된다.
		hands = GetComponentsInChildren<Hand>(true);
	}

	private void FixedUpdate()
	{
		if (!GameManager.instance.isLive)
			return;

		//normalized는 액션 창에서 적용했기 때문에 삭제한다.
		Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
		rigid.MovePosition(rigid.position + nextVec);
	}

	//InputSystem의 OnMove함수를 사용.
	//이동 이벤트(왼쪽, 오른쪽, 위쪽, 아래쪽 등)가 발생했을 때 호출됩니다.
	void OnMove(InputValue value)
	{
		inputVec = value.Get<Vector2>(); //호출된 값을 vector2값으로 변환 후 inputVec에 대입한다
	}

	//프레임이 종료되기 전 실행되는 생명주기 함수
	void LateUpdate()
	{
		if (!GameManager.instance.isLive)
			return;

		//애니메이터에서 설정한 파라메터와 동일한 타입으로 작성(speed 파라미터는 float)
		//SetFloat("파라미터 이름", 값);
		//inputVec.magnitude; 벡터의 순수한 크기값을 반환
		anim.SetFloat("Speed", inputVec.magnitude);

		//x값이 0이 아닐때, 만약 x값이 0보다 작으면 flipX는 true, 0보다 크면 flase 값을 갖게 된다.
		if (inputVec.x != 0) spriter.flipX = inputVec.x < 0;
	}


	private void OnCollisionStay2D(Collision2D collision)
	{
		if(!GameManager.instance.isLive)
			return;

		GameManager.instance.health -= Time.deltaTime * 10;

		if(GameManager.instance.health < 0)
		{
			//childCount : 자식 오브젝트의 개수
			for(int i = 2; i < transform.childCount; i++)
			{
				//GetChild : 주어진 인덱스의 자식 오브젝트를 반환하는 함수
				//해당 자식의 위치 컴포넌트로 이동 후, 해당 오브젝트로 이동해 활성화를 해제
				transform.GetChild(i).gameObject.SetActive(false);
			}

			anim.SetTrigger("Dead");
			GameManager.instance.GameOver();
		}
	}
}