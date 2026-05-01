using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainObjects;
using System.Linq;

namespace FakeDataProvider.Test
{
    [TestClass]
    public class UoWTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var ouw = UnitOfWork.CreateContext())
            {
                var repo = ouw.Repository<Area>();
                var data = repo.Add(new Area() { Name = "deneme" });

                var areas = repo.FindAll().ToList();

                var found1 = repo.FindById(data.ID);

                Assert.IsTrue(areas.Count == 1 && found1.Name == "deneme");

                data.Name = "Deneme1";
                repo.Update(data);
                areas = repo.FindAll().ToList();
                Assert.IsTrue(areas.Count == 1 && found1.Name == "Deneme1");

                var newData = repo.Find(x => x.Name == "Deneme1").FirstOrDefault();

                Assert.IsTrue(newData.Name == "Deneme1");

                repo.Delete(data);
                areas = repo.FindAll().ToList();
                Assert.IsTrue(areas.Count == 0);
            }
        }
    }
}
