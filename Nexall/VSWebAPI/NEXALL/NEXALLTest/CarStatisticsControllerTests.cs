using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXALL.Controllers;
using Nexall.Data.DataContext;
using Nexall.Data.Models;


namespace NEXALLTest
{
    [TestClass]
    public class CarStatisticsControllerTests
    {
        private NexallContext _context;
        private CarStatisticsController _controller;
        private CarStatisticsService _service;

        [TestInitialize]
        public void Initialize()
        {
            var databaseName = Guid.NewGuid().ToString();

            var optionsBuilder = new DbContextOptionsBuilder<NexallContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName);

            _context = new NexallContext(optionsBuilder.Options);
            _service = new CarStatisticsService(_context);
            _controller = new CarStatisticsController(_service);
        }

        [TestMethod]
        public void TestGetById()
        {
            var expectedId = 1;
            var statistic = new Statistics { Id = expectedId, Date = DateTime.Now, Speed = 100, RegistrationNumber = "AB1234" };
            _context.Statistics.Add(statistic);
            _context.SaveChanges();

            var result = _controller.Get(expectedId);

            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            var retrievedStatistic = okResult.Value as Statistics;
            Assert.AreEqual(expectedId, retrievedStatistic.Id);
        }

        [TestMethod]
        public void TestAddStatistic()
        {
            var newStatistic = new Statistics { Date = DateTime.Now, Speed = 200, RegistrationNumber = "CD5678" };

            var resultAction = _controller.Post(newStatistic);
            var result = resultAction.Result as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(_controller.Get), result.ActionName);
            var addedStatistic = result.Value as Statistics;
            Assert.IsNotNull(addedStatistic);
            Assert.AreEqual(newStatistic.Speed, addedStatistic.Speed);
            Assert.AreEqual(newStatistic.RegistrationNumber, addedStatistic.RegistrationNumber);

            var savedStatistic = _context.Statistics.FirstOrDefault(m => m.Id == addedStatistic.Id);
            Assert.IsNotNull(savedStatistic);
            Assert.AreEqual(newStatistic.Speed, savedStatistic.Speed);
            Assert.AreEqual(newStatistic.RegistrationNumber, savedStatistic.RegistrationNumber);
        }

        [TestMethod]
        public void TestUpdateStatistic()
        {
            var initialStat = new Statistics { Date = DateTime.Now, Speed = 100, RegistrationNumber = "KL3456" };
            _context.Statistics.Add(initialStat);
            _context.SaveChanges();

            initialStat.Speed = 150;
            _controller.Put(initialStat.Id, initialStat);

            var updatedStat = _context.Statistics.FirstOrDefault(m => m.Id == initialStat.Id);
            Assert.AreEqual(150, updatedStat.Speed);
        }
        [TestMethod]
        public void TestDeleteStatistic()
        {
            var statisticToDelete = new Statistics { Date = DateTime.Now, Speed = 170, RegistrationNumber = "MN7890" };
            _context.Statistics.Add(statisticToDelete);
            _context.SaveChanges();

            var initialCount = _context.Statistics.Count();

            _controller.Delete(statisticToDelete.Id);

            Assert.AreEqual(initialCount - 1, _context.Statistics.Count());
            Assert.IsNull(_context.Statistics.FirstOrDefault(m => m.Id == statisticToDelete.Id));
        }

        [TestMethod]
        public void TestGetStatisticsByDate()
        {
            var targetDate = DateTime.Now;
            var statistic = new Statistics { Date = targetDate, Speed = 120, RegistrationNumber = "IJ9012" };
            _context.Statistics.Add(statistic);
            _context.SaveChanges();

            var actionResult = _controller.GetByDate(targetDate);
            var result = actionResult.Result as OkObjectResult;
            var statistics = result.Value as List<Statistics>;

            Assert.AreEqual(1, statistics.Count);
            Assert.AreEqual(targetDate.Date, statistics[0].Date.Date);
        }

        [TestMethod]
        public void TestGetAllStatistics()
        {
            _context.Statistics.AddRange(
                new Statistics { Date = DateTime.Now, Speed = 100, RegistrationNumber = "EF1234" },
                new Statistics { Date = DateTime.Now.AddDays(-1), Speed = 150, RegistrationNumber = "GH5678" }
            );
            _context.SaveChanges();

            var actionResult = _controller.Get();
            var result = actionResult.Result as OkObjectResult;
            var statistics = result.Value as List<Statistics>;

            Assert.AreEqual(2, statistics.Count);
        }
    }
}