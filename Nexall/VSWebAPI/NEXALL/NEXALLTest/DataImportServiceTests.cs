using Microsoft.EntityFrameworkCore;
using NEXALL.DataContext;
using NEXALL.Services;

namespace NEXALLTest
{
    [TestClass]
    public class DataImportServiceTests
    {
        private NEXALLContext _context;
        private DataImportService _service;

        [TestInitialize]
        public void Initialize()
        {
            var databaseName = Guid.NewGuid().ToString();

            var optionsBuilder = new DbContextOptionsBuilder<NEXALLContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName);

            _context = new NEXALLContext(optionsBuilder.Options);
            _service = new DataImportService(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Statistics.RemoveRange(_context.Statistics);
            _context.SaveChanges();
        }

        [TestMethod]
        public void TestImportData()
        {
            var filePath = Path.GetTempFileName();
            File.WriteAllLines(filePath, new[] { "2023-10-29\t100\tAB1234", "2023-10-28\t90\tCD5678" });

            _service.ImportData(filePath);
            var statistics = _context.Statistics.ToList();

            Assert.AreEqual(2, statistics.Count);
        }

        [TestMethod]
        public void TestImportDataWithEmptyFile()
        {
            var filePath = Path.GetTempFileName();
            File.WriteAllLines(filePath, new string[] { });

            _service.ImportData(filePath);
            var statistics = _context.Statistics.ToList();

            Assert.AreEqual(0, statistics.Count);
        }

        [TestMethod]
        public void TestImportDataWithInvalidFilePath()
        {
            var filePath = "invalidPath.txt";

            _service.ImportData(filePath);
            var statistics = _context.Statistics.ToList();

            Assert.AreEqual(0, statistics.Count);
        }

        [TestMethod]
        public void TestImportDataWithInvalidDataFormat()
        {
            var filePath = Path.GetTempFileName();
            File.WriteAllLines(filePath, new[] { "invalidData" });

            _service.ImportData(filePath);
            var statistics = _context.Statistics.ToList();

            Assert.AreEqual(0, statistics.Count);
        }

        [TestMethod]
        public void TestImportDataWithIncorrectData()
        {
            var filePath = Path.GetTempFileName();
            File.WriteAllLines(filePath, new[] { "2023-10-29\t100\tAB1234\tExtraColumn", "2023-10-28\t90" });

            _service.ImportData(filePath);
            var statistics = _context.Statistics.ToList();

            Assert.AreEqual(0, statistics.Count);
        }
    }
}