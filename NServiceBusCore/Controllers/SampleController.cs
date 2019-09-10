using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using NServiceBusCore.Messages.Commands;
using NServiceBusCore.Models;

namespace NServiceBus_Core.Controllers
{
    [Route("api/sample")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IMessageSession _messageSession;
        public SampleController(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        [HttpPost]
        [Route("save")]
        public async Task<ActionResult> Save(SampleSaveModel model)
        {
            model.Id = Guid.NewGuid();

            await _messageSession.SendLocal(new SampleCommand { Id = model.Id, SomeData = model.SampleData });

            return Ok(model);
        }
    }
}
