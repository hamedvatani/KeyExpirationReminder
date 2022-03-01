using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;

namespace KeyExpirationReminderLibrary
{
    public class KeyExpirationReminder:IDisposable
    {
        internal class BagItem
        {
            public object Tag { get; set; }
            public int Timeout { get; set; }
            public DateTime TimeStamp { get; set; }
            public Action<string, object> Callback { get; set; }
        }

        private readonly ConcurrentDictionary<string, BagItem> dictionary;
        private readonly Timer timer;

        public KeyExpirationReminder()
        {
            dictionary = new ConcurrentDictionary<string, BagItem>();
            timer = new Timer(500);
            timer.Elapsed += timer_elapsed;
            timer.Start();
        }

        public bool TryAddKey(string key, int miliseconTimeout, object tag, Action<string, object> callback)
        {
            return dictionary.TryAdd(key, new BagItem
            {
                Timeout = miliseconTimeout,
                Tag = tag,
                TimeStamp = DateTime.Now,
                Callback = callback
            });
        }

        public bool TryAddKey(string key, int miliseconTimeout, Action<string, object> callback)
        {
            return TryAddKey(key, miliseconTimeout, null, callback);
        }

        private void timer_elapsed(object sender, ElapsedEventArgs e)
        {
            if (dictionary.Count == 0)
                return;
            timer.Stop();

            foreach (var item in dictionary)
                if (item.Value.TimeStamp.AddMilliseconds(item.Value.Timeout) <= DateTime.Now &&
                    dictionary.TryRemove(item.Key, out var value))
                    Task.Run(() => { value.Callback(item.Key, value.Tag); });

            timer.Start();
        }

        public void Dispose()
        {
            timer?.Close();
        }
    }
}