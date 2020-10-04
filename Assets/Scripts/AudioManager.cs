using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    int songProgress = 0;

    public enum SoundEffects
    {
        None,

        CharacterLanding,
        CharacterHurt,
        CharacterDead,

        IceOrb_Spawn,
        IceOrb_Split,
        IcePlatform_Shatter,

        IceDagger_Spawn,
        IceDagger_Hit,   
        
        Icetopus_Hurt,
        Icetopus_Dead,

        Portal_Warp,
        Swap,

        Bullet_Shoot,
        ShootFire,
        UseShockStream,
        
    }

    public static AudioManager instance;
    private float TemporaryMuteTime_Menu = 0.35f;

    [Range(0f, 1f)]
    public float MasterMusicVolume = 1f;
    [Range(0f, 1f)]
    public float MasterSFXVolume = 1f;

    public List<Music> MusicPlaylist;
    public List<Sound> Sounds;

    [SerializeField] private bool canPlay = true;
    [SerializeField] private bool onHold = false;
    [SerializeField] private bool isNegativeOverruled = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Music m in MusicPlaylist)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.Clip;
            m.source.volume = m.Volume;
            m.source.pitch = m.Pitch;
            m.source.loop = true;
        }
        //UnheardMusicPlaylist = new List<Music>(MusicPlaylist);


        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
        }
        
        PlaySongs();
        MixSongs();
    }

    private void Update()
    {
        //MusicPlaylist[0].source.volume = MasterMusicVolume * MusicPlaylist[0].Volume * Mixer;
        //MusicPlaylist[1].source.volume = MasterMusicVolume * MusicPlaylist[1].Volume * (1 - Mixer);
    }
    
    public void MixSongs()
    {
        //print("sp: " + songProgress);
        switch(songProgress){
            case 0:
                LeanTween.value(gameObject, val => MusicPlaylist[0].source.volume = MasterMusicVolume * MusicPlaylist[0].Volume * val, 0f, 1f, 10f);
                MusicPlaylist[1].source.volume = 0f;
                MusicPlaylist[2].source.volume = 0f;
                MusicPlaylist[3].source.volume = 0f;
                break;
            case 1:
                LeanTween.value(gameObject, val => MusicPlaylist[0].source.volume = MasterMusicVolume * MusicPlaylist[0].Volume * val, 1f, 0f, 10f);
                LeanTween.value(gameObject, val => MusicPlaylist[1].source.volume = MasterMusicVolume * MusicPlaylist[1].Volume * val, 0f, 1f, 10f);
                break;
            case 2:
                LeanTween.value(gameObject, val => MusicPlaylist[1].source.volume = MasterMusicVolume * MusicPlaylist[1].Volume * val, 1f, 0f, 10f);
                LeanTween.value(gameObject, val => MusicPlaylist[2].source.volume = MasterMusicVolume * MusicPlaylist[2].Volume * val, 0f, 1f, 10f);
                break;
            case 3:
                LeanTween.value(gameObject, val => MusicPlaylist[2].source.volume = MasterMusicVolume * MusicPlaylist[2].Volume * val, 1f, 0f, 10f);
                LeanTween.value(gameObject, val => MusicPlaylist[3].source.volume = MasterMusicVolume * MusicPlaylist[3].Volume * val, 0f, 1f, 10f);
                break;
        }
        songProgress++;
    }

    public void PlaySongs()
    {
        MusicPlaylist[0].source.volume = MasterMusicVolume * MusicPlaylist[0].Volume;
        MusicPlaylist[0].source.Play();
        MusicPlaylist[1].source.volume = MasterMusicVolume * MusicPlaylist[1].Volume;
        MusicPlaylist[1].source.Play();
        MusicPlaylist[2].source.volume = MasterMusicVolume * MusicPlaylist[2].Volume;
        MusicPlaylist[2].source.Play();
        MusicPlaylist[3].source.volume = MasterMusicVolume * MusicPlaylist[3].Volume;
        MusicPlaylist[3].source.Play();

    }   



    public void PlaySound(SoundEffects soundEffect)
    {
        Sound s = Sounds.FirstOrDefault(o => o.SoundName == soundEffect);
        //print("playing" + s.SoundName);
        if (s == null || !canPlay || onHold) return;
        //print("def playing" + s.SoundName);
        s.source.volume = MasterSFXVolume * s.Volume;
        s.source.pitch = s.Pitch;
        //print("playing: " + soundEffect + " at " + s.source.volume + " (" + MasterSFXVolume + " | " + s.Volume + ")");
        s.source.Play();
    }

    //public void Play_Positive()
    //{
    //    PlaySound(SoundEffects.Positive);
    //    TemporaryMute();
    //}

//    public void Play_Hover()
//    {
//#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE && !UNITY_EDITOR
//        return;
//#endif
//        PlaySound(SoundEffects.Hover);
//    }

//    public void Play_HoverConstant()
//    {
//        PlaySound(SoundEffects.Hover);
//    }

//    public void Play_ChangeConstellation()
//    {
//        PlaySound(SoundEffects.ChangeConstellation);
//    }

//    public void Play_SelectLevel()
//    {
//        PlaySound(SoundEffects.SelectLevel);
//    }

//    public void Play_Negative()
//    {
//        PlaySound(SoundEffects.Negative);
//    }

//    public void Play_PossibleNegative()
//    {
//        LeanTween.delayedCall(0.05f, () =>
//        {
//            if (!isNegativeOverruled) PlaySound(SoundEffects.Negative);            
//            onHold = true;
//        });
//    }

//    public void Play_Send()
//    {
//        PlaySound(SoundEffects.Send);
//    }

//    public void Play_ToggleClick()
//    {
//        PlaySound(SoundEffects.ToggleClick);
//    }

//    public void Play_Rotate()
//    {
//        Sound s = Sounds.FirstOrDefault(o => o.SoundName == SoundEffects.Rotate);
//        //s.source.pitch = Random.Range(s.Pitch - 0.02f, s.Pitch + 0.02f);
//        PlaySound(SoundEffects.Rotate);
//    }

//    public void Play_Menu()
//    {
//        PlaySound(SoundEffects.Menu);
//    }

    public void Play_Restart()
    {
        //PlaySound(SoundEffects.Restart);
    }

    //public void Play_SendPreview()
    //{
    //    isNegativeOverruled = true;
    //    PlaySound(SoundEffects.SendPreview);
    //    LeanTween.delayedCall(0.35f, () => isNegativeOverruled = false);
    //}

    //public void Play_StarPop()
    //{
    //    PlaySound(SoundEffects.StarEarned);
    //}

    //public void Play_Countdown()
    //{
    //    PlaySound(SoundEffects.Countdown);
    //}

    //public void Play_Undo()
    //{
    //    PlaySound(SoundEffects.Undo);
    //}

    //public void Play_StarCollected()
    //{
    //    PlaySound(SoundEffects.StarCollected);
    //}

    private void TemporaryMute(float muteTime = 0.35f)
    {
        canPlay = false;
        LeanTween.delayedCall(muteTime, () => canPlay = true);
    }

    public void ReleaseHold()
    {
        LeanTween.delayedCall(0.1f, () => onHold = false);
    }
}
