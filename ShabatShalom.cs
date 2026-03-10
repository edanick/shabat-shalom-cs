using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ShabatShalom {
    /// <summary>
    /// Represents city information
    /// </summary>
    public class CityInfo {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; }
        public string Country { get; set; }
    }

    /// <summary>
    /// A C# library for calculating Shabbat times
    /// </summary>
    public class ShabatShalom {
        private readonly double latitude;
        private readonly double longitude;
        private readonly string timezone;
        private readonly double? elevation;

        // City database with coordinates and timezones
        private static readonly ConcurrentDictionary<string, CityInfo> cities = new ConcurrentDictionary<string, CityInfo> {
            ["Jerusalem"] = new CityInfo { Latitude = 31.7690, Longitude = 35.2163, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Tel Aviv"] = new CityInfo { Latitude = 32.0809, Longitude = 34.7806, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Haifa"] = new CityInfo { Latitude = 32.7940, Longitude = 34.9896, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Be'er Sheva"] = new CityInfo { Latitude = 31.2530, Longitude = 34.7915, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Rishon LeZion"] = new CityInfo { Latitude = 31.9730, Longitude = 34.7925, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Petah Tikva"] = new CityInfo { Latitude = 32.0870, Longitude = 34.8870, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Ashdod"] = new CityInfo { Latitude = 31.8024, Longitude = 34.6550, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Netanya"] = new CityInfo { Latitude = 32.3215, Longitude = 34.8573, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Bat Yam"] = new CityInfo { Latitude = 32.0164, Longitude = 34.7772, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["Bnei Brak"] = new CityInfo { Latitude = 32.0838, Longitude = 34.8340, Timezone = "Asia/Jerusalem", Country = "Israel" },
            ["New York"] = new CityInfo { Latitude = 40.7143, Longitude = -74.0060, Timezone = "America/New_York", Country = "USA" },
            ["London"] = new CityInfo { Latitude = 51.5099, Longitude = -0.1181, Timezone = "Europe/London", Country = "UK" },
            ["Paris"] = new CityInfo { Latitude = 48.8566, Longitude = 2.3522, Timezone = "Europe/Paris", Country = "France" },
            ["Los Angeles"] = new CityInfo { Latitude = 34.0522, Longitude = -118.2437, Timezone = "America/Los_Angeles", Country = "USA" },
            ["Toronto"] = new CityInfo { Latitude = 43.6532, Longitude = -79.3832, Timezone = "America/Toronto", Country = "Canada" },
            ["Miami"] = new CityInfo { Latitude = 25.7617, Longitude = -80.1918, Timezone = "America/New_York", Country = "USA" },
            ["Chicago"] = new CityInfo { Latitude = 41.8781, Longitude = -87.6298, Timezone = "America/Chicago", Country = "USA" },
            ["Buenos Aires"] = new CityInfo { Latitude = -34.6037, Longitude = -58.3816, Timezone = "America/Argentina/Buenos_Aires", Country = "Argentina" },
            ["Melbourne"] = new CityInfo { Latitude = -37.8136, Longitude = 144.9631, Timezone = "Australia/Melbourne", Country = "Australia" },
            ["Johannesburg"] = new CityInfo { Latitude = -26.2041, Longitude = 28.0473, Timezone = "Africa/Johannesburg", Country = "South Africa" }
        };

        public ShabatShalom(double latitude, double longitude, string timezone, double? elevation = null) {
            this.latitude = latitude;
            this.longitude = longitude;
            this.timezone = timezone;
            this.elevation = elevation;
        }

        // Default constructor using Jerusalem
        public ShabatShalom() : this(31.7690, 35.2163, "Asia/Jerusalem", null) {
        }

        /// <summary>
        /// Get Shabbat times for the current week
        /// </summary>
        public ShabbatTimes GetShabbatTimes() {
            var today = DateTime.Today;
            var friday = GetNextFriday(today);
            var saturday = GetNextSaturday(today);

            return new ShabbatTimes {
                Date = friday.Date.ToString("yyyy-MM-dd"),
                CandleLighting = CalculateCandleLighting(friday),
                Havdalah = CalculateHavdalah(saturday)
            };
        }

        /// <summary>
        /// Calculate candle lighting time (18 minutes before sunset)
        /// </summary>
        public DateTime CalculateCandleLighting(DateTime date) {
            var sunset = CalculateSunset(date);
            return sunset.AddMinutes(-18);
        }

        /// <summary>
        /// Calculate havdalah time (42 minutes after sunset)
        /// </summary>
        public DateTime CalculateHavdalah(DateTime date) {
            var sunset = CalculateSunset(date);
            return sunset.AddMinutes(42);
        }

        /// <summary>
        /// Simplified sunrise calculation
        /// </summary>
        public DateTime CalculateSunrise(DateTime date) {
            var dayOfYear = GetDayOfYear(date);
            var declination = CalculateDeclination(dayOfYear);
            var hourAngle = CalculateHourAngle(declination, -0.83);

            var sunriseTime = 12 - hourAngle / 15;
            return ConvertToTime(sunriseTime, date);
        }

        /// <summary>
        /// Simplified sunset calculation
        /// </summary>
        public DateTime CalculateSunset(DateTime date) {
            var dayOfYear = GetDayOfYear(date);
            var declination = CalculateDeclination(dayOfYear);
            var hourAngle = CalculateHourAngle(declination, -0.83);

            var sunsetTime = 12 + hourAngle / 15;
            return ConvertToTime(sunsetTime, date);
        }

        /// <summary>
        /// Get day of year
        /// </summary>
        private int GetDayOfYear(DateTime date) {
            var start = new DateTime(date.Year, 1, 1);
            return (int)(date - start).TotalDays + 1;
        }

        /// <summary>
        /// Calculate solar declination
        /// </summary>
        private double CalculateDeclination(int dayOfYear) {
            return -23.45 * Math.Cos((360 * (dayOfYear + 10) / 365) * Math.PI / 180);
        }

        /// <summary>
        /// Calculate hour angle
        /// </summary>
        private double CalculateHourAngle(double declination, double zenith) {
            var latRad = latitude * Math.PI / 180;
            var decRad = declination * Math.PI / 180;
            var zenRad = zenith * Math.PI / 180;

            var cosHourAngle = (Math.Cos(zenRad) - Math.Sin(latRad) * Math.Sin(decRad)) /
                             (Math.Cos(latRad) * Math.Cos(decRad));

            return Math.Acos(cosHourAngle) * 180 / Math.PI;
        }

        /// <summary>
        /// Convert decimal time to DateTime
        /// </summary>
        private DateTime ConvertToTime(double decimalTime, DateTime date) {
            var hours = (int)Math.Floor(decimalTime);
            var minutes = (int)Math.Floor((decimalTime - hours) * 60);
            var seconds = (int)Math.Floor(((decimalTime - hours) * 60 - minutes) * 60);

            return new DateTime(date.Year, date.Month, date.Day, hours, minutes, seconds);
        }

        /// <summary>
        /// Get next Friday from given date
        /// </summary>
        private DateTime GetNextFriday(DateTime date) {
            var dayOfWeek = (int)date.DayOfWeek;
            var daysUntilFriday = ((5 - dayOfWeek + 7) % 7) + 7;
            return date.AddDays(daysUntilFriday);
        }

        /// <summary>
        /// Get next Saturday from given date
        /// </summary>
        private DateTime GetNextSaturday(DateTime date) {
            var dayOfWeek = (int)date.DayOfWeek;
            var daysUntilSaturday = ((6 - dayOfWeek + 7) % 7) + 7;
            return date.AddDays(daysUntilSaturday);
        }

        /// <summary>
        /// Format time for display
        /// </summary>
        public string FormatTime(DateTime time) {
            return time.ToString("HH:mm");
        }

        /// <summary>
        /// Get city information by name
        /// </summary>
        public static CityInfo GetCityInfo(string cityName) {
            if (cities.TryGetValue(cityName, out var city)) {
                return city;
            }
            throw new ArgumentException($"City '{cityName}' not found in database");
        }

        /// <summary>
        /// Get all cities in a specific country
        /// </summary>
        public static Dictionary<string, CityInfo> GetCitiesInCountry(string country) {
            var result = new Dictionary<string, CityInfo>();
            foreach (var kvp in cities) {
                if (kvp.Value.Country == country) {
                    result[kvp.Key] = kvp.Value;
                }
            }
            return result;
        }

        /// <summary>
        /// Get all Israeli cities
        /// </summary>
        public static Dictionary<string, CityInfo> GetAllIsraeliCities() {
            return GetCitiesInCountry("Israel");
        }

        /// <summary>
        /// Search cities by name pattern
        /// </summary>
        public static Dictionary<string, CityInfo> SearchCities(string query) {
            var result = new Dictionary<string, CityInfo>();
            foreach (var kvp in cities) {
                if (kvp.Key.ToLower().Contains(query.ToLower())) {
                    result[kvp.Key] = kvp.Value;
                }
            }
            return result;
        }

        /// <summary>
        /// Check if it's currently Shabbat
        /// </summary>
        public bool IsShabat(DateTime date = default) {
            if (date == default)
                date = DateTime.Now;

            var friday = GetNextFriday(date);
            var saturday = GetNextSaturday(date);
            
            var candleLighting = CalculateCandleLighting(friday);
            var havdalah = CalculateHavdalah(saturday);
            
            return date >= candleLighting && date <= havdalah;
        }
    }

    /// <summary>
    /// Represents Shabbat times
    /// </summary>
    public class ShabbatTimes {
        public string Date { get; set; }
        public DateTime CandleLighting { get; set; }
        public DateTime Havdalah { get; set; }

        public override string ToString() {
            return $"Date: {Date}\n" +
                   $"Candle Lighting: {CandleLighting:HH:mm}\n" +
                   $"Havdalah: {Havdalah:HH:mm}";
        }
    }
}
