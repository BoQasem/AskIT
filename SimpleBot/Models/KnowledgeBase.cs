using System;
using System.Collections.Generic;

namespace AskIT.Models;

public partial class KnowledgeBase
{
    public int Id { get; set; }

    public string QuestionText { get; set; } = null!;

    public string AnswerText { get; set; } = null!;

    public string? Category { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
