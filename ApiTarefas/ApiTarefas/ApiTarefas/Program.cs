using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registrando AppDbContext
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("TarefasDB")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPut("/api/pix/notification", (Notification pedido) =>
{
    return Results.Ok(pedido.pedido);
}
);


app.MapGet("frases", async () => 
    await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes")
);

//Rota retornar lista de tarefas
app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

//Rota retornar lista de tarefas
app.MapGet("/tarefas/concluidas", async (AppDbContext db) => await db.Tarefas.Where(t => t.IsConcluida).ToListAsync());

//Rota retorna tarefa pelo id
app.MapGet("/tarefas/{id}", async (int id, AppDbContext db) =>
{
    string Err = "Error: " + "Tarefa " + "ID: " + id + " não localizada";
    return await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound(Err);
}
);

//Rota atualizar Tarefa

app.MapPut("/tarefas/{id}", async (int id, Tarefa imputTarefa, AppDbContext db) => 
{ 
    var tarefa = await db.Tarefas.FindAsync(id);

    if (tarefa is null) return Results.NotFound();

    tarefa.Nome = imputTarefa.Nome;
    tarefa.IsConcluida = imputTarefa.IsConcluida;

    await db.SaveChangesAsync();
    return Results.NoContent();
});


//Rota deletar Tarefa

app.MapDelete("/tarefas/{id}", async (int id,  AppDbContext db) =>
{
    var tarefa = await db.Tarefas.FindAsync(id);

    if (tarefa is null) return Results.NotFound();

    db.Tarefas.Remove(tarefa);

    await db.SaveChangesAsync();
    return Results.Ok(tarefa);
});

//Rota criar nova tarefa no banco
app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{   //Insere os dados no banco
    db.Tarefas.Add(tarefa);
    //Salva os dados no banco
    await db.SaveChangesAsync();
    //Retorna os dados salvos
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});


app.Run();

class Tarefa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool IsConcluida { get; set; }
}

public class Pedido
{
    public string? numero { get; set; }
    public string? merchantId { get; set; }
    public string? dados { get; set; }
}

public class Notification
{
    public Pedido? pedido { get; set; }
}

class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();  
}