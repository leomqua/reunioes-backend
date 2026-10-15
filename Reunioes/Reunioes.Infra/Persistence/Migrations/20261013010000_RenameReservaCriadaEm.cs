using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reunioes.Infra.Persistence.Migrations;

public partial class RenameReservaCriadaEm : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "CriadaEmUtc",
            table: "Reservas",
            newName: "CriadaEm");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "CriadaEm",
            table: "Reservas",
            newName: "CriadaEmUtc");
    }
}
