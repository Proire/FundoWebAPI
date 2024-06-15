using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRLL.Migrations
{
    /// <inheritdoc />
    public partial class AddCollaborator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collaboraters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollaboratorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaboraters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collaboraters_Notes_NoteEntityId",
                        column: x => x.NoteEntityId,
                        principalTable: "Notes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaboraters_NoteEntityId",
                table: "Collaboraters",
                column: "NoteEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collaboraters");
        }
    }
}
