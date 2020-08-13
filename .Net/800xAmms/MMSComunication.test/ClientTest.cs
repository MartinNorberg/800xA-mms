namespace MMSComunication.test
{
    using System;    
    using NUnit.Framework;

    public class ClientTest
    {
        [Test]
        public void ClientShouldThrowArgumentExceptionIfEmptyIpAddress()
        {
            Assert.Throws<ArgumentException>(() => new Client("", 102));
        }

        [Test]
        public void PortShouldBeDefault102()
        {
            using (var client = new Client("127.0.0.1"))
            {
                Assert.AreEqual(client.Port, 102);
            }
        }

        [TestCase(new byte[] { 3, 0, 0, 32, 2, 240 })]
        public void TryGetMmsVariablesShouldReturnFalseIfArrayLessThan20(byte[] mmsData)
        {
            Assert.AreEqual(false, MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable));
        }

        [TestCase(new byte[] { 0, 0, 0, 32, 2, 240, 128, 160, 23, 2, 1, 1, 165, 18, 160, 10, 48, 8, 160, 6, 128, 4, 84, 69, 83, 84, 160, 4, 133, 2, 39, 15, 0 })]
        public void TryGetMmsVariablesShouldReturnFalseIfFirstByteNot3(byte[] mmsData)
        {
            Assert.AreEqual(false, MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable));
        }

        [TestCase(new byte[] { 3, 0, 0, 32, 2, 240, 128, 160, 23, 2, 1, 1, 165, 18, 160, 10, 48, 8, 160, 6, 128, 4, 84, 69, 83, 84, 160, 4, 133, 2, 39, 15, 0 })]
        public void TryGetMmsVariablesShouldReturnTEST(byte[] mmsData)
        {
            MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable);
            Assert.AreEqual(mmsVariable[0].Name, "TEST");
        }

        [TestCase(new byte[] { 3, 0, 0, 32, 2, 240, 128, 160, 23, 2, 1, 1, 165, 18, 160, 10, 48, 8, 160, 6, 128, 4, 84, 69, 83, 84, 160, 4, 133, 2, 39, 15, 0 })]
        public void TryGetMmsVariablesIntVariableShouldReturn9999(byte[] mmsData)
        {
            MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable);
            Assert.AreEqual(int.Parse(mmsVariable[0].Value), 9999);
        }

        [TestCase(new byte[] { 3, 0, 0, 35, 2, 240, 128, 160, 26, 2, 1, 1, 165, 21, 160, 10, 48, 8, 160, 6, 128, 4, 84, 69, 83, 84, 160, 7, 135, 5, 8, 70, 28, 63, 154, 0 })]
        public void TryGetMmsVariablesRealVariableShouldReturn9999comma9(byte[] mmsData)
        {
            MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable);
            Assert.AreEqual(float.Parse(mmsVariable[0].Value), 9999, 9);
        }

        [TestCase(new byte[] { 3, 0, 0, 50, 2, 240, 128, 160, 41, 2, 1, 1, 165, 36, 160, 26, 48, 12, 160, 10, 128, 8, 168, 77, 121, 79, 116, 104, 101, 114, 48, 10, 160, 8, 128, 6, 84, 69, 83, 84, 65, 82, 160, 6, 131, 1, 0, 133, 1, 0})]
        public void TryGetMmsVariablesShouldRetur2mmsVariables(byte[] mmsData)
        {
            MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable);
            Assert.AreEqual(2, mmsVariable.Count);
        }

        [TestCase(new byte[] { 3, 0, 0, 50, 2, 240, 128, 160, 41, 2, 1, 1, 165, 36, 160, 26, 48, 12, 160, 10, 128, 8, 168, 77, 121, 79, 116, 104, 101, 114, 48, 10, 160, 8, 128, 6, 84, 69, 83, 84, 65, 82, 160, 6, 131, 1, 0, 133, 1, 0 })]
        public void TryGetMmsVariablesMyOtherVariableANDTESTARShouldHaveValueFalseAND0(byte[] mmsData)
        {
            MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable);
            Assert.AreEqual("False", mmsVariable[0].Value);
            Assert.AreEqual("0", mmsVariable[1].Value);
        }

        [TestCase(new byte[] { 3, 0, 0, 36, 2, 240, 128, 160, 27, 2, 1, 1, 165, 22, 160, 10, 48, 8, 160, 6, 128, 4, 84, 69, 83, 84, 160, 8, 138, 6, 72, 69, 74, 83, 65, 78 })]
        public void TryGetMmsVariablesShouldReturnValueHEJSAN(byte[] mmsData)
        {
            MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable);
            Assert.AreEqual("HEJSAN", mmsVariable[0].Value);
        }

        [TestCase(new byte[] { 3, 0, 0, 54, 2, 240, 128, 160, 45, 2, 1, 1, 165, 40, 160, 10, 48, 8, 160, 6, 128, 4, 84, 69, 83, 84, 160, 26, 162, 24, 131, 1, 0, 135, 5, 8, 0, 0, 0, 0, 133, 1, 0, 133, 1, 0, 138, 0, 133, 1, 0, 133, 1, 0, 0})]
        public void TryGetMmsVariablesShouldMyDataStructBoolRealIntDintStringDatetime(byte[] mmsData)
        {
            MMSVariable.TryGetMmsVariables(mmsData, out var mmsVariable);
            Assert.AreEqual(6, mmsVariable.Count);
        }

    }
}
