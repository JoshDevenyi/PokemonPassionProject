using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JoshDevenyi_PokemonPassionProject.Models
{
    public class PokemonType
    {
        [Key]
        public int PokemonTypeId { get; set; }
        public string PokemonTypeName { get; set; }

        public string PokemonTypeColor { get; set; }

        //A Pokemon Type can belong to many Pokemon
        public ICollection<Pokemon> Pokemons { get; set; }
    }

    public class PokemonTypeDto
    {
        public int PokemonTypeId { get; set; }
        public string PokemonTypeName { get; set; }
        public string PokemonTypeColor { get; set; }

    }
}