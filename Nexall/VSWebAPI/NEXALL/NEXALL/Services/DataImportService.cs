using NEXALL.DataContext;
using NEXALL.Models;

namespace NEXALL.Services
{
    public class DataImportService
    {
        private readonly NEXALLContext _context;

        public DataImportService(NEXALLContext context)
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