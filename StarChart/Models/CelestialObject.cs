using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StarChart.Models;

namespace StarChart.Models
{
    public class CelestialObject
    {
        public int Id { get; set; }

        [Required]
        public String Name { get; set; } 
        public int? OrbitedObjectId { get; set; }
        [NotMapped]
        public List<CelestialObject> Sattellites { get; set; }
        public TimeSpan OrbitalPeriod { get; set; }
    }
}
