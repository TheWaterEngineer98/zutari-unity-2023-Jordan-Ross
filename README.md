# zutari-unity-2023-Jordan-Ross
A repository for the zutari unity technical assessment 

The upload includes the following:
A build that includes the Main Menu and Level One Scenes.
The third scene WeatherApp should be accessed by playing the scene in Unity.
C# Scripts that will be reffered to in readme and comments can be viewed in script.
Special focus should be given to scripts: GetBhishoHumidity, MoveCube and Scene Changer

Unity Editor Version: 2021.3.19f1 
******************************************************************************************************************************************
Instructions from Assessment:
Task 1:
• Create two scenes: Main Menu and Level One.
• In Main Menu scene, add a UI Button that will load in the Level One scene when the button
is clicked.
• Create a script to handle the loading of Level One.
• In Level One scene, set the camera to ‘orthographic’.
• Add a cube and set its position so that it is in the middle of the camera view.
• Write a script that moves the cube with physics (i.e. using a rigidbody on the cube) up,
down, left and right using the WASD keys.
• The cube should change to a different colour for each direction it goes (e.g., blue if moving
left, yellow if moving down etc).
• If the cube reaches the edges of the camera view it should re-appear on the opposite edge
of the view (e.g. if the cube goes off the left side of the view, it should re-appear on the right
side of the screen).
• Give the player the ability to change the velocity the cube travels at. Use your discretion to
implement this, could be UI or keyboard inputs.
*************************************************************************************************************************************************



Main Menu Scene:
A UI image object is created in the empty scene and a Textmeshpro button object is added. The Background image is set to the Zutari Background pulled from google and made uploaded as a sprite.

Zutari background is added as image and stretched to canvas by selecting stetch option and holding ALT + chossing the bottom right option. Select canvas object and change canvas scaler  --> UI scale mode: change from constant pixel size to scale with screen size.

Allow Unity to accept the TMP changes.
 A script is needed to change the scene. This is added to an empty object that is added to the on-click option for the button.
 I created a basicScenechanger function to be called when the botton is clicked.
 
 public void changeScene()
{
    SceneManager.LoadScene(sceneName); // Pass scene name string to unity scene manager/Loadscene Function
Debug.Log("Welcome to level one"); //Debug logs to console if function completes successfully
}
The scene name is a public string that can be changed in the empty object and could also be changed to go to the WeatherApp if wanted.


