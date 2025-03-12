using System;

namespace Kharazmi.AspNetMvc.Core.Timing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class DisableDateTimeNormalizationAttribute : Attribute
    {
        
    }
}