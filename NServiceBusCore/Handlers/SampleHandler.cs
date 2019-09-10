using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBusCore.Messages.Commands;

namespace NServiceBusCore.Handlers
{
    public class SampleHandler : IHandleMessages<SampleCommand>
    {
        public SampleHandler()
        {
        }

        public async Task Handle(SampleCommand message, IMessageHandlerContext context)
        {
            Debug.WriteLine($"Received command - {message.SomeData}");

            await DoSomethingUseful();
        }

        private async Task DoSomethingUseful()
        {
            await Task.Delay(1000);
        }
    }
}
