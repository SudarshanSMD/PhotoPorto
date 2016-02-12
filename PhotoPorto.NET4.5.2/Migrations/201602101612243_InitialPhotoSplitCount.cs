namespace PhotoPorto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialPhotoSplitCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photograph", "SplitColumnCount", c => c.Int());
            AddColumn("dbo.Photograph", "SplitRowCount", c => c.Int());
        }
        
        public override void Down()
        {
        }
    }
}
