using System;

namespace csaspn_COLL.Services;

public class TimeBasedGreetingService : IGreetingService
{
    public string GetGreeting()
    {
        var now = DateTime.Now;
        var hour = now.Hour;

        Console.WriteLine("Current user time: " + now.TimeOfDay);

        return hour switch
        {
            >= 6 and < 12 => "Доброе утро!",
            >= 12 and < 18 => "Добрый день!",
            >= 18 and < 24 => "Добрый вечер!",
            _ => "Доброй ночи!"
        };
    }
}
