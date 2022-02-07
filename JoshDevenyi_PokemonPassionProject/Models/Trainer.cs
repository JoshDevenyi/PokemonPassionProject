using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoshDevenyi_PokemonPassionProject.Models
{
    public class Trainer
    {   

        [Key]
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string TrainerTitle{ get; set; }
        public string TrainerBio { get; set; }


        //A Trainer can have caught many Pokemon
        public ICollection<Pokemon> Pokemons { get; set; }
    }

    public class TrainerDto
    {

        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string TrainerTitle { get; set; }
        public string TrainerBio { get; set; }

    }


}