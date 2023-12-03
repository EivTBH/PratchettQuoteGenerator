using System.Collections.Generic;
/// <summary>
/// Getter and setter for the quote text.
/// </summary>
public class Quote
{
    public string Text { get; set; }
    public string Author { get; set; }
    public string Book { get; set; }
    public List<string> Characters { get; set; }
}