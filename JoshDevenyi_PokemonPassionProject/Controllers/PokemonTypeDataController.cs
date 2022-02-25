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

        /// <summary>
        /// Returns all Pokemon Types in the system.
        /// </summary>
        /// <returns>
        /// CONTENT: All the Pokemon Types in the database
        /// </returns>
        /// <example>
        /// GET: api/PokemonTypeData/ListPokemonTypes
        /// </example> 
        [HttpGet]
        public IEnumerable<PokemonTypeDto> ListPokemonTypes()
        {
            List<PokemonType> PokemonTypes = db.PokemonTypes.ToList();
            List<PokemonTypeDto> PokemonTypeDtos = new List<PokemonTypeDto>();

            PokemonTypes.ForEach(a => PokemonTypeDtos.Add(new PokemonTypeDto()
            {
                PokemonTypeId = a.PokemonTypeId,
                PokemonTypeName = a.PokemonTypeName,
                PokemonTypeColor = a.PokemonTypeColor,
            }));

            return PokemonTypeDtos;
        }

        /// <summary>
        /// Returns all Pokemon Types in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The Pokemon Type in the system that corresponds to the provided primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The Pokeon Types primary key</param>
        /// <example>
        /// GET: api/PokemonTypeData/FindPokemonType/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PokemonType))]
        public IHttpActionResult FindPokemonType(int id)
        {
            PokemonType PokemonType = db.PokemonTypes.Find(id);
            PokemonTypeDto PokemonTypeDto = new PokemonTypeDto()
            {
                PokemonTypeId = PokemonType.PokemonTypeId,
                PokemonTypeName = PokemonType.PokemonTypeName,
                PokemonTypeColor = PokemonType.PokemonTypeColor,


            };

            if (PokemonType == null)
            {
                return NotFound();
            }

            return Ok(PokemonTypeDto);
        }

        /// <summary>
        /// Updates a specific Pokemon Type in the system with a POST data input
        /// </summary>
        /// <param name="id">The Pokemon Types' Primary Key</param>
        /// <param name="PokemonType">JSON FORM DATA of an Pokemon Type</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PokemonTypeData/UpdatePokemonType/5
        /// FORM DATA: PokemonType JSON Object
        /// </example>
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

        /// <summary>
        /// Adds a Pokemon Type to the system
        /// </summary>
        /// <param name="PokemonType">JSON FORM DATA of a PokemonType</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: PokemonType ID, PokemonType Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PokemonTypeData/AddPokemonType
        /// FORM DATA: PokemonType JSON Object
        /// </example>
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


        /// <summary>
        /// Deletes a Pokemon Type from the system based on it's ID.
        /// </summary>
        /// <param name="id">A Pokemon Types primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PokemonTypeData/DeletePokemonType/5
        /// FORM DATA: (empty)
        /// </example>
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