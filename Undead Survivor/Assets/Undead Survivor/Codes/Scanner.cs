using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
	//����, ���̾�, ��ĵ ��� �迭, ���� ����� ��ǥ�� ���� ���� ����
	public float scanRange;
	public LayerMask targetLayer;
	public RaycastHit2D[] targets;
	public Transform nearestTarget;

	private void FixedUpdate()
	{
		//1. ĳ���� ���� ��ġ, 2. ���� ������, 3. ĳ���� ����, 4. ĳ���� ����, 5. Ÿ�� ���̾�
		//��ĵ�ؼ� ã�� Ÿ�� ���̾ targets �迭�� �����Ѵ�.
		targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

		//���� ����� ���� ��ġ�� �Լ��� ���ؼ� �˾Ƴ���, neareatTarget�� �����Ѵ�.
		nearestTarget = GetNearest();

	}

	Transform GetNearest()
	{
		Transform result = null;
		
		//�Ÿ� ���� �ʱ�ȭ �����ش�.
		float diff = 100f;

		//��ĵ�� ���̾� �迭�� �ݺ����� ���� ���ƺ���.
		foreach(RaycastHit2D target in targets)
		{
			//�� ��ġ(�÷��̾� ��ġ)�� myPos�� ����
			Vector3 myPos = transform.position;
			//��ĵ�� �� ��ġ(����)
			Vector3 targetPos = target.transform.position;

			//Distance(A, B) : ���� A�� B�� �Ÿ��� ������ִ� �Լ�
			//�� ��ġ��, �� ��ġ�� ���̰��� �����Ѵ�.
			float curDiff = Vector3.Distance(myPos, targetPos);

			//���� ����� ���� diff�� �����Ѵ�.
			if(curDiff < diff)
			{
				diff = curDiff;
				result = target.transform;
			}
		}

		//���� ����� ���� ��ġ�� ��ȯ�Ѵ�.
		return result;
	}
}
