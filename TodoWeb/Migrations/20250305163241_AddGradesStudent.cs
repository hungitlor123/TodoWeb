using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddGradesStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "CourseStudent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseStudent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.CreateTable(
                name: "StudentGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AssignmentScore = table.Column<double>(type: "float", nullable: false),
                    PracticalScore = table.Column<double>(type: "float", nullable: false),
                    FinalScore = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGrades_CourseStudent_CourseId_StudentId",
                        columns: x => new { x.CourseId, x.StudentId },
                        principalTable: "CourseStudent",
                        principalColumns: new[] { "CourseId", "StudentId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_CourseId_StudentId",
                table: "StudentGrades",
                columns: new[] { "CourseId", "StudentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentGrades");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "CourseStudent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseStudent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);
        }
    }
}
