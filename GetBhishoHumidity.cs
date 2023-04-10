using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // gives access to web requests we want to use
using Newtonsoft.Json;// helps us to work with JSON data
using UnityEngine.UI;//Helps us print data to UI interfaces
using TMPro;//needed when changing textmeshpro objects
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GetBhishoHumidity : MonoBehaviour
{
public TextMeshProUGUI CTTemp;// Temporary holder of string value passed to textmeshpro on UI
#region WeatherKey
const string APIKey = "f69462e1d504ac720627a6dae35644af";//API Key from OpenWeatherMap we must keep this in a region to prevent it from being publicly available (safety cocnerns)
#endregion
   
    public enum EPhase// Keeps track of successfull phases
    {
        NotStarted,
        GetWeatherData,

        Failed,
        Succeeded
    }

       class geoPluginResponse // Coded to convert recieved JSON data from OpenWeatherMap Api to an object form that can be used by unity
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
   
    public class OpenWeather_Condition
    {
        [JsonProperty("id")] public int ConditionID { get; set; }
        [JsonProperty("main")] public string Group { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
    }
    public class OpenWeather_KeyInfo //KeyInfo class is important as this information shall be passed with a ToString()
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
    const string URL_GetWeatherData = "https://api.openweathermap.org/data/2.5/weather";//we will build the URL to be used as a combination of each countries longitude and latitude and the hidden API key.
    public EPhase Phase { get; private set; } = EPhase.NotStarted;// not started phase

    OpenWeatherResponse WeatherData;// openWeatherResponse object to store recieved JSON data
    bool ShownWeatherInfo = false;
    void Start()// Start is called before the first frame update
    {
        if(string.IsNullOrEmpty(APIKey)) // we need to check if an API is available
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

        if (Phase == EPhase.Succeeded && !ShownWeatherInfo)
        {
        ShownWeatherInfo = true;

        CTTemp = gameObject.GetComponent<TextMeshProUGUI>(); // CTTEMP is linked to identified TextMeshPro in unity software
        CTTemp.text = WeatherData.KeyInfo.Humidity.ToString("F2");// Obtain Humidity as a double and convert to string
        }
        StartCoroutine (Time_delay());// attempt to setup a time delay accessing network (this has proven to be unsuccessfull and leads to lockouts)
         StartCoroutine(GetWeather_WeatherInformation());// run coroutine to obtain API weather data
    }

     
     IEnumerator Time_delay()
     {
        yield return new WaitForSeconds(5f);
     }

    IEnumerator GetWeather_WeatherInformation()
    {
        Phase = EPhase.GetWeatherData;// Change of phase
        string weatherURL = URL_GetWeatherData; // we start to build the URL we want
        weatherURL += $"?lat={-32.847}"; // longitude of Bhisho
        weatherURL += $"&lon={27.442}"; // Latitude of Bhisho
        weatherURL += $"&APPID={APIKey}"; // API key entry
        
         using (UnityWebRequest request = UnityWebRequest.Get(weatherURL)) // Unitywebrequest object named request is used from Networking library to call API
        {
            request.timeout = 1;
            yield return request.SendWebRequest();
        // we must check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {

           WeatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(request.downloadHandler.text); // download JSON information
            Phase = EPhase.Succeeded; // success
             Debug.Log("Acquired weather data");// Debug to console if completed
        }
        else
        {
            Debug.Log($"Failed to get weather data: {request.downloadHandler.text}"); // Debud to console of failure
            Phase = EPhase.Failed;
        }
        yield return null;
    }
    }
}
