using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoshDevenyi_PokemonPassionProject.Models
{
    public class Pokemon
    {
        [Key]
        public int PokemonId { get; set; }
        public string PokemonName { get; set; }

        //Adding Pokedex number
        public int PokedexNumber { get; set; }

        //Data required for keeping track of pokemon images uploaded
        //Images will be deposited in /Content/Images/Pokemon/{id}.{extension}
        public bool PokemonHasPic { get; set; }
        public string PicExtension { get; set; }

        [ForeignKey("PokemonType")]
        //Primary Pokemon Type
        //A pokemon has one primary type. A type can belong to many pokemon.
        public int PokemonTypeId { get; set; }
        public virtual PokemonType PokemonType { get; set; }

        //A Pokemon can be caught by many trainers
        public ICollection<Trainer> Trainers { get; set; }

        //To Do: Add Other Stats

    }

    public class PokemonDto {

        public int PokemonId { get; set; }
        public string PokemonName { get; set; }
        public int PokedexNumber { get; set; }
        public int PokemonTypeId { get; set; }
        public string PokemonType { get; set; }
        public string PokemonTypeColor { get; set; }

        //Data required for keeping track of pokemon images uploaded
        //Images will be deposited in /Content/Images/Pokemon/{id}.{extension}
        public bool PokemonHasPic { get; set; }
        public string PicExtension { get; set; }

    }

}