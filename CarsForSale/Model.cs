using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsForSale
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string FuelType { get; set; }
        public int ManufacturerId { get; set; }
    }
}
