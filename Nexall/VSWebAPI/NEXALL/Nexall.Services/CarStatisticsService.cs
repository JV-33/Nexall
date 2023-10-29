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

    public IEnumerable<Statistics> GetAll()
    {
        return _context.Statistics.Take(1000).ToList();
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
