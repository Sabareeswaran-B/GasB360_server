#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasB360_server.Models;

namespace GasB360_server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly GasB360Context _context;

        public BranchController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Branch
        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var branch = await _context.TblBranches.ToListAsync();
                return Ok(new { status = "success", message = "Get all branches", data = branch });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Branch/5
        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetBranchById(Guid branchId)
        {
            try
            {
                var branch = await _context.TblBranches.FindAsync(branchId);

                if (branch == null)
                {
                    return NotFound();
                }

                return Ok(
                    new { status = "success", message = "Gell branch by id", data = branch }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Branch/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Branch
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // DELETE: api/Branch/5
        [HttpDelete("{branchId}")]
        public async Task<IActionResult> DeleteBranch(Guid branchId)
        {
            try
            {
                var tblBranch = await _context.TblBranches.FindAsync(branchId);
                if (tblBranch == null)
                {
                    return NotFound();
                }

                _context.TblBranches.Remove(tblBranch);
                await _context.SaveChangesAsync();

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Delete branch by id successful."
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool IsBranchExists(Guid branchId)
        {
            return _context.TblBranches.Any(e => e.BranchId == branchId);
        }
    }
}
