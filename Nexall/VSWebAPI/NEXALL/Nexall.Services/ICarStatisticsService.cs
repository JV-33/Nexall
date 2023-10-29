using Nexall.Data.Models;

namespace Nexall.Services
{
    public interface ICarStatisticsService
    {
        IEnumerable<Statistics> GetAll();
        IEnumerable<Statistics> GetByDate(DateTime date);
        Statistics GetById(int id);
        void Add(Statistics carStatistic);
        void Update(int id, Statistics carStatistic);
        void Delete(int id);
    }
}