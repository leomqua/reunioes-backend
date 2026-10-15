using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reunioes.Infra.Persistence.Migrations;

public partial class UseDateTimeWithoutTimeZone : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            ALTER TABLE "Reservas"
                ALTER COLUMN "CriadaEm" DROP DEFAULT;

            ALTER TABLE "Reservas"
                ALTER COLUMN "Inicio" TYPE timestamp without time zone
                    USING "Inicio" AT TIME ZONE 'America/Sao_Paulo',
                ALTER COLUMN "Fim" TYPE timestamp without time zone
                    USING "Fim" AT TIME ZONE 'America/Sao_Paulo',
                ALTER COLUMN "CriadaEm" TYPE timestamp without time zone
                    USING "CriadaEm" AT TIME ZONE 'America/Sao_Paulo',
                ALTER COLUMN "CriadaEm" SET DEFAULT LOCALTIMESTAMP;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            ALTER TABLE "Reservas"
                ALTER COLUMN "CriadaEm" DROP DEFAULT;

            ALTER TABLE "Reservas"
                ALTER COLUMN "Inicio" TYPE timestamp with time zone
                    USING "Inicio" AT TIME ZONE 'America/Sao_Paulo',
                ALTER COLUMN "Fim" TYPE timestamp with time zone
                    USING "Fim" AT TIME ZONE 'America/Sao_Paulo',
                ALTER COLUMN "CriadaEm" TYPE timestamp with time zone
                    USING "CriadaEm" AT TIME ZONE 'America/Sao_Paulo',
                ALTER COLUMN "CriadaEm" SET DEFAULT CURRENT_TIMESTAMP;
            """);
    }
}
