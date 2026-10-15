CREATE TABLE "Salas" (
    "Id" uuid NOT NULL,
    "Nome" character varying(150) NOT NULL,
    "Andar" integer NOT NULL,
    "QuantidadeAssentos" integer NOT NULL,
    CONSTRAINT "PK_Salas" PRIMARY KEY ("Id")
);

CREATE TABLE "Reservas" (
    "Id" uuid NOT NULL,
    "Inicio" timestamp without time zone NOT NULL,
    "Fim" timestamp without time zone NOT NULL,
    "CriadaEm" timestamp without time zone NOT NULL DEFAULT LOCALTIMESTAMP,
    "SalaId" uuid NOT NULL,
    CONSTRAINT "PK_Reservas" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Reservas_Salas_SalaId"
        FOREIGN KEY ("SalaId") REFERENCES "Salas" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_Reservas_SalaId_Inicio_Fim"
    ON "Reservas" ("SalaId", "Inicio", "Fim");
