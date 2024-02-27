using Newtonsoft.Json;
using CsvHelper;

namespace GeoFeatureFinder
{
  class Program
  {
    public static void Main()
    {
      double latitude = UserInput.GetDoubleFromUser("Enter latitude:");
      double longitude = UserInput.GetDoubleFromUser("Enter longitude:");

      string filePath = @"data\Priority_Habitat_Inventory_England.json";
      string json;

      try
      {
        json = File.ReadAllText(filePath);
      }
      catch (Exception err)
      {
        Console.WriteLine($"Error reading file: {err.Message}");
        return;
      }

      dynamic parsedJson = JsonConvert.DeserializeObject(json) ?? throw new ArgumentException("Invalid JSON");

      List<Feature> features = new List<Feature>();
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

          feature.ShortestDistance = shortestDistance;
          feature.ClosestLatitude = closestLatitude;
          feature.ClosestLongitude = closestLongitude;
        }
      }

      string csvFilePath = @"data\Output.csv";
      using (var writer = new StreamWriter(csvFilePath))
      using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
      {
        csv.WriteRecords(features.Where(feature => feature.HasMainHabitat() && feature.CalculateShortestDistance(latitude, longitude).shortestDistance <= 1));
      }
      Console.WriteLine($"Results written to {csvFilePath}");
    }
  }
}