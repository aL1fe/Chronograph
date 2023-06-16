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
        var startDateTime = new DateTime(now.Year, now.Month, now.Day, 12, 15, 00);
        var scheduler = new TaskScheduler(
            startDateTime,
            Every.Minute,
            10,
            false,
            job.Foo,
            cts);

        Task.Run(() => scheduler.JobRunner());

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
        cts.Cancel();
        Console.WriteLine("Main thread finished.");
    }
}