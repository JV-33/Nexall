
using Nexall.Data.DataContext;
using Nexall.Data.Models;
using Nexall.Services.Services;

namespace Nexall.Services
{
    public class DataImportService : IDataImportService

    {
        private readonly NexallContext _context;

        public DataImportService(NexallContext context)
        {
            _context = context;
        }

        public void ImportData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split('\t');
                if (parts.Length != 3)
                {
                    continue;
                }

                var date = DateTime.Parse(parts[0]);
                var speed = double.Parse(parts[1]);
                var registrationNumber = parts[2];

                var carStatistic = new Statistics
                {
                    Date = date,
                    Speed = speed,
                    RegistrationNumber = registrationNumber
                };

                _context.Statistics.Add(carStatistic);
            }

            _context.SaveChanges();
        }
    }
}
