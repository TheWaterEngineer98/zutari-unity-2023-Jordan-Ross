using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // gives access to web requests we want to use
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GetKimberleyTemp : MonoBehaviour
{
public double CelciusTemp;
public TextMeshProUGUI CTTemp;
#region WeatherKey
const string APIKey = "f69462e1d504ac720627a6dae35644af";//API Key from OpenWeatherMap
#endregion
   
    public enum EPhase// Keeps track of successfull phases
    {
        NotStarted,
        GetWeatherData,

        Failed,
        Succeeded
    }

       class geoPluginResponse // C
    {
        [JsonProperty("geoplugin_request")] public string Request { get; set; }
        [JsonProperty("geoplugin_status")] public int Status { get; set; }
        [JsonProperty("geoplugin_delay")] public string Delay { get; set; }
        [JsonProperty("geoplugin_credit")] public string Credit { get; set; }
        [JsonProperty("geoplugin_city")] public string City { get; set; }
        [JsonProperty("geoplugin_region")] public string Region { get; set; }
        [JsonProperty("geoplugin_regionCode")] public string RegionCode { get; set; }
        [JsonProperty("geoplugin_regionName")] public string RegionName { get; set; }
        [JsonProperty("geoplugin_areaCode")] public string AreaCode { get; set; }
        [JsonProperty("geoplugin_dmaCode")] public string DMACode { get; set; }
        [JsonProperty("geoplugin_countryCode")] public string CountryCode { get; set; }
        [JsonProperty("geoplugin_countryName")] public string CountryName { get; set; }
        [JsonProperty("geoplugin_inEU")] public int InEU { get; set; }
        [JsonProperty("geoplugin_euVATrate")] public bool EUVATRate { get; set; }
        [JsonProperty("geoplugin_continentCode")] public string ContinentCode { get; set; }
        [JsonProperty("geoplugin_continentName")] public string ContinentName { get; set; }
        [JsonProperty("geoplugin_latitude")] public string Latitude { get; set; }
        [JsonProperty("geoplugin_longitude")] public string Longitude { get; set; }
        [JsonProperty("geoplugin_locationAccuracyRadius")] public string LocationAccuracyRadius { get; set; }
        [JsonProperty("geoplugin_timezone")] public string TimeZone { get; set; }
        [JsonProperty("geoplugin_currencyCode")] public string CurrencyCode { get; set; }
        [JsonProperty("geoplugin_currencySymbol")] public string CurrencySymbol { get; set; }
        [JsonProperty("geoplugin_currencySymbol_UTF8")] public string CurrencySymbolUTF8 { get; set; }
        [JsonProperty("geoplugin_currencyConverter")] public double CurrencyConverter { get; set; }
    }
    public class OpenWeather_Coordinates
    {
        [JsonProperty("lon")] public double Longitude { get; set; }
        [JsonProperty("lat")] public double Latitude { get; set; }
    }
    // Condition Info: https://openweathermap.org/weather-conditions
    public class OpenWeather_Condition
    {
        [JsonProperty("id")] public int ConditionID { get; set; }
        [JsonProperty("main")] public string Group { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
    }
    public class OpenWeather_KeyInfo
    {
        [JsonProperty("temp")] public double Temperature { get; set; }
        [JsonProperty("feels_like")] public double Temperature_FeelsLike { get; set; }
        [JsonProperty("temp_min")] public double Temperature_Minimum { get; set; }
        [JsonProperty("temp_max")] public double Temperature_Maximum { get; set; }
        [JsonProperty("pressure")] public int Pressure { get; set; }
        [JsonProperty("sea_level")] public int PressureAtSeaLevel { get; set; }
        [JsonProperty("grnd_level")] public int PressureAtGroundLevel { get; set; }
        [JsonProperty("humidity")] public int Humidity { get; set; }
    }
    public class OpenWeather_Wind
    {
        [JsonProperty("speed")] public double Speed { get; set; }
        [JsonProperty("deg")] public int Direction { get; set; }
        [JsonProperty("gust")] public double Gust { get; set; }
    }
    public class OpenWeather_Clouds
    {
        [JsonProperty("all")] public int Cloudiness { get; set; }
    }
    public class OpenWeather_Rain
    {
        [JsonProperty("1h")] public int VolumeInLastHour { get; set; }
        [JsonProperty("3h")] public int VolumeInLast3Hours { get; set; }
    }
    public class OpenWeather_Snow
    {
        [JsonProperty("1h")] public int VolumeInLastHour { get; set; }
        [JsonProperty("3h")] public int VolumeInLast3Hours { get; set; }
    }
    public class OpenWeather_Internal
    {
        [JsonProperty("type")] public int Internal_Type { get; set; }
        [JsonProperty("id")] public int Internal_ID { get; set; }
        [JsonProperty("message")] public double Internal_Message { get; set; }
        [JsonProperty("country")] public string CountryCode { get; set; }
        [JsonProperty("sunrise")] public int SunriseTime { get; set; }
        [JsonProperty("sunset")] public int SunsetTime { get; set; }
    }
    class OpenWeatherResponse
    {
        [JsonProperty("coord")] public OpenWeather_Coordinates Location { get; set; }
        [JsonProperty("weather")] public List<OpenWeather_Condition> WeatherConditions { get; set; }
        [JsonProperty("base")] public string Internal_Base { get; set; }
        [JsonProperty("main")] public OpenWeather_KeyInfo KeyInfo { get; set; }
        [JsonProperty("visibility")] public int Visibility { get; set; }
        [JsonProperty("wind")] public OpenWeather_Wind Wind { get; set; }
        [JsonProperty("clouds")] public OpenWeather_Clouds Clouds { get; set; }
        [JsonProperty("rain")] public OpenWeather_Rain Rain { get; set; }
        [JsonProperty("snow")] public OpenWeather_Snow Snow { get; set; }
        [JsonProperty("dt")] public int TimeOfCalculation { get; set; }
        [JsonProperty("sys")] public OpenWeather_Internal Internal_Sys { get; set; }
        [JsonProperty("timezone")] public int Timezone { get; set; }
        [JsonProperty("id")] public int CityID { get; set; }
        [JsonProperty("name")] public string CityName { get; set; }
        [JsonProperty("cod")] public int Internal_COD { get; set; }

    }
    const string URL_GetWeatherData = "https://api.openweathermap.org/data/2.5/weather";
    public EPhase Phase { get; private set; } = EPhase.NotStarted;

    OpenWeatherResponse WeatherData;
    bool ShownWeatherInfo = false;
    void Start()// Start is called before the first frame update
    {
        if(string.IsNullOrEmpty(APIKey))
        {
            Debug.Log("No APIKey found for OpenWeatherMap");
        }
        else
        {
             Debug.Log("APIKey found for OpenWeatherMap");
        }
    }    
    void Update() // Update is called once per frame
    {
        //we do not want to request information from the online server too frequently as this can lead to being blovcked out for periods of time.
        //It would be better to setup coroutines

        if (Phase == EPhase.Succeeded && !ShownWeatherInfo)
        {
        ShownWeatherInfo = true;

        CTTemp = gameObject.GetComponent<TextMeshProUGUI>();
        CelciusTemp = WeatherData.KeyInfo.Temperature - 273.15;
        CTTemp.text = CelciusTemp.ToString("F2");

        //     Debug.Log($"Weather info for {WeatherData.CityName}");
        //     Debug.Log($"Temperature: {WeatherData.KeyInfo.Temperature}");
        //     Debug.Log($"Humidity: {WeatherData.KeyInfo.Humidity}");
        //     Debug.Log($"Pressure: {WeatherData.KeyInfo.Pressure}");

        // foreach(var condition in WeatherData.WeatherConditions)
        // {
        //     Debug.Log($"{condition.Group}: {condition.Description}");
        // }
        }
        StartCoroutine (Time_delay());
         StartCoroutine(GetWeather_WeatherInformation());
    }

     
     IEnumerator Time_delay()
     {
        yield return new WaitForSeconds(5f);
     }

    IEnumerator GetWeather_WeatherInformation()
    {
        Phase = EPhase.GetWeatherData;
        string weatherURL = URL_GetWeatherData;
        weatherURL += $"?lat={-28.732}";
        weatherURL += $"&lon={24.762}";
        weatherURL += $"&APPID={APIKey}";
        
         using (UnityWebRequest request = UnityWebRequest.Get(weatherURL))
        {
            request.timeout = 1;
            yield return request.SendWebRequest();
        // we must check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {

           WeatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(request.downloadHandler.text);
            Phase = EPhase.Succeeded;
        }
        else
        {
            Debug.Log($"Failed to get weather data: {request.downloadHandler.text}");
            Phase = EPhase.Failed;
        }
        yield return null;
    }
    }
}
