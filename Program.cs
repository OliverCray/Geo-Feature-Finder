using Newtonsoft.Json;
using CsvHelper;

namespace GeoFeatureFinder
{
  class Program
  {
    [STAThread]
    public static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    public static void RunGeoFeatureFinder(string inputFilePath, string outputFilePath, double latitude, double longitude)
    {
      string json;

      try
      {
        json = File.ReadAllText(inputFilePath);
      }
      catch (Exception err)
      {
        Console.WriteLine($"Error reading file: {err.Message}");
        return;
      }

      dynamic parsedJson = JsonConvert.DeserializeObject(json) ?? throw new ArgumentException("Invalid JSON");

      List<Feature> features = new List<Feature>();
      // Iterate over each feature from the input JSON and update the properties to always have the shortest distance and closest latitude and longitude from that feature to the given latitude and longitude
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
          Dictionary<string, object> flattenedFeature = new Dictionary<string, object>();

          foreach (var property in feature.GetFlattenedProperties())
          {
            flattenedFeature.Add(property.Key, property.Value);
          }

          flattenedFeature.Add("ShortestDistance", feature.ShortestDistance);
          flattenedFeature.Add("ClosestLatitude", feature.ClosestLatitude);
          flattenedFeature.Add("ClosestLongitude", feature.ClosestLongitude);

          flattenedFeatures.Add(flattenedFeature);
        }
      }

      using (var writer = new StreamWriter(outputFilePath))
      using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
      {
        // Write the header
        if (flattenedFeatures.Count != 0)
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
      Console.WriteLine($"Results written to {outputFilePath}");
    }
  }
}