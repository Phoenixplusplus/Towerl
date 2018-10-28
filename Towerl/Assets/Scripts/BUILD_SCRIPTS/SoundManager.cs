using UnityEngine;
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
    private bool SFXON;
    private Music PlayerPref;

    public enum Music { Techno, Rock, Smooth, Groove, Indian, Noble, Prestige, Arcade, Punk, Spiritual }


    void Start ()
    {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        sources = GetComponents<AudioSource>();
        SFXChannels = sources.Length - 1;
        Debug.Log("Sound Manager has " + SFXChannels.ToString() + " SFX Channels");
        MusicOn = Controller.Music_ON;
        SFXON = Controller.SFX_ON;
        //PlayerPref = (Music)Controller.MusicChoice; // Need to come back to this ....
        PlayMusic(Music.Punk);
    }

    public void PlayMusic (Music choice)
    {
        sources[0].clip = MusicFiles[(int)choice];
        sources[0].Play(0);
    }



    public void PauseMusic ()
    {

    }

    public void ResumeMusic ()
    {

    }

    public void KillMusic ()
    {

    }

    public void PlaySFX ()
    {

    }
    public void PauseSFX ()
    {

    }

    public void ResumeSFX ()
    {

    }

    public void KillSFX ()
    {

    }

    public void KillAllSounds ()
    {

    }
    public void PauseAllSounds ()
    {

    }
    public void ResumeAllSounds ()
    {

    }

    private void ToggleChannel ()
    {
        if (CurrSFXChannel == SFXChannels) { CurrSFXChannel = 1; }
        else { CurrSFXChannel++; }
    }

    void Update ()
    {
        // Listen for changes to bools
        // Listen for changes to PlayerMusic Pref
    }

}
