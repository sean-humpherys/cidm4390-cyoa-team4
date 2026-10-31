using cidm4390_cyoa_team4;
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

    // Return the player's information and statistics
    public string DisplayStats()
    {
        return $"Name: {Name}\n" +
           $"Race: {Race}\n" +
           $"Occupation: {Occupation}\n" +
           $"Strength: {Strength}\n" +
           $"Agility: {Agility}\n" +
           $"Health: {Health}";
}
}