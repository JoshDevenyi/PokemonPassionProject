namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class types : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.TypeId);
            
            AddColumn("dbo.Pokemons", "PokedexNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pokemons", "PokedexNumber");
            DropTable("dbo.Types");
        }
    }
}
