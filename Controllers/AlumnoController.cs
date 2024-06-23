using KalumManagement.DBContext;
using KalumManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KalumManagement.Controllers
{
    [Route("kalum-management/v1/alumnos")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDBContext kalumDBContext;
        private readonly ILogger<AlumnoController> logger;        
        public AlumnoController(KalumDBContext _KalumDBContext, ILogger<AlumnoController> _Logger)
        {
            this.kalumDBContext = _KalumDBContext;
            this.logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetAlumnos()
        {
            this.logger.LogDebug("Iniciando el proceso de consulta de alumnos");
            List<Alumno> alumnos = await this.kalumDBContext.Alumnos.ToListAsync();
            if(alumnos == null || alumnos.Count == 0)
            {
                this.logger.LogWarning("No existen registros de alumnos");
                return new NoContentResult();
            } 
            this.logger.LogInformation("Se ejecuto la consulta con exito");
            return Ok(alumnos);
        }

    }



}