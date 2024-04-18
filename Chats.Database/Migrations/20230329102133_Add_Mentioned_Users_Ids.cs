using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chats.Database.Migrations
{
    public partial class Add_Mentioned_Users_Ids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<IEnumerable<Guid>>(
                name: "MentionedUsersIds",
                table: "Message",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MentionedUsersIds",
                table: "Message");
        }
    }
}
