
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cidm4390_cyoa_team4;

namespace DragonTests;

[TestClass]
public sealed class DragonUnitTests
{
    [TestMethod]
    public void ConstructorStoresDragonName()
    {
        // Create a dragon
        Dragon dragon = new Dragon("Smaug");

        // Verify the name and default values
        Assert.AreEqual("Smaug", dragon.Name);
        Assert.AreEqual(0, dragon.Strength);
        Assert.AreEqual(0, dragon.Agility);
        Assert.AreEqual(0, dragon.Health);
        Assert.IsNull(dragon.Weapon);
    }

    [TestMethod]
    public void DragonStatisticsCanBeUpdated()
    {
        // Create a dragon and assign statistics
        Dragon dragon = new Dragon("Smaug");

        dragon.Strength = 18;
        dragon.Agility = 12;
        dragon.Health = 50;

        // Verify the statistics
        Assert.AreEqual(18, dragon.Strength);
        Assert.AreEqual(12, dragon.Agility);
        Assert.AreEqual(50, dragon.Health);
    }

    [TestMethod]
    public void DragonCanStoreWeapon()
    {
        // Create a dragon and a weapon
        Dragon dragon = new Dragon("Smaug");
        Weapon weapon = new Weapon("Fire Breath", 15, "~~~~");

        dragon.Weapon = weapon;

        // Verify the weapon was assigned
        Assert.AreSame(weapon, dragon.Weapon);
    }

    [TestMethod]
    public void DisplayStatsReturnsDragonInformation()
    {
        // Create a dragon with statistics and a weapon
        Dragon dragon = new Dragon("Smaug");

        dragon.Strength = 18;
        dragon.Agility = 12;
        dragon.Health = 50;
        dragon.Weapon = new Weapon("Fire Breath", 15, "~~~~");

        // Get the dragon's statistics
        string stats = dragon.DisplayStats();

        // Verify the information is displayed
        Assert.Contains("Name: Smaug", stats);
        Assert.Contains("Strength: 18", stats);
        Assert.Contains("Agility: 12", stats);
        Assert.Contains("Health: 50", stats);
        Assert.Contains("Weapon: Fire Breath", stats);
        Assert.Contains("Weapon Max Damage: 15", stats);
    }

    [TestMethod]
    public void DisplayStatsWorksWithoutWeapon()
    {
        // Create a dragon without a weapon
        Dragon dragon = new Dragon("Smaug");

        string stats = dragon.DisplayStats();

        // Verify the default weapon information
        Assert.Contains("Weapon: None", stats);
        Assert.Contains("Weapon Max Damage: 0", stats);
    }

    [TestMethod]
    public void MissedMeTauntReturnsValidMessage()
    {
        // Create a dragon
        Dragon dragon = new Dragon("Smaug");

        // Possible responses when an attack misses
        string[] taunts =
        {
            "Missed me, breath of a gnat!",
            "Is that all you've got? I've seen kittens with more fire in their paws!",
            "Try harder! You're making me yawn!",
            "Maybe if you squinted harder, you'd actually hit something!"
        };

        // Verify the message is one of the expected taunts
        string result = dragon.MissedMeTaunt();

        CollectionAssert.Contains(taunts, result);
    }

    [TestMethod]
    public void WasHitTauntReturnsValidMessage()
    {
        // Create a dragon
        Dragon dragon = new Dragon("Smaug");

        // Possible responses when the dragon takes damage
        string[] taunts =
        {
            "Ow! That actually stung a bit!",
            "Impressive! You might actually be worth toasting...",
            "That... was unexpected. But I assure you, it won't happen again.",
            "Well, I'll be! You have more bite than I thought!"
        };

        // Verify the message is one of the expected taunts
        string result = dragon.WasHitTaunt();

        CollectionAssert.Contains(taunts, result);
    }
}
