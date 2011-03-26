using System;

namespace Life
{
  public class Cell
  {
    private Action I_died;
    public Action When_it_dies
    {
      private get { return I_died; }
      set { I_died = value; }
    }

    private Action I_was_born;
    public Action When_its_born
    {
      private get { return I_was_born; }
      set { I_was_born = value; }
    }

    private int neighbor_count;
    private bool is_dead;

    public static Cell ThatsAlive()
    {
      return new Cell();
    }

    public void MomentPassed()
    {
      if (neighbor_count == 3 && this.is_dead)
      {
        this.is_dead = false;
        I_was_born();
      }
      else if((neighbor_count < 2 || neighbor_count > 3) && !this.is_dead)
      {
        this.is_dead = true;
        I_died();
      }
    }

    public static Cell ThatsAliveWithNeighbors(int preexisting_neighbor_count)
    {
      var cell = new Cell();
      cell.neighbor_count = preexisting_neighbor_count;
      return cell;
    }

    public static Cell ThatsDeadWithNeighbors(int preexisting_neighbor_count)
    {
      var cell = new Cell();
      cell.is_dead = true;
      cell.neighbor_count = preexisting_neighbor_count;
      return cell;
    }

    public void NeighborDied()
    {
      if(neighbor_count > 0)
        neighbor_count --;
    }

    public void NeighborWasBorn()
    {
      neighbor_count++;
    }


    public void Touched()
    {
      if (is_dead)
      {
        is_dead = false;
        I_was_born();
      }
      else
      {
        is_dead = true;
        I_died();
      }
    }
  }
}
