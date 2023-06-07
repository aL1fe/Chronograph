namespace Chronograph;

public class TaskScheduler
{
    public void JobRunner(
        DateTime startTime,
        Enum periodic,
        int interval,
        Action executeMethod,
        bool isFirstExecute, // If true launch at the first time independently from time, if false launch on time
        bool isLoop, // If true launch cyclically
        CancellationTokenSource cancellationToken)
    {
        var currentDateTime = DateTime.Now;
        if (startTime <= currentDateTime)
        {
            Console.WriteLine("ERROR: Start date time: " + startTime.ToString("dd/MM/yyyy HH:mm:ss") +
                              " less then date time now: " + currentDateTime.ToString("dd/MM/yyyy HH:mm:ss"));
            return;
        }
        
        while (!cancellationToken.IsCancellationRequested)
        {
            currentDateTime = DateTime.Now;

            if (isFirstExecute)
            {
                executeMethod.Invoke();
                isFirstExecute = false;
            }

            if (startTime > currentDateTime) continue;

            executeMethod.Invoke();

            if (!isLoop) break;

            // Calculate next date time to start
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
                case Every.Monday:
                    for (int i = 0; i < interval; i++)
                    {
                        while (nextStartDateTime.DayOfWeek != DayOfWeek.Sunday)
                        {
                            nextStartDateTime = nextStartDateTime.AddDays(1);
                        }

                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }

                    break;
                case Every.Tuesday:
                    for (int i = 0; i < interval; i++)
                    {
                        while (nextStartDateTime.DayOfWeek != DayOfWeek.Monday)
                        {
                            nextStartDateTime = nextStartDateTime.AddDays(1);
                        }

                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }

                    break;
                case Every.Wednesday:
                    for (int i = 0; i < interval; i++)
                    {
                        while (nextStartDateTime.DayOfWeek != DayOfWeek.Tuesday)
                        {
                            nextStartDateTime = nextStartDateTime.AddDays(1);
                        }

                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }

                    break;
                case Every.Thursday:
                    for (int i = 0; i < interval; i++)
                    {
                        while (nextStartDateTime.DayOfWeek != DayOfWeek.Wednesday)
                        {
                            nextStartDateTime = nextStartDateTime.AddDays(1);
                        }

                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }

                    break;
                case Every.Friday:
                    for (int i = 0; i < interval; i++)
                    {
                        while (nextStartDateTime.DayOfWeek != DayOfWeek.Thursday)
                        {
                            nextStartDateTime = nextStartDateTime.AddDays(1);
                        }

                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }

                    break;
                case Every.Saturday:
                    for (int i = 0; i < interval; i++)
                    {
                        while (nextStartDateTime.DayOfWeek != DayOfWeek.Friday)
                        {
                            nextStartDateTime = nextStartDateTime.AddDays(1);
                        }

                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }

                    break;
                case Every.Sunday:
                    for (int i = 0; i < interval; i++)
                    {
                        while (nextStartDateTime.DayOfWeek != DayOfWeek.Saturday)
                        {
                            nextStartDateTime = nextStartDateTime.AddDays(1);
                        }

                        nextStartDateTime = nextStartDateTime.AddDays(1);
                    }

                    break;
            }

            var timeDelay = nextStartDateTime - DateTime.Now;
            // Console.WriteLine("Next start date: " + nextStartDateTime.ToString("dd/MM/yyyy HH:mm:ss"));

            Thread.Sleep(timeDelay);
        }
    }
}