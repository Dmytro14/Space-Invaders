using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
// Audio sources for music and sound effects
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // Playlist of music tracks
    public List<AudioClip> musicPlaylist;
    public Dictionary<string, AudioClip> sfxClips;
    public AudioClip winMusic;

    public AudioClip ExplosionSfx;
    public AudioClip BonusSfx;
    public AudioClip BunkersRespawnSfx;
    public AudioClip ShipDeathSfx;
    public AudioClip InvaderdeathSfx;
    public AudioClip PlayerDamageSfx;
    public AudioClip ShootSfx;
    public AudioClip SelectSfx;
    public AudioClip InvadersResetSfx;

    // Current volume levels for music and sound effects
    public float musicVolume = 5f;
    public float sfxVolume = 5f;

    void Start()
    {
        // Set the initial volume levels for the audio sources
        musicSource.volume = 0.5f;
        sfxSource.volume = 0.5f;
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", sfxVolume);
        SetMusicVolume(0);
        SetSfxVolume(0);
        PlayRandomMusic();

        sfxClips = new Dictionary<string, AudioClip>();

        // Add sound effects to the dictionary
        sfxClips.Add("Explosion", ExplosionSfx); //
        sfxClips.Add("Bonus", BonusSfx); //
        sfxClips.Add("Bunkers", BunkersRespawnSfx);//
        sfxClips.Add("Ship", ShipDeathSfx); //
        sfxClips.Add("Invader", InvaderdeathSfx); //
        sfxClips.Add("Player", PlayerDamageSfx); //
        sfxClips.Add("Shoot", ShootSfx); //
        sfxClips.Add("Select", SelectSfx);
        sfxClips.Add("Reset", InvadersResetSfx); //
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayRandomMusic();
            Debug.Log("Rand");
        }
    }

    // Method to change the volume of the music audio source
    public void SetMusicVolume(int volume)
    {
        
        musicVolume += volume;
        musicSource.volume = musicVolume / 10;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        
    }

    // Method to change the volume of the sound effects audio source
    public void SetSfxVolume(int volume)
    {
        
        sfxVolume += volume;
        sfxSource.volume = sfxVolume / 10;
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
        
    }

    // Method to play a random track from the music playlist
    public void PlayRandomMusic()
    {
        // Set the music audio source to loop and play a random track from the playlist
        musicSource.loop = true;
        int index = Random.Range(0, musicPlaylist.Count);
        musicSource.clip = musicPlaylist[index];
        musicSource.Play();

        // Invoke the "PlayNextTrack" method when the current track finishes playing
        Invoke("PlayNextTrack", musicSource.clip.length);
    }
    private void PlayNextTrack()
    {
        // Select a new random track from the playlist
        int index = Random.Range(0, musicPlaylist.Count);
        musicSource.clip = musicPlaylist[index];

        // Play the new track and invoke this method again when it finishes
        musicSource.Play();
        Invoke("PlayNextTrack", musicSource.clip.length);
    }


    // Method to play a specific sound effect
    public void PlaySfx(string sfxName)
    {
        if (!PauseMenu.GameIsPaused)
        {
            // Get the audio clip for the specified sound effect
            AudioClip sfxClip = sfxClips[sfxName];

            // Set the audio source's clip and play it
            sfxSource.clip = sfxClip;
            sfxSource.Play();
        }
    }
    public void PlayWinMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        musicSource.PlayOneShot(winMusic);
    }
}
