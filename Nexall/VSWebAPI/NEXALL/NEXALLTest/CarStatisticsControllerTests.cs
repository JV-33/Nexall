using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXALL.Controllers;
using NEXALL.DataContext;
using NEXALL.Models;

namespace NEXALLTest
{
    [TestClass]
    public class CarStatisticsControllerTests
    {
        private NEXALLContext _context;
        private CarStatisticsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NEXALLContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            _context = new NEXALLContext(optionsBuilder.Options);
            _controller = new CarStatisticsController(_context);
        }


        [TestMethod]
        public void TestGetById()
        {
            var expectedId = 1;
            var expectedData = new Statistics { Id = expectedId, Date = DateTime.Now, Speed = 100, RegistrationNumber = "AB1234" };
            _context.Statistics.Add(expectedData);
            _context.SaveChanges();

            var result = _controller.Get(expectedId) as ActionResult<Statistics>;

            Assert.IsNotNull(result);
            var carStatistic = result.Value;
            Assert.IsNotNull(carStatistic);
            Assert.AreEqual(expectedId, carStatistic.Id);
        }

        [TestMethod]
        public void TestGetByIdInvalidRegistrationNumber()
        {
            var expectedId = 2;
            var invalidData = new Statistics { Id = expectedId, Date = DateTime.Now, Speed = 100, RegistrationNumber = "INVALID" };
            _context.Statistics.Add(invalidData);
            _context.SaveChanges();

            var result = _controller.Get(expectedId) as ActionResult<Statistics>;

            Assert.IsNotNull(result);
            var carStatistic = result.Value;
            Assert.IsNotNull(carStatistic);
            Assert.AreEqual(expectedId, carStatistic.Id);
            Assert.IsFalse(IsRegistrationNumberValid(carStatistic.RegistrationNumber), "Registration number is not valid");
        }

        private bool IsRegistrationNumberValid(string registrationNumber)
        {
            if (string.IsNullOrEmpty(registrationNumber))
            {
                return false;
            }

            if (registrationNumber.Length != 6)
            {
                return false;
            }

            if (!char.IsUpper(registrationNumber[0]) || !char.IsUpper(registrationNumber[1]))
            {
                return false;
            }

            for (int i = 2; i < 6; i++)
            {
                if (!char.IsDigit(registrationNumber[i]))
                {
                    return false;
                }
            }

            return true;
        }

        [TestMethod]
        public void TestGetByDate()
        {
            var date = new DateTime(2023, 10, 29);
            var data1 = new Statistics { Id = 1, Date = date, Speed = 100, RegistrationNumber = "AB1234" };
            var data2 = new Statistics { Id = 2, Date = date.AddDays(-1), Speed = 90, RegistrationNumber = "CD5678" };

            var existingData1 = _context.Statistics.Find(data1.Id);
            if (existingData1 != null)
            {
                _context.Entry(existingData1).CurrentValues.SetValues(data1);
            }
            else
            {
                _context.Statistics.Add(data1);
            }

            var existingData2 = _context.Statistics.Find(data2.Id);
            if (existingData2 != null)
            {
                _context.Entry(existingData2).CurrentValues.SetValues(data2);
            }
            else
            {
                _context.Statistics.Add(data2);
            }

            _context.SaveChanges();

            var result = _controller.GetByDate(date) as ActionResult<IEnumerable<Statistics>>;

            Assert.IsNotNull(result);
            var statistics = result.Value;
            Assert.IsNotNull(statistics);
            Assert.AreEqual(1, statistics.Count());
            Assert.AreEqual(data1.Id, statistics.First().Id);
        }
    }
}