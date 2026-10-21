/* Testing:
Die die = new Die();
int result = die.Roll();
Console.WriteLine($"Number is: {result}");
*/

public class Die
{
    private readonly Random random = new Random();
    public const int MaxValue = 20;

    public int Roll()
    {
        int number = random.Next(1, MaxValue + 1);
        return number;
    }
}