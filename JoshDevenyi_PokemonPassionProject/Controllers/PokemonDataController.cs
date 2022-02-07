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
    public class PokemonDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PokemonData/ListPokemons
        [HttpGet]
        public IEnumerable<PokemonDto> ListPokemons()
        {
            List<Pokemon> Pokemons = db.Pokemons.ToList();
            List<PokemonDto> PokemonDtos = new List<PokemonDto>();

            Pokemons.ForEach(a => PokemonDtos.Add(new PokemonDto()
            {
                PokemonId = a.PokemonId,
                PokemonName = a.PokemonName,
                PokedexNumber = a.PokedexNumber,
                PokemonType = a.PokemonType.PokemonTypeName

            })) ;

            return PokemonDtos;
        }

        // GET: api/PokemonData/FindPokemon/5
        [ResponseType(typeof(Pokemon))]
        [HttpGet]
        public IHttpActionResult FindPokemon(int id)
        {
            Pokemon Pokemon = db.Pokemons.Find(id);
            PokemonDto PokemonDto = new PokemonDto()
            {
                PokemonId = Pokemon.PokemonId,
                PokemonName = Pokemon.PokemonName,
                PokedexNumber = Pokemon.PokedexNumber,
                PokemonType = Pokemon.PokemonType.PokemonTypeName
            };
            if (Pokemon == null)
            {
                return NotFound();
            }

            return Ok(PokemonDto);
        }

        // POST: api/PokemonData/UpdatePokemon/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePokemon(int id, Pokemon pokemon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pokemon.PokemonId)
            {
                return BadRequest();
            }

            db.Entry(pokemon).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PokemonExists(id))
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

        // POST: api/PokemonData/AddPokemon
        [ResponseType(typeof(Pokemon))]
        [HttpPost]
        public IHttpActionResult AddPokemon(Pokemon pokemon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pokemons.Add(pokemon);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pokemon.PokemonId }, pokemon);
        }

        // POST: api/PokemonData/DeletePokemon/5
        [ResponseType(typeof(Pokemon))]
        [HttpPost]
        public IHttpActionResult DeletePokemon(int id)
        {
            Pokemon pokemon = db.Pokemons.Find(id);
            if (pokemon == null)
            {
                return NotFound();
            }

            db.Pokemons.Remove(pokemon);
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

        private bool PokemonExists(int id)
        {
            return db.Pokemons.Count(e => e.PokemonId == id) > 0;
        }
    }
}