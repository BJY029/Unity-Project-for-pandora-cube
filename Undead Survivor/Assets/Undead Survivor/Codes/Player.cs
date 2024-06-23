//수정된 코드
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //새로운 라이브러리가 추가되었다.

public class Player : MonoBehaviour
{
	public Vector2 inputVec;
	public float speed;
	Rigidbody2D rigid;
	SpriteRenderer spriter;
	Animator anim;


	//초기화 진행
	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
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
		//애니메이터에서 설정한 파라메터와 동일한 타입으로 작성(speed 파라미터는 float)
		//SetFloat("파라미터 이름", 값);
		//inputVec.magnitude; 벡터의 순수한 크기값을 반환
		anim.SetFloat("Speed", inputVec.magnitude);

		//x값이 0이 아닐때, 만약 x값이 0보다 작으면 flipX는 true, 0보다 크면 flase 값을 갖게 된다.
		if (inputVec.x != 0) spriter.flipX = inputVec.x < 0;
	}
}