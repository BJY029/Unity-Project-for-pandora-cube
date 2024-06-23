using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
	//Player의 Area(boxCollider)의 영역을 벗어났을 때 실행
	void OnTriggerExit2D(Collider2D collision)
	{
		//만약 타일 collider가 Area 태그와 접촉하지 않는다면 실행하지 않는다.
		if (collision.CompareTag("size") == false)
			return;

		Debug.Log("An object with the 'Area' tag exited the trigger.");
		//접촉한 경우에

		//플레이어의 위치를 받아온다.
		//플레이어의 정보는 GameManager에서 관리하고 있으며
		//GameManager를 정적으로 선언했기 때문에,
		//따로 GameManager을 선언하지 않고, 바로 다음과 같이 사용이 가능하다.
		Vector3 playerPos = GameManager.instance.player.transform.position;
		Vector3 myPos = transform.position;
		
		//거리 차이의 값을 절댓값으로 받아온다.
		float diffX = Mathf.Abs(playerPos.x - myPos.x);
		float diffY = Mathf.Abs(playerPos.y - myPos.y);

		//바라보는 방향 값 설정
		Vector3 playerDir = GameManager.instance.player.inputVec;
		float dirX = playerDir.x < 0 ? -1 : 1;
		float dirY = playerDir.y < 0 ? -1 : 1;


		//닿은 태그가 어떤건지 확인
		switch (transform.tag)
		{
			//Ground 태그와 닿은 경우
			case "Ground":
				//만약 X축의 이동이 Y축 이동보다 더 큰 경우
				if(diffX > diffY)
				{
					//오른쪽 방향으로 40만큼 이동한다.
					//x축 방향으로 1 * 방향 * 크기
					transform.Translate(Vector3.right * dirX * 40);
				}
				else if(diffX < diffY)
				{
					//오른쪽 방향으로 40만큼 이동한다.
					//y축 방향으로 1 * 방향 * 크기
					transform.Translate(Vector3.up * dirY * 40);
				}
				break;
			case "Enemy":

				break;
		}
	}
}
