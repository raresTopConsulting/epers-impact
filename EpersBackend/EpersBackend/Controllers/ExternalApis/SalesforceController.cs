using EpersBackend.Services.Salesforce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.ExternalApis
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class SalesforceController : ControllerBase
    {
        private readonly IAgentMetricsService _agentMetricsService;
        private readonly IEfAgentMetricsRepository _efAgentMetricsRepo;

        public SalesforceController(IAgentMetricsService agentMetricsService,
            IEfAgentMetricsRepository efAgentMetricsRepo)
        {
            _agentMetricsService = agentMetricsService;
            _efAgentMetricsRepo = efAgentMetricsRepo;
        }

        [HttpGet("Agent/{id}")]
        public IActionResult Get(string id)
        {
            var agentMetric = _efAgentMetricsRepo.Get(id);

            if (agentMetric != null)
                return Ok(agentMetric);
            else
                return BadRequest();
        }

        [HttpGet("Agent/all")]
        public IActionResult GetAll()
        {
            var agentMetrice = _efAgentMetricsRepo.GetAll();
            return Ok(agentMetrice);
        }

        [HttpPost("Agent/SyncAll")]
        public async Task<IActionResult> SyncAllAgentsWithSalesfoce()
        {
            var countUpdatedAgents = await _agentMetricsService.SyncAgentMetricsFromSalesforce();
            return Ok(new { countUpdatedAgents });
        }

    }
}

