using Microsoft.AspNetCore.Mvc;
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
                    return Ok(result);
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

        //[HttpPost]
        //public IActionResult

        public object ReadJSON(string filepath)
        {
            try
            {
                string jsonString = System.IO.File.ReadAllText(filepath);
                NotesJSON notes = JsonSerializer.Deserialize<NotesJSON>(jsonString);
                return notes;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
