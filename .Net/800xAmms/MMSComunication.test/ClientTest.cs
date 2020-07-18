namespace MMSComunication.test
{
    using System;
    using MMSComunication;
    using NUnit.Framework;

    public class ClientTest
    {
        [Test]
        public void ClientShouldThrowArgumentExceptionIfEmptyIpAddress()
        {
            Assert.Throws<ArgumentException>(() => new Client("",102));
        }

        [Test]
        public void PortShouldBeDefault102()
        {
            var client = new Client("127.0.0.1");
            Assert.AreEqual(client.Port, 102);
        }
    }
}
