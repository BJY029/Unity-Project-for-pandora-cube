using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
	public GameObject[] unlockCharacter;

    public GameObject uiNotice;
    WaitForSecondsRealtime wait;


    enum Achive { UnLockPoatato, UnLockCarrot }
    Achive[] achives;

	private void Awake()
	{
        //Enum.GetValues : �־��� �������� �����͸� ��� �������� �Լ�
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        wait = new WaitForSecondsRealtime(5);

        //ó�� ����ÿ��� �ʱ�ȭ �ϰ�, �� �Ŀ��� �ʱ�ȭ ���� �ʱ����� ���ǹ�
        if (!PlayerPrefs.HasKey("MyData"))
        {
            //Debug.Log("initing");
			Init();
		}
	}

    void Init()
    {
        //PlayerPrefs : ������ ���� ����� �����ϴ� ����Ƽ ���� Ŭ����
        //SetInt �Լ��� ����ؼ� Key�� ����� int�� �����͸� ����
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in achives)
        {
			PlayerPrefs.SetInt(achive.ToString(), 0);
		}
		
	}

	void Start()
    {
        UnlockCharacter();   
    }

    void UnlockCharacter()
    {
        for(int i = 0; i< lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;

            lockCharacter[i].SetActive(!isUnlock);
			unlockCharacter[i].SetActive(isUnlock);
		}
    }
    
    //Update �Լ��� ���� �Ŀ� Ȯ�� ������ ������.
    void LateUpdate()
    {
        //�� ������ ������ ���鼭 ���� �޼� ���θ� Ȯ���Ѵ�.
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);

		}
    }

    void CheckAchive(Achive achive)
    {
        //�⺻ �޼����� false�� �ʱ�ȭ
        bool isAchive = false;

        //�� achive�� �°� �޼� ���� ������ ���� switch��
        switch(achive)
        {
            //���ڳ�� �ر��� ���� �����޼� ���� Ȯ��
            case Achive.UnLockPoatato:
                //ų ���� 10 �̻��̸� isAchive�� true�� ��ȯ
                isAchive = GameManager.instance.kill >= 10;
                break;
			//��ٳ�� �ر��� ���� �����޼� ���� Ȯ��
			case Achive.UnLockCarrot:
                //���� �ð���, �ִ� �ð��� ��ġ�� ���(������ ���) isAchive�� true�� ��ȯ
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
				break;
		}

        //������ �޼��߰�, ���� �ش� ĳ���Ͱ� �رݵ��� ���� ���¶��
        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            //�ش� ĳ���͸� �رݽ�Ų��.
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int i = 0; i < uiNotice.transform.childCount; i++)
            {
                //enum�� ������ �ε����� Ȱ���� �����ϴ�.
                bool isActive = i == (int)achive;
                //uiNotice�� �ش�Ǵ� �ڽ��� Ȱ��ȭ ��Ų��.
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            //�����̸� ���� �ڷ�ƾ
            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        //�ش� uiâ�� Ȱ��ȭ ��Ų��
        uiNotice.SetActive(true);

        //5�ʰ� ���ǽð� ������
        yield return wait;

        //�ش� uiâ �ٽ� ��Ȱ��ȭ
		uiNotice.SetActive(false);
	}
}
