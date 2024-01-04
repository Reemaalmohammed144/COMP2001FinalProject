using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reena.MSSQL.Models;

namespace COMP2001FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ReemaContext _context;

        public ProfilesController(ReemaContext context)
        {
            _context = context;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
        {
            return await _context.Profiles.ToListAsync();
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(int id, Profile profile)
        {
            if (id != profile.UserId)
            {
                return BadRequest();
            }

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProfileExists(profile.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProfile", new { id = profile.UserId }, profile);
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.UserId == id);
        }

        [HttpGet("GetProfileInfo")]
        public async Task<ActionResult<Profile>> GetProfileInfo(string profileId)
        {
            if (int.TryParse(profileId, out int profileIdInt))
            {
                var usser = await _context.Profiles
                                    .Where(c => c.UserId == profileIdInt)
                                    .FirstOrDefaultAsync();

                return usser == null ? NotFound() : usser;
            }
            else
            {
                return BadRequest("Invalid profileId format");
            }
        }
        // DELETE: api/Profiles/DeleteProfile/5
        [HttpDelete("DeleteProfile/{id}")]
        public async Task<IActionResult> DeleteProfilebyid(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return NoContent();
        }


    }
}
