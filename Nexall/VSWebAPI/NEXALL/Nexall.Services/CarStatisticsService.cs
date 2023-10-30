using System.Linq;
using Nexall.Data.DataContext;
using Nexall.Data.Models;
using Nexall.Services;

public class CarStatisticsService : ICarStatisticsService
{
    private readonly NexallContext _context;

    public CarStatisticsService(NexallContext context)
    {
        _context = context;
    }

    public IEnumerable<Statistics> GetAll(int pageSize = 70000, int currentPage = 1)
    {
        return _context.Statistics
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .AsEnumerable();
    }

    public IEnumerable<Statistics> GetFiltered(int? speed = null, DateTime? fromDate = null, DateTime? toDate = null, string registrationNumber = null, int pageSize = 20) // Pievienoja registrationNumber
    {
        var query = _context.Statistics.AsQueryable();

        if (speed.HasValue)
        {
            query = query.Where(m => m.Speed == speed.Value);
        }
        if (fromDate.HasValue)
        {
            query = query.Where(m => m.Date.Date >= fromDate.Value.Date);
        }
        if (toDate.HasValue)
        {
            query = query.Where(m => m.Date.Date <= toDate.Value.Date);
        }
        if (!string.IsNullOrWhiteSpace(registrationNumber)) // Pievienoja reģistrācijas numura filtrēšanu
        {
            query = query.Where(m => m.RegistrationNumber == registrationNumber);
        }

        return query.Take(pageSize).AsEnumerable(); // Pievienoja Take(pageSize)
    }







    public IEnumerable<Statistics> GetByDate(DateTime date)
    {
        return _context.Statistics.Where(m => m.Date.Date == date.Date).ToList();
    }

    public Statistics GetById(int id)
    {
        return _context.Statistics.FirstOrDefault(m => m.Id == id);
    }

    public void Add(Statistics carStatistic)
    {
        _context.Statistics.Add(carStatistic);
        _context.SaveChanges();
    }

    public void Update(int id, Statistics carStatistic)
    {
        _context.Update(carStatistic);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var carStatistic = _context.Statistics.FirstOrDefault(m => m.Id == id);
        if (carStatistic != null)
        {
            _context.Statistics.Remove(carStatistic);
            _context.SaveChanges();
        }
    }
}
