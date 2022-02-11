using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoshDevenyi_PokemonPassionProject.Models.ViewModels
{
    public class UpdatePokemon
    {

        //This class will be used to store information that will be presented to /Pokemon/Update/{}

        //The existing pokemon information

        public PokemonDto SelectedPokemon { get; set; }

        //also like to view all types to choose from when updating this pokemon

        public IEnumerable<PokemonTypeDto> PokemonTypeOptions { get; set; }

    }
}