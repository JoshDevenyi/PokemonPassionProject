using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoshDevenyi_PokemonPassionProject.Models.ViewModels
{
    public class DetailsPokemon
    {

        public PokemonDto SelectedPokemon { get; set; }
        public IEnumerable<TrainerDto> SuccessfulTrainers { get; set; }


    }
}