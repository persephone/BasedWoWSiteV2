﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using server;
using server.Context;
using server.Data;
using server.Model.DTO;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly WebsiteContext _websiteContext;
        private readonly AuthContext _authContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public VoteController(WebsiteContext websiteContext, AuthContext authContext, UserManager<ApplicationUser> userManager)
        {
            this._websiteContext = websiteContext;
            this._userManager = userManager;
            this._authContext = authContext;
        }

        // GET: api/Vote
        [HttpGet("GetVoteSites")]
        public async Task<ActionResult<IEnumerable<VoteSite>>> GetVoteSites()
        {
            return await _websiteContext.VoteSites.ToListAsync();
        }

        [HttpGet("GetVoteTimers")]
        public async Task<IActionResult> GetVoteTimers()
        {
            var user = await GetUser();
            if (user == null)
                return BadRequest(new { message = "An error occoured when validating your identity" });

            var data = await _websiteContext.Votes.Where(acc => acc.UserId == user.Id).Select(x => new { x.Site, x.UnsetTimer }).ToListAsync();
            return Ok(data);
        }


        // GET: api/Vote/5
        [HttpGet("GetVoteSites/{id}")]
        public async Task<ActionResult<VoteSite>> GetVoteSites(byte id)
        {
            var voteSite = await _websiteContext.VoteSites.FindAsync(id);

            if (voteSite == null)
            {
                return NotFound();
            }

            return voteSite;
        }

        [HttpPost("vote/{id}")]
        public async Task<IActionResult> Vote(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Model is invalid" });

            var user = await GetUser();
            if (user == null)
                return BadRequest(new { message = "An error occoured when validating your identity" });

            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var currentVote = await _websiteContext.Votes.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Site == id);
            if (currentVote != null && (currentVote.UnsetTimer > now))
                return BadRequest(new { message = "You have already voted for this site" });

            var voteSite = await _websiteContext.VoteSites.FirstOrDefaultAsync(site => site.Id == id);
            if (voteSite == null)
                return BadRequest(new { message = "Invalid vote site" });

            long unsetTime = voteSite.UnsetTime * 3600 + now;
            if (currentVote != null)
            {
                currentVote.UnsetTimer = unsetTime;
                _websiteContext.Votes.Update(currentVote);
            }
            else
            {
                var newVote = new Vote
                {
                    Site = voteSite.Id,
                    UnsetTimer = unsetTime,
                    UserId = user.Id
                };

                await _websiteContext.Votes.AddAsync(newVote);
            }

            // Add points to ingame acc ->
            var accountData = await _authContext.AccountData.FirstOrDefaultAsync(acc => acc.Id == user.AccountId);
            if (accountData == null)
                return BadRequest(new { message = "Unable to locate any accountData. Contact an administrator." });

            accountData.Vp += voteSite.Value;
            await _authContext.SaveChangesAsync();
            await _websiteContext.SaveChangesAsync();

            return Ok( new { accountData.Vp, unsetTime });
        }

        private async Task<bool> VoteSitesExists(byte id)
        {
            return await _websiteContext.VoteSites.AnyAsync(e => e.Id == id);
        }

        private async Task<ApplicationUser> GetUser()
        {
            if (!(User.Identity is ClaimsIdentity identity))
                return null;

            var emailClaim = identity.Claims.FirstOrDefault(c => c.Type == "UserEmail");
            if (emailClaim == null)
                return null;

            var user = await _userManager.FindByEmailAsync(emailClaim.Value);
            return user;
        }
    }
}