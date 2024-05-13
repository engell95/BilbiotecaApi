using System;

namespace BibliotecaApi.DbModels;

public partial class AppLog
{
    public int Id { get; set; }

    public string? MachineName { get; set; }

    public DateTime? Date { get; set; }

    public string? Level { get; set; }

    public string? Controller { get; set; }

    public string? Action { get; set; }

    public string? RequestUrl { get; set; }

    public string? RequestIp { get; set; }

    public string? TraceId { get; set; }

    public string? Message { get; set; }

    public string? Logger { get; set; }

    public string? CallSite { get; set; }

    public string? Exceptcion { get; set; }
}
