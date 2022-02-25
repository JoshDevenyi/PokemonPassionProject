namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingpokemonimagecollumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pokemons", "PokemonHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pokemons", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pokemons", "PicExtension");
            DropColumn("dbo.Pokemons", "PokemonHasPic");
        }
    }
}
