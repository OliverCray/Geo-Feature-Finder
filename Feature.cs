using Newtonsoft.Json;

namespace GeoFeatureFinder
{
  public class Feature
  {
    public dynamic Properties { get; set; }
    public double ShortestDistance { get; set; }
    public double ClosestLatitude { get; set; }
    public double ClosestLongitude { get; set; }
    public dynamic Geometry { get; set; }
    public string FormattedProperties => JsonConvert.SerializeObject(Properties, Formatting.Indented);

    public Feature(dynamic feature)
    {
      Properties = feature.properties;
      Geometry = feature.geometry;
    }

    public bool HasMainHabitat()
    {
      return Properties.mainhabs != "No main habitat but additional habitats present";
    }

    public (double shortestDistance, double closestLatitude, double closestLongitude) CalculateShortestDistance(double latitude, double longitude)
    {
      double shortestDistance = double.MaxValue;
      double closestLatitude = 0, closestLongitude = 0;

      var multiPolygon = Geometry.type == "Polygon" ? new[] { Geometry.coordinates } : Geometry.coordinates;

      foreach (var polygon in multiPolygon)
      {
        foreach (var ring in polygon)
        {
          foreach (var coordinate in ring)
          {
            double featureLongitude = coordinate[0];
            double featureLatitude = coordinate[1];
            double distance = GeoCalculator.CalculateDistance(latitude, longitude, featureLatitude, featureLongitude);
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

    public Dictionary<string, string> GetFlattenedProperties()
    {
      return JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(Properties));
    }
  }
}