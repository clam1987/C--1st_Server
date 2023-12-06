using System;

public class NotesJSON
{
	public string username {  get; set; }
	public int id { get; set; }
	public Note[] notes { get; set; }
}

public class Note
{
	public string title { get; set; }
	public string content { get; set; }
}
