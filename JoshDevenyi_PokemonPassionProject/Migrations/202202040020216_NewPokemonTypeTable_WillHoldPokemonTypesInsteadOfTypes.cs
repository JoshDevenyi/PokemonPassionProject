namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewPokemonTypeTable_WillHoldPokemonTypesInsteadOfTypes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pokemons", "PrimaryTypeId", "dbo.Types");
            DropIndex("dbo.Pokemons", new[] { "PrimaryTypeId" });
            CreateTable(
                "dbo.PokemonTypes",
                c => new
                    {
                        PokemonTypeId = c.Int(nullable: false, identity: true),
                        PokemonTypeName = c.String(),
                    })
                .PrimaryKey(t => t.PokemonTypeId);
            
            AddColumn("dbo.Pokemons", "PokemonTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pokemons", "PokemonTypeId");
            AddForeignKey("dbo.Pokemons", "PokemonTypeId", "dbo.PokemonTypes", "PokemonTypeId", cascadeDelete: true);
            DropColumn("dbo.Pokemons", "PrimaryTypeId");
            DropTable("dbo.Types");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PokeTypes",
                c => new
                    {
                        PokeTypeId = c.Int(nullable: false, identity: true),
                        PokeTypeName = c.String(),
                    })
                .PrimaryKey(t => t.PokeTypeId);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.TypeId);
            
            AddColumn("dbo.Pokemons", "PrimaryTypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Pokemons", "PokemonTypeId", "dbo.PokemonTypes");
            DropIndex("dbo.Pokemons", new[] { "PokemonTypeId" });
            DropColumn("dbo.Pokemons", "PokemonTypeId");
            DropTable("dbo.PokemonTypes");
            CreateIndex("dbo.Pokemons", "PrimaryTypeId");
            AddForeignKey("dbo.Pokemons", "PrimaryTypeId", "dbo.Types", "TypeId", cascadeDelete: true);
        }
    }
}
