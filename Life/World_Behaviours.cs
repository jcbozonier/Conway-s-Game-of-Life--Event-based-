using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Life
{
  [TestFixture]
  public class World_Behaviours
  {
    [Test]
    public void When_I_touch_a_dead_cell_at_a_given_location_it_should_come_to_life()
    {
      var location_cell_was_born_at = Location.OfNowhere();

      var world = World.That_is_a_barren_wasteland();
      world.When_a_cell_comes_to_life = location => location_cell_was_born_at = location;
      world.When_a_cell_dies = _ => { };
      world.TouchCellAt(Location.OfOrigin());
      world.MomentPassed();

      Assert.That(location_cell_was_born_at, Is.EqualTo(Location.OfOrigin()), "it should be born at the touched location.");
    }

    [Test]
    public void When_I_touch_a_live_cell_at_a_given_location_it_should_die()
    {
      var location_cell_died_at = Location.OfNowhere();

      var world = World.Brimming_with_life();
      world.When_a_cell_dies = location => location_cell_died_at = location;
      world.TouchCellAt(Location.OfOrigin());
      world.MomentPassed();

      Assert.That(location_cell_died_at, Is.EqualTo(Location.OfOrigin()), "it should be dead at the touched location.");
    }

    [Test]
    public void Given_one_live_cell_when_a_moment_passes()
    {
      var location_cell_died_at = Location.OfNowhere();

      var world = World.That_is_a_barren_wasteland();
      world.When_a_cell_dies = location => location_cell_died_at = location;
      world.When_a_cell_comes_to_life = _ => { };
      world.TouchCellAt(Location.OfOrigin());
      world.MomentPassed();

      Assert.That(location_cell_died_at, Is.EqualTo(Location.OfOrigin()), "it should be dead at the touched location.");
    }

    [Test]
    public void Given_two_live_neighboring_cells_when_a_moment_passes()
    {
      var cell_death_locations = new List<Location>();

      var world = World.That_is_a_barren_wasteland();
      world.When_a_cell_dies = location => cell_death_locations.Add(location);
      world.When_a_cell_comes_to_life = _ => { };
      world.TouchCellAt(Location.At(0,0));
      world.TouchCellAt(Location.At(0,1));
      world.MomentPassed();

      Assert.That(cell_death_locations.ToArray(), Is.EquivalentTo(new []{Location.At(0,0), Location.At(0,1)}), "Both cells should have died.");
    }

    [Test]
    public void Given_a_stable_live_cell_environment_when_a_moment_passes()
    {
      var cell_death_locations = new List<Location>();
      var cell_birth_locations = new List<Location>();

      var world = World.That_is_a_barren_wasteland();
      world.When_a_cell_dies = location => cell_death_locations.Add(location);
      world.When_a_cell_comes_to_life = location => cell_birth_locations.Add(location);

      world.TouchCellAt(Location.At(0, 0));
      world.TouchCellAt(Location.At(0, 1));
      world.TouchCellAt(Location.At(1, 0));
      world.MomentPassed();

      Assert.That(cell_birth_locations, Is.EquivalentTo(new[] { Location.At(0, 0), Location.At(0, 1), Location.At(1, 0), Location.At(1,1) }), "it should have cause the touched cells to be born.");
      Assert.That(cell_death_locations.ToArray(), Is.Empty, "No cells should have died.");

      cell_death_locations.Clear();
      cell_birth_locations.Clear();

      world.MomentPassed();

      Assert.That(cell_birth_locations, Is.EquivalentTo(Enumerable.Empty<Location>()), "nothing eventful should have happened");
      Assert.That(cell_death_locations.ToArray(), Is.Empty, "nothing eventful should have happened");
    }
  }
}
