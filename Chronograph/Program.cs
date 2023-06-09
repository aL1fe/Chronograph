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

        var scheduler = new TaskScheduler();
        var now = DateTime.Now;
        var startDateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 25, 00);

        Task.Run(() => scheduler.JobRunner(
            startDateTime,
            Every.Second,
            30,
            job.Foo,
            false,
            true,
            cts));
        
        // var scheduler02 = new TaskScheduler();
        // var startTime2 = new DateTime(now.Year, now.Month, now.Day, now.Hour, 21, 40);
        // Task.Run(() => scheduler02.JobRunner(
        //     startTime2,
        //     Every.Minute,
        //     20,
        //     job.Boo,
        //     true,
        //     true,
        //     cts));

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
        cts.Cancel();
        Console.WriteLine("Main thread finished.");
    }
}