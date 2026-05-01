using System;

namespace MemoryGame.Models;

public class GameHistoryEntry
{
    public string PlayerName { get; set; } = string.Empty;
    public string GameName { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public int Moves { get; set; }
    public DateTime PlayedAt { get; set; }

    public string PlayedAtText => PlayedAt.ToString("g");
}
