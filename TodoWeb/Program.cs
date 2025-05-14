using TodoWeb.Application.Services;
using TodoWeb.Appllication.ActionFilters;
using TodoWeb.Appllication.MapperProfiles;
using TodoWeb.Appllication.Middleware;
using TodoWeb.Appllication.Services.CacheService;
using TodoWeb.Infrastructures;
using FluentValidation;
using FluentValidation.AspNetCore;
using TodoWeb.Application.Services.Users;
using TodoWeb.Appllication.StudentValidator;


var builder = WebApplication.CreateBuilder(args);

//
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<TestFilter>();
});
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
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddSingleton<LogMiddleware>();
builder.Services.AddSingleton<RateLimitMiddleware>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddMemoryCache();
//builder.Services.AddSingleton<LogFilter>();
builder.Services.AddAutoMapper(typeof(TodoProfile));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<StudentCreateValidator>();
// builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
// builder.Services.AddValidatorsFromAssemblyContaining<AddressValidatior>();
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


/*
app.UseExceptionHandler("/Error");
*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    Console.WriteLine("Request: " + context.Request.Method + " " + context.Request.Path);
    await next();
    Console.WriteLine("Reponse" + context.Response.StatusCode);

});

app.Use(async (context, next) =>
{
    Console.WriteLine("Request: " + context.Request.Method + " " + context.Request.Path);
    await next();
    Console.WriteLine("Reponse" + context.Response.StatusCode);

});
app.UseMiddleware<LogMiddleware>();
app.UseMiddleware<RateLimitMiddleware>();



app.Run();


