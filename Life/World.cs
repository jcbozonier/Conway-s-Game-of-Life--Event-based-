using System;
using System.Collections.Generic;
using System.Linq;

namespace Life
{
  public class World
  {
    private bool has_life;

    private Action<Location> A_cell_was_born;
    public Action<Location> When_a_cell_comes_to_life
    {
      private get { return A_cell_was_born; }
      set { A_cell_was_born = value; }
    }

    private Action<Location> A_cell_died;
    private List<KeyValuePair<Location, Cell>> cell_locations = new List<KeyValuePair<Location, Cell>>();
    private List<KeyValuePair<Location, Cell>> born_cell_locations = new List<KeyValuePair<Location, Cell>>();

    public Action<Location> When_a_cell_dies
    {
      private get { return A_cell_died; }
      set { A_cell_died = value; }
    }

    public void TouchCellAt(Location touched_location)
    {
      var was_just_born = born_cell_locations.Any(x => x.Key.Equals(touched_location));
      var already_exists = cell_locations.Any(x=>x.Key.Equals(touched_location));

      if (was_just_born)
      {
        born_cell_locations.RemoveAll(x => x.Key.Equals(touched_location));
        cell_locations.RemoveAll(x => x.Key.Equals(touched_location));
      }
      else if (already_exists)
      {
        //var touched_cell = cell_locations.Single(x => x.Key.Equals(touched_location));
        //touched_cell.Value.Touched();
      }
      else
      {
        var created_cell = Cell.ThatsAlive();
        var cell_location = AddCellToWorld(touched_location, created_cell);
        born_cell_locations.Add(cell_location);
      }
    }

    private KeyValuePair<Location, Cell> AddCellToWorld(Location touched_location, Cell created_cell)
    {
      if (cell_locations.Exists(x => x.Key.Equals(touched_location)))
      {
        throw new InvalidOperationException("cell location already exists! Something terrible happened.");
      }
      created_cell.When_it_dies = () => this.A_cell_died(touched_location);
      created_cell.When_its_born = () => this.A_cell_was_born(touched_location);

      var cell_location = new KeyValuePair<Location, Cell>(touched_location, created_cell);
      cell_locations.Add(cell_location);
      return cell_location;
    }

    public static World That_is_a_barren_wasteland()
    {
      return new World();
    }

    public static World Brimming_with_life()
    {
      return new World();
    }

    public void MomentPassed()
    {
      foreach (var born_location in born_cell_locations)
      {
        CreateDeadCellsWhereThereAreNoneSurrounding(born_location);
      }

      foreach (var born_location in born_cell_locations)
      {
        A_cell_was_born(born_location.Key);

        var neighboring_cells = from a_cell in cell_locations
                                where a_cell.Key.IsNeighborsWith(born_location.Key)
                                select a_cell.Value;

        foreach (var neighboring_cell in neighboring_cells)
          neighboring_cell.NeighborWasBorn();
      }

      born_cell_locations.Clear();

      foreach(var cell_location in cell_locations)
        cell_location.Value.MomentPassed();
    }

    private void CreateDeadCellsWhereThereAreNoneSurrounding(KeyValuePair<Location, Cell> born_location)
    {
      var locations = born_location.Key.GetNeighboringLocations();

      foreach (var location in locations)
      {
        if (!cell_locations.Exists(x => x.Key.Equals(location)))
        {
          var dead_cell = Cell.ThatsDeadWithNeighbors(0);
          AddCellToWorld(location, dead_cell);
        }
      }
    }
  }
}