using System;

namespace csaspn_COLL.Services;

public class RandomNumberService : IRandomNumberService
{
    public int Number { get; }

    public RandomNumberService()
    {
        var rnd = new Random();
        Number = rnd.Next(1, 101);
    }
}
