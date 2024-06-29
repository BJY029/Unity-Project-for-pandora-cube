using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
	//Collider 2D�� �⺻ ������ ��� �ݶ��̴�2D�� �����Ѵ�.
	Collider2D coll;

	private void Awake()
	{
		coll = GetComponent<Collider2D>();
	}


	//Player�� Area(boxCollider)�� ������ ����� �� ����
	void OnTriggerExit2D(Collider2D collision)
	{
		//���� Ÿ�� collider�� Area �±׿� �������� �ʴ´ٸ� �������� �ʴ´�.
		if (collision.CompareTag("size") == false)
			return;

		Debug.Log("An object with the 'Area' tag exited the trigger.");
		//������ ��쿡

		//�÷��̾��� ��ġ�� �޾ƿ´�.
		//�÷��̾��� ������ GameManager���� �����ϰ� ������
		//GameManager�� �������� �����߱� ������,
		//���� GameManager�� �������� �ʰ�, �ٷ� ������ ���� ����� �����ϴ�.
		Vector3 playerPos = GameManager.instance.player.transform.position;
		Vector3 myPos = transform.position;
		
		
		//���� �±װ� ����� Ȯ��
		switch (transform.tag)
		{
			//Ground �±׿� ���� ���
			case "Ground":

				//�Ÿ� ������ ���� �������� �޾ƿ´�.
				float diffX = (playerPos.x - myPos.x);
				float diffY = (playerPos.y - myPos.y);

				//�ٶ󺸴� ���� �� ����
				float dirX = diffX < 0 ? -1 : 1;
				float dirY = diffY < 0 ? -1 : 1;

				diffX = Mathf.Abs(diffX);
				diffY = Mathf.Abs(diffY);

				//���� X���� �̵��� Y�� �̵����� �� ū ���
				if (diffX > diffY)
				{
					//������ �������� 40��ŭ �̵��Ѵ�.
					//x�� �������� 1 * ���� * ũ��
					transform.Translate(Vector3.right * dirX * 40);
				}
				else if(diffX < diffY)
				{
					//������ �������� 40��ŭ �̵��Ѵ�.
					//y�� �������� 1 * ���� * ũ��
					transform.Translate(Vector3.up * dirY * 40);
				}
				break;
			case "Enemy":
				if (coll.enabled) //���Ͱ� ����ִ� ���
				{
					Vector3 dist = playerPos - myPos;
					Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
					//�÷��̾��� �̵� ���⿡ ���� ���� ���� �����ϵ��� �̵�
					//������ ��ġ���� �����ϵ��� ���� ���ϱ�
					transform.Translate(ran + dist * 2);
				}
				break;
		}
	}

}
