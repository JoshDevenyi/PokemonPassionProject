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
using System.Diagnostics;

namespace JoshDevenyi_PokemonPassionProject.Controllers
{
    public class PokemonDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Return all Pokemon in the database
        /// </summary>
        /// <returns>
        /// CONTENT: all the pokemon, including their associated type.
        /// </returns>
        /// <example>
        /// GET: api/PokemonData/ListPokemons
        /// </example>
        [HttpGet]
        public IEnumerable<PokemonDto> ListPokemons()
        {
            List<Pokemon> Pokemons = db.Pokemons.OrderBy(p=>p.PokedexNumber).ToList(); //Ordered by pokedex number to match organizaiton from the games
            List<PokemonDto> PokemonDtos = new List<PokemonDto>();

            Pokemons.ForEach(a => PokemonDtos.Add(new PokemonDto()
            {
                PokemonId = a.PokemonId,
                PokemonName = a.PokemonName,
                PokedexNumber = a.PokedexNumber,
                PokemonType = a.PokemonType.PokemonTypeName

            })) ;

            //return Ok(PokemonDtos); Tried to use the OK function like in your example but couldn't find solution for the error it would cause in time. 
            return PokemonDtos;
        }

        /// <summary>
        /// Gather information about all Pokemon that belong to a particular Type
        /// </summary>
        /// <returns>
        /// CONTENT: all Pokemon in the database, including their associated type, matching to a particular PokemonType id
        /// </returns>
        /// <param name="id">The Type Id</param>
        /// <example>
        /// GET: api/PokemonData/ListPokemonsForPokemonType/5
        /// </example>
        [HttpGet]
        public IEnumerable<PokemonDto> ListPokemonsForPokemonType(int id)
        {
            List<Pokemon> Pokemons = db.Pokemons.Where(p=>p.PokemonTypeId==id).OrderBy(p => p.PokedexNumber).ToList();
            List<PokemonDto> PokemonDtos = new List<PokemonDto>();

            Pokemons.ForEach(a => PokemonDtos.Add(new PokemonDto()
            {
                PokemonId = a.PokemonId,
                PokemonName = a.PokemonName,
                PokedexNumber = a.PokedexNumber,
                PokemonType = a.PokemonType.PokemonTypeName

            }));

            return PokemonDtos;
        }

        /// <summary>
        /// Gathers information about Pokemons related to a particular trainer
        /// </summary>
        /// <returns>
        /// CONTENT: all Pokemon in the database, including their associated type, matching to a particular Trainer id
        /// </returns>
        /// <param name="id">The Trainer Id</param>
        /// <example>
        /// GET: api/PokemonData/ListPokemonsForTrainer/5
        /// </example>
        [HttpGet]
        public IEnumerable<PokemonDto> ListPokemonsForTrainer(int id)
        {
            //All pokemon that have have trainers which match with the id
            List<Pokemon> Pokemons = db.Pokemons.Where(
                p => p.Trainers.Any(
                    t=>t.TrainerId == id
                )).OrderBy(p => p.PokedexNumber).ToList();
            List<PokemonDto> PokemonDtos = new List<PokemonDto>();

            Pokemons.ForEach(a => PokemonDtos.Add(new PokemonDto()
            {
                PokemonId = a.PokemonId,
                PokemonName = a.PokemonName,
                PokedexNumber = a.PokedexNumber,
                PokemonType = a.PokemonType.PokemonTypeName

            }));

            return PokemonDtos;
        }

        /// <summary>
        /// Gather information about all Pokemon not belonging to a particular Trainer
        /// </summary>
        /// <param name="id">The Trainer Id</param>
        /// <returns>
        /// CONTENT: all Pokemon in the database, including their associated type, not matching to a particular Trainer id
        /// </returns>
        /// <example>
        /// GET: api/PokemonData/ListPokemonsNotCaughtForTrainer/5
        /// </example>
        [HttpGet]
        public IEnumerable<PokemonDto> ListPokemonsNotCaughtForTrainer(int id)
        {
            //All pokemon that have have trainers which match with the id
            List<Pokemon> Pokemons = db.Pokemons.Where(
                p => !p.Trainers.Any(
                    t => t.TrainerId == id
                )).OrderBy(p => p.PokedexNumber).ToList();
            List<PokemonDto> PokemonDtos = new List<PokemonDto>();

            Pokemons.ForEach(a => PokemonDtos.Add(new PokemonDto()
            {
                PokemonId = a.PokemonId,
                PokemonName = a.PokemonName,
                PokedexNumber = a.PokedexNumber,
                PokemonType = a.PokemonType.PokemonTypeName
            }));

            return PokemonDtos;
        }

        /// <summary>
        /// Returns any pokemon in the database.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An pokemon in the database that matches the pokemon ID provided
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">A Pokemon's Primary Key</param>
        /// <example>
        /// GET: api/PokemonData/FindPokemon/5
        /// </example>
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

        /// <summary>
        /// Updates a selected pokemon in the system
        /// </summary>
        /// <param name="id">Represents the Pokemon ID primary key</param>
        /// <param name="pokemon">JSON FORM DATA of a Pokemon</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PokemonData/UpdatePokemon/5
        /// FORM DATA: Pokemon JSON Object
        /// </example>
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

        /// <summary>
        /// Adds an Pokemon to the system
        /// </summary>
        /// <param name="pokemon">JSON FORM DATA of an pokemon</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Pokemon ID, Pokemon Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PokemonData/AddPokemon
        /// FORM DATA: Pokeon JSON Object
        /// </example>
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


        /// <summary>
        /// Deletes an Pokemon from the system based on it's Id
        /// </summary>
        /// <param name="id">The primary key of a Pokemon</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PokemonData/DeletePokemon/5
        /// FORM DATA: (empty)
        /// </example>
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