using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoshDevenyi_PokemonPassionProject.Models.ViewModels
{
    public class DetailsPokemonType
    {

        public PokemonTypeDto SelectedPokemonType { get; set; }

        public IEnumerable<PokemonDto> RelatedPokemons { get; set; }

    }
}