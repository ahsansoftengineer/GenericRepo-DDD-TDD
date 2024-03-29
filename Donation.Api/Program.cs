using Donation.Api;
using Donation.Application;
using Donation.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
  builder.Services
    .AddPresentation() // Api Layer
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

}

var app = builder.Build();

{
  app.UseExceptionHandler("/error");
  // app.UseCors("AllowAll");
  app.UseHttpsRedirection();
  //app.UseAuthentication(); // This is already Set up by AddControllers
  //app.UseAuthorization(); // This middleware decide weather the user can access the endpoints

  app.MapControllers();

  if (app.Environment.IsDevelopment())
  {
    // Developer Exception Page
    // and Swagger not Work with Exception Filters
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
  }


  app.Run();
}


