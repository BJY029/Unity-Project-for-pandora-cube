using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
	//범위, 레이어, 스캔 결과 배열, 가장 가까운 목표를 담을 변수 선언
	public float scanRange;
	public LayerMask targetLayer;
	public RaycastHit2D[] targets;
	public Transform nearestTarget;

	private void FixedUpdate()
	{
		//1. 캐스팅 시작 위치, 2. 원의 반지름, 3. 캐스팅 방향, 4. 캐스팅 길이, 5. 타깃 레이어
		//스캔해서 찾은 타깃 레이어를 targets 배열에 삽입한다.
		targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

		//가장 가까운 적의 위치를 함수를 통해서 알아내고, neareatTarget에 저장한다.
		nearestTarget = GetNearest();

	}

	Transform GetNearest()
	{
		Transform result = null;
		
		//거리 값을 초기화 시켜준다.
		float diff = 100f;

		//스캔된 레이어 배열을 반복문을 통해 돌아본다.
		foreach(RaycastHit2D target in targets)
		{
			//내 위치(플레이어 위치)를 myPos에 저장
			Vector3 myPos = transform.position;
			//스캔된 적 위치(몬스터)
			Vector3 targetPos = target.transform.position;

			//Distance(A, B) : 벡터 A와 B의 거리를 계산해주는 함수
			//내 위치와, 적 위치의 차이값을 저장한다.
			float curDiff = Vector3.Distance(myPos, targetPos);

			//가장 가까운 적을 diff에 저장한다.
			if(curDiff < diff)
			{
				diff = curDiff;
				result = target.transform;
			}
		}

		//가장 가까운 적의 위치를 반환한다.
		return result;
	}
}
