using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using AutoMapper;
using KalumManagement.DBContext;
using KalumManagement.Dtos;
using KalumManagement.Entities;
using KalumManagement.Models;
using KalumManagement.Services;
using KalumManagement.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace KalumManagement.Controllers
{
    [ApiController]
    [Route("kalum-management/v1/aspirantes")]
    public class AspiranteController : ControllerBase
    {
        private AppLog log = new AppLog();
        private readonly KalumDBContext kalumDBContext;
        private readonly ILogger<AspiranteController> logger;
        private readonly IMapper mapper;
        private readonly IUtilities utilities;

        private readonly IKalumQueueService kalumQueueService;

        public AspiranteController(IUtilities _utilities, KalumDBContext _KalumDBContext, ILogger<AspiranteController> _logger, IMapper _mapper, IKalumQueueService _kalumQueueService)
        {
            this.kalumDBContext = _KalumDBContext;
            this.logger = _logger;
            this.mapper = _mapper;
            this.kalumQueueService = _kalumQueueService; 
            this.utilities = _utilities;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> GetAspirantes()
        {
            log.ResponseTime = Convert.ToInt16(DateTime.Now.ToString("fff"));
            this.logger.LogDebug("Inicianbdo proceso de consulta de aspirantes");
            List<Aspirante> aspirantes = await this.kalumDBContext.Aspirantes
                .Include(a => a.CarreraTecnica)
                .Include(a => a.Jornada)
                .Include(a => a.ExamenAdmision).ToListAsync();
            if(aspirantes == null || aspirantes.Count == 0)
            {
                this.logger.LogWarning("No existen registros en la base de datos de aspirantes");
                return new NoContentResult();
            }
            //this.logger.LogInformation("Se realizo la consulta de los aspirantes de forma exitosa");
            //Request.Headers.TryGetValue("Authorization", out StringValues token );
            //Console.WriteLine(token.ToString().Split(" ")[1]);
            Console.WriteLine(Request.Method);
            this.utilities.LogPrint(log,200,"Se realizo la consulta de los aspirantes de forma exitosa","Information",Request);
            return Ok(this.mapper.Map<List<AspiranteListDTO>>(aspirantes));
        }

        [HttpGet("{noExpediente}")]
        public async Task<ActionResult<AspiranteListDTO>> GetAspirante(string noExpediente)
        {
            this.logger.LogDebug($"Iniciando consulta del aspirante con numero de expediente {noExpediente}");
            Aspirante aspirante = await this.kalumDBContext.Aspirantes
                .Include(a => a.CarreraTecnica)
                .Include(a => a.Jornada)
                .Include(a => a.ExamenAdmision).FirstOrDefaultAsync(a => a.NoExpediente == noExpediente);
            if(aspirante == null)
            {
                this.logger.LogDebug($"No existe el aspirante con el número de expediente {noExpediente}");
                return new NoContentResult();
            }
            this.logger.LogInformation("La consulta se ejecuto con éxito");
            return Ok(this.mapper.Map<AspiranteListDTO>(aspirante));
        }

        [HttpPost]
        public async Task<ActionResult<AspiranteResponseOrderDTO>> PostOrderAspirante([FromBody] AspiranteCreateDTO aspirante)
        {
            AspiranteCreateOrderDTO orden = new AspiranteCreateOrderDTO();
            CarreraTecnica carreraTecnica = await this.kalumDBContext.CarrerasTecnicas.FirstOrDefaultAsync(ct => ct.CarreraId == aspirante.CarreraId);
            if(carreraTecnica == null)
            {
                return StatusCode(400,new ApiResponseDTO() {StatusCode = 400, Message = $"La carrera técnica con el id {aspirante.CarreraId} no existe"});
            }
            Jornada jornada = await this.kalumDBContext.Jornadas.FirstOrDefaultAsync(j => j.JornadaId == aspirante.JornadaId);
            if(jornada == null)
            {
                return StatusCode(400,new ApiResponseDTO() {StatusCode = 400, Message = $"La jornada con el id {aspirante.JornadaId} no existe"});
            }
            ExamenAdmision examenAdmision = await this.kalumDBContext.ExamenesAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == aspirante.ExamenId);
            if(examenAdmision == null)
            {
                return StatusCode(400,new ApiResponseDTO() {StatusCode = 400, Message = $"El examén de admisión con el id {aspirante.ExamenId} no existe"});
            }
            orden.OrderId = Guid.NewGuid().ToString();
            orden.OrderDate = DateTime.Now;
            orden.Status = "INPROGRESS";
            orden.Data = this.mapper.Map<DataCreateOrderDTO>(aspirante);
            bool result = await this.kalumQueueService.CandidateCreateOrderAsync(orden);
            return StatusCode(201,new AspiranteResponseOrderDTO() {Message = "Orden enviada exitosamente", OrderId = orden.OrderId});
        }
    }
}