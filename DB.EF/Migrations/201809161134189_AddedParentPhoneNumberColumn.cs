namespace DB.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedParentPhoneNumberColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pacients", "ParentPhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pacients", "ParentPhoneNumber");
        }
    }
}
