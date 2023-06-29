namespace Chronograph;
class Program
{
    static void Main()
    {
        var cts = new CancellationTokenSource();

        var job = new Joba();
        
        var now = DateTime.Now;
        var startDateTime = new DateTime(now.Year, now.Month, now.Day, 16, 48, 00);
        var scheduler = new TaskScheduler(
            startDateTime,
            Periodic.Minute,
            2,
            true,
            job.Foo,
            cts);

        Task.Run(() => scheduler.JobRunner());

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
        cts.Cancel();
        Console.WriteLine("Main thread finished.");
    }
}