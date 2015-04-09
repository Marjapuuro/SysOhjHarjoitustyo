using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CarService.Models;

namespace CarService.Controllers
{
    public class ModelsController : ApiController
    {
        private CarServiceContext db = new CarServiceContext();

        // GET api/Models
        public IQueryable<ModelDTO> GetModels()
        {
            var models = from m in db.Models
                        select new ModelDTO()
                        {
                            Id = m.Id,
                            Name = m.Name,
                            ManufacturerName = m.Manufacturer.Name
                        };

            return models;
        }

        // GET api/Models/5
        [ResponseType(typeof(ModelDetailDTO))]
        public async Task<IHttpActionResult> GetModel(int id)
        {
            var model = await db.Models.Include(m => m.Manufacturer).Select(m =>
                new ModelDetailDTO()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Year = m.Year,
                    Price = m.Price,
                    ManufacturerName = m.Manufacturer.Name,
                    FuelType = m.FuelType
                }).SingleOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        // PUT: api/Models/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutModel(int id, Model model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            db.Entry(model).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Models
        [ResponseType(typeof(Model))]
        public async Task<IHttpActionResult> PostModel(Model model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Models.Add(model);
            await db.SaveChangesAsync();

            //Load manufacturer name
            db.Entry(model).Reference(x => x.Manufacturer).Load();

            var dto = new ModelDTO()
            {
                Id = model.Id,
                Name = model.Name,
                ManufacturerName = model.Manufacturer.Name
            };

            return CreatedAtRoute("DefaultApi", new { id = model.Id }, dto);
        }

        // DELETE: api/Models/5
        [ResponseType(typeof(Model))]
        public async Task<IHttpActionResult> DeleteModel(int id)
        {
            Model model = await db.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            db.Models.Remove(model);
            await db.SaveChangesAsync();

            return Ok(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModelExists(int id)
        {
            return db.Models.Count(e => e.Id == id) > 0;
        }
    }
}