using System.Buffers.Text;
using APBDTest1.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBDTest1.Controllers
{
    [Route("api/doctor")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        public TeamController(IDbService dbService)
        {
            _dbService = dbService;
        }

        private readonly IDbService _dbService;

        [HttpGet("@index")]
        public IActionResult GetTeamMemberData(string index)
        {
            var teamMemberData = _dbService.GetTeamMemberData(index);

            if (teamMemberData is null) return Ok(teamMemberData);
            return BadRequest();
        }

        [HttpDelete("@index")]
        public IActionResult DeleteDoctor(string index)
        {
            if (!_dbService.DeleteProject(index)) return BadRequest();
            return StatusCode(200);
        }
    }
}