using Microsoft.AspNetCore.Mvc;
using NEXALL.DataContext;
using NEXALL.Models;

namespace NEXALL.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CarStatisticsController : ControllerBase
    {
        private readonly NEXALLContext _context;

        public CarStatisticsController(NEXALLContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Statistics>> Get()
        {
            return _context.Statistics.Take(500).ToList();
        }

        [HttpGet("DayStats/{date}")]
        public ActionResult<IEnumerable<Statistics>> GetByDate(DateTime date)
        {
            var statisticsForTheDay = _context.Statistics.Where(m => m.Date.Date == date.Date).ToList();

            if (!statisticsForTheDay.Any())
            {
                return NotFound();
            }

            return statisticsForTheDay;
        }

        [HttpGet("{id}")]
        public ActionResult<Statistics> Get(int id)
        {
            var carStatistic = _context.Statistics.FirstOrDefault(m => m.Id == id);

            if (carStatistic == null)
            {
                return NotFound();
            }

            return carStatistic;
        }

        [HttpPost]
        public ActionResult<Statistics> Post([FromBody] Statistics carStatistic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Statistics.Add(carStatistic);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = carStatistic.Id }, carStatistic);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Statistics carStatistic)
        {
            if (id != carStatistic.Id)
            {
                return BadRequest("ID does not match");
            }

            if (!_context.Statistics.Any(x => x.Id == id))
            {
                return NotFound();
            }

            _context.Update(carStatistic);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var carStatistic = _context.Statistics.FirstOrDefault(m => m.Id == id);
            if (carStatistic == null)
            {
                return NotFound();
            }

            _context.Statistics.Remove(carStatistic);
            _context.SaveChanges();

            return NoContent();
        }
    }
}