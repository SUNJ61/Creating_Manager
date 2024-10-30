using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private float BG_sound = 0f; //���ú� ���� -40 ~ 0 
    public float BG_Sound
    {
        get { return BG_sound; }
        set
        {
            BG_sound = (value * 40.0f) - 40.0f;
            PlayerPrefs.SetFloat("BG_Volume", value);
            ApplySound(0, BG_Sound);
        }
    }

    private float SFX_sound = 0f; //���ú� ���� -40 ~ 0 (-80~20�� (value * 100.0f) - 80.0f; ���)
    public float SFX_Sound
    {
        get { return SFX_sound; }
        set
        {
            SFX_sound = (value * 40.0f) - 40.0f;
            PlayerPrefs.SetFloat("SFX_Volume", value);
            ApplySound(1, SFX_Sound);
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        BG_Sound = PlayerPrefs.GetFloat("BG_Volume", 1.0f);
        SFX_Sound = PlayerPrefs.GetFloat("SFX_Volume", 1.0f);
    }


    private void ApplySound(int option, float value)
    {
        switch (option)
        {
            case 0: //BG ����
                if (LobbySoundManager.instance != null)
                    LobbySoundManager.instance.SoundSetting(option, value);
                else if (InGameSoundManager.instance != null)
                    InGameSoundManager.instance.SoundSetting(option, value);

                break;

            case 1: //SFX 
                if (LobbySoundManager.instance != null)
                    LobbySoundManager.instance.SoundSetting(option, value);
                else if (InGameSoundManager.instance != null)
                    InGameSoundManager.instance.SoundSetting(option, value);

                break;
        }
    }
}
