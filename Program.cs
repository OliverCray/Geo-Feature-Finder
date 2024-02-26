using Newtonsoft.Json;

namespace GeoFeatureFinder
{
  class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Enter latitude and longitude separated by a space:");
      string[] input = Console.ReadLine().Split(' ');

      double latitude = double.Parse(input[0]);
      double longitude = double.Parse(input[1]);

      string filePath = @"data\Priority_Habitat_Inventory_England.json";

      string json = File.ReadAllText(filePath);

      dynamic parsedJson = JsonConvert.DeserializeObject(json);

      foreach (var feature in parsedJson.features)
      {
        string geometryType = feature.geometry.type;
        double featureLatitude, featureLongitude;

        switch (geometryType)
        {
          case "Polygon":
            // Access the first coordinate of the first polygon
            featureLongitude = feature.geometry.coordinates[0][0][0];
            featureLatitude = feature.geometry.coordinates[0][0][1];
            break;
          case "MultiPolygon":
            // Access the first coordinate of the first polygon of the first MultiPolygon
            featureLongitude = feature.geometry.coordinates[0][0][0][0];
            featureLatitude = feature.geometry.coordinates[0][0][0][1];
            break;
          default:
            // Handle other geometry types as needed
            continue;
        }

        // Calculate distance in kilometers between the input coordinates and the feature coordinates
        double distance = CalculateDistance(latitude, longitude, featureLatitude, featureLongitude);

        if (distance <= 1 && feature.properties.mainhabs != "No main habitat but additional habitats present")
        {
          Console.WriteLine($"Properties: {feature.properties}");
          Console.WriteLine($"Distance: {distance} km");
          Console.WriteLine($"Latitude: {featureLatitude}");
          Console.WriteLine($"Longitude: {featureLongitude}");
          Console.WriteLine();
        }
      }
    }

    // Method to calculate distance between two coordinates in kilometers using the Haversine formula
    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
      const int R = 6371; // Radius of the earth in km
      double dLat = Deg2Rad(lat2 - lat1);
      double dLon = Deg2Rad(lon2 - lon1);
      double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
      double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
      double distance = R * c; // Distance in km
      return distance;
    }

    public static double Deg2Rad(double deg)
    {
      return deg * (Math.PI / 180);
    }
  }
}