using cidm4390_cyoa_team4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cidm4390_cyoa_team4.Tests;

/// <summary>
/// Tests for Combat.
///
/// Combat creates its own Die internally, so the rolls can't be controlled directly.
/// Instead, these tests force outcomes with stat values, assuming Die.Roll(n) returns 1..n:
///   Strength/Agility = 20 -> any d20 roll is always &lt;= 20, so it always succeeds
///   Strength/Agility = 0  -> any d20 roll is always &gt; 0, so it always fails
///   Weapon.MaxDamage = 1  -> damage roll is always exactly 1
///
/// The taunt methods return a random line each call, so the tests check that a
/// taunt was printed (the last line of output) rather than which taunt it was.
/// </summary>
[TestClass]
[DoNotParallelize] // Tests swap Console.Out, which is shared by the whole process
public class CombatTests
{
    private const int AlwaysSucceed = 20;
    private const int AlwaysFail = 0;

    private StringWriter _output = null!;
    private TextWriter _originalOut = null!;

    [TestInitialize]
    public void Setup()
    {
        _originalOut = Console.Out;
        _output = new StringWriter();
        Console.SetOut(_output);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Console.SetOut(_originalOut);
        _output.Dispose();
    }

    // ---------- Helpers: adjust these if your constructors differ ----------

    private static Player MakePlayer(int strength, int agility, int health = 100, int maxDamage = 1)
    {
        return new Player
        {
            Strength = strength,
            Agility = agility,
            Health = health,
            Weapon = new Weapon("Sword", maxDamage, "Test weapon")
        };
    }

    private static Dragon MakeDragon(int strength, int agility, int health = 100, int maxDamage = 1)
    {
        return new Dragon("Smaug")
        {
            Strength = strength,
            Agility = agility,
            Health = health,
            Weapon = new Weapon("Claws", maxDamage, "Test weapon")
        };
    }

    private string Output => _output.ToString();

    /// <summary>Each non-empty line the game printed, in order.</summary>
    private string[] Lines =>
        Output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

    // ======================= Player attacks Dragon =======================

    [TestMethod]
    public void PlayerAttacks_WhenPlayerMisses_DragonHealthUnchanged()
    {
        var player = MakePlayer(strength: AlwaysFail, agility: 10);
        var dragon = MakeDragon(strength: 10, agility: 10, health: 50);
        var combat = new Combat(player, dragon);

        combat.PlayAttacksDragonSequence();

        Assert.AreEqual(50, dragon.Health);
        Assert.HasCount(1, Lines, "A miss should print only one line");
        StringAssert.Contains(Lines[0], "You missed Smaug");
    }

    [TestMethod]
    public void PlayerAttacks_WhenDragonDefends_DragonHealthUnchanged()
    {
        var player = MakePlayer(strength: AlwaysSucceed, agility: 10);
        var dragon = MakeDragon(strength: 10, agility: AlwaysSucceed, health: 50);
        var combat = new Combat(player, dragon);

        combat.PlayAttacksDragonSequence();

        Assert.AreEqual(50, dragon.Health);
        Assert.HasCount(3, Lines, "Expected: hit line, defend line, taunt");
        StringAssert.Contains(Lines[0], "You hit Smaug");
        StringAssert.Contains(Lines[1], "Smaug successfully defends your attack");
        Assert.IsFalse(string.IsNullOrWhiteSpace(Lines[2]), "The dragon should taunt you");
    }

    [TestMethod]
    public void PlayerAttacks_WhenDragonFailsToDefend_DragonTakesDamage()
    {
        var player = MakePlayer(strength: AlwaysSucceed, agility: 10, maxDamage: 1);
        var dragon = MakeDragon(strength: 10, agility: AlwaysFail, health: 50);
        var combat = new Combat(player, dragon);

        combat.PlayAttacksDragonSequence();

        Assert.AreEqual(49, dragon.Health);
        Assert.HasCount(3, Lines, "Expected: hit line, damage line, taunt");
        StringAssert.Contains(Lines[0], "You hit Smaug");
        StringAssert.Contains(Lines[1], "Your weapon delivers 1 damage points");
        StringAssert.Contains(Lines[1], "health point is now 49");
        Assert.DoesNotContain("defeated", Lines[2], "The dragon is still alive, so this should be a taunt");
    }

