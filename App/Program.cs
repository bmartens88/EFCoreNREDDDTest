using App;
using App.Data.Context;
using App.Data.Models.Extensions;
using App.Data.Models.ValueObjects;
using App.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(opts =>
{
    opts.UseSqlite("Data Source = test.sqlite");
    opts.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapGet("/users", async (AppDbContext context, CancellationToken ctx) =>
{
    var users = await context.Users.Select(u => u.AsDto()).ToListAsync(ctx);
    return Results.Ok(users);
});

app.MapPost("/users", async ([FromBody] UserDto user, AppDbContext context, CancellationToken ctx) =>
{
    var newUser = user.AsUser();
    var added = await context.Users.AddAsync(newUser, ctx);
    await context.SaveChangesAsync();
    return Results.CreatedAtRoute<UserDto>("GetClientById", new { id = added.Entity.Id.Value }, added.Entity.AsDto());
});

app.MapGet("/users/{id:guid}", async (Guid id, AppDbContext context, CancellationToken ctx) =>
{
    var user = await context.Users.FindAsync(new UserId(id), ctx);
    return Results.Ok(user?.AsDto());
})
.WithName("GetClientById");

app.MapPut("/users/{id:int}/name", async (int id, [FromQuery] string firstName, AppDbContext context, CancellationToken ctx) =>
{
    var user = await context.Users.FindAsync(id, ctx);
    if (user is null) return Results.NotFound();
    user.UpdateFirstName(firstName);
    await context.SaveChangesAsync(ctx);
    return Results.Ok();
});

app.MapDelete("/users/{id:guid}", async (Guid id, AppDbContext context, CancellationToken ctx) =>
{
    var user = await context.Users.FindAsync(new UserId(id), ctx);
    if (user is null) return Results.NotFound();
    context.Users.Remove(user);
    await context.SaveChangesAsync(ctx);
    return Results.NoContent();
});

app.Run();