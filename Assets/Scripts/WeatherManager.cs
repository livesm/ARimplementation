using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;

public class WeatherManager : MonoBehaviour //script to access and update current weather conditions using api key 
{
    public GameObject rainEffect; // ref rain system in unity(in scene)
    public GameObject snowEffect; // snow particles in unity(in scene)
    private bool effectsEnabled = true; 

    public Text weatherInfoText; // display weather information
    public Image backgroundPanel; 

    public string apiKey = "17f8f2bb04a7e1fecc7dcacfe96a620c";  //api to access weater - api key blir deaktivert 07.08.24 for å unngå misbruk
    public string city = "Trondheim";  // Trondheim, Norway

    public bool useLatlng = true;  // 

    public string latitude = "63.4305";//coordinates to trondheim
    public string longitude = "10.3951";


    void Start(){
        rainEffect.SetActive(false);
        snowEffect.SetActive(false);

        Debug.Log("Weather Manager started, weather effects set to inactive.");
    }

    public void GetRealWeather(){
        string uri = "https://api.openweathermap.org/data/2.5/weather?"; //chose to use openweathermap for api and current weather conditions
        if (useLatlng)
        {
            uri += "lat=" + latitude + "&lon=" + longitude + "&units=metric&appid=" + apiKey;


            Debug.Log($"Using latitude and longitude: {latitude}, {longitude}");
        }
        else{
            uri += "q=" + city + "&units=metric&appid=" + apiKey;
            Debug.Log($"Using city name: {city}");
        }
        Debug.Log($"Weather API URI: {uri}");


        StartCoroutine(GetWeatherCoroutine(uri));
    }

    IEnumerator GetWeatherCoroutine(string uri){
        Debug.Log("Starting weather data request...");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError){
                Debug.LogError("Web request error: " + webRequest.error);
            }
            else
            {

                Debug.Log("Weather data received successfully.");
                ParseWeatherData(webRequest.downloadHandler.text);
            }
        }
    }

    void ParseWeatherData(string jsonData)
    {
        Debug.Log($"Parsing weather data: {jsonData}");
        SimpleJSON.JSONNode data = JSON.Parse(jsonData);
        var currentWeather = data["weather"][0]["main"].Value;
        var currentTemperature = data["main"]["temp"].AsFloat;

        Debug.Log($"Parsed Weather: {currentWeather}, Temperature: {currentTemperature}°C");
        string weatherPrompt = GenerateWeatherPrompt(currentWeather, currentTemperature);
        weatherInfoText.text = $"Weather: {currentWeather}\nTemperature: {currentTemperature}°C\n{weatherPrompt}";

        UpdateBackgroundColor(currentTemperature); 
        ApplyWeatherEffects(currentWeather, currentTemperature);
    }

    void UpdateBackgroundColor(float temperature)
    {
        Debug.Log($"Updating background color based on temperature: {temperature}°C");
        
    }

    string GenerateWeatherPrompt(string weather, float temperature)// give prompts to guide actions/tips
    {
        Debug.Log($"Generating prompt for Weather: {weather}, Temperature: {temperature}");
        if (temperature <= 0)
        {
            return "It's cold today, maybe your plants needs some extra light and warmth.";
        }
        else if (temperature >= 21)
        {
            return "It's hot outside! Make sure your plants get enough water and give them some shadow – they burn in the sun too.";
        }
        else if (weather.Contains("Rain"))
        {
            return "Your plants love the rain. Mabye open a window and let them enjoy it";
        }
        return "It's a mild day. Perfect weather for your plants to thrive ";
    }

    public void ApplyWeatherEffects(string weather, float temperature)
    {
        if (effectsEnabled)
        {
            
            Debug.Log($"Applying weather effects for Weather: {weather}, Temperature: {temperature}");// weather effects based on the current conditions
            rainEffect.SetActive(false);
            snowEffect.SetActive(false);

            if (temperature <= 0){
                snowEffect.SetActive(true);
                AudioManager.instance.PlaySnowSound();  //endre lyd
                //Debug.Log("Snow effect activated.");
            }
            else if (weather.Contains("Rain") || weather.Contains("Drizzle")){
                rainEffect.SetActive(true);
                AudioManager.instance.PlayRainSound();  // Play rain sound
                Debug.Log("Rain effect activated.");
            }
            else{
                Debug.Log("No specific weather conditions met. All effects are deactivated.");
                AudioManager.instance.StopWeatherSounds();  // Stop all weather sounds
            }
        }
        else
        {
            // Disable all weather effects
            rainEffect.SetActive(false);
            snowEffect.SetActive(false);
            AudioManager.instance.StopWeatherSounds();  // Stop all weather sounds when effects are toggled off
            Debug.Log("Weather effects deactivated.");
        }

    
        effectsEnabled = !effectsEnabled;
    }






}

