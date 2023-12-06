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
                var serializedNote = JsonSerializer.Serialize(note, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogInformation("Recieved Note: {@NoteObj}", serializedNote);

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");
                NotesJSON result = ReadJSON(filePath);
                //var serializedDB = JsonSerializer.Serialize(result, new JsonSerializerOptions
                //{
                //    WriteIndented = true
                //});
                //_logger.LogInformation("Data in db.json: {@NotesJSON}", serializedDB);
                
                return Ok(result);

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        public NotesJSON ReadJSON(string filepath)
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
