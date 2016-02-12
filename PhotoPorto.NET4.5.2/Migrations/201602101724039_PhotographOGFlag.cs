namespace PhotoPorto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotographOGFlag : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Photograph", "og", c => c.Boolean(defaultValue : false));
        }
        
        public override void Down()
        {
        }
    }
}
