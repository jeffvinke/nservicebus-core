using System;
using System.Net.Http;
using System.Threading.Tasks;
using NServiceBus_Core.Controllers;
using NServiceBusCore.Messages.Commands;
using NServiceBusCore.Models;
using NUnit.Framework;

namespace NServiceBusCore.Test
{
    public class SampleControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test_TestGetMethod()
        {
            var messageSession = new NServiceBus.Testing.TestableMessageSession();
            var sampleController = new SampleController(messageSession);
            var values = await sampleController.GetSomeValues();
            Assert.AreEqual("testGet", values);
        }
        [Test]
        public async Task Test_PostMethod()
        {
            var messageSession = new NServiceBus.Testing.TestableMessageSession();
            var sampleController = new SampleController(messageSession);
            var testGuid = Guid.NewGuid();
            var sampleData = "my test data";
            var model = new SampleSaveModel
            {
                Id = testGuid,
                SampleData = sampleData
            };
            var ret = await sampleController.Save(model);
            var messagesSent = messageSession.SentMessages;
            Assert.AreEqual(1, messagesSent.Length);
            Assert.IsInstanceOf<SampleCommand>(messagesSent[0].Message);
            var sentMessage = (Messages.Commands.SampleCommand) messagesSent[0].Message;
            Assert.AreEqual(testGuid, sentMessage.Id);
            Assert.AreEqual(sampleData, sentMessage.SomeData);

        }
        
    }
}