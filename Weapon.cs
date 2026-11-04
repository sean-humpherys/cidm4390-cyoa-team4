
public class Weapon
{
    //Weapon information
    public string Type {get; set;}
    public int MaxDamage {get; set;}
    public string AsciiArt {get; set;}

    //Weapon
    public Weapon (string type, int maxDamage, string asciiArt){

        Type = type;
        MaxDamage = maxDamage;
        AsciiArt = asciiArt;
        
    }

}