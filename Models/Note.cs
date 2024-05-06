using Camera.MAUI;

namespace Notes.Models;
internal class Note
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public Image MyImage { get; set; }
}