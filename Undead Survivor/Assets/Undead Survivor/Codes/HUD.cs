using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
	//UI 정보를 열거형으로 정의
	public enum InfoType { Exp, Level, Kill, Time, Health}
	//해당 타입 선언
	public InfoType type;

	//UI에서 텍스트와 게이지바 구현을 위한 슬라이더 선언
	Text myText;
	Slider mySlider;

	private void Awake()
	{
		//초기화
		myText = GetComponent<Text>();
		mySlider = GetComponent<Slider>();
	}

	private void LateUpdate()
	{
		//각 타입마다 조건문 실행
		switch(type)
		{
			case InfoType.Exp:
				//슬라이더에 적용할 값 : 현재 경험치 / 최대 경험치
				//각 경험치의 값은 GameManager에서 관리하므로, 해당 값을 불러온다.
				float curExp = GameManager.instance.exp;
				float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
				mySlider.value = curExp / maxExp;
				break;
			case InfoType.Level:
				//Format : 각 숫자 인자값을 지정된 형태의 문자열로 만들어 주는 함수
				myText.text = string.Format("Lv.{0:F0}",GameManager.instance.level);
				break;
			case InfoType.Kill:
				myText.text = string.Format("{0:F0}", GameManager.instance.kill);
				break;
			case InfoType.Time:
				//남은 시간 구하기
				float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
				int min = Mathf.FloorToInt(remainTime / 60);
				int sec = Mathf.FloorToInt(remainTime % 60);
				myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
				break;
			case InfoType.Health:
				float curHealth = GameManager.instance.health;
				float maxHealth= GameManager.instance.maxHealth;
				mySlider.value = curHealth / maxHealth;
				break;
		}
	}
}
