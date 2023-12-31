﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Get([FromQuery] int id)
        {
            try
            {
                Console.WriteLine(id.GetType());
                var notes = _dbContext.Notes.Where(note => note.id == id).ToList();
                if(notes == null)
                {
                    return Ok(new List<Note>());
                }
                return Ok(notes);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }

        // POST Route for Notes
        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] Note note)
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

                var user = await _dbContext.Users.FirstAsync(user => user.id == note.user_id);

                //var serializedUser = JsonSerializer.Serialize(user, new JsonSerializerOptions
                //{
                //    WriteIndented = true
                //});
                //if (serializedUser == null)
                //{
                //    return BadRequest("Invalid note format.");
                //}

                //_logger.LogInformation("Recieved Note: {@Note}", serializedUser);

                if (user == null)
                {
                    return BadRequest("User not found!");
                };

                var new_note = new Note
                {
                    title = note.title,
                    content = note.content,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    user_id = note.user_id,
                };

                user.notes.Add(new_note);
                await _dbContext.SaveChangesAsync();

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
                var notes = _dbContext.Notes.FirstOrDefault(note_in_db => note_in_db.id == note.id);
                if(notes == null)
                {
                    return BadRequest("Note not found!");
                }

                notes.updated_at = DateTime.Now;
                notes.title = note.title;
                notes.content = note.content;

                _dbContext.Notes.Update(notes);
                _dbContext.SaveChanges();

                return Ok("Note updated!");

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
                var note_entity = _dbContext.Notes.FirstOrDefault(note_in_db => note_in_db.id == note.id);
                if(note_entity == null)
                {
                    return BadRequest("Note not found");
                }

                _dbContext.Remove(note_entity);
                _dbContext.SaveChanges();

                return Ok("Note Deleted!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
