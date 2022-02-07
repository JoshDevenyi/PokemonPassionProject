namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trainertitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainers", "TrainerTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainers", "TrainerTitle");
        }
    }
}
