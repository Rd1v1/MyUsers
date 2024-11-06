using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyUsers.Models;
using MyUsers.Models.DTO;
using MyUsers.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapPost("/", async (UserDTO userDTO, ApplicationDbContext context) =>
{
	context.Users.Add(new User()
	{
		Name = userDTO.Name,
		Email = userDTO.Email
	});
	await context.SaveChangesAsync();
});
app.MapPut("/{id:int}", async Task<Results<Ok<User>, NotFound>>(int id, UserDTO userDTO, ApplicationDbContext context) =>
{
	var user = await context.FindAsync<User>(id);
	if (user is null) return TypedResults.NotFound();
	user.Name = userDTO.Name;
	user.Email = userDTO.Email;
	context.Users.Update(user);
	await context.SaveChangesAsync();
	return TypedResults.Ok(user);
}); 
app.MapGet("/", async (ApplicationDbContext context) => await context.Users.ToListAsync());
app.MapGet("/{id:int}",
	async Task<Results<Ok<User>, NotFound>>  (int id, ApplicationDbContext context) =>
	{
		var user = await context.Users.FindAsync(id);
		if (user is null) return TypedResults.NotFound();
		return TypedResults.Ok(user);
	}
);
app.MapDelete("/{id:int}", async Task<Results<Ok, NotFound>>(int id, ApplicationDbContext context) =>
{
	var user = await context.FindAsync<User>(id);
	if (user is null) return TypedResults.NotFound();
	context.Users.Remove(user);
	await context.SaveChangesAsync();
	return TypedResults.Ok();
});
app.Run();