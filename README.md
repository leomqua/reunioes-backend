# Reunioes Backend

API REST para cadastro de salas de reunião, reservas e consulta de disponibilidade. A solução usa .NET 8, ASP.NET Core, Entity Framework Core 9 e PostgreSQL.

## Requisitos

- .NET 8 SDK
- PostgreSQL
- Visual Studio 2022 ou terminal PowerShell

## Configuração e execução

Configure a connection string `DefaultConnection` da API. Em PowerShell, é possível fornecê-la sem gravar credenciais nos arquivos do projeto:

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=reunioes_db;Username=postgres;Password=SUA_SENHA"
```

Na pasta `Reunioes`, aplique as migrations e inicie a API:

```powershell
dotnet tool install --global dotnet-ef --version 9.0.20
dotnet ef database update --project .\Reunioes.Infra --startup-project .\Reunioes.API
dotnet run --project .\Reunioes.API
```

Instale a ferramenta `dotnet-ef` somente uma vez e pule essa linha se ela já estiver instalada. No Visual Studio, a migration pendente pode ser aplicada pelo Package Manager Console:

```powershell
Update-Database -Project Reunioes.Infra -StartupProject Reunioes.API
```

O perfil local usa `http://localhost:5242`; durante o desenvolvimento, o Swagger fica disponível em `http://localhost:5242/swagger`. O script SQL completo para criar o esquema do zero está em [`Reunioes/Database/schema.sql`](./Reunioes/Database/schema.sql).

## Endpoints

### Salas

- `POST /api/salas` — cria uma sala (nome, andar e quantidade de assentos).
- `GET /api/salas/{id}` — consulta uma sala.
- `GET /api/salas?nome={filtro}&pagina=1` — lista salas ordenadas por andar e nome, 10 por página.
- `PUT /api/salas/{id}` — atualiza os dados da sala.
- `DELETE /api/salas/{id}` — exclui a sala; retorna conflito se houver reservas associadas.

Não é permitido cadastrar ou atualizar duas salas com o mesmo nome no mesmo andar (a comparação do nome ignora maiúsculas, minúsculas e espaços externos). O mesmo nome pode ser usado em andares diferentes, e um andar pode ter várias salas com nomes diferentes. Uma duplicidade retorna `409 Conflict`.

### Reservas

- `POST /api/reservas` — cria uma reserva para uma sala existente.
- `GET /api/reservas` — lista todas as reservas em ordem de início; não é necessário informar o GUID.
- `GET /api/reservas/{id}` — consulta uma reserva.
- `PUT /api/reservas/{id}` — reagenda uma reserva futura, podendo alterar a sala.
- `DELETE /api/reservas/{id}` — cancela uma reserva.

Para criar ou reagendar uma reserva, envie a data e os horários em campos separados: `"data": "2030-05-15"`, `"horaInicio": "09:00"` e `"horaFim": "10:00"`. A data usa `yyyy-MM-dd` e os horários usam `HH:mm`. O expediente é das **08:00 às 19:00**, e início e fim devem estar no mesmo dia. Reservas não podem se sobrepor na mesma sala. Reservas adjacentes são permitidas (uma pode terminar no instante em que a seguinte começa). Somente reservas futuras podem ser reagendadas.

### Relatório de disponibilidade

- `GET /api/relatorios/salas/disponibilidade` — retorna cada sala e calcula as horas livres para hoje, considerando o expediente completo das 08:00 às 19:00. Também informa quantas reservas foram criadas nos últimos sete dias.

Somente reservas que se sobrepõem ao expediente de hoje reduzem as horas livres. A contagem de reservas recentes é independente da disponibilidade de hoje.
<<<<<<< HEAD
=======

## Testes manuais

O arquivo [`Reunioes/Reunioes.API/Reunioes.API.http`](./Reunioes/Reunioes.API/Reunioes.API.http) contém exemplos de chamadas para todos os endpoints. Atualize os IDs de sala e reserva conforme as respostas de criação.
>>>>>>> 358fa0f (implementa métodos de salas e reservas e adiciona migrations)
