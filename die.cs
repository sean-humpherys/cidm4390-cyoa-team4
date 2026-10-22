
public class Die
{
    private readonly Random random = new Random();
    private readonly int _sides;

    public Die(int sides)
    {
        if (sides < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(sides), sides, "A die must have at least 1 side.");
        }

        _sides = sides;
    }

    public int Sides
    {
        get { return _sides; }
    }

    public int Roll()
    {
        return random.Next(1, _sides + 1);
    }
}