using UnityEngine;
using UnityEngine.UI;

public class PetStatsManager : MonoBehaviour //this script controls the sicrular slining bars that indecates the dogs stats.
{
    public Slider hungerSlider;
    public Slider thirstSlider;

    public Slider healthSlider;//knappen til fuglespelet - endra spilldesignet ()

    public float hungerDecreaseRate = 0.5f; //valgt rate bassert på behov
    public float thirstDecreaseRate = 0.9f;//tenkt at vann er det vi er mest opptatt av
    public float healthImpactRate = 0.4f; 

    //  - tre indikasjonsfarga for å reflektere/varsle om tilstandsnivå
    public Color normalColor = new Color(0.5f, 0.8f, 0.5f, 1.0f); // Green
    public Color warningColor = new Color(0.8f, 0.8f, 0.4f, 1.0f); // Yellow
    public Color criticalColor = new Color(0.9f, 0.5f, 0.5f, 1.0f); // Red

    void Update()
    {
        // Decrease hunger and thirst over time
        hungerSlider.value -= Time.deltaTime * hungerDecreaseRate;
        thirstSlider.value -= Time.deltaTime * thirstDecreaseRate;

        // Check for low stats and decrease health accordingly
        if (hungerSlider.value < 20 || thirstSlider.value < 20)
        {
            healthSlider.value -= Time.deltaTime * healthImpactRate;
        }

        // Update slider colors based on their current values
        UpdateSliderColor(hungerSlider, hungerSlider.value);
        UpdateSliderColor(thirstSlider, thirstSlider.value);

        UpdateSliderColor(healthSlider, healthSlider.value);
    }

    void UpdateSliderColor(Slider slider, float currentValue)
    {
        if (currentValue < 20) {
            slider.fillRect.GetComponentInChildren<Image>().color = criticalColor;
        }
        else if (currentValue < 50)
        {
            slider.fillRect.GetComponentInChildren<Image>().color = warningColor;
        }
        else{
            slider.fillRect.GetComponentInChildren<Image>().color = normalColor;
        }
    }




    public void Feed()
    {
        hungerSlider.value = 100;
    }

    public void GiveWater()
    {
        thirstSlider.value = 100;
    }
}
