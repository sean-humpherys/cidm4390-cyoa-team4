

public class Combat
{
    private Player Player {get;set;}
    private Dragon Dragon {get; set;}

    public Combat(Player player, Dragon dragon)
    {
        Player = player;
        Dragon = dragon;
    }

        // Language file, set on the player by Game
    private Messages M => Player.LanguageMessages!;

    public string RetreatOrAttackPrompt() => M.GetMessage(9);

    // Messages 28 + 29 + 30 on one line, then the weapon art
    public string AttackIntro() =>
        $"{M.GetMessage(28)}{Player.Occupation}, {M.GetMessage(29)}{Player.Weapon!.Type}. {M.GetMessage(30)}" +
        $"{Environment.NewLine}{Player.Weapon.AsciiArt}";

    public void PlayAttacksDragonSequence()
    {
        Die die = new Die();

        int playerAttackRoll = die.Roll(20);


        if (playerAttackRoll <= Player.Strength)
        {
             Console.WriteLine($"{M.GetMessage(32)}{Dragon.Name}");
            int dragonDefendRoll = die.Roll(20);

            if (dragonDefendRoll <= Dragon.Agility)
            {
                Console.WriteLine($"{Dragon.Name}{M.GetMessage(33)}");
                Console.WriteLine(Dragon.MissedMeTaunt());
            }
            else
            {
                int dragonDamage = die.Roll(Player.Weapon!.MaxDamage);
                Dragon.Health -= dragonDamage;
                
                Console.WriteLine($"{M.GetMessage(39)}{dragonDamage}{M.GetMessage(40)} {Dragon.Health}");

                if (Dragon.Health <= 0)
                {
                    Console.WriteLine(M.GetMessage(41));
                    
                }
                else
                {
                    Console.WriteLine(Dragon.WasHitTaunt());
                }
            }
        }
        else
        {
            Console.WriteLine($"{M.GetMessage(31)} {Dragon.Name}");
        }
    }
    public void DragonAttacksPlayerSequence()
    {
        Die die = new Die();

        int dragonAttackRoll = die.Roll(20);

        if (dragonAttackRoll <= Dragon.Strength)
        {
            Console.WriteLine($"{Dragon.Name}{M.GetMessage(47)}");
            int playerDefendRoll = die.Roll(20);

            if (playerDefendRoll <= Player.Agility)
            {
                Console.WriteLine(Player.MissedMeTaunt());
            }
            else
            {
                int playerDamage = die.Roll(Dragon.Weapon!.MaxDamage);
                Player.Health -= playerDamage;
                
                Console.WriteLine($"{M.GetMessage(49)}{playerDamage}{M.GetMessage(50)}{Player.Health}");

                if (Player.Health <= 0)
                {
                    Console.WriteLine(M.GetMessage(51));
                   
                }
                else
                {
                    Console.WriteLine(Player.WasHitTaunt());
                }
            }

        }
        else
        {
            Console.WriteLine(M.GetMessage(46));
        }
    }
}

