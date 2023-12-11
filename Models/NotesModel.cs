using System;

public class Note
{
	public int id { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

    public string user_id { get; set; }

    public Note()
    {
        title = string.Empty; // Initialize the string properties
        content = string.Empty;
        user_id = string.Empty;
    }
}