using Newtonsoft.Json;

namespace GeoFeatureFinder
{
  class Program
  {
    private const int earthRadius = 6371; // Radius of the earth in km

    public static void Main()
    {
      double latitude = GetDoubleFromUser("Enter latitude:");
      double longitude = GetDoubleFromUser("Enter longitude:");

      string filePath = @"data\Priority_Habitat_Inventory_England.json";

      string json = File.ReadAllText(filePath);

      dynamic parsedJson = JsonConvert.DeserializeObject(json) ?? throw new ArgumentNullException(nameof(json));

      foreach (var feature in parsedJson.features)
      {
        double shortestDistance = double.MaxValue;
        double closestLatitude = 0, closestLongitude = 0;

        // Treat everything as a MultiPolygon
        var multiPolygon = feature.geometry.type == "Polygon" ? new[] { feature.geometry.coordinates } : feature.geometry.coordinates;

        foreach (var polygon in multiPolygon)
        {
          foreach (var ring in polygon)
          {
            foreach (var coordinate in ring)
            {
              double featureLongitude = coordinate[0];
              double featureLatitude = coordinate[1];
              double distance = CalculateDistance(latitude, longitude, featureLatitude, featureLongitude);
              if (distance < shortestDistance)
              {
                shortestDistance = distance;
                closestLatitude = featureLatitude;
                closestLongitude = featureLongitude;
              }
            }
          }
        }

        if (shortestDistance <= 1 && feature.properties.mainhabs != "No main habitat but additional habitats present")
        {
          Console.WriteLine($"Properties: {feature.properties}");
          Console.WriteLine($"Shortest Distance: {shortestDistance} km");
          Console.WriteLine($"Closest Latitude: {closestLatitude}");
          Console.WriteLine($"Closest Longitude: {closestLongitude}");
          Console.WriteLine();
        }
      }
    }

    public static double GetDoubleFromUser(string prompt)
    {
      while (true)
      {
        Console.WriteLine(prompt);
        if (double.TryParse(Console.ReadLine(), out double value))
        {
          return value;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid number.");
        }
      }
    }

    // Method to calculate distance between two coordinates in kilometers using the Haversine formula
    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
      double dLat = Deg2Rad(lat2 - lat1);
      double dLon = Deg2Rad(lon2 - lon1);
      double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
      double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
      double distance = earthRadius * c; // Distance in km
      return distance;
    }

    public static double Deg2Rad(double deg)
    {
      return deg * (Math.PI / 180);
    }
  }
}