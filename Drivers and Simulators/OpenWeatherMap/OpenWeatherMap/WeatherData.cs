using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ASCOM.OpenWeatherMap
{
    [DataContract]
    public class WeatherList
    {
        //  "message":"accurate",
        [DataMember]
        public string message { get; set; }
        //  "cod":"200",
        [DataMember]
        public string cod { get; set; }
        //  "count":10,
        [DataMember]
        public int count { get; set; }
        //  "list":[{
        [DataMember]
        public List<WeatherData> list { get; set; }
    }

    [DataContract]
    public class WeatherData
    {
        [DataMember]
        public Coord coord { get; set; }

        [DataMember]
        public List<Weather> weather { get; set; }        //"weather":[{

        [DataMember(Name = "@base")]
        public string basestr { get; set; }        // "base":"cmc stations",

        [DataMember]
        public Main main { get; set; }

        [DataMember]
        public int visibility { get; set; }

        [DataMember]
        public Wind wind { get; set; }

        [DataMember]
        public Rain rain { get; set; }

        [DataMember]
        public Snow snow { get; set; }

        [DataMember]
        public Clouds clouds { get; set; }

        [DataMember]
        public int dt { get; set; }        // "dt":1442474132,

        [DataMember]
        public Sys sys { get; set; }

        [DataMember]
        public int id { get; set; }        // "id":2634053,

        [DataMember]
        public string name { get; set; }        // "name":"White Waltham",

        [DataMember]
        public int cod { get; set; }        // "cod":200}

    }

    public class Coord
    {
        // "lon":-0.78,
        public double lon { get; set; }
        // "lat":51.49},
        public double lat { get; set; }
    }    //{"coord":{

    public class Weather
    {
        // "id":500,
        public int id { get; set; }
        //"main":"Rain",
        public string main { get; set; }
        // "description":"light rain",
        public string description { get; set; }
        // "icon":"10d"},
        public string icon { get; set; }
    }

    public class Main
    {
        // "temp":282.53,
        public double? temp { get; set; }
        // "pressure":996,
        public double? pressure { get; set; }
        // "humidity":93,
        public int? humidity { get; set; }
        // "temp_min":281.15,
        public double? temp_min { get; set; }
        // "temp_max":284.26
        public double? temp_max { get; set; }
        public double? sea_level { get; set; }
        public double? grnd_level { get; set; }
    }    // "main":{

    public class Wind    // "wind":{
    {
        public double? speed { get; set; }        // "speed":4.1,

        public double? deg { get; set; }        // "deg":270,

        public double? gust { get; set; }       // "gust": 3.58}
    }

    public class Rain
    {
        // "1h":0.25},  change 1h to rate1h and 3h to rate3h
        public double? rate1h { get; set; }
        public double? rate3h { get; set; }
    }    // "rain":{

    public class Snow
    {
        // "1h":0.25},  change 1h to rate1h and 3h to rate3h
        public double? rate1h { get; set; }
        public double? rate3h { get; set; }
    }

    public class Clouds
    {
        // "all":92},
        public int? all { get; set; }
    }    // "clouds":{

    public class Sys
    {
        // "type":1,
        public int type { get; set; }
        // "id":5093,
        public int id { get; set; }
        // "message":0.0046,
        public double message { get; set; }
        // "country":"GB",
        public string country { get; set; }
        // "sunrise":1442468462,
        public int? sunrise { get; set; }
        // "sunset":1442513593}
        public int? sunset { get; set; }
    }    // "sys":{

}
