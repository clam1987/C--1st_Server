using System;
using System.Collections;
using System.Collections.Generic;

public class User
{
    public string id {  get; set; }
    public string username { get; set; }

    public ICollection<Note> notes { get; set; }

    public User()
    {
        id = string.Empty;
        username = string.Empty; // Initialize the string property
        notes = new List<Note>(); // Initialize the collection property
    }
}
