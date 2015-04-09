using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CarService.Models
{
    public class Model
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string FuelType { get; set; }

        // Foreign Key
        public int ManufacturerId { get; set; }
        // Navigation property
        public Manufacturer Manufacturer { get; set; }
    }
}