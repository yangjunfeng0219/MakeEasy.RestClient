namespace MakeEasy.RestClient.WebApiServer;

public class Server
{
    private WebApplication? app;

    public Task StartAsync()
    {
        var builder = WebApplication.CreateBuilder();

        var mvcBuilder = builder.Services.AddMvc();
        var asm = typeof(Server).Assembly;
        mvcBuilder.AddApplicationPart(asm);
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        return app.RunAsync("http://127.0.0.1:12321");
    }

    public Task StopAsync()
    {
        if (app == null) return Task.CompletedTask;
        else return app.StopAsync();
    }
}
