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
        //Enum.GetValues : 주어진 열거형의 데이터를 모두 가져오는 함수
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        wait = new WaitForSecondsRealtime(5);

        //처음 실행시에만 초기화 하고, 그 후에는 초기화 하지 않기위한 조건문
        if (!PlayerPrefs.HasKey("MyData"))
        {
            //Debug.Log("initing");
			Init();
		}
	}

    void Init()
    {
        //PlayerPrefs : 간단한 저장 기능을 제공하는 유니티 제공 클래스
        //SetInt 함수를 사용해서 Key와 연결된 int형 데이터를 저장
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
    
    //Update 함수가 끝난 후에 확인 절차를 가진다.
    void LateUpdate()
    {
        //각 열거형 정보를 돌면서 업적 달성 여부를 확인한다.
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);

		}
    }

    void CheckAchive(Achive achive)
    {
        //기본 달성여부 false로 초기화
        bool isAchive = false;

        //각 achive에 맞게 달성 조건 나누기 위한 switch문
        switch(achive)
        {
            //감자농부 해금을 위한 업적달성 여부 확인
            case Achive.UnLockPoatato:
                //킬 수가 10 이상이면 isAchive을 true로 변환
                isAchive = GameManager.instance.kill >= 10;
                break;
			//당근농부 해금을 위한 업적달성 여부 확인
			case Achive.UnLockCarrot:
                //현재 시간과, 최대 시간이 일치한 경우(생존한 경우) isAchive를 true로 변환
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
				break;
		}

        //업적을 달성했고, 현재 해당 캐릭터가 해금되지 않은 상태라면
        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            //해당 캐릭터를 해금시킨다.
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int i = 0; i < uiNotice.transform.childCount; i++)
            {
                //enum은 순서를 인덱스로 활용이 가능하다.
                bool isActive = i == (int)achive;
                //uiNotice의 해당되는 자식을 활성화 시킨다.
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            //딜레이를 위한 코루틴
            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        //해당 ui창을 활성화 시킨다
        uiNotice.SetActive(true);

        //5초간 현실시간 딜레이
        yield return wait;

        //해당 ui창 다시 비활성화
		uiNotice.SetActive(false);
	}
}
