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

        // GET: api/TrainerData/ListTrainers
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

        // GET: api/TrainerData/FindTrainer/5
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


        // POST: api/TrainerData/UpdateTrainer/5
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

        // POST: api/TrainerData/AddTrainer
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

        // POST: api/TrainerData/DeleteTrainer/5
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