![image](https://user-images.githubusercontent.com/130214018/230980599-011dad0d-c1c8-4ab6-876d-1d99761e2cc3.png)














In level one a script is needed for the functionality of the cube object. The basic 3D cube object is added with dimensions (2,2,2) and placed at the origin (0,0,0)

The main camera projection must be changed from Perspective to Orthographic in the Inspector window.


********************************************************************************************************************************************************
Refer to the following script and comments for the MoveableCube:
********************************************************************************************************************************************************


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 movePosition;// this will change when WASD input is given
    private Renderer rend;// call renderer to change rigidbody material when new direction is used
  [SerializeField] private Material red;// [SeriealizeField] allows for external input as to colour to change both during and before initializing build
  [SerializeField] private Material green;
  [SerializeField] private Material orange;
  [SerializeField] private Material blue;
  [SerializeField] private Material black;
  [SerializeField] private float speed;// Player can input cube speed here
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();// object rb is associated to the rigidbody that this script is attached to
         rend = GetComponent<Renderer>();
         rend.enabled = true;
         rend.material = black;// cube will first appear black
    }
    
    void FixedUpdate() // FixedUpdate() gives the advantage to Update as it is called 100x per second whereas Update() calls script every frame 
    {

    float horizontalInput = Input.GetAxis("Horizontal");// built into unity , associated with WASD and up-down-left right keys. It is easier to use these than the Input.GetKey methods although both could be used

    float verticalInput = Input.GetAxis("Vertical");

    transform.Translate(new Vector3(horizontalInput,verticalInput,0)*speed*Time.fixedDeltaTime);// Multiply movement on action with user inputted speed and multiplied by Time.fixedDeltaTime. This does the same job as Time.deltaTime but used when in FixedUpdate.
       if(horizontalInput<0)
         rend.material = red;// cube is red when translating to left 
         if(horizontalInput>0)
         rend.material = blue;// cube is blue translating going to right 
         if(verticalInput<0)
         rend.material=orange;// cube is orange when transalting down
         if(verticalInput>0)
         rend.material=green;//cube is green when translating up
        
        //From main camera view the borders by whuch the cube is out of sight are (-14;14) in the x-direction and (-7;7) in the y direction. Therefore when the cube crosses these threshholds, the following code snaps the respective x or y positions to the other side of the players view. 
        if( transform.position.x <-14)
        transform.position = new Vector3 (14,transform.position.y, 0);
        if( transform.position.x >14)
        transform.position = new Vector3 (-14,transform.position.y, 0);
        if( transform.position.y <-7)
        transform.position = new Vector3 (transform.position.x,7, 0);
        if( transform.position.y >7)
        transform.position = new Vector3 (transform.position.x,-7, 0);
     }
}
****************************************************************************************************************************************************************
 At this point the cube should move left, right, up and down along the X and Y axis when pressing the respective WASD keys. The cube must be given a rigidbody component. Deselct gravity and constrain rotation in all 3 degrees of freedom.
 
 ![image](https://user-images.githubusercontent.com/130214018/230980710-8d9a1b32-98de-44f1-a0c5-63e9d2285800.png)



![image](https://user-images.githubusercontent.com/130214018/230980782-48ea6eef-a73f-48b9-91d7-d5c9ca684ff0.png)


![image](https://user-images.githubusercontent.com/130214018/230980839-e2056f32-cd10-4268-812f-86736622272c.png)


![image](https://user-images.githubusercontent.com/130214018/230980889-846ad821-598f-4265-98ce-90c4c2265fbd.png)

 
 
 Instructions for Task 2:
 Task 2:
 
• Create a new scene called WeatherApp.
• Sign up to https://openweathermap.org/ - there is a free tier.
• In Unity, develop the functionality to call the OpenWeather API and get the weather info for
the capital cities of each province in South Africa.
• Display the latest weather data (city name, current temperature, description (cloudy, sunny
etc) on a basic Unity UI.
• Call the API automatically upon playing the scene.

*****************************************************************************************************************************************************************

The WeatherApp scene is created and a UI object is created with TextMeshPro objects created showing the location, of the 9 Capital cities of South Africa's provinces, the respective temperatures and Humidity in each location.

It is noted at this point that the weatherApp does successfully run, although the means by which it was made to work are by no means optimal. A script was made to call the specific information with the latitudes and longitudes inputted for each capital city. These were obtained from OpenWeatherMap and correspond to the following:

Bhisho - [-32.847; 27.442]
Bloemfontein - [-29.121; 26.214]
Johannesburg - [-26.202; 28.044]
Pietermaritzburg - [-29.617; 30.393]
Polokwane - [-23.904; 29.469]
Mbombela - [-25.474; 30.970]
Kimberley - [-28.732; 24.762]
Mafikeng - [-25.865; 25.644]
Cape Town - [-33.926; 18.423]

Each script has this information corresponding to each city and either the temperature in kelvin is converted from JSON data to a double and a ToString() function is used to added this to the public TextMeshPro.text string.

Information was pulled from a coroutine method and an additional coroutine was added for a 5 second delay. This combination in the Update() function successfully populate the UI screen when the scene is played. However too many requests are logged with the API and this leads to being blocked out.

The GetBhishoHumidity script has full comments added that apply to all similar scripts.

 ********************************************************************************************************************************************************************
 
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
            Debug.Log($"Failed to get weather data: {request.downloadHandler.text}"); // Debug to console of failure
            Phase = EPhase.Failed;
        }
        yield return null;
    }
    }
}
*********************************************************************************************************************************************************************
 
 WeatherApp screen as seen in Game - Error in retrieving Mbombela Temperature.
 
 
 ![image](https://user-images.githubusercontent.com/130214018/230979823-40e474ba-9e05-4659-b5c5-e12ce033342b.png)


Weather App as seen in Scene:


 ![image](https://user-images.githubusercontent.com/130214018/230980319-f6760d03-3778-4b2a-b2aa-74402c7fbe90.png)

 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 

