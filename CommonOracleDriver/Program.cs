using System;

namespace CommonOracleDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please provide an account id as parameter.");
            }
            else
            {
                var accountId = args[0];
                Console.WriteLine($"Get account details of ({accountId})");

                try
                {
                    var repository = new AccountRepository();
                    repository.PrintAccountDetails(accountId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }

            Console.WriteLine("Press ENTER to exit!");
            Console.ReadLine();
        }
    }
}
