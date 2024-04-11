using System.Collections;
using WebApplication1.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new List<Pet>
{
     new Pet {id = 1, name = "Burek", category = Category.Pies, color = "Szary", weight = 20, Visits = new List<Visit>()},
     new Pet {id = 2, name = "Garfield", category = Category.Kot, color = "Rudy", weight = 24, Visits = new List<Visit>()},
     new Pet {id = 3, name = "Maciej", category = Category.Lis, color = "Rudy", weight = 57.5, Visits = new List<Visit>()},
     new Pet {id = 4, name = "Adrian", category = Category.Chomik, color = "Czarny", weight = 2, Visits = new List<Visit>()},
     
};

app.MapGet("/api/pets", () => Results.Ok(summaries))
    .WithName("GetPets")
    .WithOpenApi();

app.MapGet("/api/pets/{id:int}", (int id) =>
    {
        Pet pet = summaries.FirstOrDefault(p => p.id == id);
        return pet == null ? Results.NotFound($"Pet with id {id} was not found") : Results.Ok(pet);
    })
    .WithName("GetPet")
    .WithOpenApi();

app.MapPost("api/pets", (Pet pet) =>
    {
        summaries.Add(pet);
        return Results.StatusCode(StatusCodes.Status201Created);
    })
    .WithName("CreatePet")
    .WithOpenApi();

app.MapPut("/api/pets/{id:int}", (int id, Pet pet) =>
    {
        Pet toEdit = summaries.FirstOrDefault(p => p.id == id);
        if (toEdit == null)
        {
            return Results.NotFound($"Pet with id {id} was not found");
        }

        summaries.Remove(toEdit);
        summaries.Add(pet);
        return Results.NoContent();
    })
    .WithName("UpdatePet")
    .WithOpenApi();

app.MapPut("/api/pets/visits/{id:int}", (int id, Visit visit) =>
    {
        Pet toEdit = summaries.FirstOrDefault(p => p.id == id);
        if (toEdit == null)
        {
            return Results.NotFound($"Pet with id {id} was not found");
        }

        summaries.Remove(toEdit);
        toEdit.Visits.Add(visit);
        summaries.Add(toEdit);
        return Results.NoContent();
    })
    .WithName("UpdatePetsVisit")
    .WithOpenApi();

app.MapDelete("/api/pets/{id:int}", (int id) =>
    {
        Pet toDelete = summaries.FirstOrDefault(p => p.id == id);
        if (toDelete == null)
        {
            return Results.NoContent();
        }

        summaries.Remove(toDelete);
        return Results.NoContent();
    })
    .WithName("DeletePet")
    .WithOpenApi();

app.MapGet("/api/pets/visits/{id:int}", (int id) =>
    {
        Pet pet = summaries.FirstOrDefault(p => p.id == id);
        return pet == null ? Results.NotFound($"Pet with id {id} was not found") : Results.Ok(pet.Visits);
    })
    .WithName("GetPetsVistis")
    .WithOpenApi();


app.Run();
