using UnityEngine;

public class SoundData
{ //오브젝트에 상주시키는 사운드 박스 데이터를 저장한다.
    public GameObject SoundBox;
    public AudioSource SoundBox_AudioSource;

    public float? PlayLength;

    public SoundData (GameObject soundBox, AudioSource soundBox_AudioSource, float? playLength)
    {
        SoundBox = soundBox;
        SoundBox_AudioSource = soundBox_AudioSource;
        PlayLength = playLength;
    }
}
