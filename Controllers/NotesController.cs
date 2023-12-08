using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotesServer.Models;
using System;
using System.IO;
using System.Text.Json;

namespace NotesServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        // Logging services
        private readonly ILogger<NotesController> _logger;

        public NotesController(ILogger<NotesController> logger)
        {
            _logger = logger;
        }

        // GET Route for Notes
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                Console.WriteLine("Accessing db.json");
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");
                object result = ReadJSON(filePath);
                if (result is NotesJSON notes)
                {
                    return Ok(notes);
                }
                else
                {
                    return BadRequest("Unable to deserialize JSON.");
                }
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found.");
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON format.");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }

        // POST Route for Notes
        [HttpPost]
        public IActionResult AddNote([FromBody] NoteObj note)
        {
            try
            {
                // This is to try and read the note in the logs - ignore this and just pass in notes for solution
                //var serializedNote = JsonSerializer.Serialize(note, new JsonSerializerOptions
                //{
                //    WriteIndented = true
                //});
                //if (serializedNote == null)
                //{
                //    return BadRequest("Invalid note format.");
                //}

                //_logger.LogInformation("Recieved Note: {@NoteObj}", serializedNote);


                // read the json here and added it
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");
                NotesJSON result = ReadJSON(filePath);
                note.id = result.notes.Count;
                result.notes.Add(note);

                // Serialize and write over here
                var serializedDB = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                //_logger.LogInformation("Data in db.json: {@NotesJSON}", serializedDB);

                System.IO.File.WriteAllText(filePath, serializedDB);

                return Ok("Note added");

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult EditNote([FromBody] NoteObj note)
        {
            try
            {
                // Logging to make sure I recieve the correct information
                //var serializedNote = JsonSerializer.Serialize(note, new JsonSerializerOptions
                //{
                //    WriteIndented = true
                //});
                //if (serializedNote == null)
                //{
                //    return BadRequest("Invalid note format.");
                //}

                //_logger.LogInformation("Recieved Note: {@NoteObj}", serializedNote);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");
                NotesJSON result = ReadJSON(filePath);

                int noteFound = result.notes.FindIndex(noteobj => noteobj.id == note.id);

                if(noteFound == -1)
                {
                    return Ok("Note not found");
                }
                else
                {
                    result.notes[noteFound] = note;

                    var serializedDB = JsonSerializer.Serialize(result, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    _logger.LogInformation("Recieved Note: {@NoteObj}", serializedDB);
                    System.IO.File.WriteAllText(filePath, serializedDB);

                    return Ok("Note edited");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteNote([FromBody] NoteObj note)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");
                NotesJSON result = ReadJSON(filePath);

                int noteFound = result.notes.FindIndex(noteobj => noteobj.id == note.id);

                if (noteFound == -1)
                {
                    return Ok("Note not found");
                }
                else
                {
                    result.notes.RemoveAt(noteFound);

                    var serializedDB = JsonSerializer.Serialize(result, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    _logger.LogInformation("Recieved Note: {@NoteObj}", serializedDB);
                    System.IO.File.WriteAllText(filePath, serializedDB);

                    return Ok("Note deleted");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        private NotesJSON ReadJSON(string filepath)
        {
            try
            {
                string jsonString = System.IO.File.ReadAllText(filepath);
                NotesJSON notes = JsonSerializer.Deserialize<NotesJSON>(jsonString);
                return notes ?? new NotesJSON();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new NotesJSON();
            }
        }
    }
}
