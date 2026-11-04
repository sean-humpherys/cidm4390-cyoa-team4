
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

    // Messages for the selected language
    public Messages? LanguageMessages { get; set; }
    private Messages? defaultMessages;

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

    // Return the translated message when the player defends
    public string MissedMeTaunt()
    {
        return GetLanguageMessages().GetMessage(48);
    }

    // Randomly select a translated response when the player is hit
    public string WasHitTaunt()
    {
        int messageNumber = Random.Shared.Next(52, 56);
        return GetLanguageMessages().GetMessage(messageNumber);
    }

    // Use the game's selected language, or English by default
    private Messages GetLanguageMessages()
    {
        if (LanguageMessages != null)
        {
            return LanguageMessages;
        }

        if (defaultMessages == null)
        {
            defaultMessages = new Messages();
            defaultMessages.ReadDictionary(Languages.English);
        }

        return defaultMessages;
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
