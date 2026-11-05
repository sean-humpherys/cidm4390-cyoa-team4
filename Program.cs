﻿Game game = new Game();

while (game.IsRunning)
{
    game.StartGame();

    while (game.IsGameActive)
    {
        // Retreat or attack (no equivalent in Combat, so this is the one JSON message here)
        Console.WriteLine();
        Console.WriteLine(game.Messages.GetMessage(9));
        string fightChoice = (Console.ReadLine() ?? "").Trim().ToLower();

        while (fightChoice != "r" && fightChoice != "a")
        {
            fightChoice = (Console.ReadLine() ?? "").Trim().ToLower();
        }

        if (fightChoice == "r")
        {
            game.EndGame();
            break;
        }

        // Combat prints the attack intro, hits, misses and taunts itself
        game.Combat.PlayAttacksDragonSequence();
        if (game.Dragon.Health <= 0)
        {
            game.EndGame();   // Combat already printed "With the last words of 'Arggg!'..."
            break;
        }

        game.Combat.DragonAttacksPlayerSequence();
        if (game.Player.Health <= 0)
        {
            game.EndGame();   // Combat already printed "With the last words of 'Oh No!'..."
            break;
        }
    }
}