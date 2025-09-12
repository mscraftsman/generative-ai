using System;

namespace Mscc.GenerativeAI.Microsoft;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class DescriptionAttribute : Attribute
{
    public DescriptionAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}