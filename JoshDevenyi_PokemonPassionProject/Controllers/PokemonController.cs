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
    public class PokemonController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer(); //Allows for conversion into JSON


        static PokemonController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44393/api/"); //Set Base Address For File Paths
        }


        // GET: Pokemon/List
        public ActionResult List()
        {
            //objective: communicate with pokemon data api to retrieve a list of pokemon
            //curl https://localhost:44393/api/pokemondata/listpokemons

            string url = "pokemondata/listpokemons";
            HttpResponseMessage response = client.GetAsync(url).Result; //Responce we are anticipating

            //Check to make sure we successful connected to data controller
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);


            //Parses the content of the responce method into an IEnumerable
            IEnumerable<PokemonDto> pokemons = response.Content.ReadAsAsync<IEnumerable<PokemonDto>>().Result;
            //Debug.WriteLine("Number of Pokemon received ");
            //Debug.WriteLine(pokemons.Count());

            return View(pokemons);
        }

        // GET: Pokemon/Details/5
        public ActionResult Details(int id)
        {

            DetailsPokemon ViewModel = new DetailsPokemon();

            //objective: communicate with pokemon data api to one pokemon
            //curl https://localhost:44393/api/pokemondata/listpokemons/{id}

            string url = "pokemondata/findpokemon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result; //Responce we are anticipating

            //Check to make sure we successful connected to data controller
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            //Parses the content of the responce method into an IEnumerable
            PokemonDto SelectedPokemon = response.Content.ReadAsAsync<PokemonDto>().Result;
            //Debug.WriteLine("Pokemon received: ");
            //Debug.WriteLine(selectedPokemon.PokemonName);

            ViewModel.SelectedPokemon = SelectedPokemon;

            //Show trainers who have caught this pokemon
            url = "trainerdata/listtrainersforpokemon/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TrainerDto> SuccessfulTrainers = response.Content.ReadAsAsync<IEnumerable<TrainerDto>>().Result;

            ViewModel.SuccessfulTrainers = SuccessfulTrainers;

            return View(ViewModel);
        }

        // GET: Pokemon/New
        public ActionResult New()
        {
            //information about all types in the system
            //Get api/pokemontypedata/listpokemontypes

            string url = "pokemontypedata/listpokemontypes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PokemonTypeDto> PokemonTypeOptions = response.Content.ReadAsAsync<IEnumerable<PokemonTypeDto>>().Result;

            return View(PokemonTypeOptions);

        }

        // POST: Pokemon/Create
        [HttpPost]
        public ActionResult Create(Pokemon pokemon)
        {
            //Debug.WriteLine("the JSON payload is:");
            //Debug.WriteLine(pokemon.PokemonName);

            //objective: add a new pokemon into our system using the API
            //curl -H "Content-Type:application/json" -d @pokemon.json https://localhost:44393/api/pokemondata/addpokemon
            string url = "pokemondata/addpokemon";

            //Converting form data into JSON object

            string jsonpayload = jss.Serialize(pokemon);

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

        // GET: Pokemon/Edit/5
        public ActionResult Edit(int id)
        {
            UpdatePokemon ViewModel = new UpdatePokemon();


            //The existing pokemon information
            string url = "pokemondata/findpokemon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PokemonDto SelectedPokemon = response.Content.ReadAsAsync<PokemonDto>().Result;
            ViewModel.SelectedPokemon = SelectedPokemon;

            //also like to view all types to choose from when updating this pokemon
            url = "pokemontypedata/listpokemontypes/";
            response = client.GetAsync(url).Result;
            IEnumerable<PokemonTypeDto> PokemonTypeOptions = response.Content.ReadAsAsync<IEnumerable<PokemonTypeDto>>().Result;


            ViewModel.PokemonTypeOptions = PokemonTypeOptions;

            return View(ViewModel);
        }

        // POST: Pokemon/Update/5
        [HttpPost]
        public ActionResult Update(int id, Pokemon pokemon)
        {

            //objective: update the details of a pokemon already in our system
            //curl -H "Content-Type:application/json" -d @pokemon.json https://localhost:44393/api/pokemondata/updatepokemon/{id}
            string url = "pokemondata/updatepokemon/" + id;

            //Converting form data into JSON object
            string jsonpayload = jss.Serialize(pokemon);

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

        // GET: Pokemon/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            //The existing pokemon information
            string url = "pokemondata/findpokemon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PokemonDto SelectedPokemon = response.Content.ReadAsAsync<PokemonDto>().Result;
            return View(SelectedPokemon);
        }

        // POST: Pokemon/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            string url = "pokemondata/deletepokemon/" + id;

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

        public ActionResult Error()
        {
            return View();
        }

    }
}
