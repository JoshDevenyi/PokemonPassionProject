namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pokemontrainers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        TrainerId = c.Int(nullable: false, identity: true),
                        TrainerName = c.String(),
                    })
                .PrimaryKey(t => t.TrainerId);
            
            CreateTable(
                "dbo.TrainerPokemons",
                c => new
                    {
                        Trainer_TrainerId = c.Int(nullable: false),
                        Pokemon_PokemonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trainer_TrainerId, t.Pokemon_PokemonId })
                .ForeignKey("dbo.Trainers", t => t.Trainer_TrainerId, cascadeDelete: true)
                .ForeignKey("dbo.Pokemons", t => t.Pokemon_PokemonId, cascadeDelete: true)
                .Index(t => t.Trainer_TrainerId)
                .Index(t => t.Pokemon_PokemonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerPokemons", "Pokemon_PokemonId", "dbo.Pokemons");
            DropForeignKey("dbo.TrainerPokemons", "Trainer_TrainerId", "dbo.Trainers");
            DropIndex("dbo.TrainerPokemons", new[] { "Pokemon_PokemonId" });
            DropIndex("dbo.TrainerPokemons", new[] { "Trainer_TrainerId" });
            DropTable("dbo.TrainerPokemons");
            DropTable("dbo.Trainers");
        }
    }
}
