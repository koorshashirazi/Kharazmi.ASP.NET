using System;

namespace Kharazmi.Aop.Autofac
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheMethodAttribute : Attribute
    {
        public CacheMethodAttribute(double timeDuration)
        {
            TimeDuration = timeDuration;

            switch (TimeType)
            {
                case TimeType.Second:
                    DateTimeToCache = DateTime.Now.AddSeconds(TimeDuration);
                    break;
                case TimeType.Minute:
                    DateTimeToCache = DateTime.Now.AddMinutes(TimeDuration);

                    break;
                case TimeType.Hours:
                    DateTimeToCache = DateTime.Now.AddHours(TimeDuration);

                    break;
                case TimeType.Day:
                    DateTimeToCache = DateTime.Now.AddDays(TimeDuration);

                    break;
                default:
                    DateTimeToCache = DateTime.Now.AddSeconds(TimeDuration);
                    break;
            }
        }

        public double TimeDuration { get; set; }

        public TimeType TimeType { get; set; }

        public DateTime DateTimeToCache { get; set; }
    }
}