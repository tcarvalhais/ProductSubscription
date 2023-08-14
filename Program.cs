using ProductSubscription.Repositories;

// Add services to the container
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUsersRepository, UsersData>();
builder.Services.AddSingleton<IProductsRepository, ProductsData>();

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the HTTP request pipeline
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Run the app
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
