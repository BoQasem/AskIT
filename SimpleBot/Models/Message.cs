using System;
using System.Collections.Generic;

namespace AskIT.Models;

public partial class Message
{
    public int Id { get; set; }

    public string? UserPhone { get; set; }

    public string? MessageText { get; set; }

    public string? SenderType { get; set; }

    public int? KnowledgeBaseId { get; set; }

    public bool? IsMatched { get; set; }

    public double? MatchPercentage { get; set; }

    public DateTime? Timestamp { get; set; }

    public bool? NeedsHumanSupport { get; set; }

    public virtual KnowledgeBase? KnowledgeBase { get; set; }
}
