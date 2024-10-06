using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmosPostgresAPI.Models;
using CosmosPostgresAPI.Services;

namespace CosmosPostgresAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PharmacyService _pharmacyService;

        public PharmacyController(AppDbContext context, PharmacyService pharmacyService)
        {
            _context = context;
            _pharmacyService = pharmacyService;
        }

        // GET: api/Pharmacy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pharmacy>>> GetPharmacies()
        {
            return await _context.Pharmacies.ToListAsync();
        }

        // GET: api/Pharmacy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pharmacy>> GetPharmacy(int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);

            if (pharmacy == null)
            {
                return NotFound();
            }

            return pharmacy;
        }

        // PUT: api/Pharmacy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPharmacy(int id, Pharmacy pharmacy)
        {
            if (id != pharmacy.PharmacyId)
            {
                return BadRequest();
            }

            _context.Entry(pharmacy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PharmacyExists(id))
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

        // POST: api/Pharmacy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pharmacy>> PostPharmacy(Pharmacy pharmacy)
        {
            _context.Pharmacies.Add(pharmacy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPharmacy", new { id = pharmacy.PharmacyId }, pharmacy);
        }

        // DELETE: api/Pharmacy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacy(int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            _context.Pharmacies.Remove(pharmacy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PharmacyExists(int id)
        {
            return _context.Pharmacies.Any(e => e.PharmacyId == id);
        }

        [HttpPost("distribute")]
        public async Task<IActionResult> DistributePharmacyTable()
        {
            try
            {
                await _context.DistributeTableAsync();
                return Ok("Table distributed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error distributing table: {ex.Message}");
            }
        }


        [HttpPost("load-data")]
        public async Task<IActionResult> LoadPharmacyData(string filePath)
        {
            try
            {
                await _pharmacyService.LoadDataFromCsvAsync(filePath);
                return Ok("Data loaded successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error loading data: {ex.Message}");
            }
        }


    }
}
