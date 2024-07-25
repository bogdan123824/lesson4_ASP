using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Test;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly CarContext _context;

        public CarController(CarContext context)
        {
            _context = context;
        }

        [HttpGet("GetCar", Name = "GetCar")]
        public ActionResult<Car> Get(int id)
        {
            var car = _context.Cars.Include(c => c.Brand).Include(c => c.Color).FirstOrDefault(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        [HttpPost("AddCar", Name = "AddCar")]
        public ActionResult<Car> Post(string model, string vin, string licensePlate, int brandId, int colorId)
        {
            var newCar = new Car
            {
                Model = model,
                Vin = vin,
                LicensePlate = licensePlate,
                BrandId = brandId,
                ColorId = colorId
            };

            var error = _context.Cars.FirstOrDefault(c => c.Vin == vin || c.LicensePlate == licensePlate);
            if (error != null)
            {
                return BadRequest("A car with the same VIN or license plate have in DataBase ");
            }

            if(string.IsNullOrWhiteSpace(vin) || string.IsNullOrWhiteSpace(licensePlate) || string.IsNullOrWhiteSpace(model))
            {
                return BadRequest("VIN, license plate number and model must be filled");
            }

            _context.Cars.Add(newCar);
            _context.SaveChanges();
            return CreatedAtAction("GetCar", new { id = newCar.Id }, newCar);
        }

        [HttpPut("UpdateCar", Name = "UpdateCar")]
        public ActionResult Put(int id, string model, string vin, string licensePlate, int brandId, int colorId)
        {
            var carUpdate = _context.Cars.Find(id);
            if (carUpdate == null)
            {
                return NotFound();
            }

            carUpdate.Model = model;
            carUpdate.Vin = vin;
            carUpdate.LicensePlate = licensePlate;
            carUpdate.BrandId = brandId;
            carUpdate.ColorId = colorId;

            _context.Entry(carUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("DeleteCar", Name = "DeleteCar")]
        public ActionResult Delete(int id)
        {
            var carDelete = _context.Cars.Find(id);

            if (carDelete == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(carDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}
