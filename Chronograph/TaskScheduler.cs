using System.Reflection;

namespace Chronograph;

public class Periodic
{
    public static readonly Periodic Monday = new Periodic();
    public static readonly Periodic Tuesday = new Periodic();
    public static readonly Periodic Wednesday = new Periodic();
    public static readonly Periodic Thursday = new Periodic();
    public static readonly Periodic Friday = new Periodic();
    public static readonly Periodic Saturday = new Periodic();
    public static readonly Periodic Sunday = new Periodic();
    public static readonly Periodic Second = new Periodic();
    public static readonly Periodic Minute = new Periodic();
    public static readonly Periodic Hour = new Periodic();
    public static readonly Periodic Day = new Periodic();
    public static readonly Periodic Week = new Periodic();
    public static readonly Periodic Month = new Periodic();
    public static readonly Periodic Year = new Periodic();

    private Periodic()
    {
    }
}

public class TaskScheduler
{
    private DateTime _startDateTime;
    private readonly Periodic? _periodic;
    private readonly int _interval;
    private readonly Action _executableMethod;
    private readonly bool _isFirstExecute; // If true launch at the first time independently from time
    private readonly CancellationTokenSource _cancellationToken;

    public TaskScheduler(
        DateTime startDateTime,
        Periodic? periodic,
        int interval,
        bool isFirstExecute,
        Action executeMethod,
        CancellationTokenSource cancellationToken)
    {
        _startDateTime = startDateTime;
        _periodic = periodic;
        _interval = interval;
        _isFirstExecute = isFirstExecute;
        _executableMethod = executeMethod;
        _cancellationToken = cancellationToken;
    }

    public async Task JobRunner()
    {
        if (_interval <= 0 && _periodic != null)
        {
            Console.WriteLine("ERROR: Interval cannot be zero or less.");
            return;
        }

        if (_isFirstExecute)
            _executableMethod.Invoke();

        var currentDateTime = DateTime.Now;
        if (_startDateTime <= currentDateTime)
        {
            if (_periodic == null)
            {
                Console.WriteLine(
                    $"ERROR: Start date time: \"{_startDateTime:dd/MM/yyyy HH:mm:ss}\" " +
                    $"less then date time now: \"{currentDateTime:dd/MM/yyyy HH:mm:ss}\"");
                return;
            }

            var timeDelay = DelayUntilNextDateTimeToStart(_periodic, _interval, _startDateTime);
            while (timeDelay < TimeSpan.Zero)
            {
                timeDelay = DelayUntilNextDateTimeToStart(_periodic, _interval, _startDateTime);
            }

            await Task.Delay(timeDelay);
        }

        currentDateTime = DateTime.Now;
        if (_startDateTime > currentDateTime)
            await Task.Delay(_startDateTime - currentDateTime);

        while (!_cancellationToken.IsCancellationRequested)
        {
            _executableMethod.Invoke();

            if (_periodic == null) break;

            await Task.Delay(DelayUntilNextDateTimeToStart(_periodic, _interval, _startDateTime));
        }
    }

    // Calculate time delay until next date time to start
    private TimeSpan DelayUntilNextDateTimeToStart(Periodic? periodic, int interval, DateTime startDateTime)
    {
        var nextStartDateTime = startDateTime;
        
        if (periodic == Periodic.Second) nextStartDateTime = nextStartDateTime.AddSeconds(interval);
        else if (periodic == Periodic.Minute) nextStartDateTime = nextStartDateTime.AddMinutes(interval);
        else if (periodic == Periodic.Hour) nextStartDateTime = nextStartDateTime.AddHours(interval);
        else if (periodic == Periodic.Day) nextStartDateTime = nextStartDateTime.AddDays(interval);
        else if (periodic == Periodic.Week) nextStartDateTime = nextStartDateTime.AddDays(7 * interval);
        else if (periodic == Periodic.Month) nextStartDateTime = nextStartDateTime.AddMonths(interval);
        else if (periodic == Periodic.Year) nextStartDateTime = nextStartDateTime.AddYears(interval);
        else if (periodic == Periodic.Monday)
            for (var i = 0; i < interval; i++)
            {
                while (nextStartDateTime.DayOfWeek != DayOfWeek.Sunday) nextStartDateTime = nextStartDateTime.AddDays(1);
                nextStartDateTime = nextStartDateTime.AddDays(1);
            }
        else if (periodic == Periodic.Tuesday)
            for (var i = 0; i < interval; i++)
            {
                while (nextStartDateTime.DayOfWeek != DayOfWeek.Monday) nextStartDateTime = nextStartDateTime.AddDays(1);
                nextStartDateTime = nextStartDateTime.AddDays(1);
            }
        else if (periodic == Periodic.Wednesday)
            for (var i = 0; i < interval; i++)
            {
                while (nextStartDateTime.DayOfWeek != DayOfWeek.Tuesday) nextStartDateTime = nextStartDateTime.AddDays(1);
                nextStartDateTime = nextStartDateTime.AddDays(1);
            }
        else if (periodic == Periodic.Thursday)
            for (var i = 0; i < interval; i++)
            {
                while (nextStartDateTime.DayOfWeek != DayOfWeek.Wednesday) nextStartDateTime = nextStartDateTime.AddDays(1);
                nextStartDateTime = nextStartDateTime.AddDays(1);
            }
        else if (periodic == Periodic.Friday)
            for (var i = 0; i < interval; i++)
            {
                while (nextStartDateTime.DayOfWeek != DayOfWeek.Thursday) nextStartDateTime = nextStartDateTime.AddDays(1);
                nextStartDateTime = nextStartDateTime.AddDays(1);
            }
        else if (periodic == Periodic.Saturday)
            for (var i = 0; i < interval; i++)
            {
                while (nextStartDateTime.DayOfWeek != DayOfWeek.Friday) nextStartDateTime = nextStartDateTime.AddDays(1);
                nextStartDateTime = nextStartDateTime.AddDays(1);
            }
        else if (periodic == Periodic.Sunday)
            for (var i = 0; i < interval; i++)
            {
                while (nextStartDateTime.DayOfWeek != DayOfWeek.Saturday) nextStartDateTime = nextStartDateTime.AddDays(1);
                nextStartDateTime = nextStartDateTime.AddDays(1);
            }
        
        var timeDelay = nextStartDateTime - DateTime.Now;

        Console.WriteLine($"Next start date method \"{_executableMethod.GetMethodInfo().Name}\": {nextStartDateTime:dd/MM/yyyy HH:mm:ss:fff}");
        _startDateTime = nextStartDateTime;
        return timeDelay;
    }
}