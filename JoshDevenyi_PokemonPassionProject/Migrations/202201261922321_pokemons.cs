namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pokemons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pokemons",
                c => new
                    {
                        PokemonId = c.Int(nullable: false, identity: true),
                        PokemonName = c.String(),
                    })
                .PrimaryKey(t => t.PokemonId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pokemons");
        }
    }
}
