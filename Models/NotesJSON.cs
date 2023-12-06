using System;

public class NotesJSON
{
	public string username {  get; set; }
	public int id { get; set; }
	public NoteObj[] notes { get; set; }
}

public class NoteObj
{
	public string title { get; set; }
	public string content { get; set; }
}
