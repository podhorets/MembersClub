using MembersClub.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddDbContext<MembersClubDbContext>(opt => opt.UseInMemoryDatabase("MembersClub"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/member/{id}", ([FromServices] MemberRepository repository, string id) =>
{
    if (int.TryParse(id, out int memberId))
    {
        var member = repository.Get(memberId);
        if (member.Result == null) {
            return Results.NotFound(new { message = "Member not found" }); 
        }

        return Results.Json(member.Result);
    }
    
    return Results.BadRequest(new { message = "Id is not a number!" });
});

app.MapGet("/members", ([FromServices] MemberRepository repository) =>
{
    return repository.GetAll();
});

app.MapPost("/member", ([FromServices] MemberRepository repository, Member member) =>
{
    if (string.IsNullOrEmpty(member.Email) || string.IsNullOrEmpty(member.Email))
    {
        return Results.BadRequest(new { message = "All fields should be filled!" });
    }

    var emailPattern = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
    var emailMatch = Regex.Match(member.Email, emailPattern);

    if (!emailMatch.Success)
    {
        return Results.BadRequest(new { message = "Email is not valid!" });
    }

    var namePattern = "^[A-Za-z\\s]{1,}[\\.]{0,1}[A-Za-z\\s]{0,}$";
    var nameMatch = Regex.Match(member.Name, namePattern);

    if (!nameMatch.Success)
    {
        return Results.BadRequest(new { message = "Name is not valid!" });
    }

    return Results.Json(repository.Add(member).Result);
});

app.MapDelete("/member/{id}", async ([FromServices] MemberRepository repository, string id) =>
{
    if (int.TryParse(id, out int memberId))
    {
        var member = repository.Get(memberId);
        if (member.Result == null)
        {
            return Results.NotFound(new { message = "Member not found" });
        }

        await repository.Delete(member.Result.Id);

        return Results.Json(member.Result);
    }

    return Results.BadRequest(new { message = "Id is not a number!" });
});

app.Run();
