namespace Chronograph;

public class Joba
{
    public void Foo()
    {
        Console.WriteLine("Foo joba " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:fff"));
    }
    
    public void Boo()
    {
        Console.WriteLine("Boo joba - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:fff"));
    }
}