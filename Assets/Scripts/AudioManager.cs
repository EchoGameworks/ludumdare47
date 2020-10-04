using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public enum SoundEffects
    {
        None,
        CharacterLanding,
        CharacterHurt,
        CharacterDead,
        Restart,
        Powerup,
        IceOrbSpawn,
        IceOrbSplit,
        IceDagger,
        Swap,
        ShootBullet,
        ShootFire,
        UseShockStream,
        IcetopusHurt
    }

    public static AudioManager instance;
    private float TemporaryMuteTime_Menu = 0.35f;

    [Range(0f, 1f)]
    public float MasterMusicVolume = 1f;
    [Range(0f, 1f)]
    public float MasterSFXVolume = 1f;

    public List<Music> MusicPlaylist;
    public List<Sound> Sounds;

    float Mixer = 1f;

    [SerializeField] private bool canPlay = true;
    [SerializeField] private bool onHold = false;
    [SerializeField] private bool isNegativeOverruled = false;

    public void MixToReverse()
    {
        LeanTween.value(gameObject, mix => Mixer = mix, Mixer, 0f, 1f);
 
    }

    public void MixToForward()
    {
        LeanTween.value(gameObject, mix => Mixer = mix, Mixer, 1f, 1f);
        //MusicPlaylist[0].source.volume = MasterMusicVolume * MusicPlaylist[0].Volume * Mixer;
        //MusicPlaylist[1].source.volume = MasterMusicVolume * MusicPlaylist[1].Volume * (1 - Mixer);
    }

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
        PlaySong();
    }

    private void Update()
    {
        //MusicPlaylist[0].source.volume = MasterMusicVolume * MusicPlaylist[0].Volume * Mixer;
        //MusicPlaylist[1].source.volume = MasterMusicVolume * MusicPlaylist[1].Volume * (1 - Mixer);
    }

    public void PlaySong()
    {
        //MusicPlaylist[0].source.volume = MasterMusicVolume * MusicPlaylist[0].Volume * Mixer;
        //MusicPlaylist[0].source.Play();
        //MusicPlaylist[1].source.volume = MasterMusicVolume * MusicPlaylist[1].Volume * (1 - Mixer);
        //MusicPlaylist[1].source.Play();
    }

    

    //public void UpdateAudio(PlayerData pd)
    //{
    //    MasterMusicVolume = pd.vfx / 100f;
    //    MasterSFXVolume = pd.sfx / 100f;
    //    if(currentSong != null)
    //    {
    //        if(MasterMusicVolume == 0)
    //        {
    //            currentSong.source.Pause();
    //        }
    //        else
    //        {
    //            currentSong.source.volume = MasterMusicVolume * currentSong.Volume;
    //            if (!currentSong.source.isPlaying) PlaySong();
    //        }            
    //    }
    //}

    public void PlaySound(SoundEffects soundEffect)
    {
        Sound s = Sounds.FirstOrDefault(o => o.SoundName == soundEffect);
        if (s == null || !canPlay || onHold) return;
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
        PlaySound(SoundEffects.Restart);
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
