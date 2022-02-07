using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using JoshDevenyi_PokemonPassionProject.Models;

namespace JoshDevenyi_PokemonPassionProject.Controllers
{
    public class PokemonTypeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PokemonTypeData/ListPokemonTypes
        [HttpGet]
        public IEnumerable<PokemonTypeDto> ListPokemonTypes()
        {
            List<PokemonType> PokemonTypes = db.PokemonTypes.ToList();
            List<PokemonTypeDto> PokemonTypeDtos = new List<PokemonTypeDto>();

            PokemonTypes.ForEach(a => PokemonTypeDtos.Add(new PokemonTypeDto()
            {
                PokemonTypeId = a.PokemonTypeId,
                PokemonTypeName = a.PokemonTypeName,
            }));

            return PokemonTypeDtos;
        }

        // GET: api/PokemonTypeData/FindPokemonType/5
        [ResponseType(typeof(PokemonType))]
        [HttpGet]
        public IHttpActionResult FindPokemonType(int id)
        {
            PokemonType PokemonType = db.PokemonTypes.Find(id);
            PokemonTypeDto PokemonTypeDto = new PokemonTypeDto()
            {
                PokemonTypeId = PokemonType.PokemonTypeId,
                PokemonTypeName = PokemonType.PokemonTypeName,

            };

            if (PokemonType == null)
            {
                return NotFound();
            }

            return Ok(PokemonTypeDto);
        }

        // PUT: api/PokemonTypeData/UpdatePokemonType/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePokemonType(int id, PokemonType pokemonType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pokemonType.PokemonTypeId)
            {
                return BadRequest();
            }

            db.Entry(pokemonType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PokemonTypeExists(id))
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

        // POST: api/PokemonTypeData/AddPokemonType
        [ResponseType(typeof(PokemonType))]
        [HttpPost]
        public IHttpActionResult AddPokemonType(PokemonType pokemonType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PokemonTypes.Add(pokemonType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pokemonType.PokemonTypeId }, pokemonType);
        }

        // DELETE: api/PokemonTypeData/DeletePokemonType/5
        [ResponseType(typeof(PokemonType))]
        [HttpPost]
        public IHttpActionResult DeletePokemonType(int id)
        {
            PokemonType pokemonType = db.PokemonTypes.Find(id);
            if (pokemonType == null)
            {
                return NotFound();
            }

            db.PokemonTypes.Remove(pokemonType);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PokemonTypeExists(int id)
        {
            return db.PokemonTypes.Count(e => e.PokemonTypeId == id) > 0;
        }
    }
}