using Prometheus;
using PrometheusNetSignalRIssue;

var builder = WebApplication.CreateBuilder(args);

Metrics.SuppressDefaultMetrics(new SuppressDefaultMetricOptions()
{
  SuppressMeters = false,
  SuppressDebugMetrics = true,
  SuppressEventCounters = true,
  SuppressProcessMetrics = true
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSignalR();
builder.Services.AddMetricServer(options =>
{
  options.Url = "/metrics";
});

var app = builder.Build();

app.UseRouting();

app.UseMetricServer();

app.UseEndpoints(endpoints =>
{ 
  endpoints.MapHub<Hub>("/signalr");
});

app.MapGet("/", async (context) =>
{
  await context.Response.SendFileAsync("index.html");
});

TestMetricListener.Listen();

app.Run();