using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoshDevenyi_PokemonPassionProject.Models.ViewModels
{
    public class DetailsTrainer
    {

        public TrainerDto SelectedTrainer { get; set; }
        public IEnumerable<PokemonDto> CaughtPokemons { get; set; }

        public IEnumerable<PokemonDto> RemainingPokemons { get; set; }
    }
}