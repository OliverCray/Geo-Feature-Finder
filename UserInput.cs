namespace GeoFeatureFinder
{
  public static class UserInput
  {
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
  }
}