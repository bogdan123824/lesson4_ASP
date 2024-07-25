using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Test;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly CarContext _context;

        public BrandController(CarContext context)
        {
            _context = context;
        }

        [HttpGet("GetBrandCar", Name = "GetBrandCar")]
        public ActionResult<Brand> Get(int id)
        {
            var brand = _context.Brands.Include(b => b.Cars).FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return NotFound();
            }
            return brand;
        }

        [HttpPost("AddBrandCar", Name = "AddBrandCar")]
        public ActionResult<Brand> Post(string name)
        {
            var newBrand = new Brand
            {
                Name = name
            };

            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Brand name must be filled.");
            }

            _context.Brands.Add(newBrand);
            _context.SaveChanges();
            return CreatedAtAction("GetBrandCar", new { id = newBrand.Id }, newBrand);
        }

        [HttpPut("UpdateBrandCar", Name = "UpdateBrandCar")]
        public ActionResult Put(int id, string name)
        {
            var brandUpdate = _context.Brands.Find(id);

            if (brandUpdate == null)
            {
                return NotFound();
            }

            brandUpdate.Name = name;
            _context.Entry(brandUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("DeleteBrandCar", Name = "DeleteBrandCar")]
        public ActionResult Delete(int id)
        {
            var brandDelete = _context.Brands.Find(id);
            if (brandDelete == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brandDelete);
            _context.SaveChanges();
            return Ok();
        }
    }

}
