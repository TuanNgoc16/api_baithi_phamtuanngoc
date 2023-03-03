using Microsoft.EntityFrameworkCore;
using tuanngoc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MemDb>(opt => opt.UseInMemoryDatabase("HRM_Human"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/memitems", async (MemDb db) =>
    await db.Mems.ToListAsync()
);

app.MapGet("/memitems/complete", async (MemDb db) =>
    await db.Mems.Where(t => t.IsComple).ToListAsync()
);

app.MapGet("/memitems/search/{name}", async (string name, MemDb db) =>
{
    var mems = await db.Mems
        .Where(e => e.Name!.Contains(name))
        .ToListAsync();

    if (mems is null || mems.Count == 0) return Results.NotFound();

    return Results.Ok(mems);
});

app.MapGet("/memitems/{id}", async (int id, MemDb db) =>
    await db.Mems.FindAsync(id)
        is Mem mem
            ? Results.Ok(mem)
            : Results.NotFound()
);


app.MapPost("/memitems", async (Mem mem, MemDb db) =>
{
    db.Mems.Add(mem);
    await db.SaveChangesAsync();

    return Results.Created($"/memitems/{mem.Id}", mem);
});

app.MapPut("/memitems/{id}", async (int id, Mem inputMem, MemDb db) =>
{
    var mem = await db.Mems.FindAsync(id);

    if (mem is null) return Results.NotFound();

    mem.Name = inputMem.Name;
    mem.IsComple = inputMem.IsComple;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/memitems/{id}", async (int id, MemDb db) =>
{
    if (await db.Mems.FindAsync(id) is Mem mem)
    {
        db.Mems.Remove(mem);
        await db.SaveChangesAsync();
        return Results.Ok(mem);
    }

    return Results.NotFound();
});

app.Run();