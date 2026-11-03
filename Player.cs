


public class Player
{
    // Basic player information
    public string Name { get; set; } = "";
    public string Race { get; set; } = "";
    public string Occupation { get; set; } = "";

    // Player statistics
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Health { get; set; }

    // Player weapon
    public Weapon? Weapon { get; set; }

    // Default constructor
    public Player()
    {
    }

    // Constructor with basic character information
    public Player(string name, string race, string occupation)
    {
        Name = name;
        Race = race;
        Occupation = occupation;
    }

    // Player taunt when an attack misses
    public string MissedMeTaunt()
    {
        return "You missed me!";
    }

    // Player taunt when hit by an attack
    public string WasHitTaunt()
    {
        return "Ouch! That hurt!";
    }

    // Return the player's information and statistics
    public string DisplayStats()
    {
        return $"Name: {Name}\n" +
               $"Race: {Race}\n" +
               $"Occupation: {Occupation}\n" +
               $"Strength: {Strength}\n" +
               $"Agility: {Agility}\n" +
               $"Health: {Health}\n" +
               $"Weapon: {Weapon?.Type ?? "None"}\n" +
               $"Weapon Max Damage: {Weapon?.MaxDamage ?? 0}";
    }
}
