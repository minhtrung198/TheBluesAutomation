using Microsoft.VisualStudio.TestTools.UnitTesting;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace TheBluesAutomation
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void SampleTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
