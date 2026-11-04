﻿Game game = new Game();

while (game.IsRunning)
{
    game.StartGame();

    bool firstRound = true;   // the attack intro prints only once

    while (game.IsGameActive)
    {
        // Retreat or attack (message 9)
        Console.WriteLine();
        Console.WriteLine(game.Messages.GetMessage(9));
        string fightChoice = (Console.ReadLine() ?? "").Trim().ToLower();

        while (fightChoice != "r" && fightChoice != "a")
        {
            fightChoice = (Console.ReadLine() ?? "").Trim().ToLower();
        }

        if (fightChoice == "r")
        {
            Console.WriteLine(game.Messages.GetMessage(57));   // You quickly retreat... The End.
            game.EndGame();
            break;
        }

        // First attack only: messages 28 + 29 + 30 on one line, then the occupation message
        if (firstRound)
        {
            Console.WriteLine($"{game.Messages.GetMessage(28)}{game.Player.Occupation}, {game.Messages.GetMessage(29)}{game.Player.Weapon.Type}. {game.Messages.GetMessage(30)}");
            Console.WriteLine(game.Player.Weapon.AsciiArt);   // messages 61/63/65/67, set when the weapon was created
            firstRound = false;
        }

        game.Combat.PlayAttacksDragonSequence();
        if (game.Dragon.Health <= 0)
        {
            Console.WriteLine(game.Messages.GetMessage(58));   // With the dragon vanquished... The End.
            game.EndGame();
            break;
        }

        game.Combat.DragonAttacksPlayerSequence();
        if (game.Player.Health <= 0)
        {
            Console.WriteLine(game.Messages.GetMessage(59));   // The dragon laughs as your eyesight fades... The End.
            game.EndGame();
            break;
        }
    }
}