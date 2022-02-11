using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using JoshDevenyi_PokemonPassionProject.Models;
using JoshDevenyi_PokemonPassionProject.Models.ViewModels;
using System.Web.Script.Serialization;


namespace JoshDevenyi_PokemonPassionProject.Controllers
{
    public class TrainerController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer(); //Allows for conversion into JSON

        static TrainerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44393/api/"); //Set Base Address For File Paths
        }

        // GET: Trainer/List
        public ActionResult List()
        {
            //curl https://localhost:44393/api/trainerdata/listtrainers

            string url = "trainerdata/listtrainers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TrainerDto> trainers = response.Content.ReadAsAsync<IEnumerable<TrainerDto>>().Result;

            return View(trainers);
        }

        // GET: Trainer/Details/5
        //curl https://localhost:44393/api/trainerdata/findtrainer/{id}
        public ActionResult Details(int id)
        {
            DetailsTrainer ViewModel = new DetailsTrainer();

            string url = "trainerdata/findtrainer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            TrainerDto SelectedTrainer = response.Content.ReadAsAsync<TrainerDto>().Result;

            Debug.WriteLine("Trainer received: ");
            Debug.WriteLine(SelectedTrainer.TrainerName);

            ViewModel.SelectedTrainer = SelectedTrainer;

            

            //show all pokemon species caught by this trainer
            url = "pokemondata/listpokemonsfortrainer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PokemonDto> CaughtPokemons = response.Content.ReadAsAsync<IEnumerable<PokemonDto>>().Result;

            ViewModel.CaughtPokemons = CaughtPokemons;


            //List all pokemon the trainer has yet to catch
            url = "pokemondata/listpokemonsnotcaughtfortrainer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PokemonDto> RemainingPokemons = response.Content.ReadAsAsync<IEnumerable<PokemonDto>>().Result;

            ViewModel.RemainingPokemons = RemainingPokemons;


            return View(ViewModel);

        }

        // GET: Trainer/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Trainer/Create
        [HttpPost]
        public ActionResult Create(Trainer trainer)
        {
            //Debug.WriteLine("the JSON payload is:");

            //objective: add a new trainer into our system using the API
            //curl -H "Content-Type:application/json" -d @trainer.json https://localhost:44393/api/trainerdata/addtrainer
            string url = "trainerdata/addtrainer";

            //Converting form data into JSON object
            string jsonpayload = jss.Serialize(trainer);

           //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload); //Convert payload to string content for sending
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload

            //Sending Javascript Payload to URL through the client
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //Checking that response was successful
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Trainer/Edit/5
        public ActionResult Edit(int id)
        {
            //The existing trainer information
            string url = "trainerdata/findtrainer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TrainerDto SelectedTrainer = response.Content.ReadAsAsync<TrainerDto>().Result;
            return View(SelectedTrainer);
        }

        // POST: Trainer/Update/5
        [HttpPost]
        public ActionResult Update(int id, Trainer trainer)
        {
            //Debug.WriteLine("the JSON payload is:");

            //objective: update the details of a trainer already in our system
            //curl -H "Content-Type:application/json" -d @trainer.json https://localhost:44393/api/trainerdata/updatetrainer/{id}
            string url = "trainerdata/updatetrainer/" + id;

            //Converting form data into JSON object
            string jsonpayload = jss.Serialize(trainer);

            //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload); //Convert payload to string content for sending
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload

            //Sending Javascript Payload to URL through the client
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //Console.WriteLine(content);

            //Checking that response was successful
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Trainer/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            //The existing trainer information
            string url = "trainerdata/findtrainer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TrainerDto SelectedTrainer = response.Content.ReadAsAsync<TrainerDto>().Result;
            return View(SelectedTrainer);
        }

        // POST: Trainer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "trainerdata/deletetrainer/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //Checking that response was successful
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //POST: Trainer/Associate/{trainerid}
        [HttpPost]
        public ActionResult Associate(int id, int PokemonId)
        {

            Debug.WriteLine("Attempting to associate trainer: " + id + " with pokemon " + PokemonId);

            //call our api to associate trainer with pokemon
            string url = "trainerdata/associatetrainerwithpokemon/" + id + "/" + PokemonId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("details/" + id);
        }


        //GET: Trainer/Associate/{id}?PokemonId={pokemonid}
        [HttpGet]
        public ActionResult Unassociate(int id, int PokemonId)
        {

            Debug.WriteLine("Attempting to unassociate trainer: " + id + " with pokemon " + PokemonId);

            //call our api to associate trainer with pokemon
            string url = "trainerdata/unassociatetrainerwithpokemon/" + id + "/" + PokemonId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("details/" + id);
        }


        public ActionResult Error()
        {
            return View();
        }

    }
}
