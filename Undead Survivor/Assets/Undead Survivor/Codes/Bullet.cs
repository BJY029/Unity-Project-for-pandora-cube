using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    //강체 선언
    Rigidbody2D rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>(); 
	}

	public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        //관통이 -1(무한)보다 큰 것에 대해서는 속도를 적용한다.
        //즉, 총알인 경우
        if(per > -1)
        {
            //총알의 속도를 dir 값으로 초기화한다.
            rigid.velocity = dir * 15f;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        //적과 닿았고, 관통력이 -1이 아닌 경우 실행
        //관통력을 -1 한다.
        per--;

        //만약 관통력이 -1이 되면
        if(per == -1)
        {
            //해당 강체의 속도를 0으로 초기화 하고
            rigid.velocity = Vector2.zero;
            //해당 강체를 비활성화 한다.
            gameObject.SetActive(false);
        }
	}
}
