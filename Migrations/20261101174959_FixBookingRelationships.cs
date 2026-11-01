using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Screens_ScreenId1",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_ScreenId1",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "ScreenId1",
                table: "Shows");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BookingSeats",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeats_BookingId",
                table: "BookingSeats",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeats_SeatId",
                table: "BookingSeats",
                column: "SeatId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BookingSeats_Price",
                table: "BookingSeats",
                sql: "[Price] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingSeats_Bookings_BookingId",
                table: "BookingSeats",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingSeats_Seats_SeatId",
                table: "BookingSeats",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "SeatId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingSeats_Bookings_BookingId",
                table: "BookingSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingSeats_Seats_SeatId",
                table: "BookingSeats");

            migrationBuilder.DropIndex(
                name: "IX_BookingSeats_BookingId",
                table: "BookingSeats");

            migrationBuilder.DropIndex(
                name: "IX_BookingSeats_SeatId",
                table: "BookingSeats");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BookingSeats_Price",
                table: "BookingSeats");

            migrationBuilder.AddColumn<int>(
                name: "ScreenId1",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BookingSeats",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ScreenId1",
                table: "Shows",
                column: "ScreenId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Screens_ScreenId1",
                table: "Shows",
                column: "ScreenId1",
                principalTable: "Screens",
                principalColumn: "ScreenId");
        }
    }
}
