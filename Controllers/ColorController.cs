using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Test;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColorController : ControllerBase
    {
        private readonly CarContext _context;

        public ColorController(CarContext context)
        {
            _context = context;
        }

        [HttpGet("GetColorCar", Name = "GetColorCar")]
        public ActionResult<Color> Get(int id)
        {
            var color = _context.Colors.Include(c => c.Cars).FirstOrDefault(c => c.Id == id);
            if (color == null)
            {
                return NotFound();
            }
            return color;
        }

        [HttpPost("AddColorCar", Name = "AddColorCar")]
        public ActionResult<Color> Post(string color)
        {
            var newColor = new Color
            {
                Name = color
            };

            if(string.IsNullOrWhiteSpace(color))
            {
                return BadRequest("Color car must be filled");
            }

            _context.Colors.Add(newColor);
            _context.SaveChanges();
            return CreatedAtAction("GetColorCar", new { id = newColor.Id }, newColor);
        }

        [HttpPut("UpdateColorCar", Name = "UpdateColorCar")]
        public ActionResult Put(int id, string color)
        {
            var colorUpdate = _context.Colors.Find(id);
            if (colorUpdate == null)
            {
                return NotFound();
            }

            colorUpdate.Name = color;
            _context.Entry(colorUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("DeleteColorCar", Name = "DeleteColorCar")]
        public ActionResult Delete(int id)
        {
            var colorDelete = _context.Colors.Find(id);
            if (colorDelete == null)
            {
                return NotFound();
            }

            _context.Colors.Remove(colorDelete);
            _context.SaveChanges();
            return Ok();
        }
    }

}
