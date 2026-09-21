using ZoomMeetings.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddZoomMeetings(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
