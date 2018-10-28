using UnityEngine;

public enum Music { Techno, Rock, Smooth, Groove, Indian, Noble, Prestige, Arcade, Punk, Spiritual }
public enum SFX { Bang, Spring, Boom, Laser, Clang, Titter, Smack, Whip1, Whip2, Whoosh }

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    // All Music and SFX are purchased and Free-to-use licence
    // https://www.promusicpack.com/license.html
    // http://www.soundeffectpack.com/license.html

    public AudioClip[] MusicFiles = new AudioClip[10];
    public AudioClip[] SFXFiles = new AudioClip[10];

    private AudioSource[] sources;
    private int SFXChannels = 0;
    private int CurrSFXChannel = 1;
    private MGC Controller;
    private bool MusicOn;
    private float MusicVol;
    private bool SFXON;
    private float SFXVol;
    private Music PlayerPref;

    /// <summary>
    /// DEVELOPER NOTE
    /// Loads up all AudioSources attached to parent object and polulates the sources[] array
    /// sources[0] is reserved for the BackGround Music (and loops)
    /// All others are assigned as SFX channels and used in sequence (as required, and don't loop)
    /// </summary>

    void Start ()
    {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        // Populate Controller with Music settings
        GetPlayerSoundPref();
        sources = GetComponents<AudioSource>();
        SFXChannels = sources.Length - 1;
        Debug.Log("Sound Manager has " + SFXChannels.ToString() + " SFX Channels");
        MusicOn = Controller.Music_ON;
        SFXON = Controller.SFX_ON;
        PlayerPref = Controller.MusicChoice;
        PlayMusic(PlayerPref);
    }

    public void PlayMusic(Music choice)
    {
        sources[0].clip = MusicFiles[(int)choice];
        sources[0].loop = true;
        sources[0].Play(0);
        if (!MusicOn) PauseMusic();
    }

    public void PauseMusic ()
    {
        sources[0].Pause();
    }

    public void ResumeMusic ()
    {
        if (MusicOn)
        {
            sources[0].UnPause();
        }
    }

    public void KillMusic ()
    {
        sources[0].clip = null;
    }

    public void SetMusicVolume (float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1f);
        sources[0].volume = volume;
    }

    public void PlaySFX(SFX choice)
    {
            sources[CurrSFXChannel].clip = SFXFiles[(int)choice];
            sources[CurrSFXChannel].loop = false;
            sources[CurrSFXChannel].Play(0);
            if (!SFXON) PauseSFX();
            ToggleChannel();
    }

    public void PauseSFX ()
    {
        for (int i = 1; i <= SFXChannels; i++)
        {
            sources[i].Pause();
        }
    }

    public void ResumeSFX ()
    {
        if (SFXON)
        {
            for (int i = 1; i <= SFXChannels; i++)
            {
                sources[i].UnPause();
            }
        }
    }

    public void KillSFX ()
    {
        for (int i = 1; i <= SFXChannels; i++)
        {
            sources[i].clip = null;
        }
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1f);
        for (int i = 1; i <= SFXChannels; i++)
        {
            sources[i].volume = volume;
        }
    }

    public void KillAllSounds ()
    {
        KillMusic();
        KillSFX();
    }

    public void PauseAllSounds ()
    {
        PauseMusic();
        PauseSFX();
    }
    public void ResumeAllSounds ()
    {
        ResumeMusic();
        ResumeSFX();
    }

    private void ToggleChannel ()
    {
        if (CurrSFXChannel == SFXChannels) { CurrSFXChannel = 1; }
        else { CurrSFXChannel++; }
    }

    void Update ()
    {
        // probably better to have this as a struct one day
        if (Controller.Music_ON != MusicOn)
        {
            MusicOn = Controller.Music_ON;
            if (Controller.Music_ON) // music switched ON
            {
                if (sources[0].clip !=null)
                    { ResumeMusic(); }
                else
                    { PlayMusic(PlayerPref); }
            }
            else // music switched OFF
            {
                PauseMusic();
            }
        }

        if (Controller.SFX_ON != SFXON)
        {
            SFXON = Controller.SFX_ON;
            if (Controller.SFX_ON) // SFX switched ON
                {
                    // nothing to do 
                }
            else // SFX switched OFF
                { KillSFX(); }
        }

        if (Controller.Music_Vol != MusicVol)
        {
            MusicVol = Controller.Music_Vol;
            SetMusicVolume(Controller.Music_Vol);
        }

        if (Controller.SFX_Vol != SFXVol)
        {
            SFXVol = Controller.SFX_Vol;
            SetSFXVolume(Controller.SFX_Vol);
        }

        if (Controller.MusicChoice != PlayerPref)
        {
            PlayerPref = Controller.MusicChoice;
            PlayMusic(PlayerPref);
            SetPlayerSoundPref();
        }

    }
    ////////////////////////////////////
    // PLAYER MUSIC PREFERENCE Get/Sets
    void GetPlayerSoundPref ()
    {
        if (!PlayerPrefs.HasKey("MusicChoice")) // new User best populate with game Defaults
        {
            SetPlayerSoundPref();
        }
        else
        {
            int MyC = PlayerPrefs.GetInt("MusicChoice");
            Controller.MusicChoice = (Music)MyC;
            Controller.Music_Vol = PlayerPrefs.GetFloat("MusicVol");
            int myBool = PlayerPrefs.GetInt("MusicOn");
            if (myBool == 0) { Controller.Music_ON = false;  }
            else { Controller.Music_ON = true; }
            Controller.SFX_Vol = PlayerPrefs.GetFloat("SFXVol");
            myBool = PlayerPrefs.GetInt("SFXOn");
            if (myBool == 0) { Controller.SFX_ON = false; }
            else { Controller.SFX_ON = true; }
        }


    }

    // IMPORTANT ... Currently not implemeted in the GUI.  Needs to be called when e.g. any "Sounds options select" window is CLOSED
    // it "is" called when someone updates "Music Choice" in teh Inspector ... because it would be annoying oherwise ;p
    // but clearly Users won't have that luxury
    public void SetPlayerSoundPref ()
    {
        PlayerPrefs.SetInt("MusicChoice", (int)Controller.MusicChoice);
        PlayerPrefs.SetFloat("MusicVol", Controller.Music_Vol);
        int MyBool = 0;
        if (Controller.Music_ON) MyBool = 1;
        PlayerPrefs.SetInt("MusicOn", MyBool);
        PlayerPrefs.SetFloat("SFXVol", Controller.SFX_Vol);
        MyBool = 0;
        if (Controller.SFX_ON) MyBool = 1;
        PlayerPrefs.SetInt("SFXOn", MyBool);
        PlayerPrefs.Save();
    }
    // End of PLAYER MUSIC PREFERENCE Get/Sets
    ////////////////////////////////////
}
