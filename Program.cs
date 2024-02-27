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
        var featureObj = new Feature(feature);
        var (shortestDistance, closestLatitude, closestLongitude) = featureObj.CalculateShortestDistance(latitude, longitude);
        featureObj.ShortestDistance = shortestDistance;
        featureObj.ClosestLatitude = closestLatitude;
        featureObj.ClosestLongitude = closestLongitude;
        features.Add(featureObj);
      }

      foreach (var feature in features)
      {
        if (feature.ShortestDistance <= 1 && feature.HasMainHabitat())
        {
          Console.WriteLine($"Properties: {feature.Properties}");
          Console.WriteLine($"Shortest Distance: {feature.ShortestDistance} km");
          Console.WriteLine($"Closest Latitude: {feature.ClosestLatitude}");
          Console.WriteLine($"Closest Longitude: {feature.ClosestLongitude}");
          Console.WriteLine();
        }
      }

      List<Dictionary<string, object>> flattenedFeatures = new List<Dictionary<string, object>>();

      foreach (var feature in features)
      {
        if (feature.ShortestDistance <= 1 && feature.HasMainHabitat())
        {
          Dictionary<string, object> flattenedFeature = new Dictionary<string, object>
          {
            { "ShortestDistance", feature.ShortestDistance },
            { "ClosestLatitude", feature.ClosestLatitude },
            { "ClosestLongitude", feature.ClosestLongitude },
          };

          foreach (var property in feature.GetFlattenedProperties())
          {
            flattenedFeature.Add(property.Key, property.Value);
          }

          flattenedFeatures.Add(flattenedFeature);
        }
      }

      string csvFilePath = @"data\Output.csv";
      using (var writer = new StreamWriter(csvFilePath))
      using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
      {
        // Write the header
        if (flattenedFeatures.Any())
        {
          foreach (var key in flattenedFeatures[0].Keys)
          {
            csv.WriteField(key);
          }
          csv.NextRecord();
        }

        // Write the records
        foreach (var record in flattenedFeatures)
        {
          foreach (var value in record.Values)
          {
            csv.WriteField(value);
          }
          csv.NextRecord();
        }
      }
      Console.WriteLine($"Results written to {csvFilePath}");
    }
  }
}