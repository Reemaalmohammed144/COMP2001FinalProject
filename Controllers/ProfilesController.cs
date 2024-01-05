using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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

        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest loginRequest)
        {
            // Validate the input
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest("Invalid login request");
            }

            // Call the external authentication API
            var authenticationApiUrl = "https://web.socem.plymouth.ac.uk/COMP2001/auth/api/users";
            var authenticationRequest = new
            {
                email = loginRequest.Email,
                password = loginRequest.Password
            };

            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(authenticationApiUrl, authenticationRequest);

                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<string>>(resultString);

                    if (result.Count == 2 && result[0] == "Verified" && result[1] == "True")
                    {
                        // Authentication successful, find the user by email
                        var user = await _context.Profiles
                            .Where(c => c.Email == loginRequest.Email)
                            .FirstOrDefaultAsync();

                        // Generate and save the access token
                        user.AccessToken = GenerateJwtToken(user.UserId, user.Email);
                        _context.SaveChanges();

                        // Return the user profile with the access token
                        return new
                        {
                            user = user,
                            token = user.AccessToken
                        };
                    }
                }
            }

            return Unauthorized("Invalid email or password");
        }


        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles(string accessToken)
        {
            // Validate the token or perform any necessary authentication/authorization checks here

            // Check if a profile with the given access token exists in the database
            var profileCheck = await _context.Profiles.FirstOrDefaultAsync(p => p.AccessToken == accessToken);

            if (profileCheck == null)
            {
                // If no profile has the provided access token, return Unauthorized
                return StatusCode(401, "Unauthorized: Invalid access token");
            }

            // If the token is valid, proceed to fetch profiles
            return await _context.Profiles.ToListAsync();
        }


        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(int id, string accessToken)
        {

            // Check if a profile with the given access token exists in the database
            var profileCheck = await _context.Profiles.FirstOrDefaultAsync(p => p.AccessToken == accessToken);

            if (profileCheck == null)
            {
                // If no profile has the provided access token, return Unauthorized
                return StatusCode(401, "Unauthorized: Invalid access token");
            }

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
        public async Task<IActionResult> PutProfile(int id, Profile updatedProfile, string accessToken)
        {
            // Find the existing profile in the database based on the ID
            var existingProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == id);

            if (existingProfile == null)
            {
                return NotFound();
            }

            // Check if the provided access token matches the access token of the existing profile
            if (existingProfile.AccessToken != accessToken)
            {
                return StatusCode(401, "Unauthorized: Invalid access token");
            }

            // Update all properties of the existing profile except AccessToken
            existingProfile.Email = updatedProfile.Email;
            existingProfile.UserName = updatedProfile.UserName;
            existingProfile.Password = updatedProfile.Password;
            existingProfile.FirstName = updatedProfile.FirstName;
            existingProfile.LastName = updatedProfile.LastName;
            existingProfile.AboutMe = updatedProfile.AboutMe;
            existingProfile.Birthday = updatedProfile.Birthday;
            // Update other properties as needed...

            // Update the modified profile in the context and save changes
            _context.Entry(existingProfile).State = EntityState.Modified;

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

            return StatusCode(200, "Profile updated successfully");
        }


        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile, string accessToken)
        {

            // Check if a profile with the given access token exists in the database
            var profileCheck = await _context.Profiles.FirstOrDefaultAsync(p => p.AccessToken == accessToken);

            if (profileCheck == null)
            {
                // If no profile has the provided access token, return Unauthorized
                return StatusCode(401, "Unauthorized: Invalid access token");
            }

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

            return StatusCode(200, "Profile added successfuly");
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id, string accessToken)
        {
            // Check if a profile with the given access token exists in the database
            var profileCheck = await _context.Profiles.FirstOrDefaultAsync(p => p.AccessToken == accessToken);

            if (profileCheck == null)
            {
                // If no profile has the provided access token, return Unauthorized
                return StatusCode(401, "Unauthorized: Invalid access token");
            }

            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            // Create an archived profile entry and copy all fields
            var archivedProfile = new ArchivedProfile();

            // Using reflection to copy all properties from Profile to ArchivedProfile
            var profileProperties = typeof(Profile).GetProperties();
            var archivedProfileProperties = typeof(ArchivedProfile).GetProperties();

            foreach (var profileProperty in profileProperties)
            {
                var archivedProfileProperty = archivedProfileProperties.FirstOrDefault(p => p.Name == profileProperty.Name);
                if (archivedProfileProperty != null && archivedProfileProperty.CanWrite)
                {
                    var value = profileProperty.GetValue(profile);
                    archivedProfileProperty.SetValue(archivedProfile, value);
                }
            }

            _context.ArchivedProfiles.Add(archivedProfile);

            // Remove the profile from the original Profiles table
            _context.Profiles.Remove(profile);

            await _context.SaveChangesAsync();

            return StatusCode(200, "Profile deleted successfully");
        }


        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.UserId == id);
        }
        // Add this method to your controller
        private string GenerateJwtToken(int userId, string userEmail)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your-secret-key"); // Replace with your secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userEmail)
        }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



    }
}
// Modify the LoginRequest model
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

