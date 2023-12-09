using System;

public class Note
{
	public int id { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

    public Guid? user_id { get; set; }
    public User user { get; set; }

    public Note()
    {
        title = string.Empty; // Initialize the string properties
        content = string.Empty;
        user_id = Guid.Empty;
        user = new User(); // Initialize the User property
    }
}