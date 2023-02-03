using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TUF.Database.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TblCommonCode",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "TblCommonCode",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                table: "TblCommonCode",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bkey",
                table: "TblBoardComment",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "TblBoardComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "TblBoardComment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Depth",
                table: "TblBoardComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrpNo",
                table: "TblBoardComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrpOrd",
                table: "TblBoardComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UseYn",
                table: "TblBoardComment",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIpAddr",
                table: "TblBoardComment",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bkey",
                table: "TblBoard",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BoardPassword",
                table: "TblBoard",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contents",
                table: "TblBoard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentsHtml",
                table: "TblBoard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReadCount",
                table: "TblBoard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "TblBoard",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UseYn",
                table: "TblBoard",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserIpAddr",
                table: "TblBoard",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TblBoardInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Bkey = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BoardName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BoardDesc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EditorYn = table.Column<bool>(type: "bit", nullable: true),
                    CommentYn = table.Column<bool>(type: "bit", nullable: true),
                    ImageYn = table.Column<bool>(type: "bit", nullable: true),
                    Expiredate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UseYn = table.Column<bool>(type: "bit", nullable: true),
                    sort = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblBoardInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblImageInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bkey = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageSize = table.Column<int>(type: "int", nullable: true),
                    SortNum = table.Column<int>(type: "int", nullable: true),
                    ContainerName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ExprireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UseYn = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblImageInfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblBoardInfo");

            migrationBuilder.DropTable(
                name: "TblImageInfo");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "TblCommonCode");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "TblCommonCode");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                table: "TblCommonCode");

            migrationBuilder.DropColumn(
                name: "Bkey",
                table: "TblBoardComment");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "TblBoardComment");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "TblBoardComment");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "TblBoardComment");

            migrationBuilder.DropColumn(
                name: "GrpNo",
                table: "TblBoardComment");

            migrationBuilder.DropColumn(
                name: "GrpOrd",
                table: "TblBoardComment");

            migrationBuilder.DropColumn(
                name: "UseYn",
                table: "TblBoardComment");

            migrationBuilder.DropColumn(
                name: "UserIpAddr",
                table: "TblBoardComment");

            migrationBuilder.DropColumn(
                name: "Bkey",
                table: "TblBoard");

            migrationBuilder.DropColumn(
                name: "BoardPassword",
                table: "TblBoard");

            migrationBuilder.DropColumn(
                name: "Contents",
                table: "TblBoard");

            migrationBuilder.DropColumn(
                name: "ContentsHtml",
                table: "TblBoard");

            migrationBuilder.DropColumn(
                name: "ReadCount",
                table: "TblBoard");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "TblBoard");

            migrationBuilder.DropColumn(
                name: "UseYn",
                table: "TblBoard");

            migrationBuilder.DropColumn(
                name: "UserIpAddr",
                table: "TblBoard");
        }
    }
}
