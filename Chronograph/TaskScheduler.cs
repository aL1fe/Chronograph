namespace Chronograph;

public class TaskScheduler
{
    // DI in ctor

    public async Task JobRunner(
        DateTime startDateTime,
        Enum periodic,
        int interval,
        Action executeMethod,
        bool isFirstExecute, // If true launch at the first time independently from time, if false launch on time
        bool isLoop, // If true launch cyclically
        CancellationTokenSource cancellationToken)
    {
        var currentDateTime = DateTime.Now;
        if (startDateTime <= currentDateTime) //&& !isLoop)
        {
            Console.WriteLine("ERROR: Start date time: " + startDateTime.ToString("dd/MM/yyyy HH:mm:ss") +
                              " less then date time now: " + currentDateTime.ToString("dd/MM/yyyy HH:mm:ss"));
            return;
        }

        if (isFirstExecute) executeMethod.Invoke();

        currentDateTime = DateTime.Now;
        if (startDateTime > currentDateTime)
            await Task.Delay(startDateTime - currentDateTime);

        while (!cancellationToken.IsCancellationRequested)
        {
            executeMethod.Invoke();

            if (!isLoop) break;

            
            var nextStartDateTime = NextDateTimeToStart(periodic, interval, startDateTime);
            
            var timeDelay = nextStartDateTime - DateTime.Now;
            Console.WriteLine("Next start date: " + nextStartDateTime.ToString("dd/MM/yyyy HH:mm:ss:fff"));

            await Task.Delay(timeDelay);
            startDateTime = nextStartDateTime;
        }
    }

    // Calculate delay to next date time to start
    public DateTime NextDateTimeToStart(Enum periodic, int interval, DateTime startDateTime)
    {
        var nextStartDateTime = startDateTime;
        switch (periodic)
        {
            case Every.Second:
                nextStartDateTime = nextStartDateTime.AddSeconds(interval);
                break;
            case Every.Minute:
                nextStartDateTime = nextStartDateTime.AddMinutes(interval);
                break;
            case Every.Hour:
                nextStartDateTime = nextStartDateTime.AddHours(interval);
                break;
            case Every.Day:
                nextStartDateTime = nextStartDateTime.AddDays(interval);
                break;
            case Every.Week:
                nextStartDateTime = nextStartDateTime.AddDays(interval);
                break;
            case Every.Month:
                nextStartDateTime = nextStartDateTime.AddMonths(interval);
                break;
            case Every.Year:
                nextStartDateTime = nextStartDateTime.AddYears(interval);
                break;
            case Every.Monday:
                for (int i = 0; i < interval; i++)
                {
                    while (nextStartDateTime.DayOfWeek != DayOfWeek.Sunday)
                    {
                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }
                    nextStartDateTime = nextStartDateTime.AddDays(1);
                }
                break;
            case Every.Tuesday:
                for (int i = 0; i < interval; i++)
                {
                    while (nextStartDateTime.DayOfWeek != DayOfWeek.Monday)
                    {
                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }
                    nextStartDateTime = nextStartDateTime.AddDays(1);
                }
                break;
            case Every.Wednesday:
                for (int i = 0; i < interval; i++)
                {
                    while (nextStartDateTime.DayOfWeek != DayOfWeek.Tuesday)
                    {
                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }
                    nextStartDateTime = nextStartDateTime.AddDays(1);
                }
                break;
            case Every.Thursday:
                for (int i = 0; i < interval; i++)
                {
                    while (nextStartDateTime.DayOfWeek != DayOfWeek.Wednesday)
                    {
                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }
                    nextStartDateTime = nextStartDateTime.AddDays(1);
                }
                break;
            case Every.Friday:
                for (int i = 0; i < interval; i++)
                {
                    while (nextStartDateTime.DayOfWeek != DayOfWeek.Thursday)
                    {
                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }
                    nextStartDateTime = nextStartDateTime.AddDays(1);
                }
                break;
            case Every.Saturday:
                for (int i = 0; i < interval; i++)
                {
                    while (nextStartDateTime.DayOfWeek != DayOfWeek.Friday)
                    {
                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }
                    nextStartDateTime = nextStartDateTime.AddDays(1);
                }
                break;
            case Every.Sunday:
                for (int i = 0; i < interval; i++)
                {
                    while (nextStartDateTime.DayOfWeek != DayOfWeek.Saturday)
                    {
                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }
                    nextStartDateTime = nextStartDateTime.AddDays(1);
                }
                break;
        }
        //return nextStartDateTime.AddMilliseconds(-nextStartDateTime.Millisecond);
        return nextStartDateTime;
    }
}