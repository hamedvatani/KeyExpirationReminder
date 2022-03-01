using System;
using System.Collections.Concurrent;
using KeyExpirationReminderLibrary;

namespace KeyExpirationReminderTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var reminder = new KeyExpirationReminder();
            reminder.TryAddKey("Key 1", 5000, (k, t) => { Console.WriteLine("Key Expiration : " + k); });
            reminder.TryAddKey("Key 2", 4000, (k, t) => { Console.WriteLine("Key Expiration : " + k); });
            reminder.TryAddKey("Key 3", 6000, (k, t) => { Console.WriteLine("Key Expiration : " + k); });
            reminder.TryAddKey("Key 4", 1500, (k, t) => { Console.WriteLine("Key Expiration : " + k); });

            Console.ReadKey();
        }
    }
}
