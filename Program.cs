using Newtonsoft.Json;

namespace GeoFeatureFinder
{
  class Program
  {
    public static void Main()
    {
      double latitude = UserInput.GetDoubleFromUser("Enter latitude:");
      double longitude = UserInput.GetDoubleFromUser("Enter longitude:");

      string filePath = @"data\Priority_Habitat_Inventory_England.json";

      string json = File.ReadAllText(filePath);

      dynamic parsedJson = JsonConvert.DeserializeObject(json) ?? throw new ArgumentException("Invalid JSON");

      List<Feature> features = [];
      foreach (var feature in parsedJson.features)
      {
        features.Add(new Feature(feature));
      }

      foreach (var feature in features)
      {
        var (shortestDistance, closestLatitude, closestLongitude) = feature.CalculateShortestDistance(latitude, longitude);

        if (shortestDistance <= 1 && feature.HasMainHabitat())
        {
          Console.WriteLine($"Properties: {feature.Properties}");
          Console.WriteLine($"Shortest Distance: {shortestDistance} km");
          Console.WriteLine($"Closest Latitude: {closestLatitude}");
          Console.WriteLine($"Closest Longitude: {closestLongitude}");
          Console.WriteLine();
        }
      }
    }
  }
}