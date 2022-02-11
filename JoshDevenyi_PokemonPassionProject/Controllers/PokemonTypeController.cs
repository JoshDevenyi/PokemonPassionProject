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
    public class PokemonTypeController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer(); //Allows for conversion into JSON

        static PokemonTypeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44393/api/"); //Set Base Address For File Paths
        }

        // GET: PokemonType
        public ActionResult List()
        {
            //curl https://localhost:44393/api/pokemontypedata/listpokemontypes

            string url = "pokemontypedata/listpokemontypes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PokemonTypeDto> pokemontypes = response.Content.ReadAsAsync<IEnumerable<PokemonTypeDto>>().Result;

            return View(pokemontypes);
        }

        // GET: PokemonType/Details/5
        //curl https://localhost:44393/api/pokemontypedata/findpokemontype/{id}
        public ActionResult Details(int id)
        {

            DetailsPokemonType ViewModel = new DetailsPokemonType();

            //information about the pokemon type itself

            string url = "pokemontypedata/findpokemontype/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("1. The response code is ");
            //Debug.WriteLine(response.StatusCode);

            PokemonTypeDto SelectedPokemonType = response.Content.ReadAsAsync<PokemonTypeDto>().Result;

            //Debug.WriteLine("Type received: ");
            //Debug.WriteLine(SelectedPokemonType.PokemonTypeName);

            ViewModel.SelectedPokemonType = SelectedPokemonType;


            //Showcase information about pokemon related to this type
            //Send a request to gather information about pokemon related to a particualr pokemon type ID

            url = "pokemondata/listpokemonsforpokemontype/" + id;
            response = client.GetAsync(url).Result;

            //Check to make sure we successful connected to data controller
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PokemonDto> RelatedPokemons = response.Content.ReadAsAsync<IEnumerable<PokemonDto>>().Result;

            foreach (var pokemon in RelatedPokemons)
            {
                Debug.WriteLine("Pokemons received : ");
                Debug.WriteLine(pokemon.PokemonName);
            }
           
            ViewModel.RelatedPokemons = RelatedPokemons;

            return View(ViewModel);
        }

        // GET: PokemonType/New
        public ActionResult New()
        {
            return View();
        }

        // POST: PokemonType/Create
        [HttpPost]
        public ActionResult Create(PokemonType pokemonType)
        {
            //curl -H "Content-Type:application/json" -d @pokemontype.json https://localhost:44393/api/pokemontypedata/addpokemontype
            string url = "pokemontypedata/addpokemontype";

            //Converting form data into JSON object
            string jsonpayload = jss.Serialize(pokemonType);

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

        // GET: PokemonType/Edit/5
        public ActionResult Edit(int id)
        {
            //The existing type information
            string url = "pokemontypedata/findpokemontype/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PokemonTypeDto SelectedPokemonType = response.Content.ReadAsAsync<PokemonTypeDto>().Result;
            return View(SelectedPokemonType);
        }

        // POST: PokemonType/Update/5
        [HttpPost]
        public ActionResult Update(int id, PokemonType pokemonType)
        {

            //objective: update the details of a pokemon type already in our system
            //curl -H "Content-Type:application/json" -d @pokemontype.json https://localhost:44393/api/pokemontype/updatepokemontype/{id}
            string url = "pokemontypedata/updatepokemontype/" + id;

            //Converting form data into JSON object
            string jsonpayload = jss.Serialize(pokemonType);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload); //Convert payload to string content for sending
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload

            //Sending Javascript Payload to URL through the client
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Console.WriteLine(content);

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

        // GET: PokemonType/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            //The existing pokemon type information
            string url = "pokemontypedata/findpokemontype/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PokemonTypeDto SelectedPokemonType = response.Content.ReadAsAsync<PokemonTypeDto>().Result;
            return View(SelectedPokemonType);
        }

        // POST: PokemonType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                string url = "pokemontypedata/deletepokemontype/" + id;

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
            catch
            {
                return View();
            }
        }
    }
}
