using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Cache
{
    public static class LockerFactory
    {
        private static readonly object Lock = new object();

        private static RedLockFactory RedLockFactory { get; set; }

        public static RedLockFactory Get(CacheSettings cacheSettings)
        {
            try
            {
                if (RedLockFactory == null)
                {
                    lock (Lock)
                    {
                        if (RedLockFactory == null)
                        {
                            var endpoint = new RedLockEndPoint
                            {
                                EndPoint = new DnsEndPoint(cacheSettings.Host, cacheSettings.Port),
                                Password = cacheSettings.Password,
                                Ssl = cacheSettings.Ssl,
                                RedisDatabase = cacheSettings.LockerDb,
                                SyncTimeout = cacheSettings.TimeoutInMs,
                                ConnectionTimeout = cacheSettings.TimeoutInMs,
                                
                            };

                            var hosts = new List<RedLockEndPoint> { endpoint };
                            RedLockFactory = RedLockFactory.Create(hosts);
                        }
                    }
                }
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }

            return RedLockFactory;
        }

        public static void CloseConnection()
        {
            lock (Lock)
            {
                RedLockFactory?.Dispose();
                RedLockFactory = null;
            }
        }
    }

    public interface ILocker
    {
        Task<IRedLock> GetDistributedLockerAsync(string key, int? ttlInSeconds = null);
    }

    public class Locker : ILocker
    {
        private CacheSettings CacheSettings { get; set; }

        public Locker(CacheSettings cacheSettings)
        {
            this.CacheSettings = cacheSettings;
        }

        public async Task<IRedLock> GetDistributedLockerAsync(string key, int? ttlInSeconds = null)
        {
            if (ttlInSeconds == null)
            {
                ttlInSeconds = this.CacheSettings.LockerTtlInSeconds;
            }

            var lockerFactory = LockerFactory.Get(this.CacheSettings);

            var fullKey = (this.CacheSettings?.LockerPrefix ?? "") + key;
            var ttl = TimeSpan.FromSeconds(ttlInSeconds.Value);

            return await lockerFactory.CreateLockAsync(fullKey, ttl);
        }
    }
}
