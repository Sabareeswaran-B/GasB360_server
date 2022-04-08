#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasB360_server.Models;
using GasB360_server.Helpers;

namespace GasB360_server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        private readonly GasB360Context _context;

        public BranchController(GasB360Context context)
        {
            _context = context;
        }

        //API To Get All Of The Gas Stations Branches
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var branch = await _context.TblBranches
                    .Where(x => x.Active == "true")
                    .ToListAsync();
                return Ok(new { status = "success", message = "Get all branches", data = branch });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Get The Branch By Passing BranchId As Parameter
        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetBranchById(Guid branchId)
        {
            try
            {
                var branch =  await _context.TblBranches
                    .Where(x => x.Active == "true")
                    .Where(x => x.BranchId == branchId)
                    .FirstOrDefaultAsync();

                if (branch == null)
                {
                    return NotFound();
                }

                return Ok(new { status = "success", message = "Gell branch by id", data = branch });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Update The Branch Details By Passing BranchId As Parameter
        [HttpPut("{branchId}")]
        public async Task<IActionResult> UpdateBranch(Guid branchId, TblBranch tblBranch)
        {
            if (branchId != tblBranch.BranchId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblBranch).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var branch = await _context.TblBranches.FindAsync(branchId);
                return Ok(
                    new { status = "success", message = "Update branch successful.", data = branch }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsBranchExists(branchId))
                {
                    return NotFound();
                }
                else
                {
                    Sentry.SentrySdk.CaptureException(ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        //API To Add New Branch By Passing tblBranch Object As Parameter
        [HttpPost]
        public async Task<IActionResult> AddNewBranch(TblBranch tblBranch)
        {
            try
            {
                _context.TblBranches.Add(tblBranch);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetBranchById",
                    new { branchId = tblBranch.BranchId },
                    new
                    {
                        status = "success",
                        message = "Add new branch successful.",
                        data = tblBranch
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Delete The Branch By Passing BranchId As Parameter
        [HttpDelete("{branchId}")]
        public async Task<IActionResult> DeleteBranch(Guid branchId)
        {
            try
            {
                var tblBranch = await _context.TblBranches
                    .Where(x => x.Active == "true")
                    .Where(x => x.BranchId == branchId)
                    .FirstOrDefaultAsync();
                if (tblBranch == null)
                {
                    return NotFound();
                }

                tblBranch.Active = "false";
                _context.Entry(tblBranch).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Delete branch by id successful." });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //Function To Check Whether The Branch Already Exists Or Not By Passing BranchId As Parameter
        private bool IsBranchExists(Guid branchId)
        {
            return _context.TblBranches.Any(e => e.BranchId == branchId);
        }
    }
}
