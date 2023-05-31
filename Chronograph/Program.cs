namespace Chronograph;
enum Every
{
    // Monday,
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
        
        var d = new TaskScheduler();
        TaskScheduler.VoidMethod method = MyMethod;

        var task = d.JobRunner(method, "hello", cts);
     
        Console.ReadLine();
        cts.Cancel();
        Console.WriteLine("Main thread finished.");
        return Task.CompletedTask;
    }

    static void MyMethod(string message)
    {
        Console.WriteLine(message);  
    }
}