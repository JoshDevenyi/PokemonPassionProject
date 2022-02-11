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
    public class TrainerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns All Trainers in the system.
        /// </summary>
        /// <returns>
        /// CONTENT: All the Trainers in the database
        /// </returns>
        /// <example>
        /// GET: api/TrainerData/ListTrainers
        /// </example>
        [HttpGet]
        public IEnumerable<TrainerDto> ListTrainers()
        {
            List<Trainer> Trainers = db.Trainers.ToList();
            List<TrainerDto> TrainerDtos = new List<TrainerDto>();

            Trainers.ForEach(t => TrainerDtos.Add(new TrainerDto()
            {
                TrainerId = t.TrainerId,
                TrainerName = t.TrainerName,
                TrainerTitle = t.TrainerTitle,
                TrainerBio = t.TrainerBio

            }));

            return TrainerDtos;
        }

        /// <summary>
        /// Returns all Trainers in the system who have caught a particular Pokemon.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Trainers in the database who have caught a particular Pokemon
        /// </returns>
        /// <param name="id">The Trainer's Primary Key</param>
        /// <example>
        /// GET: api/TrainerData/ListTrainersForPokemon/5
        /// </example>
        [HttpGet]
        public IEnumerable<TrainerDto> ListTrainersForPokemon(int id)
        {
            List<Trainer> Trainers = db.Trainers.Where( 
                
                t => t.Pokemons.Any(
                        p=>p.PokemonId == id)                                  
                ).ToList();

            List<TrainerDto> TrainerDtos = new List<TrainerDto>();

            Trainers.ForEach(t => TrainerDtos.Add(new TrainerDto()
            {
                TrainerId = t.TrainerId,
                TrainerName = t.TrainerName,
                TrainerTitle = t.TrainerTitle,
                TrainerBio = t.TrainerBio

            }));

            return TrainerDtos;
        }

        /// <summary>
        /// Associates a particular trainer wtih a particular pokemon when one is caught
        /// </summary>
        /// <param name="trainerid">The trainer's primary id key</param>
        /// <param name="pokemonid">The pokemon's primary id key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 400 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/TrainerData/AssociateTrainerWithPokemon/1/17
        /// </example>
        [HttpPost]
        [Route("api/trainerdata/AssociateTrainerWithPokemon/{trainerid}/{pokemonid}")]
        public IHttpActionResult AssociateTrainerWithPokemon (int trainerid, int pokemonid)
        {

            Trainer SelectedTrainer = db.Trainers.Include(t=>t.Pokemons).Where(t=>t.TrainerId == trainerid).FirstOrDefault();
            Pokemon SelectedPokemon = db.Pokemons.Find(pokemonid);


            //In case of error
            if(SelectedTrainer == null || SelectedPokemon == null)
            {
                return NotFound();
            }


            SelectedTrainer.Pokemons.Add(SelectedPokemon);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Removes an association between a particular trainer and a particular pokemon
        /// </summary>
        /// <param name="trainerid">The trainer's primary id key</param>
        /// <param name="pokemonid">The pokemon's primary id key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 400 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/TrainerData/UnassociateTrainerWithPokemon/1/17
        /// </example>
        [HttpPost]
        [Route("api/trainerdata/UnassociateTrainerWithPokemon/{trainerid}/{pokemonid}")]
        public IHttpActionResult UnassociateTrainerWithPokemon(int trainerid, int pokemonid)
        {

            Trainer SelectedTrainer = db.Trainers.Include(t => t.Pokemons).Where(t => t.TrainerId == trainerid).FirstOrDefault();
            Pokemon SelectedPokemon = db.Pokemons.Find(pokemonid);


            //In case of error
            if (SelectedTrainer == null || SelectedPokemon == null)
            {
                return NotFound();
            }


            SelectedTrainer.Pokemons.Remove(SelectedPokemon);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all trainers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An trainer in the system that corresponds to the provided primary key. 
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Trainer</param>
        /// <example>
        /// GET: api/TrainerData/FindTrainer/5
        /// </example>
        [ResponseType(typeof(Trainer))]
        [HttpGet]
        public IHttpActionResult FindTrainer(int id)
        {
            Trainer Trainer = db.Trainers.Find(id);
            TrainerDto TrainerDto = new TrainerDto()
            {
                TrainerId = Trainer.TrainerId,
                TrainerName = Trainer.TrainerName,
                TrainerTitle = Trainer.TrainerTitle,
                TrainerBio = Trainer.TrainerBio

            };

            if (Trainer == null)
            {
                return NotFound();
            }

            return Ok(TrainerDto);
        }


        /// <summary>
        /// Updates a specified trainer in the system with a POST Data input
        /// </summary>
        /// <param name="id">Represents the Trainers primary key id</param>
        /// <param name="trainer">JSON FORM DATA of an Trainer</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TrainerData/UpdateTrainer/5
        /// FORM DATA: Trainer JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTrainer(int id, Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trainer.TrainerId)
            {
                return BadRequest();
            }

            db.Entry(trainer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainerExists(id))
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
        /// Adds a new Trainer to the system
        /// </summary>
        /// <param name="trainer">JSON FORM DATA of a Trainer</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Trainer ID, Trainer Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/TrainerData/AddTrainer
        /// FORM DATA: Trainer JSON Object
        /// </example>
        [ResponseType(typeof(Trainer))]
        [HttpPost]
        public IHttpActionResult AddTrainer(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trainers.Add(trainer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trainer.TrainerId }, trainer);
        }

        /// <summary>
        /// Deletes an Trainer from the system by a provided id.
        /// </summary>
        /// <param name="id">A Trainers Primary Key Id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/TrainerData/DeleteTrainer/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Trainer))]
        [HttpPost]
        public IHttpActionResult DeleteTrainer(int id)
        {
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return NotFound();
            }

            db.Trainers.Remove(trainer);
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

        private bool TrainerExists(int id)
        {
            return db.Trainers.Count(e => e.TrainerId == id) > 0;
        }
    }
}