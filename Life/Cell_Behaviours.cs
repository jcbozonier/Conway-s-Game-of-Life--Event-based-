using NUnit.Framework;

namespace Life
{
  [TestFixture]
  public class Cell_Behaviours
  {
    [Test]
    public void When_a_cell_has_no_neighbors_and_a_moment_passes()
    {
      var cell = Cell.ThatsAlive();

      var cell_died = false;
      cell.When_it_dies = () => cell_died = true;
      cell.MomentPassed();

      Assert.That(cell_died, "It should die.");
    }

    [Test]
    public void When_a_cell_has_no_neighbors_and_more_than_one_moment_passes()
    {
      var cell = Cell.ThatsAlive();

      var died_count = 0;
      cell.When_it_dies = () => died_count++;
      cell.MomentPassed();
      cell.MomentPassed();

      Assert.That(died_count, Is.EqualTo(1), "It should die only once.");
    }

    [Test]
    public void When_a_cell_has_one_neighbor_and_a_moment_passes()
    {
      var cell = Cell.ThatsAlive();
      cell.NeighborWasBorn();

      var cell_died = false;
      cell.When_it_dies = () => cell_died = true;
      cell.MomentPassed();

      Assert.That(cell_died, "It should die.");
    }

    [Test]
    public void When_a_cell_has_two_neighbors_and_a_moment_passes()
    {
      var cell = Cell.ThatsAliveWithNeighbors(2);

      var cell_died = false;
      cell.When_it_dies = () => cell_died = true;
      cell.MomentPassed();

      Assert.That(cell_died, Is.False, "It should live on.");
    }

    [Test]
    public void When_a_dead_cell_has_one_neighbors_and_a_moment_passes()
    {
      var cell = Cell.ThatsDeadWithNeighbors(1);

      var cell_died = false;
      cell.When_it_dies = () => cell_died = true;
      cell.MomentPassed();

      Assert.That(cell_died, Is.False, "It should not die when it's already dead.");
    }

    [Test]
    public void When_a_cell_has_three_neighbors_and_a_moment_passes()
    {
      var cell = Cell.ThatsAliveWithNeighbors(3);

      var cell_died = false;
      cell.When_it_dies = () => cell_died = true;
      cell.MomentPassed();

      Assert.That(cell_died, Is.False, "It should live on.");
    }

    [Test]
    public void When_a_live_cell_has_more_than_three_neighbors_and_a_moment_passes()
    {
      var cell = Cell.ThatsAliveWithNeighbors(4);

      var cell_died = false;
      cell.When_it_dies = () => cell_died = true;
      cell.MomentPassed();

      Assert.That(cell_died, "it should die as if from overcrowding.");
    }

    [Test]
    public void When_a_dead_cell_has_exactly_three_neighbors()
    {
      var dead_cell = Cell.ThatsDeadWithNeighbors(3);

      var cell_was_born = false;
      dead_cell.When_its_born = () => cell_was_born = true;
      dead_cell.MomentPassed();

      Assert.That(cell_was_born, "it should come to life");
    }

    [Test]
    public void When_a_dead_cell_has_exactly_three_neighbors_and_more_than_one_moment_passes()
    {
      var dead_cell = Cell.ThatsDeadWithNeighbors(3);

      var number_of_times_born = 0;
      dead_cell.When_its_born = () => number_of_times_born++;
      dead_cell.MomentPassed();
      dead_cell.MomentPassed();

      Assert.That(number_of_times_born, Is.EqualTo(1), "it should be born only once.");
    }

    [Test]
    public void When_a_live_cell_is_notified_that_a_neighbor_died()
    {
      var cell_died = false;
      var cell = Cell.ThatsAliveWithNeighbors(2);
      cell.When_it_dies = () => cell_died = true;
      cell.NeighborDied();

      cell.MomentPassed();

      Assert.That(cell_died, "it should affect its view of the world.");
    }

    [Test]
    public void When_a_neighboring_cell_dies_without_knowing_of_any_neighbors()
    {
      var cell_born = false;
      var cell = Cell.ThatsDeadWithNeighbors(0);
      cell.When_its_born = () => cell_born = true;
      cell.NeighborDied();
      cell.NeighborWasBorn();
      cell.NeighborWasBorn();
      cell.NeighborWasBorn();
      cell.MomentPassed();

      Assert.That(cell_born, "it should count neighbors up from zero and not go into negatives.");
    }
  }
}
