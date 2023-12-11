using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotesServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace NotesServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly NotesDbContext _dbContext;

        public UserController(ILogger<UserController> logger, NotesDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetUser([FromQuery] string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid parsedId))
                {
                    return BadRequest("Invalid GUID format");
                }

                _logger.LogInformation($"Searching for user with id: {parsedId.GetType()}...");
                var user = _dbContext.Users.Include(user => user.notes).FirstOrDefault(user => user.id == parsedId.ToString());
                if (user == null)
                {
                    return Ok("User not found");
                }

                _logger.LogInformation("User retrieved successfully!");
                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing the request.");
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            try
            {
                // This is to try and read the note in the logs - ignore this and just pass in notes for solution
                //var serializedUser = JsonSerializer.Serialize(user, new JsonSerializerOptions
                //{
                //    WriteIndented = true
                //});
                //if (serializedUser == null)
                //{
                //    return BadRequest("Invalid note format.");
                //}

                //_logger.LogInformation("Recieved Note: {@Note}", serializedUser);



                if (user.username == null)
                {
                    return Ok("Username needed, failed to create user");
                }

                var user_found = _dbContext.Users.FirstOrDefault(user_in_db => user_in_db.username == user.username);
                if (user_found != null)
                {
                    return Ok("User has been created already!");
                }

                UUIDGenerator uuid = new UUIDGenerator();
                User newUser = new User
                {
                    id = Guid.NewGuid().ToString(), // You can set Id explicitly if needed
                    username = user.username,
                    notes = new List<Note>() // Initialize with any initial notes
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                return Ok("User created!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing the request.");
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }
        public class UUIDGenerator
        {
            public string GenerateUUID()
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}