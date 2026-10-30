using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PlayerUnitTests
{
    [TestMethod]
    public void DefaultConstructorInitializesPlayer()
    {
        // Create a player without providing information
        Player player = new Player();

        // Verify the initial values
        Assert.AreEqual("", player.Name);
        Assert.AreEqual("", player.Race);
        Assert.AreEqual("", player.Occupation);

        Assert.AreEqual(0, player.Strength);
        Assert.AreEqual(0, player.Agility);
        Assert.AreEqual(0, player.Health);
    }

    [TestMethod]
    public void ParameterizedConstructorStoresPlayerInformation()
    {
        // Create a player with chosen information
        Player player = new Player("Arthur", "Human", "Fighter");

        // Verify the information was stored correctly
        Assert.AreEqual("Arthur", player.Name);
        Assert.AreEqual("Human", player.Race);
        Assert.AreEqual("Fighter", player.Occupation);
    }

    [TestMethod]
    public void DisplayStatsReturnsPlayerInformation()
    {
        // Create a player and assign statistics
        Player player = new Player("Arthur", "Human", "Fighter");

        player.Strength = 10;
        player.Agility = 8;
        player.Health = 20;

        // Get the player's statistics
        string stats = player.DisplayStats();

        // Verify all information is included
        Assert.Contains("Name: Arthur", stats);
        Assert.Contains("Race: Human", stats);
        Assert.Contains("Occupation: Fighter", stats);
        Assert.Contains("Strength: 10", stats);
        Assert.Contains("Agility: 8", stats);
        Assert.Contains("Health: 20", stats);
    }
}