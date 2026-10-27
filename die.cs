public class Die
{
    private readonly Random random = new Random();

    public int Roll(int sides)
    {
        if (sides < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(sides), sides, "A die must have at least 1 side.");
        }

        return random.Next(1, sides + 1);
    }
}