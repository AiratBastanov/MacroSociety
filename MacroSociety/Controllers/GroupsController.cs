﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroSociety.Models;
using System.Text.RegularExpressions;

[Route("api/group")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly MacroSocietyContext _context;

    public GroupsController(MacroSocietyContext context)
    {
        _context = context;
    }

    /*[HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        return await _context.Groups.ToListAsync();
    }*/

    [HttpGet("{id}")]
    public async Task<ActionResult<MacroSociety.Models.Group>> GetGroup(int id)
    {
        var group = await _context.Groups.Where(p => p.Id == id).FirstOrDefaultAsync();

        if (group == null)
        {
            return NotFound();
        }

        return group;
    }

    [HttpGet("list/{id}")]
    public async Task<ActionResult<IEnumerable<MacroSociety.Models.Group>>> GetGroups(int id)
    {
        var groupsCreatedByUser = await _context.Groups
            .Where(g => g.CreatedBy == id)
            .ToListAsync();

        var groupsUserIsMemberOf = await _context.GroupMembers
            .Where(gm => gm.UserId == id)
            .Select(gm => gm.Group)
            .ToListAsync();

        var allGroups = groupsCreatedByUser.Union(groupsUserIsMemberOf).Distinct().ToList();

        if (!allGroups.Any())
        {
            return NotFound();
        }

        return allGroups;
    }


    [HttpPost]
    public async Task<ActionResult<MacroSociety.Models.Group>> PostGroup(MacroSociety.Models.Group group)
    {
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetGroup", new { id = group.Id }, group);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroup(int id, MacroSociety.Models.Group group)
    {
        if (id != group.Id)
        {
            return BadRequest();
        }

        _context.Entry(group).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroupExists(id))
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GroupExists(int id)
    {
        return _context.Groups.Any(e => e.Id == id);
    }
}
