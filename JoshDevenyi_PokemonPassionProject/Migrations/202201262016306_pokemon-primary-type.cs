namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pokemonprimarytype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pokemons", "PrimaryTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pokemons", "PrimaryTypeId");
            AddForeignKey("dbo.Pokemons", "PrimaryTypeId", "dbo.Types", "TypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pokemons", "PrimaryTypeId", "dbo.Types");
            DropIndex("dbo.Pokemons", new[] { "PrimaryTypeId" });
            DropColumn("dbo.Pokemons", "PrimaryTypeId");
        }
    }
}
