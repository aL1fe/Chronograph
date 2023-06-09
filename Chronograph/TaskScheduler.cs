namespace Chronograph;

public class TaskScheduler
{
    private DateTime _startDateTime;
    private readonly Enum _periodic;
    private readonly int _interval;
    private readonly Action _executableMethod;
    private readonly bool _isFirstExecute;  // If true launch at the first time independently from time, if false launch on time
    private readonly bool _isLoop;  // If true launch cyclically
    private readonly CancellationTokenSource _cancellationToken;

    public TaskScheduler(
        DateTime startDateTime,
        Enum periodic,
        int interval,
        Action executeMethod,
        bool isFirstExecute,
        bool isLoop,
        CancellationTokenSource cancellationToken)
    {
        _startDateTime = startDateTime;
        _periodic = periodic;
        _interval = interval;
        _executableMethod = executeMethod;
        _isFirstExecute = isFirstExecute;
        _isLoop = isLoop;
        _cancellationToken = cancellationToken;
    }

    public async Task JobRunner()
    {
        if (_isFirstExecute) _executableMethod.Invoke();
        
        var currentDateTime = DateTime.Now;
        if (_startDateTime <= currentDateTime)
        {
            if (!_isLoop && !_isFirstExecute)
            {
                Console.WriteLine(
                    $"ERROR: Start date time: \"{_startDateTime.ToString("dd/MM/yyyy HH:mm:ss")}\" " +
                    $"less then date time now: \"{currentDateTime.ToString("dd/MM/yyyy HH:mm:ss")}\"");
                return;
            }

            var timeDelay = DelayUntilNextDateTimeToStart(_periodic, _interval, _startDateTime);
            if (timeDelay < TimeSpan.Zero)
            {
                Console.WriteLine(
                    $"ERROR: Next start date time: \"{_startDateTime.ToString("dd/MM/yyyy HH:mm:ss")}\" " +
                    $"less then date time now: \"{currentDateTime.ToString("dd/MM/yyyy HH:mm:ss")}\"");
                Console.WriteLine($"ERROR: Periodic: \"{_periodic}\" and interval: \"{_interval}\" " +
                                  "are not consist with start date time.");
                return;
            }
            await Task.Delay(timeDelay);
        }

        currentDateTime = DateTime.Now;
        if (_startDateTime > currentDateTime)
            await Task.Delay(_startDateTime - currentDateTime);

        while (!_cancellationToken.IsCancellationRequested)
        {
            _executableMethod.Invoke();

            if (!_isLoop) break;

            await Task.Delay(DelayUntilNextDateTimeToStart(_periodic, _interval, _startDateTime));
        }
    }

    // Calculate time delay until next date time to start
    public TimeSpan DelayUntilNextDateTimeToStart(Enum periodic, int interval, DateTime startDateTime)
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
        var timeDelay = nextStartDateTime - DateTime.Now;
        
        Console.WriteLine("Next start date: " + nextStartDateTime.ToString("dd/MM/yyyy HH:mm:ss:fff"));
        _startDateTime = nextStartDateTime;
        return timeDelay;
    }
}