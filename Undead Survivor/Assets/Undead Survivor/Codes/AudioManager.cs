using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

	[Header("#BGM")]
	public AudioClip bgmClip;
	public float bgmVolume;
	AudioSource bgmPlayer;
	AudioHighPassFilter bgmEffect;

	[Header("#SFX")]
	public AudioClip[] sfxClips;
	public float sfxVolume;
	//다량의 효과음을 낼 수 있도록 채널 사용
	public int channels;//채널 개수 변수 선언
	AudioSource[] sfxPlayers;
	int channelIdx;


	public enum Sfx {Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win}


	private void Awake()
	{
		instance = this;
		Init();
	}

	void Init()
	{
		//..배경음 플레이어 초기화

		GameObject bgmObject = new GameObject("BgmPlayer"); //이름까지 지정

		//배경음을 담당하는 자식 오브젝트 생성
		bgmObject.transform.parent = transform;

		//AddComponent 함수로 오디오 소스를 생성하고 변수에 저장
		bgmPlayer = bgmObject.AddComponent<AudioSource>();

		//해당 오디오 소스의 속성 변경
		bgmPlayer.playOnAwake = false;
		bgmPlayer.loop = true;
		bgmPlayer.volume = bgmVolume;
		bgmPlayer.clip = bgmClip;
		bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();


		//..효과음 플레이어 초기화

		GameObject sfxObject = new GameObject("SfxPlayer");
		sfxObject.transform.parent = transform;
		sfxPlayers = new AudioSource[channels];

		for(int i = 0; i < channels; i++)
		{
			sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();

			sfxPlayers[i].bypassListenerEffects = true;
			sfxPlayers[i].playOnAwake = false ;
			sfxPlayers[i].volume = sfxVolume;
		}
	}


	public void PlayBgm(bool isPlay)
	{
		if(isPlay)
		{
			bgmPlayer.Play();
		}
        else
        {
			{
				bgmPlayer.Stop();
			}
        }
    }

	public void EffectBgm(bool isPlay)
	{
		bgmEffect.enabled = isPlay;
	}

	//재생하고자 하는 효과음을 인자로 받으면
	public void PlaySfx(Sfx sfx)
	{
		//채널 수만큼 반복문을 돈다.
		for(int i = 0; i < sfxPlayers.Length; ++i)
		{
			//모듈러 연산
			int  loopIndex = (channelIdx + i) % sfxPlayers.Length;

			//해당 인덱스에 속한 플레이어가 재생중이면, 그냥 넘어간다.
			if (sfxPlayers[loopIndex].isPlaying)
			{
				continue;
			}

			//무기 효과음은 1개 이상이므로, 다음과 같은 랜덤 변수를 선언해서 활용
			int ranIndex = 0;
			if (sfx == Sfx.Hit || sfx == Sfx.Melee)
			{
				ranIndex = Random.Range(0, 2);
			}

			//해당 인덱스의 플레이어가 현재 재생중이지 않으면,
			//해당 인덱스 값을 초기화
			channelIdx = loopIndex;
			//해당 인덱스에 속한 플레이어의 오디오 클립에, 재생하고자 하는 클립 삽입
			sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
			//재생
			sfxPlayers[loopIndex].Play();
			//빠져나오기
			break;
		}
	}

}
