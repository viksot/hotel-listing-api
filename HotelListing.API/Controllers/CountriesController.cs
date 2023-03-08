using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext context;
        private readonly IMapper mapper;

        public CountriesController(HotelListingDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryToGetDto>>> GetCountries()
        {
            var countries = await context.Countries.ToListAsync();
            var countryDtos = mapper.Map<List<CountryToGetDto>>(countries);

            return Ok(countryDtos);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await context.Countries
                                            .Include(q => q.Hotels)
                                            .FirstOrDefaultAsync(q => q.Id == id);

            if (country is null)
                return NotFound();

            var countryDto = mapper.Map<CountryDto>(country);

            return Ok(countryDto);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, CountryToUpdateDto countryDto)
        {
            if (id != countryDto.Id)
                return BadRequest("Invalid Record ID");

            //context.Entry(country).State = EntityState.Modified;

            var country = await context.Countries.FindAsync(id);

            if (country is null)
                return NotFound();

            mapper.Map(countryDto, country);

            try 
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CountryToCreateDto countryDto)
        {
            var country = mapper.Map<Country>(countryDto);
             
            context.Countries.Add(country);

            await context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            context.Countries.Remove(country);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return (context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
