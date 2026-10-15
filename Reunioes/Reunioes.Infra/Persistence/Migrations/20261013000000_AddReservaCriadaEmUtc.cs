using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reunioes.Infra.Persistence.Migrations;

public partial class AddReservaCriadaEmUtc : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "CriadaEmUtc",
            table: "Reservas",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CriadaEmUtc",
            table: "Reservas");
    }
}
