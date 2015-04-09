using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarService.Models
{
    public class ModelDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string ManufacturerName { get; set; }
        public string FuelType { get; set; }
    }
}