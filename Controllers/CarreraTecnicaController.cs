using AutoMapper;
using KalumManagement.DBContext;
using KalumManagement.Dtos;
using KalumManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KalumManagement.Controllers
{
    [ApiController]
    [Route("kalum-management/v1/carreras-tecnicas")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDBContext DBContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        private readonly IMapper Mapper;

        public CarreraTecnicaController(KalumDBContext _dbContext, ILogger<CarreraTecnicaController> _logger, IMapper _mapper)
        {
            this.DBContext = _dbContext;
            this.Logger = _logger;
            this.Mapper = _mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarreraTecnicaListDTO>>> Get()
        {
            this.Logger.LogDebug("Iniciando el proceso de consulta de las carreras técnicas");
            List<CarreraTecnica> carrerasTecnicas = await this.DBContext.CarrerasTecnicas.ToListAsync();
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                this.Logger.LogWarning("No existen registros en la tabla carrera técnica");
                return new NoContentResult();
            }
            this.Logger.LogInformation("Se ejecuto corretamente la consulta a la tabla carreras técnicas");
            List<CarreraTecnicaListDTO> lista = this.Mapper.Map<List<CarreraTecnicaListDTO>>(carrerasTecnicas);
            return Ok(lista);
        }

        [HttpGet("{id}", Name = "GetCarreraTecnicaById")]
        public async Task<ActionResult<CarreraTecnicaListDTO>> GetCarreraTecnicaById(string id)
        {
            CarreraTecnica carreraTecnica =  await this.DBContext.CarrerasTecnicas.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carreraTecnica == null)
            {
                this.Logger.LogWarning($"No existe la carrera técnica con el id {id}");
                return new NoContentResult();
            }
            this.Logger.LogInformation($"Se ejecuto la consulta de forma exitosa con el id {id}");
            CarreraTecnicaListDTO value = this.Mapper.Map<CarreraTecnicaListDTO>(carreraTecnica);
            return Ok(value);
        }  

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CarreraTecnicaListDTO>>> GetCarreraTecnicaByParameter([FromQuery] String field, [FromQuery] string value)
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            if(field.Equals("name"))
            {
                carrerasTecnicas = await this.DBContext.CarrerasTecnicas.Where(ct => ct.Nombre.Contains(value)).ToListAsync();
            }
            if(carrerasTecnicas == null  || carrerasTecnicas.Count == 0)
            {
                this.Logger.LogWarning($"No existen registros con el filtro {field} y valor {value}");
                return new NoContentResult();
            }
            this.Logger.LogInformation($"Se ejecuto exitosamente la consulta con el filtro {field}");            
            return Ok(this.Mapper.Map<List<CarreraTecnicaListDTO>>(carrerasTecnicas));
        }

        [HttpPost]
        public async Task<ActionResult<CarreraTecnicaListDTO>> Post([FromBody] CarreraTecnicaCreateOrUpdateDTO carreraTecnica)
        {
            this.Logger.LogDebug("Iniciando el proceso para crear una nueva carrera técnica");
            CarreraTecnica values = this.Mapper.Map<CarreraTecnica>(carreraTecnica);
            values.CarreraId = Guid.NewGuid().ToString().ToUpper();
            this.Logger.LogDebug("Agregando la carrera técnica a la BD");
            await this.DBContext.CarrerasTecnicas.AddAsync(values);
            await this.DBContext.SaveChangesAsync();
            this.Logger.LogInformation($"Se creo una nueva carrera técnica con el id {values.CarreraId}");
            return new CreatedAtRouteResult("GetCarreraTecnicaById", new {id = values.CarreraId}, values);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CarreraTecnicaListDTO>> DeleteCarreraTecnicaById(string id)
        {
            this.Logger.LogDebug($"Iniciando el proceso de busqueda con el id {id}");
            CarreraTecnica carreraTecnica = this.DBContext.CarrerasTecnicas.FirstOrDefault(ct => ct.CarreraId == id);
            if(carreraTecnica == null)
            {
                return NoContent();
            }
            this.Logger.LogDebug($"Iniciando el proceso de eliminarción del registro con id {id}");
            this.DBContext.CarrerasTecnicas.Remove(carreraTecnica);
            await this.DBContext.SaveChangesAsync();
            this.Logger.LogInformation("El registro fue eliminado con éxito");
            return StatusCode(202,this.Mapper.Map<CarreraTecnicaListDTO>(carreraTecnica));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCarreraTecnicaById(string id, [FromBody] CarreraTecnicaCreateOrUpdateDTO values)
        {
            this.Logger.LogDebug($"Iniciando la busqueda del registro con el id {id}");
            CarreraTecnica carreraTecnica = await this.DBContext.CarrerasTecnicas.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carreraTecnica == null)
            {
                this.Logger.LogWarning($"No existe registro con el id {id}");
                return NoContent();
            }           
            carreraTecnica.Nombre = values.Nombre;
            this.DBContext.Entry(carreraTecnica).State = EntityState.Modified;
            await this.DBContext.SaveChangesAsync();
            this.Logger.LogInformation("Los datos fueron actualizados con éxito");
            return NoContent();
        }

    }
}