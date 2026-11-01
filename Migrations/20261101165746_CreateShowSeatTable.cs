using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class CreateShowSeatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScreenId1",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShowSeats",
                columns: table => new
                {
                    ShowSeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowSeats", x => x.ShowSeatId);
                    table.CheckConstraint("CK_ShowSeats_Status", "[Status] IN ('Available', 'Booked')");
                    table.ForeignKey(
                        name: "FK_ShowSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "SeatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShowSeats_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ScreenId1",
                table: "Shows",
                column: "ScreenId1");

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeats_SeatId",
                table: "ShowSeats",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeats_ShowId_SeatId",
                table: "ShowSeats",
                columns: new[] { "ShowId", "SeatId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Screens_ScreenId1",
                table: "Shows",
                column: "ScreenId1",
                principalTable: "Screens",
                principalColumn: "ScreenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Screens_ScreenId1",
                table: "Shows");

            migrationBuilder.DropTable(
                name: "ShowSeats");

            migrationBuilder.DropIndex(
                name: "IX_Shows_ScreenId1",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "ScreenId1",
                table: "Shows");
        }
    }
}
