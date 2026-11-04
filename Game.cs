

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

        Console.WriteLine("Please enter '1' to continue or '2' to exit the game");
        string menuChoice = (Console.ReadLine() ?? "").Trim();

        while (menuChoice != "1" && menuChoice != "2")
        {
            menuChoice = (Console.ReadLine() ?? "").Trim();
        }

        if (menuChoice == "2")
        {
            IsRunning = false;
            EndGame();
            return;
        }

        // new Dragon(name)
        Dragon = new Dragon(DragonName);
        //Dragon.Health = 100;
        //Dragon.Strength = 10;

        // getMessage(3) ask player's name
        Console.WriteLine(Messages.GetMessage(12));
        string name = Console.ReadLine() ?? "";

        // getMessage(4) ask race
        Race race;
        Console.WriteLine(Messages.GetMessage(11));
        Console.WriteLine(string.Join(", ", Enum.GetNames(typeof(Race))));
        while (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out race) || !Enum.IsDefined(race))
        {
            Console.WriteLine($"Please choose one of: {string.Join(", ", Enum.GetNames(typeof(Race)))}");
        }

        // getMessage(5) ask occupation
        Occupation occupation;
        Console.WriteLine(Messages.GetMessage(13));
        Console.WriteLine(string.Join(", ", Enum.GetNames(typeof(Occupation))));
        while (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out occupation) || !Enum.IsDefined(occupation))
        {
            Console.WriteLine($"Please choose one of: {string.Join(", ", Enum.GetNames(typeof(Occupation)))}");
        }

        // new Player(name, race, occupation)
        const int StatPoints = 30;
        int strength, agility;

        while (true)
        {
            Console.WriteLine($"You have {StatPoints} points to split between Strength and Agility.");

            Console.WriteLine("Enter Strength (1-20):");
            while (!int.TryParse(Console.ReadLine()?.Trim(), out strength) || strength < 1 || strength > 20)
            {
                Console.WriteLine("Please enter a whole number from 1 to 20.");
            }

            Console.WriteLine("Enter Agility (1-20):");
            while (!int.TryParse(Console.ReadLine()?.Trim(), out agility) || agility < 1 || agility > 20)
            {
                Console.WriteLine("Please enter a whole number from 1 to 20.");
            }

            if (strength + agility <= StatPoints) break;

            Console.WriteLine($"That totals {strength + agility}. It must be {StatPoints} or less. Try again.");
        }

        Player = new Player(name, race.ToString(), occupation.ToString())
        {
            Strength = strength,
            Agility = agility,
            Health = 50,
            Weapon = new Weapon("Sword", 10, "")
        };

        // new Dragon(name)
        Dragon = new Dragon(DragonName)
        {
            Health = 100,
            Strength = 10,
            Weapon = new Weapon("Fire Breath", 12, "")
        };

        // new Combat(player, dragon)
        Combat = new Combat(Player, Dragon);

        // Display the character summary
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
        Console.WriteLine();

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

        // Retreat or attack
        Console.WriteLine(Messages.GetMessage(9));
        string fightChoice = (Console.ReadLine() ?? "").Trim().ToLower();

        while (fightChoice != "r" && fightChoice != "a")
        {
            fightChoice = (Console.ReadLine() ?? "").Trim().ToLower();
        }

        if (fightChoice == "r")
        {
            EndGame();
            return;   // skip IsGameActive = true
        }

// Attack activity

        Console.WriteLine("Player is attacking");

        IsGameActive = true;
    }



    public void EndGame()
    {
        IsGameActive = false;
        Player = null;
        Dragon = null;
        Combat = null;

        Console.WriteLine();
        Console.WriteLine("Game over. Thanks for playing!");
    }
}