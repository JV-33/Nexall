using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXALL.Controllers;
using Nexall.Data.DataContext;
using Nexall.Data.Models;
using Nexall.Services;
using Moq;
using NSubstitute;
using Nexall.Data;

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
        public void GetById_Returns_Statistic_With_Matching_Id()
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
        public void AddStatistic_ValidNewStatistic_EntryAddedToDatabase()
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
        public void UpdateStatistic_ValidStatisticId_UpdatesSpeedInDatabase()
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
        public void DeleteStatistic_ValidStatisticId_RemovesStatisticFromDatabase()
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
        public void GetStatisticsByDate_ValidDateProvided_ReturnsStatisticsForGivenDate()
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
        public void Get_With_Default_Params_Returns_Statistics()
        {
            var mockService = new Mock<ICarStatisticsService>();
            var expectedStatistics = new List<Statistics>
            {
                new Statistics { Date = DateTime.Now, Speed = 100, RegistrationNumber = "EF1234" },
                new Statistics { Date = DateTime.Now.AddDays(-1), Speed = 150, RegistrationNumber = "GH5678" }
            };
            mockService.Setup(s => s.GetAll(70000, 1)).Returns(expectedStatistics);

            var controller = new CarStatisticsController(mockService.Object);

            var result = controller.Get().Result as OkObjectResult;

            Assert.IsNotNull(result);
            var returnedStatistics = result.Value as List<Statistics>;
            Assert.AreEqual(2, returnedStatistics.Count);
        }

        [TestMethod]
        public void GetAll_Returns_Correct_Subset_Of_Data()
        {
            var data = new List<Statistics>
        {
            new Statistics { Id = 1, Date = DateTime.Now, Speed = 100, RegistrationNumber = "EF1234" },
            new Statistics { Id = 2, Date = DateTime.Now.AddDays(-1), Speed = 150, RegistrationNumber = "GH5678" },
            new Statistics { Id = 3, Date = DateTime.Now.AddDays(-2), Speed = 200, RegistrationNumber = "IJ9012" }
        }.AsQueryable();

            var mockSet = Substitute.For<DbSet<Statistics>, IQueryable<Statistics>>();
            ((IQueryable<Statistics>)mockSet).Provider.Returns(data.Provider);
            ((IQueryable<Statistics>)mockSet).Expression.Returns(data.Expression);
            ((IQueryable<Statistics>)mockSet).ElementType.Returns(data.ElementType);
            ((IQueryable<Statistics>)mockSet).GetEnumerator().Returns(data.GetEnumerator());

            var mockContext = Substitute.For<INexallContext>();
            mockContext.Statistics.Returns(mockSet);

            var service = new CarStatisticsService(mockContext);

            var pageSize = 2;
            var currentPage = 1;

            var result = service.GetAll(pageSize, currentPage).ToList();

            Assert.AreEqual(pageSize, result.Count);
            Assert.AreEqual("EF1234", result[0].RegistrationNumber);
            Assert.AreEqual("GH5678", result[1].RegistrationNumber);
        }

        [TestMethod]
        public void GetFiltered_Returns_Correct_Subset_Of_Data()
        {
            var data = new List<Statistics>
        {
            new Statistics { Id = 1, Date = DateTime.Now, Speed = 100, RegistrationNumber = "EF1234" },
            new Statistics { Id = 2, Date = DateTime.Now.AddDays(-1), Speed = 150, RegistrationNumber = "GH5678" },
            new Statistics { Id = 3, Date = DateTime.Now.AddDays(-2), Speed = 200, RegistrationNumber = "IJ9012" }
        }.AsQueryable();

            var options = new DbContextOptionsBuilder<NexallContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new NexallContext(options))
            {
                context.Statistics.AddRange(data);
                context.SaveChanges();

                var service = new CarStatisticsService(context);

                var speedFilter = 150;
                var fromDateFilter = DateTime.Now.AddDays(-3);
                var toDateFilter = DateTime.Now;
                var registrationNumberFilter = "GH5678";
                var pageSize = 1;

                var result = service.GetFiltered(speedFilter, fromDateFilter, toDateFilter, registrationNumberFilter, pageSize).ToList();

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("GH5678", result[0].RegistrationNumber);
            }
        }
    }
}