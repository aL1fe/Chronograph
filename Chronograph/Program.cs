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
        Console.WriteLine("Press Enter to exit...");
        var cts = new CancellationTokenSource();

        var job = new Joba();
        
        var scheduler = new TaskScheduler();
        var now = DateTime.Now;
        var startTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 09, 33);
        
        var task = Task.Run(() => scheduler.JobRunner(
            startTime, 
            Every.Second, 
            11,
            job.Foo, 
            true,
            true,
            cts));
     
        Console.ReadLine();
        cts.Cancel();
        Console.WriteLine("Main thread finished.");
    }
}