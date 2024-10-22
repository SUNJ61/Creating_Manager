using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LobbySoundManager : MonoBehaviour
{
    //로비 음악(BG)
    //로비 배경 촛불 타는 소리(SFX)

    public static LobbySoundManager instance;
    public Dictionary<string, SoundData> Data = new Dictionary<string, SoundData>();

    private AudioMixer audioMixer;
    private AudioClip Lobby_BG;

    private Slider BG_Slider;
    private Slider SFX_Slider;

    private GameObject SoundBox;
    private GameObject MainCamera;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SoundBox = Resources.Load<GameObject>("SoundBox");
        audioMixer = Resources.Load<AudioMixer>("Sound/AudioMixer");
        Lobby_BG = Resources.Load<AudioClip>("Sound/BG/LobbyBG");

        //BG_Slider = GameObject.Find("Ui").transform.GetChild(2).GetChild(3).GetChild(2).GetComponent<Slider>();
        //SFX_Slider = GameObject.Find("Ui").transform.GetChild(2).GetChild(3).GetChild(3).GetComponent<Slider>();
        MainCamera = GameObject.Find("MainCamera");

        //BG_Slider.onValueChanged.AddListener(value => SoundManager.instance.BG_Sound = value); // 슬라이더의 값이 변경되면 자동으로 프로퍼티에 적용
        //SFX_Slider.onValueChanged.AddListener(value => SoundManager.instance.SFX_Sound = value); // 슬라이더의 값이 변경되면 자동으로 프로퍼티에 적용

        ActiveSound(MainCamera, Lobby_BG, 5.0f, true, true, true, 0);
    }
    private void Start()
    {
        SoundSetting(0, SoundManager.instance.BG_Sound); //씬 시작시 이전씬의 옵션을 그대로 가져온다.
        SoundSetting(1, SoundManager.instance.SFX_Sound); //씬 시작시 이전씬의 옵션을 그대로 가져온다.

        //BG_Slider.value = (SoundManager.instance.BG_Sound + 40.0f) / 40.0f;
        //SFX_Slider.value = (SoundManager.instance.SFX_Sound + 40.0f) / 40.0f;
    }

    public void SoundSetting(int option, float value) //로비씬의 사운드 관리
    {
        switch (option)
        {
            case 0: //BG 음량 조절
                audioMixer.SetFloat("BG_Volume", value);

                break;

            case 1: //SFX 음량 조절
                audioMixer.SetFloat("SFX_Volume", value);

                break;
        }
    }

    public void ActiveSound(GameObject target, AudioClip clip, float MaxDist, bool awakePlay, bool loop, bool reside, int option, float? clipLength = null)
    {//소리가 나는 오브젝트, 소리, 소리가 들릴 최대 거리, 바로 재생할지,반복 유무, 사운드 박스를 상주 시킬지,소리 타입, (미입력시:클립 길이만큼 재생/입력시:값 만큼 클립 재생)
        switch (option)
        {
            case 0: //BG
                GameObject BackGroundObj = Instantiate(SoundBox);
                AudioSource BG_audioSource = BackGroundObj.GetComponent<AudioSource>();
                BackGroundObj.transform.SetParent(target.transform); //소리를 내는 오브젝트에 자식으로 이동.
                BackGroundObj.transform.localPosition = Vector3.zero; //부모의 위치로 이동.

                BG_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BackGround")[0]; //오디오 믹서에서 찾은 BackGound 그룹중 1번째 그룹을 할당.
                BG_audioSource.playOnAwake = awakePlay;
                BG_audioSource.clip = clip;
                BG_audioSource.loop = loop;
                BG_audioSource.rolloffMode = AudioRolloffMode.Custom;
                BG_audioSource.maxDistance = MaxDist;
                BG_audioSource.volume = 1.0f;

                BackGroundObj.SetActive(true);

                if (loop == false && reside == false && clipLength == null) //클립의 길이만큼 한번 재생, 사운드 박스 비활성화
                {
                    BG_audioSource.Play();
                    Destroy(BackGroundObj, clip.length);
                }
                else if (loop == false && reside == false && clipLength != null) //지정한 만큼 한번 재생, 사운드 박스 비활성화
                {
                    BG_audioSource.Play();
                    Destroy(BackGroundObj, clipLength.Value);
                }
                else if (loop == false && reside == true && clipLength == null) //사운드 박스를 상주시키고 원할 때만 클립의 길이 만큼 재생을 위해 데이터 저장
                {
                    Data.Add(clip.name, new SoundData(BackGroundObj, BG_audioSource, clip.length));
                }
                else if (loop == false && reside == true && clipLength != null) //사운드 박스를 상주시키고 원할 때만 지정한 값 만큼 재생을 위해 데이터 저장
                {
                    Data.Add(clip.name, new SoundData(BackGroundObj, BG_audioSource, clipLength));
                }
                else if (loop == true && awakePlay == true) //사운드 박스를 상주시키고 반복 재생.
                {
                    BG_audioSource.Play();
                    Data.Add(clip.name, new SoundData(BackGroundObj, BG_audioSource, clip.length));
                }
                else if (loop == true && awakePlay == false) //사운드 박스를 상주시키고 사용자가 필요할 때만 반복 재생.
                {
                    Data.Add(clip.name, new SoundData(BackGroundObj, BG_audioSource, clip.length));
                }

                break;

            case 1: //SFX
                GameObject SFXObj = Instantiate(SoundBox);
                AudioSource SFX_audioSource = SFXObj.GetComponent<AudioSource>();
                SFXObj.transform.SetParent(target.transform); //소리를 내는 오브젝트에 자식으로 이동.
                SFXObj.transform.localPosition = Vector3.zero; //부모의 위치로 이동.

                SFX_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; //오디오 믹서에서 찾은 SFX그룹중 1번째 그룹을 할당.
                SFX_audioSource.playOnAwake = awakePlay;
                SFX_audioSource.clip = clip;
                SFX_audioSource.loop = loop;
                SFX_audioSource.rolloffMode = AudioRolloffMode.Custom;
                SFX_audioSource.maxDistance = MaxDist;
                SFX_audioSource.volume = 1.0f;
                SFX_audioSource.spatialBlend = 1.0f; //공간 음향 설정, 거리에 따라 소리감쇠가 생긴다.

                SFXObj.SetActive(true);

                if (loop == false && reside == false && clipLength == null) //클립의 길이만큼 한번 재생, 사운드 박스 비활성화
                {
                    SFX_audioSource.Play();
                    Destroy(SFXObj, clip.length);
                }
                else if (loop == false && reside == false && clipLength != null) //지정한 만큼 한번 재생, 사운드 박스 비활성화
                {
                    SFX_audioSource.Play();
                    Destroy(SFXObj, clipLength.Value);
                }
                else if (loop == false && reside == true && clipLength == null) //사운드 박스를 상주시키고 원할 때만 클립의 길이 만큼 재생을 위해 데이터 저장
                {
                    Data.Add(clip.name, new SoundData(SFXObj, SFX_audioSource, clip.length));
                }
                else if (loop == false && reside == true && clipLength != null) //사운드 박스를 상주시키고 원할 때만 지정한 값 만큼 재생을 위해 데이터 저장
                {
                    Data.Add(clip.name, new SoundData(SFXObj, SFX_audioSource, clipLength));
                }
                else if (loop == true && awakePlay == true) //사운드 박스를 상주시키고 반복 재생.
                {
                    SFX_audioSource.Play();
                    Data.Add(clip.name, new SoundData(SFXObj, SFX_audioSource, clip.length));
                }
                else if (loop == true && awakePlay == false) //사운드 박스를 상주시키고 사용자가 필요할 때만 반복 재생.
                {
                    Data.Add(clip.name, new SoundData(SFXObj, SFX_audioSource, clip.length));
                }

                break;
        }
    }

    public void EditSoundBox(string key, bool setActive, AudioClip clip = null) //루프상태인 사운드 박스를 편집하는 함수 (편집할 사운드박스 키, 활성화 여부, 교체할 사운드 클립)
    {
        if (setActive) // 사운드 박스를 계속 활성화 한다면 사운드 클립 교체
            Data[key].SoundBox_AudioSource.clip = clip;
        else // 사운드 박스를 비활성화 한다면 0.1초후 사운드 박스 비활성화
            Destroy(Data[key].SoundBox);
    }
}
