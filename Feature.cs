using Newtonsoft.Json;

namespace GeoFeatureFinder
{
  public class Feature
  {
    // I don't like the use of dynamic here but I couldn't find a better way to handle the JSON properties, I'm sure there's a better way to do this but I'm not sure how
    public dynamic Properties { get; set; }
    public double ShortestDistance { get; set; }
    public double ClosestLatitude { get; set; }
    public double ClosestLongitude { get; set; }
    public dynamic Geometry { get; set; }
    public string FormattedProperties => JsonConvert.SerializeObject(Properties, Formatting.Indented);

    public Feature(dynamic feature)
    {
      // Initialize feature properties and geometry
      Properties = feature.properties;
      Geometry = feature.geometry;
    }

    // Used to filter out features that don't have a main habitat
    public bool HasMainHabitat()
    {
      return Properties.mainhabs != "No main habitat but additional habitats present";
    }

    // Method to calculate the shortest distance between a given latitude and longitude and the feature's geometry
    public (double shortestDistance, double closestLatitude, double closestLongitude) CalculateShortestDistance(double latitude, double longitude)
    {
      double shortestDistance = double.MaxValue;
      double closestLatitude = 0, closestLongitude = 0;

      var multiPolygon = Geometry.type == "Polygon" ? new[] { Geometry.coordinates } : Geometry.coordinates;

      // Iterate over each polygon, ring, and coordinate in the feature's geometry to calculate the shortest distance
      foreach (var polygon in multiPolygon)
      {
        foreach (var ring in polygon)
        {
          foreach (var coordinate in ring)
          {
            double featureLongitude = coordinate[0];
            double featureLatitude = coordinate[1];
            double distance = GeoCalculator.CalculateDistance(latitude, longitude, featureLatitude, featureLongitude);
            // Update shortest distance and closest latitude and longitude if the current distance is shorter than the previous shortest distance
            if (distance < shortestDistance)
            {
              shortestDistance = Math.Round(distance, 2);
              closestLatitude = featureLatitude;
              closestLongitude = featureLongitude;
            }
          }
        }
      }

      return (shortestDistance, closestLatitude, closestLongitude);
    }

    // Method to flatten the feature's properties into a dictionary, used to obtain some of the headers for the CSV file
    public Dictionary<string, string> GetFlattenedProperties()
    {
      return JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(Properties));
    }
  }
}