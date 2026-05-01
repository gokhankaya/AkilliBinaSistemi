using ItemModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimulationObjects.Test
{
    [TestClass]
    public class IsikTest
    {
        [TestMethod]
        public void AdaptorTest1()
        {
            Device.Devices.Reset();

            Isik item = new Isik("Işık1");
            item.IpV4 = "10.0.0.1";
            item.IpV6 = "";
            Assert.IsFalse(item.CurrentState);

            item.OpenLight();
            Assert.IsTrue(item.CurrentState);

            item.CloseLight();
            Assert.IsFalse(item.GetLigthState());

            item.OpenLight();
            Assert.IsTrue(item.GetLigthState());
        }

        [TestMethod]
        public void AdaptorTest2()
        {
            SharedObject.AdleAreaBase area = new SharedObject.AdleAreaBase("Oturma Odası");

            Isik isik = new Isik(area, "Işık1");
            isik.IpV4 = "10.0.0.1";
            isik.IpV6 = "";

            ItemModel.PIR pir = new ItemModel.PIR(area, "PIR1");
            pir.IpV4 = "10.0.0.2";
            pir.IpV6 = "";

            Device.Devices.Reset();
            Device.Devices.Register(new Device.PIR()
            {
                ip = pir.IpV4,
                AreaID = 1,
                Name = pir.Name,
                state = false,
                AreaBase = new AreaBase()
                {
                    Name = area.Name,
                    ID = 1
                }
            });

            Assert.IsFalse(pir.GetPIRState());

            var device = SimulationObjects.Device.Devices.find("10.0.0.2");
            Assert.IsTrue(device is SimulationObjects.Device.PIR);

            ((SimulationObjects.Device.PIR)device).Activate("10.0.0.2");

            Assert.IsTrue(pir.GetPIRState());
        }

        [TestMethod]
        public void AdaptorTest3()
        {
            SharedObject.AdleAreaBase area = new SharedObject.AdleAreaBase("Oturma Odası");

            Isik isik = new Isik(area, "Işık1");
            isik.IpV4 = "10.0.0.1";
            isik.IpV6 = "";

            ItemModel.PIR pir = new ItemModel.PIR(area, "PIR1");
            pir.IpV4 = "10.0.0.2";
            pir.IpV6 = "";

            Device.Devices.Reset();
            Device.Devices.Register(new Device.Light() { ID = 1, ip = isik.IpV4, AreaID = 1, AreaBase = new AreaBase() { Name = area.Name, ID = 1 }, Name = isik.Name, state = false });
            Assert.IsFalse(pir.GetPIRState());

            Device.Devices.Register(new Device.PIR() { ID = 1, ip = pir.IpV4, AreaID = 1, AreaBase = new AreaBase() { ID = 1, Name = area.Name }, Name = pir.Name, state = false });
            Assert.IsFalse(pir.GetPIRState());


            var device = SimulationObjects.Device.Devices.find("10.0.0.2");
            Assert.AreEqual(device.Type, "PIR");

            device = SimulationObjects.Device.Devices.find("10.0.0.1");
            Assert.AreEqual(device.Type, "Light");
        }

    }
}
