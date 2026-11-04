

public class Game
{
    private const string DragonName = "Smaug";

    public Die Die { get; private set; }
    public Dragon Dragon { get; private set; }
    public Messages Messages { get; private set; }
    public Player Player { get; private set; }
    public Combat Combat { get; private set; }
    public Weapon Weapon {get; private set;}

    // True until the player chooses to end the game from the menu
    public bool IsRunning { get; private set; } = true;

    // True while a game is in progress (after "new game", before EndGame)
    public bool IsGameActive { get; private set; }

    public void StartGame()
    {
        // new Die(), only needed once
        Die ??= new Die();

        // new Message(), language chosen once, then the dictionary is read
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

        // getMessage(2) menu message: 1 = start a new game, 2 = end the game
        Console.WriteLine(Messages.GetMessage(2));

        // new Dragon(name)
        Dragon = new Dragon(DragonName);
        //Dragon.Health = 100;
        //Dragon.Strength = 10;

        // getMessage(3) ask player's name
        Console.WriteLine(Messages.GetMessage(12));
        string name = Console.ReadLine() ?? "";



        // getMessage(5) ask occupation
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
        Console.WriteLine(Messages.GetMessage(14));   // Next, we are going to roll for strength. Type 'roll'
        while (!(Console.ReadLine() ?? "").Trim().Equals("roll", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Please type 'roll'.");
        }
        int strength = Die.Roll(20);
        //Console.WriteLine($"Current strength: {strength}");

        // d4 strength modifier: subtracted for Halfling, added for everyone else
        int strengthModifier = Die.Roll(4);
        //Console.WriteLine($"Current strengthMod: {strengthModifier}");

        if (race == Race.Halfling)
        {
            strength -= strengthModifier;
        }
        else
        {
            strength += strengthModifier;
        }
        strength = Math.Max(1, strength);   // keep strength from dropping to 0 or below
        //Console.WriteLine($"{Messages.GetMessage(22)} {strength}");

        // ---- Agility ----
        Console.WriteLine(Messages.GetMessage(15));   // Next, we will roll for agility. Type 'roll'
        while (!(Console.ReadLine() ?? "").Trim().Equals("roll", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Please type 'roll'.");
        }
        int agility = Die.Roll(20);
        //Console.WriteLine($"Current agility: {agility}");

        int agilityModifier = Die.Roll(4);
        //Console.WriteLine($"Current agilityMod: {agilityModifier}");

        if (race == Race.Halfling || race == Race.Elf)
        {
            agility += agilityModifier;
        }
        //Console.WriteLine($"{Messages.GetMessage(23)} {agility}");

        // ---- Health ----
        Console.WriteLine(Messages.GetMessage(16));   // Next, we will roll for health points. Type 'roll'
        while (!(Console.ReadLine() ?? "").Trim().Equals("roll", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Please type 'roll'.");
        }
        int health = Die.Roll(20);
        //Console.WriteLine($"Current health: {health}");

        if (race != Race.Halfling)
        {
            health += strengthModifier;   // the d4 rolled for strength earlier
        }
        //Console.WriteLine($"{Messages.GetMessage(24)} {health}");

        Player = new Player(name, race.ToString(), occupation.ToString())
        {
            Strength = strength,
            Agility = agility,
            Health = health,
            Weapon = weapon
        };

        // new Dragon(name)
        Dragon = new Dragon(DragonName)
        {
            Health = Die.Roll(20),
            Strength = Die.Roll(20),
            Agility = Die.Roll(20),
            Weapon = new Weapon("Fire Breath", 12, "")
        };

        // new Combat(player, dragon)
        Combat = new Combat(Player, Dragon);

         // ---- What next? (message 17) ----
        Console.WriteLine(Messages.GetMessage(17));
        string nextChoice = (Console.ReadLine() ?? "").Trim();

        while (nextChoice != "1" && nextChoice != "2")
        {
            nextChoice = (Console.ReadLine() ?? "").Trim();
        }

        if (nextChoice == "1")
        {
            Console.WriteLine();
            Console.WriteLine(Messages.GetMessage(18));
            Console.WriteLine($"{Messages.GetMessage(19)} {Player.Name}");
            Console.WriteLine($"{Messages.GetMessage(20)} {Player.Race}");
            Console.WriteLine($"{Messages.GetMessage(21)} {Player.Occupation}");
            Console.WriteLine($"{Messages.GetMessage(22)} {Player.Strength}");
            Console.WriteLine($"{Messages.GetMessage(23)} {Player.Agility}");
            Console.WriteLine($"{Messages.GetMessage(24)} {Player.Health}");
            Console.WriteLine($"{Messages.GetMessage(25)} {Player.Weapon.Type}");
            Console.WriteLine($"{Messages.GetMessage(26)} {Player.Weapon.MaxDamage}");
        }

        // '2' falls through with no stats shown

        // ...then the path choice (message 3), dragon stats, retreat/attack, and IsGameActive = true as before

        // Path choice: message 3 prompts, 'n' = message 4, 's' = message 5, 'e' = exit
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
        Console.WriteLine($"{Messages.GetMessage(6)}{DragonName}{Messages.GetMessage(7)}");
        Console.ResetColor();

        // Magic ring shows the dragon's stats
        Console.WriteLine(Messages.GetMessage(8));
        Console.WriteLine($"Name: {Dragon.Name}");
        Console.WriteLine($"Health: {Dragon.Health}");
        Console.WriteLine($"Strength: {Dragon.Strength}");
        Console.WriteLine($"Agility: {Dragon.Agility}");

        IsGameActive = true;
    }



    public void EndGame()
    {
        IsGameActive = false;
        IsRunning = false;
        Player = null;
        Dragon = null;
        Combat = null;

        Console.WriteLine();
        Console.WriteLine("Game over. Thanks for playing!");
    }
}