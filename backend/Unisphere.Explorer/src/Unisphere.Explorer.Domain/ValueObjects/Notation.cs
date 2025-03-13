namespace Unisphere.Explorer.Domain;

public class Notation
{
    public static Notation Zero { get; } = new Notation(0, 0, 0);

    public Notation(int cleanlinessNote, int communicationNote, int locationNote)
    {
        GlobalNote = (cleanlinessNote + communicationNote + locationNote) / 3;
        CleanlinessNote = cleanlinessNote;
        CommunicationNote = communicationNote;
        LocationNote = locationNote;
    }

    public int GlobalNote { get; set; }

    public int CleanlinessNote { get; set; }

    public int CommunicationNote { get; set; }

    public int LocationNote { get; set; }
}