    [TestMethod]
    public void PlayerAttacks_DamageIsWithinWeaponRange()
    {
        const int maxDamage = 6;
        const int startHealth = 1000;

        for (int i = 0; i < 100; i++)
        {
            var player = MakePlayer(strength: AlwaysSucceed, agility: 10, maxDamage: maxDamage);
            var dragon = MakeDragon(strength: 10, agility: AlwaysFail, health: startHealth);
            var combat = new Combat(player, dragon);

            combat.PlayAttacksDragonSequence();

            int damage = startHealth - dragon.Health;
            Assert.IsTrue(damage >= 1 && damage <= maxDamage,
                $"Damage {damage} was outside the range 1..{maxDamage}");
        }
    }

    [TestMethod]
    public void PlayerAttacks_WhenDragonHealthReachesZero_DragonIsDefeated()
    {
        var player = MakePlayer(strength: AlwaysSucceed, agility: 10, maxDamage: 1);
        var dragon = MakeDragon(strength: 10, agility: AlwaysFail, health: 1);
        var combat = new Combat(player, dragon);

        combat.PlayAttacksDragonSequence();

        Assert.AreEqual(0, dragon.Health);
        Assert.HasCount(3, Lines, "Expected: hit line, damage line, defeat line (no taunt)");
        StringAssert.Contains(Lines[2], "you have defeated the dragon");
    }

    // ======================= Dragon attacks Player =======================

    [TestMethod]
    public void DragonAttacks_WhenDragonMisses_PlayerHealthUnchanged()
    {
        var player = MakePlayer(strength: 10, agility: 10, health: 50);
        var dragon = MakeDragon(strength: AlwaysFail, agility: 10);
        var combat = new Combat(player, dragon);

        combat.DragonAttacksPlayerSequence();

        Assert.AreEqual(50, player.Health);
        Assert.HasCount(1, Lines, "A miss should print only one line");
        StringAssert.Contains(Lines[0], "The dragon missed you.");
    }

    [TestMethod]
    public void DragonAttacks_WhenPlayerDefends_PlayerHealthUnchanged()
    {
        var player = MakePlayer(strength: 10, agility: AlwaysSucceed, health: 50);
        var dragon = MakeDragon(strength: AlwaysSucceed, agility: 10);
        var combat = new Combat(player, dragon);

        combat.DragonAttacksPlayerSequence();

        Assert.AreEqual(50, player.Health);
        Assert.HasCount(3, Lines, "Expected: hit line, defend line, taunt");
        StringAssert.Contains(Lines[0], "Smaug hits you");
        StringAssert.Contains(Lines[1], "You successfully defend the attack");
        Assert.IsFalse(string.IsNullOrWhiteSpace(Lines[2]), "The player should taunt the dragon");
    }

    [TestMethod]
    public void DragonAttacks_WhenPlayerFailsToDefend_PlayerTakesDamage()
    {
        var player = MakePlayer(strength: 10, agility: AlwaysFail, health: 50);
        var dragon = MakeDragon(strength: AlwaysSucceed, agility: 10, maxDamage: 1);
        var combat = new Combat(player, dragon);

        combat.DragonAttacksPlayerSequence();

        Assert.AreEqual(49, player.Health);
        Assert.HasCount(3, Lines, "Expected: hit line, damage line, taunt");
        StringAssert.Contains(Lines[0], "Smaug hits you");
        StringAssert.Contains(Lines[1], "The dragon attack delivers 1 damage points");
        StringAssert.Contains(Lines[1], "Your health point is now 49");
        Assert.DoesNotContain("defeated", Lines[2], "The player is still alive, so this should be a taunt");
    }

    [TestMethod]
    public void DragonAttacks_DamageIsWithinWeaponRange()
    {
        const int maxDamage = 8;
        const int startHealth = 1000;

        for (int i = 0; i < 100; i++)
        {
            var player = MakePlayer(strength: 10, agility: AlwaysFail, health: startHealth);
            var dragon = MakeDragon(strength: AlwaysSucceed, agility: 10, maxDamage: maxDamage);
            var combat = new Combat(player, dragon);

            combat.DragonAttacksPlayerSequence();

            int damage = startHealth - player.Health;
            Assert.IsTrue(damage >= 1 && damage <= maxDamage,
                $"Damage {damage} was outside the range 1..{maxDamage}");
        }
    }

    [TestMethod]
    public void DragonAttacks_WhenPlayerHealthReachesZero_PlayerIsDefeated()
    {
        var player = MakePlayer(strength: 10, agility: AlwaysFail, health: 1);
        var dragon = MakeDragon(strength: AlwaysSucceed, agility: 10, maxDamage: 1);
        var combat = new Combat(player, dragon);

        combat.DragonAttacksPlayerSequence();

        Assert.AreEqual(0, player.Health);
        Assert.HasCount(3, Lines, "Expected: hit line, damage line, defeat line (no taunt)");
        StringAssert.Contains(Lines[2], "you have been defeated");
    }
}