using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotesServer.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace NotesServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        // Logging services
        private readonly ILogger<NotesController> _logger;
        private readonly NotesDbContext _dbContext;

        public NotesController(ILogger<NotesController> logger, NotesDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // GET Route for Notes
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok("Place Holder to grab notes from database.");
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
        public IActionResult AddNote([FromBody] Note note)
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

                //_logger.LogInformation("Recieved Note: {@Note}", serializedNote);


                return Ok("Note added");

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult EditNote([FromBody] Note note)
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

                //_logger.LogInformation("Recieved Note: {@Note}", serializedNote);
                return Ok("Placeholder for editting database");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteNote([FromBody] Note note)
        {
            try
            {
                return Ok("Placeholder for deleting entities.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
