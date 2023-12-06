using System;

public class NotesJSON
{
	public string username {  get; set; }
	public int id { get; set; }
	public NoteObj[] notes { get; set; }

	public NotesJSON()
	{
		// Must initialize this way
		username = string.Empty; // Empting a string
		id = 0; // Initialize id
		notes = Array.Empty<NoteObj>(); // Initialize Arrays
	}
}

public class NoteObj
{
	public string title { get; set; }
	public string content { get; set; }

	public NoteObj()
	{
		title = string.Empty;
		content = string.Empty;
	}
}
