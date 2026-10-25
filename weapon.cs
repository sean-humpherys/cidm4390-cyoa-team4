namespace cidm4390_cyoa_team4;


public class Weapon
{
    //Weapon information
    public string Type {get; set;}
    public int MaxDamage {get; set;}
    public string AsciiArt {get; set;}

    public Weapon (string type, int maxDamage, string asciiArt){

        Type = type;
        MaxDamage = maxDamage;
        AsciiArt = asciiArt;
    }

    public string DisplayWeapon() {

       return $"Weapon: {Type}\n" +
       $"Max Damage: {MaxDamage}";
    }   
}

