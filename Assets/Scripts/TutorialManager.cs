using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour//script that controls the introduction designed to introduce users to all features of the app.
{
    public TMP_Text speechText;  


    public GameObject speechBubble;  
    public Button introNextButton;  
    public Button tutorialNextButton; 

    public GameObject inboxPanel; 
    public TMP_Text infoText;  

    public GameObject foodButton; 
    public GameObject waterButton;  
    public GameObject healthButton; 

    public GameObject weatherButton;  

    private Vector3 originalScaleFood; 
    private Vector3 originalScaleWater;
    private Vector3 originalScaleHealth;
    private Vector3 originalScaleWeather;
    private Coroutine pulseCoroutine; 

    private string[] introMessages = new string[] {
        "Welcome to BloomBuddy, the AR game where your care makes everything bloom! Like a real pet, your digital companion needs your attention, affection, and care — and so do your plants! Maintain good care levels and you'll see your real-world plants flourish in augmented reality, creating a blossoming environment as a result of your nurturing.",
        "As you tend to your dog’s needs, you’ll see an immediate impact on the plant life around you. A happy dog brings a garden full of flowers, while neglect reflects in a wilting world. Both your dog and your plants depend on regular care from you. Follow the instructions, maintain balance, have fun, and watch your world bloom!"
        
        
    };

    private string[] tutorialMessages = new string[]
    {
        " ",
        " ",
        "Hey! I'm Dog and I’m your virtual pet. If you take care of me you will take care of your plants too.",
        "I'm hungry! Can I get some food?", //trigger pulsating food-button
        "Aah thank you!",
        "Both the plants and I are so thirsty now, scan the green-wall to see how your plants are doing.",
        "Can you give us some water?", //trigger pulsating water-button
        "Aah, we really needed that - just check the flowers again.",
        "A lot of animals love it when the flowers bloom, so lets play a game!",//health-button play
        "Before you start, touch the screen to plant your flowers. ",
        "You now have 30seconds to collect as many flowers as you can. Just press play. Press the plant-button again when the game is over.",
        "Wow, good job! Can you get even more next time?",
        "Finally, the weather affects how both me and the plants are doing. ",
        "Check the weather and get some tips on how to care for us.",//trigger weather button pulsating
        "Check the weather and get some tips on how to care for us.",
        "Well done, now you know how to take care of me. And remember if you keep me happy, the plants are happy!",
        "Have Fun!"


    };

    private int currentIntroStep = 0;
    private int currentStep = 0;

    void Start()
    {
        originalScaleFood = foodButton.transform.localScale;  
        originalScaleWater = waterButton.transform.localScale;
        originalScaleHealth = healthButton.transform.localScale;
        originalScaleWeather = weatherButton.transform.localScale;
        inboxPanel.SetActive(true);  
        infoText.text = introMessages[currentIntroStep];  
        introNextButton.onClick.AddListener(NextIntroStep);
        speechBubble.SetActive(false);  // Initially hide the main tutorial speech bubble
        tutorialNextButton.gameObject.SetActive(false);  // Initially hide the tutorial next button
        foodButton.GetComponent<Button>().onClick.AddListener(ButtonClickedFood); 
        waterButton.GetComponent<Button>().onClick.AddListener(ButtonClickedWater);
        healthButton.GetComponent<Button>().onClick.AddListener(ButtonClickedHealth);
        weatherButton.GetComponent<Button>().onClick.AddListener(ButtonClickedWeather);
    }

    void NextIntroStep(){
        currentIntroStep++;
        if (currentIntroStep < introMessages.Length)
        {
            infoText.text = introMessages[currentIntroStep];
            
        }
        else{
            StartMainTutorial();
        }
    }

    void StartMainTutorial(){
        // Transition UI from intro to main tutorial
        inboxPanel.SetActive(false);
        introNextButton.gameObject.SetActive(false);
        introNextButton.onClick.RemoveListener(NextIntroStep);//

        
        
        tutorialNextButton.gameObject.SetActive(true);
        tutorialNextButton.onClick.AddListener(NextStep);

        speechBubble.SetActive(true);

        ShowStep(currentStep);
    }





    void UpdateNextButtonVisibility(int stepIndex)
    {
       
        if (tutorialMessages[stepIndex] == "I'm hungry! Can I get some food?" ||
            tutorialMessages[stepIndex] == "Can you give us some water?" ||
            tutorialMessages[stepIndex] == "A lot of animals love it when the flowers bloom, so lets play a game!" ||
            tutorialMessages[stepIndex] == "You now have 30seconds to collect as many flowers as you can. Just press play. Press the plant-button again when the game is over." ||
            tutorialMessages[stepIndex] == "Check the weather and get some tips on how to care for us.")
        {
            tutorialNextButton.gameObject.SetActive(false);  // Hide the Next button
        }
        else
        {
            tutorialNextButton.gameObject.SetActive(true);  // Show the Next button
        }
    }

   void ShowStep(int stepIndex)
    {
        speechText.text = tutorialMessages[stepIndex];  // Update the speech bubble text

        if (tutorialMessages[stepIndex] == "I'm hungry! Can I get some food?"){
            if (pulseCoroutine != null)
                StopCoroutine(pulseCoroutine);
            pulseCoroutine = StartCoroutine(PulseButton(foodButton));  // Start pulsating the food button
        }
        else if (tutorialMessages[stepIndex] == "Can you give us some water?")
        {
            if (pulseCoroutine != null)
                StopCoroutine(pulseCoroutine);
            pulseCoroutine = StartCoroutine(PulseButton(waterButton));  // Start pulsating the water button
        }
        else if (tutorialMessages[stepIndex] == "A lot of animals love it when the flowers bloom, so lets play a game!"){
            if (pulseCoroutine != null)
                StopCoroutine(pulseCoroutine);
            pulseCoroutine = StartCoroutine(PulseButton(healthButton));  // Start pulsating the health button
        }
        else if (tutorialMessages[stepIndex] == "Check the weather and get some tips on how to care for us."){
            if (pulseCoroutine != null)
                StopCoroutine(pulseCoroutine);
            pulseCoroutine = StartCoroutine(PulseButton(weatherButton));  // Start pulsating the health button
        }
        UpdateNextButtonVisibility(stepIndex);
    }

    IEnumerator PulseButton(GameObject button)
    {
        Vector3 originalScale = button.transform.localScale;  // Use the actual button's original scale
        Vector3 maxScale = originalScale * 1.2f;  // Scale up button (max size)
        while (true)
        {
            for (float timer = 0; timer < 1; timer += Time.deltaTime)
            {
                button.transform.localScale = Vector3.Lerp(originalScale, maxScale, Mathf.Sin(timer * Mathf.PI));
                yield return null;
            }
        }
    }

    void ButtonClickedFood()
    {
        StopPulsing(foodButton);
        StartCoroutine(HideSpeechBubbleAndContinue());
        //NextStep();
    }

    void ButtonClickedWater()
    {
        StopPulsing(waterButton);
        StartCoroutine(HideSpeechBubbleAndContinue());
        
    }

    void ButtonClickedHealth(){
        StopPulsing(healthButton);
        
        NextStep();
    }

    void ButtonClickedWeather(){
        StopPulsing(weatherButton);
        NextStep();
    }


    void StopPulsing(GameObject button)
    {
        if (pulseCoroutine != null){
            StopCoroutine(pulseCoroutine);
            // Reset the button scale to its original scale depending on which button was clicked
            if (button == foodButton)
                button.transform.localScale = originalScaleFood;
                //-
            else if (button == waterButton)
                button.transform.localScale = originalScaleWater;
            else if (button == healthButton)
                button.transform.localScale = originalScaleHealth;
            else if (button == weatherButton)
                button.transform.localScale = originalScaleWeather;
        }
    }


    IEnumerator HideSpeechBubbleAndContinue()
    {
        speechBubble.SetActive(false);  // Hide the speech bubble
        yield return new WaitForSeconds(10);  //test for matching av timing
        speechBubble.SetActive(true);  // Show the speech bubble
        NextStep(); 
    }

    public void NextStep(){
        currentStep++;
        if (currentStep < tutorialMessages.Length){
            ShowStep(currentStep);
        }
        else{
            EndTutorial();
        }
    }

    void EndTutorial(){
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        // Hide the speech bubble and deactivate the tutorial next button
        speechBubble.SetActive(false);
        tutorialNextButton.gameObject.SetActive(false);  

       
        tutorialNextButton.onClick.RemoveAllListeners(); // Clean up all listeners

        Debug.Log("Tutorial completed successfully.");
    }



}
