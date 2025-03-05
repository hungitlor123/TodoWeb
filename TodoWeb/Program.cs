using TodoWeb.Application.Services;
using TodoWeb.Infrastructures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// IApplicationDbContext dbContext = new ApplicationDbContext()
builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddTransient<IGuidGenerator, GuidGenerator>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentGradeService, StudentGradeService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


