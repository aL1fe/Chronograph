namespace Chronograph;
enum Every
{
    Monday,
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
    static Task Main()
    {
        var cts = new CancellationTokenSource();

        var job = new Joba();
        
        var scheduler = new TaskScheduler();
        var now = DateTime.Now;
        var startTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 14, 20);
        
        var task2 = Task.Run(() => scheduler.JobRunner(
            startTime, 
            Every.Second, 
            30,
            job.Foo, 
            false,
            true,
            cts));
     
        Console.ReadLine();
        cts.Cancel();
        Console.WriteLine("Main thread finished.");
        return Task.CompletedTask;
    }
}