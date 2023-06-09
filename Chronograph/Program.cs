namespace Chronograph;

enum Every
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday,

    // LastDayOfMonth
    // FirstDayOfMonth
    Second,
    Minute,
    Hour,
    Day,
    Week,
    Month,
    Year
}

class Program
{
    static void Main()
    {
        var cts = new CancellationTokenSource();

        var job = new Joba();
        
        var now = DateTime.Now;
        var startDateTime = new DateTime(now.Year, now.Month, now.Day, 17, 00, 00);
        var scheduler = new TaskScheduler(startDateTime,
            Every.Hour,
            3,
            job.Foo,
            false,
            true,
            cts);

        Task.Run(() => scheduler.JobRunner());

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
        cts.Cancel();
        Console.WriteLine("Main thread finished.");
    }
}