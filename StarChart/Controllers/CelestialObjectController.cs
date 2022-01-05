using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var item = _context.CelestialObjects.FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            var satellites = _context.CelestialObjects.Where(i => i.OrbitedObjectId == item.Id);
            item.Satellites = satellites.ToList();
            return Ok(item);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var item = _context.CelestialObjects.FirstOrDefault(i => i.Name == name);
            if (item == null) return NotFound();
            var satellites = _context.CelestialObjects.Where(i => i.OrbitedObjectId == item.Id);
            item.Satellites = satellites.ToList();
            return Ok(item);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // var items = _context.CelestialObjects.ToList();
            // var satellites = _context.CelestialObjects.Where(i => i.OrbitedObjectId == null);
            // items.ForEach(i =>
            // {
            //     if(i.)
            // });
            SeedDatabase();
            
            // var query = _context.CelestialObjects
            //     .Join(
            //         _context.CelestialObjects, 
            //         c1 => c1.OrbitedObjectId,
            //         c2 => c2.Id,
            //     (c1, c2) => c2
            //         );
            // var a2 = query.ToList();

            var a3 = _context.CelestialObjects.Include(i => i.Satellites).ToList();
            
            return Ok(a3);
        }

        private void SeedDatabase()
        {
            var fakeItem1 = new CelestialObject {Id = 1, Name = "hello", OrbitalPeriod = TimeSpan.Zero,};
            _context.CelestialObjects.Add(fakeItem1);

            var fakeItem2 =
                new CelestialObject {Id = 2, Name = "two", OrbitalPeriod = TimeSpan.Zero, OrbitedObjectId = 1};
            _context.CelestialObjects.Add(fakeItem2);
            
            var fakeItem3 = new CelestialObject {Id = 3, Name = "hello", OrbitalPeriod = TimeSpan.Zero,};
            _context.CelestialObjects.Add(fakeItem3);

            _context.SaveChanges();
        }
    }
}
