using SalutemCRM;
using SalutemCRM.Database;

namespace Test.DockerDBConnection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Docker DB connection");

            try
            {
                using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                {
                    var x = db.CurrencyUnits.Where(s => s.Name.Length > 0);

                    Console.WriteLine("Currency list:");
                    x.DoForEach(s => Console.WriteLine(s.Name));
                }
                 
                Console.WriteLine($"Connected to Docker Database successfully!");
            }
            catch
            {
                Console.WriteLine($"Connection troubles...");
            }

            Console.WriteLine("Press any key.");
            Console.ReadKey();
        }
    }
}
