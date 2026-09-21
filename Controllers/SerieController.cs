

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIRESTSTATE.Models.EntityFramework;

namespace APIRESTSTATE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SerieController : ControllerBase
{
    private readonly SerieDbContext _context;
    public SerieController(SerieDbContext context)
    {
        _context = context;
    }


    /// <summary>
    /// Retrieve the list of all series.
    /// </summary>
    /// <returns>A list of Serie objects.</returns>
    // pas de <param>
    // GET: api/Serie
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)] // ce que peut renvoyer la requete rapport au code
    public async Task<ActionResult<IEnumerable<Serie>>> GetSeries()
    {
        return await _context.Series.ToListAsync();
    }


    /// <summary>
    /// Retrieve a specific serie by its identifier.
    /// </summary>
    /// <param name="serieid">The identifier of the serie to find.</param>
    /// <returns>The corresponding Serie object.</returns>
    // GET: api/Serie/5
    [HttpGet("{serieid}")]
    [ProducesResponseType(StatusCodes.Status200OK)] // ce que peut renvoyer la requete rapport au code
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // ce que peut renvoyer (auto) la requete rapport a [ApiController]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // ce que peut renvoyer la requete rapport au code
    public async Task<ActionResult<Serie>> GetSerie(int serieid)
    {
        var serie = await _context.Series.FindAsync(serieid);

        if (serie == null)
        {
            return NotFound();
        }

        return serie;
    }

    /// <summary>
    /// Updates an existing serie.
    /// </summary>
    /// <param name="serieid">The identifier of the serie to modify.</param>
    /// <param name="serie">The updated serie data.</param>
    // PUT: api/Serie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{serieid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // ce que peut renvoyer la requete rapport au code
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // ce que peut renvoyer la requete rapport au code
    [ProducesResponseType(StatusCodes.Status404NotFound)] // ce que peut renvoyer la requete rapport au code
    public async Task<IActionResult> PutSerie(int? serieid, Serie serie)
    {
        if (serieid != serie.Serieid)
        {
            return BadRequest();
        }

        _context.Entry(serie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SerieExists(serieid))
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


    /// <summary>
    /// Creates a new serie.
    /// </summary>
    /// <param name="serie">The Serie object to create.</param>
    /// <returns>The newly created serie with its location.</returns>
    // POST: api/Serie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)] // ce que peut renvoyer la requete rapport au code
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // ce que peut renvoyer (auto) la requete rapport a [ApiController]
    public async Task<ActionResult<Serie>> PostSerie(Serie serie)
    {
        _context.Series.Add(serie);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSerie", new { serieid = serie.Serieid }, serie);
    }

    /// <summary>
    /// Deletes a serie by its identifier.
    /// </summary>
    /// <param name="serieid">The identifier of the serie to delete.</param>
    // DELETE: api/Serie/5
    [HttpDelete("{serieid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // ce que peut renvoyer la requete rapport au code
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // ce que peut renvoyer (auto) la requete rapport a [ApiController]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // ce que peut renvoyer la requete rapport au code
    public async Task<IActionResult> DeleteSerie(int? serieid)
    {
        var serie = await _context.Series.FindAsync(serieid);
        if (serie == null)
        {
            return NotFound();
        }

        _context.Series.Remove(serie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SerieExists(int? serieid)
    {
        return _context.Series.Any(e => e.Serieid == serieid);
    }
}
