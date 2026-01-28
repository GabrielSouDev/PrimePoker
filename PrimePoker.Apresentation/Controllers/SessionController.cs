using Microsoft.AspNetCore.Mvc;
using PrimePoker.Application.Services;
using PrimePoker.Domain.Player;

namespace PrimePoker.Apresentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : ControllerBase
    {
        private SessionService _sessionService;

        public SessionController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public async Task<IActionResult> SearchSession([FromBody]SessionRequest playerInfo)
        {
            await _sessionService.AddToQueueAsync(playerInfo);

            return NoContent();
        }

        [HttpGet]
        public IActionResult GetSessions()
        {
            var sessions = _sessionService.GetSessions();

            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public IActionResult GetSession(Guid id)
        {
            var sessions = _sessionService.GetSession(id);
            if(sessions is not null)
                return Ok(sessions);

            return NotFound($"Sessão com Id {id} não encontrada!");
        }

        [HttpGet("queue")]
        public IActionResult GetPlayersQueue()
        {
            var players = _sessionService.GetPlayersQueue();

            return Ok(players);
        }
    }
}


