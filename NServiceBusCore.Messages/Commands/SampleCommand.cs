using System;
namespace NServiceBusCore.Messages.Commands
{
    public class SampleCommand
    {
        public Guid Id { get; set; }
        public string SomeData { get; set; }
    }
}
