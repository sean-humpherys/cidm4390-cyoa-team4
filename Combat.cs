using System.Security;

public class Combat
{
    private Player player {get;set;}
    private Dragon dragon {get; set;}

    public Combat(Player player, Dragon dragon)
    {
        Player = player;
        Dragon = dragon;
    }

    public void PlayAttacksDragonSequence()
    {
        int attackRoll = Roll();

        if (attackRoll > Player.Strength)
        {
            
        }
    }
    public void DragonAttacksPlayerSequence()
    {
        
    }
}

