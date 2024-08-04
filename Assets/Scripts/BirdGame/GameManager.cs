using UnityEngine;
using TMPro; // bruka textmeshpro, husk i inspector 
using UnityEngine.UI; //
using System.Collections;

public class GameManager : MonoBehaviour //script to control the bird game - from start button is pressed to the end
{
    public static GameManager instance;

    public int score = 0;
    public float gameTime = 30.0f; // Total game time in seconds (countdown to create a sense of pressure)
    public TMP_Text scoreText; // score displayed and updated throughout
    public TMP_Text timerText; // timer to keep track of time
    public TMP_Text countdownText; // added countdown when the start button is pressed
    public GameObject bird; 
    public GameObject joystick; // joystick that controls the movements of the bird
    public Button healthButton; // button that enables the game (called health button from start because game logic was initially different)
    public Button startButton; //button to start the game and the countdown
    public TMP_Text finalScoreText; // display the final score after the 30 seconds have gone

    private bool isGameActive = false;
    private bool isGameEnabled = false;
    private bool isGameEnded = false;

    private bool isCountdownSoundPlayed = false;

    private void Awake()
    {
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }

    }

    private void Start(){
        UpdateScoreText();
        UpdateTimerText();
        bird.SetActive(false);
        joystick.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false); 
        healthButton.onClick.AddListener(ToggleGameEnabled);
        startButton.onClick.AddListener(StartCountdown);
    }

    private void Update()
    {
        if (isGameActive && gameTime > 0)
        {
            gameTime -= Time.deltaTime;
            UpdateTimerText();

            if (gameTime <= 8 && !isCountdownSoundPlayed){
                AudioManager.instance.PlayCountdownSound();
                isCountdownSoundPlayed = true; // Set the flag to true to prevent repeated playing
            }

            if (gameTime <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddScore(int amount){
        score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.text = "Score: " + score;
    }
    private void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.Ceil(gameTime);
    }

    private void EndGame(){
        // end game logic
        Debug.Log("Game Over! Final Score: " + score);

        // Display the final score in the middle of the screen
        finalScoreText.text = "Final Score: " + score;
        finalScoreText.gameObject.SetActive(true);

        // Stop game music and resume background music
        AudioManager.instance.StopGameMusic();

        isGameActive = false;
        isGameEnded = true;
        isCountdownSoundPlayed = false; // Reset the flag
    }

    private void ToggleGameEnabled() //litt kronglete måte å gjere det på - men brukt toggle ellers også
    { 
        isGameEnabled = !isGameEnabled;

        //active state of the game elements based on the game enabled state
        bird.SetActive(isGameEnabled);
        joystick.SetActive(isGameEnabled);
        scoreText.gameObject.SetActive(isGameEnabled);
        timerText.gameObject.SetActive(isGameEnabled);
        startButton.gameObject.SetActive(isGameEnabled);

        // when disabled, reset everything
        if (!isGameEnabled){
            finalScoreText.gameObject.SetActive(false);
            countdownText.gameObject.SetActive(false);
            isGameActive = false;
            isGameEnded = false;
            isCountdownSoundPlayed = false; 
        }
    }

    private void StartCountdown(){

        // Play the start sound
        AudioManager.instance.PlayStartSound();

        // Disable the start button
        startButton.gameObject.SetActive(false);
        // Start the countdown coroutine
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        // Display the countdown text
        countdownText.gameObject.SetActive(true);

        
        for (int i = 3; i > 0; i--)// Countdown
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // Hide the countdown text and start the game
        countdownText.gameObject.SetActive(false);
        StartGame();
    }

    private void StartGame()
    {
        // Stop continuous background music and play game music
        AudioManager.instance.PlayGameMusic();

        // Reset the game state if starting the game 
        score = 0;
        gameTime = 30.0f;
        UpdateScoreText();
        UpdateTimerText();
        isGameActive = true;
    }
}