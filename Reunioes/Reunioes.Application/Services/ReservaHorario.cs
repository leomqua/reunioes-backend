using System.Globalization;

namespace Reunioes.Application.Services;

internal static class ReservaHorario
{
    private static readonly TimeSpan InicioExpediente = TimeSpan.FromHours(8);
    private static readonly TimeSpan FimExpediente = TimeSpan.FromHours(19);

    public static DateTime Combinar(DateOnly data, string hora)
    {
        var horario = TimeOnly.ParseExact(hora, "HH:mm", CultureInfo.InvariantCulture);
        return data.ToDateTime(horario);
    }

    public static (DateTime Inicio, DateTime Fim) Validar(DateTime inicio, DateTime fim)
    {
        if (fim <= inicio)
            throw new ArgumentException(
                "O término da reserva deve ser posterior ao início.");

        if (inicio.Date != fim.Date || inicio.TimeOfDay < InicioExpediente || fim.TimeOfDay > FimExpediente)
        {
            throw new ArgumentException(
                "As reservas devem ocorrer no mesmo dia, entre 08:00 e 19:00.");
        }

        return (inicio, fim);
    }

    public static (DateTime Inicio, DateTime Fim) JanelaDoDia(DateTime data)
    {
        var inicio = data.Date.AddHours(8);
        var fim = data.Date.AddHours(19);

        return (inicio, fim);
    }
}
