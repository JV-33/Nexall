using Nexall.Data.Models;

namespace Nexall.Services
{
    public interface ICarStatisticsService
    {
        IEnumerable<Statistics> GetAll(int pageSize = 70000, int currentPage = 1);
        IEnumerable<Statistics> GetFiltered(int? speed = null, DateTime? fromDate = null, DateTime? toDate = null, string registrationNumber = null, int pageSize = 20);
        IEnumerable<Statistics> GetByDate(DateTime date);
        Statistics GetById(int id);
        void Add(Statistics carStatistic);
        void Update(int id, Statistics carStatistic);
        void Delete(int id);
    }
}