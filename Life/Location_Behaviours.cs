using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Life
{
  [TestFixture]
  public class Location_Behaviours
  {
    [Test]
    public void Get_neighboring_locations()
    {
      var theTestLocation = Location.At(0, 0);
      var neighboringLocations = theTestLocation.GetNeighboringLocations();
      var expected_locations = new[] 
      {
        Location.At(0,1),
        Location.At(1,0),
        Location.At(1,1),
        Location.At(0,-1),
        Location.At(-1,0),
        Location.At(-1,-1),
        Location.At(-1,1),
        Location.At(1,-1),
      };

      Assert.That(neighboringLocations.Count(), Is.EqualTo(8), "should have eight locations.");
      Assert.That(neighboringLocations, Is.EquivalentTo(expected_locations), "It should get all surrounding locations.");
    
    }

    [Test]
    public void Test_location_neighbors_another_when_it_does()
    {
      Assert.That(Location.At(1, 2).IsNeighborsWith(Location.At(2, 2)), "the locations should neighbor one another.");
    }
  }
}
