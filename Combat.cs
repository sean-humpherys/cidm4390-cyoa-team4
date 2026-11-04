

public class Combat
{
    private Player Player {get;set;}
    private Dragon Dragon {get; set;}

    public Combat(Player player, Dragon dragon)
    {
        Player = player;
        Dragon = dragon;
    }

    public void PlayAttacksDragonSequence()
    {
        Die die = new Die();

        int playerAttackRoll = die.Roll(20);

        Console.WriteLine($"You prepare to attack the dragon with your weapon. As a {Player.Occupation} you withdraw your weapon {Player.Weapon!.AsciiArt}. You attack!");

        if (playerAttackRoll <= Player.Strength)
        {
            Console.WriteLine($"You hit {Dragon.Name}");
            int dragonDefendRoll = die.Roll(20);

            if (dragonDefendRoll <= Dragon.Agility)
            {
                Console.WriteLine($"{Dragon.Name} successfully defends your attack");
                Console.WriteLine(Dragon.MissedMeTaunt());
            }
            else
            {
                int dragonDamage = die.Roll(Player.Weapon!.MaxDamage);
                Dragon.Health -= dragonDamage;
                
                Console.WriteLine($"The dragon failed to defend your attack. Your weapon delivers {dragonDamage} damage points. The Dragons health point is now {Dragon.Health}");

                if (Dragon.Health <= 0)
                {
                    Console.WriteLine($"With the last words of 'Arggg!', you have defeated the dragon.");
                    //Uncomment when Game.cs is added
                    //Game.EndGame();
                }
                else
                {
                    Console.WriteLine(Dragon.WasHitTaunt());
                }
            }
        }
        else
        {
            Console.WriteLine($"You missed {Dragon.Name}");
        }
    }
    public void DragonAttacksPlayerSequence()
    {
        Die die = new Die();

        int dragonAttackRoll = die.Roll(20);

        if (dragonAttackRoll <= Dragon.Strength)
        {
            Console.WriteLine($"{Dragon.Name} hits you");
            int playerDefendRoll = die.Roll(20);

            if (playerDefendRoll <= Player.Agility)
            {
                Console.WriteLine("You successfully defend the attack");
                Console.WriteLine(Player.MissedMeTaunt());
            }
            else
            {
                int playerDamage = die.Roll(Dragon.Weapon!.MaxDamage);
                Player.Health -= playerDamage;
                
                Console.WriteLine($"The dragon attack delivers {playerDamage} damage points. Your health point is now {Player.Health}");

                if (Player.Health <= 0)
                {
                    Console.WriteLine($"With the last words of 'Oh No!', you have been defeated.");
                    //Uncomment when Game.cs is added
                    //Game.EndGame();
                }
                else
                {
                    Console.WriteLine(Player.WasHitTaunt());
                }
            }

        }
        else
        {
            Console.WriteLine($"The dragon missed you.");
        }
    }
}

