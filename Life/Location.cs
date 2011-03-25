using System.Collections.Generic;
using System.Linq;
using System;
namespace Life
{
  public class Location
  {
    private bool is_nowhere;
    private int horizontal_offset;
    private int vertical_offset;

    public static Location OfNowhere()
    {
      var location = new Location();
      location.is_nowhere = true;
      return location;
    }

    public static Location OfOrigin()
    {
      var location = new Location()
      {
        horizontal_offset = 0,
        vertical_offset = 0
      };
      return location;
    }

    public override string ToString()
    {
      return is_nowhere ? "Location: Nowhere" : string.Format("Location: H:{0}, V:{1}", horizontal_offset, vertical_offset);
    }

    public override bool Equals(object obj)
    {
      var other_location = obj as Location;
      return is_nowhere.Equals(other_location.is_nowhere) && horizontal_offset == other_location.horizontal_offset && vertical_offset == other_location.vertical_offset;
    }

    internal static Location At(int horizontal_offset, int vertical_offset)
    {
      var location = new Location()
      {
        horizontal_offset = horizontal_offset,
        vertical_offset = vertical_offset,
      };
      return location;
    }

    public IEnumerable<Location> GetNeighboringLocations()
    {
      var neighboring_locations = new List<Location>();

      foreach (var h_offset in Enumerable.Range(this.horizontal_offset - 1, 3))
        foreach (var v_offset in Enumerable.Range(this.vertical_offset - 1, 3))
          if (!(h_offset == 0 && v_offset == 0)) 
            neighboring_locations.Add(Location.At(h_offset, v_offset));

      return neighboring_locations;
    }

    public bool IsNeighborsWith(Location location)
    {
      var horizontal_distance = Math.Abs(horizontal_offset - location.horizontal_offset);
      var vertical_distance = Math.Abs(vertical_offset - location.vertical_offset);

      return horizontal_distance <= 1 && vertical_distance <= 1 && !this.Equals(location);
    }
  }
}