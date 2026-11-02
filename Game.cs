using cidm4390_cyoa_team4;

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

            // getMessage(1) first message
            Console.WriteLine(Messages.GetMessage(1));
        }

        // getMessage(2) menu message: 1 = start a new game, 2 = end the game
        Console.WriteLine(Messages.GetMessage(2));
        string menuChoice = (Console.ReadLine() ?? "").Trim();
        while (menuChoice != "1" && menuChoice != "2")
        {
            Console.WriteLine("Please enter 1 or 2.");
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

        // getMessage(3) ask player's name
        Console.WriteLine(Messages.GetMessage(3));
        string name = Console.ReadLine() ?? "";

        // getMessage(4) ask race
        Race race;
        Console.WriteLine(Messages.GetMessage(4));
        Console.WriteLine(string.Join(", ", Enum.GetNames(typeof(Race))));
        while (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out race) || !Enum.IsDefined(race))
        {
            Console.WriteLine($"Please choose one of: {string.Join(", ", Enum.GetNames(typeof(Race)))}");
        }

        // getMessage(5) ask occupation
        Occupation occupation;
        Console.WriteLine(Messages.GetMessage(5));
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

        // new Combat(player, dragon)
        Combat = new Combat(Player, Dragon);

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