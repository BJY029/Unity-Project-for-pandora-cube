using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	//해당 오브젝트가 왼손이면 True, 오른손이면 False을 가지게 된다.
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

	//각 상황에 맞는 위치를 저장
	Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
	Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
	Quaternion leftRot = Quaternion.Euler(0, 0, -35);
	Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

	private void Awake()
	{
		//player를 해당 스크립트의 부모인 플레이어로 초기화
		player = GetComponentsInParent<SpriteRenderer>()[1];
	}

	private void LateUpdate()
	{
		//player가 반전되어있는지 확인
		bool isReverse = player.flipX;
		
		//왼손이면
		if(isLeft) //근접무기
		{
			//현재 플레이어가 반전된 상태면, 반전된 위치값 적용, 아니면 그대로
			transform.localRotation = isReverse ? leftRotReverse : leftRot;
			//왼손을 player 반전 여부에 맞게 변경
			spriter.flipY = isReverse;
			//반전이 되면 레이어를 4로, 아니면 6으로 변경
			spriter.sortingOrder = isReverse ? 4 : 6;
		}
		//오른손이면
		else //원거리 무기
		{
			//현재 플레이어가 반전된 상태면, 반전된 위치값 적용, 아니면 그대로
			transform.localPosition = isReverse ? rightPosReverse : rightPos;
			//오른손을 player 반전 여부에 맞게 변경
			spriter.flipX = isReverse;
			//반전이 되면 레이어를 6으로, 아니면 4로 변경
			spriter.sortingOrder = isReverse ? 6 : 4;
		}
	}
}
