using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();
            foreach(var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach(var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject id)
        {
            _context.CelestialObjects.Add(id);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = id.Id }, id);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject currentObj)
        {
            var updatedObject = _context.CelestialObjects.Find(id);
            if (updatedObject == null)
            {
                return NotFound();
            }
            else
            {
                updatedObject.Name = currentObj.Name;
                updatedObject.OrbitalPeriod = currentObj.OrbitalPeriod;
                updatedObject.OrbitedObjectId = currentObj.OrbitedObjectId;
                _context.CelestialObjects.Update(updatedObject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var renamedObject = _context.CelestialObjects.Find(id);
            if (renamedObject == null)
            {
                return NotFound();
            }
            else
            {
                renamedObject.Name = name;
                _context.CelestialObjects.Update(renamedObject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleteObj = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id).ToList();
            if (!deleteObj.Any())
                return NotFound();
            _context.CelestialObjects.RemoveRange(deleteObj);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
