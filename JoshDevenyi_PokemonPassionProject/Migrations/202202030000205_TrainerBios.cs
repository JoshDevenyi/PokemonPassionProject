namespace JoshDevenyi_PokemonPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrainerBios : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainers", "TrainerBio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainers", "TrainerBio");
        }
    }
}
