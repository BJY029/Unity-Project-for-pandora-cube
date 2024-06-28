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
	//�ٷ��� ȿ������ �� �� �ֵ��� ä�� ���
	public int channels;//ä�� ���� ���� ����
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
		//..����� �÷��̾� �ʱ�ȭ

		GameObject bgmObject = new GameObject("BgmPlayer"); //�̸����� ����

		//������� ����ϴ� �ڽ� ������Ʈ ����
		bgmObject.transform.parent = transform;

		//AddComponent �Լ��� ����� �ҽ��� �����ϰ� ������ ����
		bgmPlayer = bgmObject.AddComponent<AudioSource>();

		//�ش� ����� �ҽ��� �Ӽ� ����
		bgmPlayer.playOnAwake = false;
		bgmPlayer.loop = true;
		bgmPlayer.volume = bgmVolume;
		bgmPlayer.clip = bgmClip;
		bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();


		//..ȿ���� �÷��̾� �ʱ�ȭ

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

	//����ϰ��� �ϴ� ȿ������ ���ڷ� ������
	public void PlaySfx(Sfx sfx)
	{
		//ä�� ����ŭ �ݺ����� ����.
		for(int i = 0; i < sfxPlayers.Length; ++i)
		{
			//��ⷯ ����
			int  loopIndex = (channelIdx + i) % sfxPlayers.Length;

			//�ش� �ε����� ���� �÷��̾ ������̸�, �׳� �Ѿ��.
			if (sfxPlayers[loopIndex].isPlaying)
			{
				continue;
			}

			//���� ȿ������ 1�� �̻��̹Ƿ�, ������ ���� ���� ������ �����ؼ� Ȱ��
			int ranIndex = 0;
			if (sfx == Sfx.Hit || sfx == Sfx.Melee)
			{
				ranIndex = Random.Range(0, 2);
			}

			//�ش� �ε����� �÷��̾ ���� ��������� ������,
			//�ش� �ε��� ���� �ʱ�ȭ
			channelIdx = loopIndex;
			//�ش� �ε����� ���� �÷��̾��� ����� Ŭ����, ����ϰ��� �ϴ� Ŭ�� ����
			sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
			//���
			sfxPlayers[loopIndex].Play();
			//����������
			break;
		}
	}

}
