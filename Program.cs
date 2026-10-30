// Game:


Game game = new Game();

while (game.IsRunning)
{
    // Shows the menu; sets up a new game, or ends the program if the player chooses to end
    game.StartGame();

    while (game.IsGameActive)
    {
        // Attack or end the current game
        Console.WriteLine(game.Messages.GetMessage(6));
        string choice = (Console.ReadLine() ?? "").Trim();

        if (choice.Equals("Q", StringComparison.OrdinalIgnoreCase))
        {
            game.EndGame();   // back to the menu
            break;
        }

        game.Combat.PlayerAttacksDragonSequence();
        if (game.Dragon.Health <= 0)
        {
            game.EndGame();
            break;
        }

        game.Combat.DragonAttacksPlayerSequence();
        if (game.Player.Health <= 0)
        {
            game.EndGame();
        }
    }
}