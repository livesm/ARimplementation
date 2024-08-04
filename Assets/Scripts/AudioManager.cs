using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; //this script manages the sounds of the game. Including sounds for the weather, background music and sounds connected to the buttons. All sounds are assigned in inspector.

    [SerializeField] AudioSource buttonClick;
    [SerializeField] AudioSource buttonWaterSource;
    [SerializeField] AudioSource buttonFoodSource;
    [SerializeField] AudioSource backgroundSoundSource;
    [SerializeField] AudioSource continuousBackgroundMusic;
    [SerializeField] AudioSource startSoundSource;
    [SerializeField] AudioSource countdownSoundSource;

    [SerializeField] AudioSource gameMusicSource;


    [SerializeField] private AudioSource rainAudioSource;  
    [SerializeField] private AudioSource snowAudioSource;


    private bool isBackgroundSoundPlaying = false;




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
        }
    }
    public void PlayButtonClick()
    {
        buttonClick.Play();
    }

    public void PlayWaterSound()
    {
        buttonWaterSource.Play();
    }



    public void PlayFoodSound()
    {
        buttonFoodSource.Play();
    }

    public void ToggleBackgroundSound() 
    {
        if (!isBackgroundSoundPlaying)
        {
            backgroundSoundSource.Play();
            isBackgroundSoundPlaying = true;
        }
        else{
            backgroundSoundSource.Stop();
            isBackgroundSoundPlaying = false;
        }
    }

    public void PlayContinuousMusic()
    {
        continuousBackgroundMusic.Play();
    }
    public void StopContinuousMusic()
    {
        continuousBackgroundMusic.Stop();
    }
    public void PlayStartSound()
    {
        startSoundSource.Play();
    }

    public void PlayCountdownSound()
    {
        countdownSoundSource.Play();
    }



    public void PlayGameMusic()
    {
        //Stops the backgroundmusic and plays the game music for the bird game
        continuousBackgroundMusic.Stop();
        gameMusicSource.Play();
    }

    public void StopGameMusic(){
        // stops the game music for the bird game and resumes with the continued background music 
        gameMusicSource.Stop();
        continuousBackgroundMusic.Play();
    }


    public void PlayRainSound() //next three is connected to the weather button and plays the sounds that corresponds with the current weatherconditions.
    {
        rainAudioSource.Play();
    }

    public void PlaySnowSound()//fjern?
    {
        snowAudioSource.Play();
    }

    public void StopWeatherSounds(){
        rainAudioSource.Stop();
        snowAudioSource.Stop();
       
    }

}
