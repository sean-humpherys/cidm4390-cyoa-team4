
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PlayerUnitTests
{
    [TestMethod]
    public void DefaultConstructorInitializesPlayer()
    {
        Player player = new Player();

        Assert.AreEqual("", player.Name);
        Assert.AreEqual("", player.Race);
        Assert.AreEqual("", player.Occupation);

        Assert.AreEqual(0, player.Strength);
        Assert.AreEqual(0, player.Agility);
        Assert.AreEqual(0, player.Health);
        Assert.IsNull(player.Weapon);
    }

    [TestMethod]
    public void ParameterizedConstructorStoresPlayerInformation()
    {
        Player player = new Player("Arthur", "Human", "Fighter");

        Assert.AreEqual("Arthur", player.Name);
        Assert.AreEqual("Human", player.Race);
        Assert.AreEqual("Fighter", player.Occupation);
    }

    [TestMethod]
    public void DisplayStatsReturnsPlayerInformation()
    {
        Player player = new Player("Arthur", "Human", "Fighter");

        player.Strength = 10;
        player.Agility = 8;
        player.Health = 20;

        string stats = player.DisplayStats();

        Assert.Contains("Name: Arthur", stats);
        Assert.Contains("Race: Human", stats);
        Assert.Contains("Occupation: Fighter", stats);
        Assert.Contains("Strength: 10", stats);
        Assert.Contains("Agility: 8", stats);
        Assert.Contains("Health: 20", stats);
        Assert.Contains("Weapon: None", stats);
    }

    [TestMethod]
    public void WeaponPropertyStoresAssignedWeapon()
    {
        Player player = new Player();
        Weapon weapon = new Weapon("Long sword", 12, "-)=====>");

        player.Weapon = weapon;

        Assert.AreSame(weapon, player.Weapon);
    }

    [TestMethod]
    public void DisplayStatsIncludesWeaponInformation()
    {
        Player player = new Player("Arthur", "Human", "Fighter");
        player.Weapon = new Weapon("Long sword", 12, "-)=====>");

        string stats = player.DisplayStats();

        Assert.Contains("Weapon: Long sword", stats);
        Assert.Contains("Weapon Max Damage: 12", stats);
    }

    [TestMethod]
    [DataRow(Languages.English)]
    [DataRow(Languages.Spanish)]
    [DataRow(Languages.French)]
    public void MissedMeTauntUsesSelectedLanguage(Languages language)
    {
        Messages messages = new Messages();
        messages.ReadDictionary(language);

        Player player = new Player();
        player.LanguageMessages = messages;

        string result = player.MissedMeTaunt();

        Assert.AreEqual(messages.GetMessage(48), result);
    }

    [TestMethod]
    [DataRow(Languages.English)]
    [DataRow(Languages.Spanish)]
    [DataRow(Languages.French)]
    public void WasHitTauntReturnsTranslatedResponse(Languages language)
    {
        Messages messages = new Messages();
        messages.ReadDictionary(language);

        Player player = new Player();
        player.LanguageMessages = messages;

        string[] validResponses =
        {
            messages.GetMessage(52),
            messages.GetMessage(53),
            messages.GetMessage(54),
            messages.GetMessage(55)
        };

        // Check multiple randomly selected responses
        for (int i = 0; i < 20; i++)
        {
            string result = player.WasHitTaunt();
            CollectionAssert.Contains(validResponses, result);
        }
    }

    [TestMethod]
    public void PlayerDefaultsToEnglishMessages()
    {
        Player player = new Player();

        Messages englishMessages = new Messages();
        englishMessages.ReadDictionary(Languages.English);

        Assert.AreEqual(
            englishMessages.GetMessage(48),
            player.MissedMeTaunt()
        );
    }
}
