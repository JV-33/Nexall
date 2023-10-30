using Microsoft.AspNetCore.Mvc;
using Nexall.Data.Models;
using Nexall.Services;
using System;

namespace NEXALL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarStatisticsController : ControllerBase
    {
        private readonly ICarStatisticsService _service;

        public CarStatisticsController(ICarStatisticsService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Statistics>> Get(int pageSize = 70000, int currentPage = 1)
        {
            return Ok(_service.GetAll(pageSize, currentPage));
        }

        // Controller
        [HttpGet("filtered")]
        public ActionResult<IEnumerable<Statistics>> GetFiltered(int? speed = null, DateTime? fromDate = null, DateTime? toDate = null, string registrationNumber = null)
        {
            return Ok(_service.GetFiltered(speed, fromDate, toDate, registrationNumber));
        }




        [HttpGet("DayStats/{date}")]
        public ActionResult<IEnumerable<Statistics>> GetByDate(DateTime date)
        {
            var statisticsForTheDay = _service.GetByDate(date);

            if (statisticsForTheDay == null || !statisticsForTheDay.Any())
            {
                return NotFound();
            }

            return Ok(statisticsForTheDay);
        }

        [HttpGet("{id}")]
        public ActionResult<Statistics> Get(int id)
        {
            var carStatistic = _service.GetById(id);

            if (carStatistic == null)
            {
                return NotFound();
            }

            return Ok(carStatistic);
        }

        [HttpPost]
        public ActionResult<Statistics> Post([FromBody] Statistics carStatistic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _service.Add(carStatistic);
            return CreatedAtAction(nameof(Get), new { id = carStatistic.Id }, carStatistic);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Statistics carStatistic)
        {
            if (id != carStatistic.Id)
            {
                return BadRequest("ID does not match");
            }

            if (_service.GetById(id) == null)
            {
                return NotFound();
            }

            _service.Update(id, carStatistic);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var carStatistic = _service.GetById(id);
            if (carStatistic == null)
            {
                return NotFound();
            }

            _service.Delete(id);
            return NoContent();
        }
    }
}
