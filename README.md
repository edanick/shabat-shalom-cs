# Details

### Author: Edan
### Version: 1.0.0

A C# library for calculating Shabbat times with accurate astronomical calculations

---

# Documentation

## Steps

### 1. Installation

> Add the project to your solution or reference the compiled DLL

### 2. Import

> Import ShabatShalom namespace

```csharp
using ShabatShalom;
```

---

# Samples

> Sample of Shabbat time calculation for Jerusalem

Jerusalem coordinates (31.7683° N, 35.2137° E) with timezone 'Asia/Jerusalem' will calculate candle lighting 18 minutes before sunset and havdalah 42 minutes after sunset for the upcoming Shabbat

> The library supports all major Israeli cities and international cities with built-in city database

---

# Functions

## Introduction

> There are 7 main functions in total

1. GetShabbatTimes
2. IsShabat
3. CalculateCandleLighting
4. CalculateHavdalah
5. CalculateSunrise
6. CalculateSunset
7. FormatTime

> Plus 4 static city database functions

1. GetCityInfo
2. GetCitiesInCountry
3. GetAllIsraeliCities
4. SearchCities

---

## GetShabbatTimes

> Returns Shabbat times for the current or specified week

```csharp
shabatShalom.GetShabbatTimes()
```

---

#### Example

> Gets Shabbat times for Jerusalem (default)

```csharp
using ShabatShalom;

// Default - Jerusalem (no parameters needed)
var shabatShalom = new ShabatShalom();

// Get Shabbat times for this week in Jerusalem
var shabbatTimes = shabatShalom.GetShabbatTimes();
Console.WriteLine(shabbatTimes);
```

**Result**:
```
Date: 2024-09-13
Candle Lighting: 18:23
Havdalah: 19:45
```

---

## IsShabat

> Returns boolean indicating if it's currently Shabbat at the given location

### Parameters

`date`: **DateTime** (optional)

```csharp
shabatShalom.IsShabat()
```

---

#### Example

> Checks if it's currently Shabbat in Jerusalem

```csharp
using ShabatShalom;

var shabatShalom = new ShabatShalom();
var currentlyShabat = shabatShalom.IsShabat();

Console.WriteLine(currentlyShabat);
```

**Result**:
`True` or `False`

---

## GetCityInfo

> Returns city information including coordinates and timezone

### Parameters

`cityName`: **string**

```csharp
ShabatShalom.GetCityInfo(cityName)
```

---

#### Example

> Gets information for Tel Aviv

```csharp
using ShabatShalom;

var telAvivInfo = ShabatShalom.GetCityInfo("Tel Aviv");
Console.WriteLine($"Lat: {telAvivInfo.Latitude}, Lon: {telAvivInfo.Longitude}");
```

**Result**:
```
Lat: 32.0853, Lon: 34.7818
```

---

## GetAllIsraeliCities

> Returns all Israeli cities in the database

```csharp
ShabatShalom.GetAllIsraeliCities()
```

---

#### Example

> Lists all Israeli cities

```csharp
using ShabatShalom;

var israeliCities = ShabatShalom.GetAllIsraeliCities();
foreach (var city in israeliCities.Keys)
{
    Console.WriteLine(city);
}
```

**Result**:
```
Jerusalem
Tel Aviv
Haifa
Be'er Sheva
Rishon LeZion
Petah Tikva
Ashdod
Netanya
Bat Yam
Bnei Brak
```

---

## Constructor with Specific Cities

> Create instances for specific cities using coordinates

### Parameters

`latitude`: **double**\
`longitude`: **double**\
`timezone`: **string**\
`elevation`: **double?** (optional)

```csharp
new ShabatShalom(latitude, longitude, timezone, elevation)
```

---

#### Example #1

> Creates instance for Jerusalem

```csharp
using ShabatShalom;

var jerusalem = new ShabatShalom(31.7690, 35.2163, "Asia/Jerusalem");
var jerusalemTimes = jerusalem.GetShabbatTimes();
Console.WriteLine(jerusalemTimes);
```

---

#### Example #2

> Creates instance for New York

```csharp
using ShabatShalom;

var newYork = new ShabatShalom(40.7143, -74.0060, "America/New_York");
var newYorkTimes = newYork.GetShabbatTimes();
Console.WriteLine(newYorkTimes);
```

---

## FormatTime

> Formats time to HH:MM format

### Parameters

`time`: **DateTime**

```csharp
shabatShalom.FormatTime(time)
```

---

#### Example

> Formats candle lighting time

```csharp
using ShabatShalom;

var shabatShalom = new ShabatShalom();
var times = shabatShalom.GetShabbatTimes();
var formattedTime = shabatShalom.FormatTime(times.CandleLighting);

Console.WriteLine(formattedTime);
```

**Result**:
`18:23`

---

## License

This project is licensed under the GNU General Public License v3.0. See the LICENSE file for details.
