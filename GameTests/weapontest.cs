using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cidm4390_cyoa_team4;   // use whatever name is in Weapon.cs

[TestClass]
public class WeaponTests
{
    private const string FighterWeapon = @"
    -)=====> 
";

    // ---------- Constructor ----------

    [TestMethod]
    public void Constructor_SetsType()
    {
        var weapon = new Weapon("Sword", 8, FighterWeapon);

        Assert.AreEqual("Sword", weapon.Type);
    }

    [TestMethod]
    public void Constructor_SetsMaxDamage()
    {
        var weapon = new Weapon("Sword", 8, FighterWeapon);

        Assert.AreEqual(8, weapon.MaxDamage);
    }

    [TestMethod]
    public void Constructor_SetsAsciiArt()
    {
        var weapon = new Weapon("Sword", 8, FighterWeapon);

        Assert.AreEqual(FighterWeapon, weapon.AsciiArt);
    }

    [TestMethod]
    [DataRow("Dagger", 4)]
    [DataRow("Axe", 10)]
    [DataRow("Greatsword", 12)]
    public void Constructor_WorksForDifferentWeapons(string type, int maxDamage)
    {
        var weapon = new Weapon(type, maxDamage, "");

        Assert.AreEqual(type, weapon.Type);
        Assert.AreEqual(maxDamage, weapon.MaxDamage);
    }

    // ---------- Properties ----------

    [TestMethod]
    public void Properties_CanBeChangedAfterCreation()
    {
        var weapon = new Weapon("Sword", 8, FighterWeapon);

        weapon.Type = "Rusty Sword";
        weapon.MaxDamage = 5;
        weapon.AsciiArt = "";

        Assert.AreEqual("Rusty Sword", weapon.Type);
        Assert.AreEqual(5, weapon.MaxDamage);
        Assert.AreEqual("", weapon.AsciiArt);
    }

    // ---------- DisplayWeapon ----------

    [TestMethod]
    public void DisplayWeapon_ReturnsTypeAndDamageOnSeparateLines()
    {
        var weapon = new Weapon("Sword", 8, FighterWeapon);

        string result = weapon.DisplayWeapon();

        Assert.AreEqual("Weapon: Sword\nMax Damage: 8", result);
    }

    [TestMethod]
    public void DisplayWeapon_ContainsType()
    {
        var weapon = new Weapon("Axe", 10, "");

        StringAssert.Contains(weapon.DisplayWeapon(), "Weapon: Axe");
    }

    [TestMethod]
    public void DisplayWeapon_ContainsMaxDamage()
    {
        var weapon = new Weapon("Axe", 10, "");

        StringAssert.Contains(weapon.DisplayWeapon(), "Max Damage: 10");
    }

    [TestMethod]
    public void DisplayWeapon_DoesNotIncludeAsciiArt()
    {
        var weapon = new Weapon("Sword", 8, FighterWeapon);

        Assert.IsFalse(weapon.DisplayWeapon().Contains(FighterWeapon));
    }

    [TestMethod]
    public void DisplayWeapon_ReflectsChangedProperties()
    {
        var weapon = new Weapon("Sword", 8, FighterWeapon);
        weapon.Type = "Broken Sword";
        weapon.MaxDamage = 1;

        Assert.AreEqual("Weapon: Broken Sword\nMax Damage: 1", weapon.DisplayWeapon());
    }
}