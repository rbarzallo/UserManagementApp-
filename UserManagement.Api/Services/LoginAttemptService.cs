using System;
using System.Collections.Concurrent;

namespace UserManagement.Api.Services
{
    public class LoginAttemptService
    {
        private readonly int _maxAttempts;
        private readonly TimeSpan _lockoutTime;
        private static readonly ConcurrentDictionary<string, AttemptInfo> _attempts = new();

        public LoginAttemptService(IConfiguration config)
        {
            var settings = config.GetSection("BruteForceSettings");
            _maxAttempts = settings.GetValue<int>("MaxAttempts");
            _lockoutTime = TimeSpan.FromMinutes(settings.GetValue<int>("LockoutMinutes"));
        }

        public bool IsBlocked(string key)
        {
            if (_attempts.TryGetValue(key, out var info))
            {
                if (DateTime.UtcNow < info.LockoutEnd)
                {
                    return true;
                }

                // Reiniciar si el tiempo de bloqueo ha terminado
                _attempts.TryRemove(key, out _);
            }

            return false;
        }

        public void RegisterFailedAttempt(string key)
        {
            _attempts.AddOrUpdate(key,
                new AttemptInfo { Attempts = 1, LockoutEnd = DateTime.MinValue },
                (_, existing) => new AttemptInfo
                {
                    Attempts = existing.Attempts + 1,
                    LockoutEnd = existing.Attempts + 1 >= _maxAttempts ? DateTime.UtcNow.Add(_lockoutTime) : existing.LockoutEnd
                });
        }

        public void ResetAttempts(string key)
        {
            _attempts.TryRemove(key, out _);
        }

        private class AttemptInfo
        {
            public int Attempts { get; set; }
            public DateTime LockoutEnd { get; set; }
        }
    }
}
