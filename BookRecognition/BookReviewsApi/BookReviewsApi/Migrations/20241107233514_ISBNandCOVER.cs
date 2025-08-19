using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewsApi.Migrations
{
    /// <inheritdoc />
    public partial class ISBNandCOVER : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "BookReviews");

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                table: "BookReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "BookReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "BookReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverUrl",
                table: "BookReviews");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "BookReviews");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "BookReviews");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "BookReviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
