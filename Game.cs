public class Game
{
    private const string DragonName = "Smaug";

    public Die Die { get; private set; }
    public Dragon Dragon { get; private set; }
    public Messages Messages { get; private set; }
    public Player Player { get; private set; }
    public Combat Combat { get; private set; }
    public Weapon Weapon { get; private set; }

    // True until the game ends
    public bool IsRunning { get; private set; } = true;

    // True while a game is in progress (after setup, before EndGame)
    public bool IsGameActive { get; private set; }

    public void StartGame()
    {
        Die ??= new Die();

        // Language chosen once, then the dictionary is read
        if (Messages == null)
        {
            Messages = new Messages();

            Languages language;
            Console.WriteLine($"Choose a language: {string.Join(", ", Enum.GetNames(typeof(Languages)))}");
            while (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out language) || !Enum.IsDefined(language))
            {
                Console.WriteLine($"Please choose one of: {string.Join(", ", Enum.GetNames(typeof(Languages)))}");
            }
            Messages.ReadDictionary(language);
        }

        Console.WriteLine(Messages.GetMessage(2));   // Welcome

        // Name
        Console.WriteLine(Messages.GetMessage(12));
        string name = Console.ReadLine() ?? "";

        // Occupation
        Occupation occupation;
        Console.WriteLine(Messages.GetMessage(13));
        Console.WriteLine(string.Join(", ", Enum.GetNames(typeof(Occupation))));
        while (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out occupation) || !Enum.IsDefined(occupation))
        {
            Console.WriteLine($"Please choose one of: {string.Join(", ", Enum.GetNames(typeof(Occupation)))}");
        }

        // Weapon is determined by the chosen occupation
        Weapon weapon = occupation switch
        {
            Occupation.Fighter  => new Weapon(Messages.GetMessage(60), 12, Messages.GetMessage(61)),
            Occupation.Magician => new Weapon(Messages.GetMessage(62), 12, Messages.GetMessage(63)),
            Occupation.Thief    => new Weapon(Messages.GetMessage(64), 6, Messages.GetMessage(65)),
            Occupation.Archer   => new Weapon(Messages.GetMessage(66), 8, Messages.GetMessage(67)),
            _                   => new Weapon("Sword", 10, "")   // fallback, shouldn't happen
        };

        // Race
        Race race;
        Console.WriteLine(Messages.GetMessage(11));
        while (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out race) || !Enum.IsDefined(race))
        {
            Console.WriteLine($"Please choose one of: {string.Join(", ", Enum.GetNames(typeof(Race)))}");
        }

        // ---- Strength ----
        do
        {
            Console.WriteLine(Messages.GetMessage(14));
        }
        while (!(Console.ReadLine() ?? "").Trim().Equals("roll", StringComparison.OrdinalIgnoreCase));
        int strength = Die.Roll(20);
        Console.WriteLine($"{Messages.GetMessage(22)} {strength}");

        // d4 strength modifier: subtracted for Halfling, added for everyone else
        int strengthModifier = Die.Roll(4);
        if (race == Race.Halfling)
        {
            strength -= strengthModifier;
            Console.WriteLine($"Modifier: -{strengthModifier}");
        }
        else
        {
            strength += strengthModifier;
            Console.WriteLine($"Modifier: +{strengthModifier}");
        }
        strength = Math.Max(1, strength);   // keep strength from dropping to 0 or below
        Console.WriteLine($"{Messages.GetMessage(22)} {strength}");

        // ---- Agility ----
        do
        {
            Console.WriteLine(Messages.GetMessage(15));
        }
        while (!(Console.ReadLine() ?? "").Trim().Equals("roll", StringComparison.OrdinalIgnoreCase));
        int agility = Die.Roll(20);
        Console.WriteLine($"{Messages.GetMessage(23)} {agility}");

        int agilityModifier = Die.Roll(4);
        if (race == Race.Halfling || race == Race.Elf)
        {
            agility += agilityModifier;
            Console.WriteLine($"Modifier: +{agilityModifier}");
        }
        Console.WriteLine($"{Messages.GetMessage(23)} {agility}");

        // ---- Health ----
        do
        {
            Console.WriteLine(Messages.GetMessage(16));
        }
        while (!(Console.ReadLine() ?? "").Trim().Equals("roll", StringComparison.OrdinalIgnoreCase));
        int health = Die.Roll(20);
        Console.WriteLine($"{Messages.GetMessage(24)} {health}");

        if (race != Race.Halfling)
        {
            health += strengthModifier;   // the d4 rolled for strength earlier
            Console.WriteLine($"Modifier: +{strengthModifier}");
        }
        Console.WriteLine($"{Messages.GetMessage(24)} {health}");

        // LanguageMessages makes the player's own taunts use the chosen language
        Player = new Player(name, race.ToString(), occupation.ToString())
        {
            Strength = strength,
            Agility = agility,
            Health = health,
            Weapon = weapon,
            LanguageMessages = Messages
        };

        // The dragon is created once, here
        Dragon = new Dragon(DragonName)
        {
            Health = Die.Roll(20),
            Strength = Die.Roll(20),
            Agility = Die.Roll(20),
            Weapon = new Weapon("Fire Breath", 12, "")
        };

        Combat = new Combat(Player, Dragon);

        // ---- What next? ----
        Console.WriteLine(Messages.GetMessage(17));
        string nextChoice = (Console.ReadLine() ?? "").Trim();

        while (nextChoice != "1" && nextChoice != "2")
        {
            nextChoice = (Console.ReadLine() ?? "").Trim();
        }

        if (nextChoice == "1")
        {
            Console.WriteLine();
            Console.WriteLine(Player.DisplayStats());   // stats text comes from Player
            Console.WriteLine();
        }

        // ---- Path choice: 'n' or 's' continue, 'e' exits ----
        Console.WriteLine(Messages.GetMessage(3));
        string pathChoice = (Console.ReadLine() ?? "").Trim().ToLower();

        while (pathChoice != "n" && pathChoice != "s" && pathChoice != "e")
        {
            pathChoice = (Console.ReadLine() ?? "").Trim().ToLower();
        }

        if (pathChoice == "e")
        {
            EndGame();
            return;   // stop here so IsGameActive isn't set back to true
        }

        Console.WriteLine(Messages.GetMessage(pathChoice == "n" ? 4 : 5));

        // Dragon speaks in red: message 6 + dragon's name + message 7 on one line
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{Messages.GetMessage(6)}{Dragon.Name}{Messages.GetMessage(7)}");
        Console.ResetColor();

        // Magic ring shows the dragon's stats
        Console.WriteLine(Messages.GetMessage(8));
        Console.WriteLine(Dragon.DisplayStats());   // stats text comes from Dragon

        IsGameActive = true;
    }

    public void EndGame()
    {
        // Only print an ending if a fight was in progress.
        // Choosing 'e' at the path prompt ends the game before any fight, so it prints none.
        if (IsGameActive)
        {
            if (Dragon != null && Dragon.Health <= 0)
            {
                Console.WriteLine(Messages.GetMessage(58));   // dragon defeated
            }
            else if (Player != null && Player.Health <= 0)
            {
                Console.WriteLine(Messages.GetMessage(59));   // player defeated
            }
            else
            {
                Console.WriteLine(Messages.GetMessage(57));   // player retreated
            }
        }

        IsGameActive = false;
        IsRunning = false;
        Player = null;
        Dragon = null;
        Combat = null;

        Console.WriteLine();
        Console.WriteLine("Game over. Thanks for playing!");
    }
}