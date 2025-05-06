using System;
using System.Collections.Generic;

namespace CanadianVisaChatbot.Shared.Models;

public class ApplicationProgress
{
    public double Percentage { get; set; }
    public List<string> PendingRequirements { get; set; } = new();
    public Dictionary<string, DateTime?> Timeline { get; set; } = new();
    public ApplicationStep CurrentStep { get; set; }
}