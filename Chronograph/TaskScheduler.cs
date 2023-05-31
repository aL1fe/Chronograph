namespace Chronograph;

public class TaskScheduler
{
    public async Task JobRunner(
        DateTime startTime,
        Enum periodic,
        int interval,
        Action executeMethod, 
        bool isFirstExecute, // If true launch at the first time independently from time, if false launch on time
        bool isLoop, // If true launch cyclically
        CancellationTokenSource cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var currentDateTime = DateTime.Now;
            if (startTime <= currentDateTime && !isLoop)
            {
                Console.WriteLine("ERROR: You entered the time: " + startTime + " less then now: " + currentDateTime);
                break;
            }
            
            if (startTime > currentDateTime) continue;
            
            executeMethod.Invoke();
            
            if (!isLoop) break;

            DateTime nextStartDateTime = currentDateTime;
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
                case Every.Monday: // todo need to check
                    for (int i = 0; i < interval; i++)
                    {
                        while (nextStartDateTime.DayOfWeek != DayOfWeek.Monday)
                        {
                            nextStartDateTime = nextStartDateTime.AddDays(1);
                        }  
                    }
                    break;
            }

            var delta = nextStartDateTime - DateTime.Now;
            Console.WriteLine("Next start date: " + nextStartDateTime.ToString("dd/MM/yyyy HH:mm:ss"));

            await Task.Delay(delta);
        }
    }
}
