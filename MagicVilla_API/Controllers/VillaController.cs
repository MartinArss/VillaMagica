using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        // Inyectamos ILoger para usar sus mensajes de informacion en los endpoints
        private readonly ILogger<VillaController> _logger;
        // Inyectamos la db
        private readonly ApplicationDbContext _context;
        // Despues de lo anterior inyectamos el mapeo
        private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener todas las villas");

            IEnumerable<Villa> villaList = await _context.Villas.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer villa con ID:" + id); // Esta es una de las tantas formas de usar ILogger
                return BadRequest();
            }

            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
                return NotFound();

            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            // Validaciones ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validaciones personalizadas
            if (await _context.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (createDto is null)
                return BadRequest();

            //villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //VillaStore.villaList.Add(villaDto); Estas dos lineas no se necesitaran

            // Todo esto se reemplaza por el automapper
            //Villa modelo = new()
            //{
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad
            //};

            Villa modelo = _mapper.Map<Villa>(createDto);

            await _context.Villas.AddAsync(modelo);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest();

            var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa is null) 
                return NotFound();

            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //[HttpPut("id:int")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult UpdateVilla(int id, [FromBody] VillaUpdateDto villaDto)
        //{
        //    if (villaDto == null || id != villaDto.Id)
        //        return BadRequest();

        //    //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
        //    //villa.Nombre = villaDto.Nombre;
        //    //villa.Ocupantes = villaDto.Ocupantes;
        //    //villa.MetrosCuadrados = villaDto.MetrosCuadrados;  sOLO REEMPLKAZA EN EL VILLASTORE

        //    Villa modelo = new()
        //    {
        //        Id = villaDto.Id,
        //        Nombre = villaDto.Nombre,
        //        Detalle = villaDto.Detalle,
        //        ImagenUrl = villaDto.ImagenUrl,
        //        Ocupantes = villaDto.Ocupantes,
        //        Tarifa = villaDto.Tarifa,
        //        MetrosCuadrados = villaDto.MetrosCuadrados,
        //        Amenidad = villaDto.Amenidad
        //    };

        //    _context.Villas.Update(modelo);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
                return BadRequest();

            //var modelo = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);

            //if (modelo == null)
            //    return NotFound();

            // Se reemplaza por el mapper
            //modelo.Nombre = villaDto.Nombre;
            //modelo.Detalle = villaDto.Detalle;
            //modelo.ImagenUrl = villaDto.ImagenUrl;
            //modelo.Ocupantes = villaDto.Ocupantes;
            //modelo.Tarifa = villaDto.Tarifa;
            //modelo.MetrosCuadrados = villaDto.MetrosCuadrados;
            //modelo.Amenidad = villaDto.Amenidad;

            Villa modelo = _mapper.Map<Villa>(updateDto);

            try
            {
                _context.Villas.Update(modelo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Villas.Any(v => v.Id == id))
                {
                    // La villa ya no existe en la base de datos, puede manejar el error apropiadamente
                    return NotFound();
                }
                else
                {
                    // Ocurrió una excepción de concurrencia optimista, puede manejarla apropiadamente
                    return StatusCode(StatusCodes.Status409Conflict, "Conflicto de concurrencia. Los datos han sido modificados por otro usuario.");
                }
            }

            return NoContent();
        }

        // HttpPatch en accion, usado solamente para cambniar una sola propiedad de un modelo
        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
                return BadRequest();

            //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            // Se reemplaza por mapper
            //VillaUpdateDto villaDto = new()
            //{
            //    Id = villa.Id,
            //    Nombre = villa.Nombre,
            //    Detalle = villa.Detalle,
            //    ImagenUrl = villa.ImagenUrl,
            //    Ocupantes = villa.Ocupantes,
            //    Tarifa = villa.Tarifa,
            //    MetrosCuadrados = villa.MetrosCuadrados,
            //    Amenidad = villa.Amenidad
            //};

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null)
                return NotFound();


            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Igual se reemplaza por mapper
            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad
            //};

            Villa modelo = _mapper.Map<Villa>(villaDto);

            _context.Villas.Update(modelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
