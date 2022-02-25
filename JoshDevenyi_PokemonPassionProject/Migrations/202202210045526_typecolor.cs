namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typecolor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PokemonTypes", "PokemonTypeColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PokemonTypes", "PokemonTypeColor");
        }
    }
}
