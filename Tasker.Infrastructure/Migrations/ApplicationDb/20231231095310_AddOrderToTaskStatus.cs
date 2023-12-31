using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Infrastructure.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class AddOrderToTaskStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {           
             migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "TaskStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);      
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.DropColumn(
                name: "Order",
                table: "TaskStatuses");        
        }
    }
}
