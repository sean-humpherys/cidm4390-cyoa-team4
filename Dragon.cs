


public class Dragon
{
    // Basic dragon information
    public string Name { get; set; }

    // Dragon statistics
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Health { get; set; }

    // Dragon weapon
    public Weapon? Weapon { get; set; }

    // Constructor with the dragon's name
    public Dragon(string name)
    {
        Name = name;
    }

    // Random taunt when the dragon avoids an attack
    public string MissedMeTaunt()
    {
        string[] taunts =
        {
            "Missed me, breath of a gnat!",
            "Is that all you've got? I've seen kittens with more fire in their paws!",
            "Try harder! You're making me yawn!",
            "Maybe if you squinted harder, you'd actually hit something!"
        };

        return taunts[Random.Shared.Next(taunts.Length)];
    }

    // Random taunt when the dragon is hit
    public string WasHitTaunt()
    {
        string[] taunts =
        {
            "Ow! That actually stung a bit!",
            "Impressive! You might actually be worth toasting...",
            "That... was unexpected. But I assure you, it won't happen again.",
            "Well, I'll be! You have more bite than I thought!"
        };

        return taunts[Random.Shared.Next(taunts.Length)];
    }

    // Return the dragon's information and statistics
    public string DisplayStats()
    {
        return $"Name: {Name}\n" +
               $"Strength: {Strength}\n" +
               $"Agility: {Agility}\n" +
               $"Health: {Health}\n" +
               $"Weapon: {Weapon?.Type ?? "None"}\n" +
               $"Weapon Max Damage: {Weapon?.MaxDamage ?? 0}";
    }
}
