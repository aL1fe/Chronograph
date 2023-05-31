namespace Chronograph;

public class TaskScheduler
{
    public delegate void VoidMethod(string message);

    public async Task JobRunner(VoidMethod executeMethod, string message, CancellationTokenSource cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            executeMethod.Invoke(message);
            await Task.Delay(1000);
        }
    }
}


// (DateTime startTime,
//         Action action,
//         bool isFirstExecute, // If true launch at the first time independently from time, if false launch on time
//         bool isLoop, // If true launch cyclically
//         Enum periodic
//     )