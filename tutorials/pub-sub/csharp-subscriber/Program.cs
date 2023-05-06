using Dapr;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

string [] candidatos=new string[5];
int [] votos= new int[5];

int contador=0;
int contador2=0;
int nulos=0;
bool bandera=false;
string palabra="";
string palabra2="";
// Dapr configurations
app.UseCloudEvents();

app.MapSubscribeHandler();

app.MapPost("/A", [Topic("pubsub", "A")] (ILogger<Program> logger, MessageEvent item) => {
    if(contador<=4)
    {
    Console.WriteLine($"{item.MessageType}: {item.Message}");
    candidatos[contador]=item.Message;
    Console.WriteLine(candidatos[contador]);
    contador++;
    
    return Results.Ok();
    }
    else
    Console.WriteLine("no se aceptan mas candidatos");
    return Results.BadRequest();
    
});

app.MapPost("/B", [Topic("pubsub", "B")] (ILogger<Program> logger, MessageEvent item) => {
    Console.WriteLine($"{item.MessageType}: {item.Message}");
   if(palabra=="")
   {
    for(int i=0;i<=4;i++)
    {
        if(candidatos[i]==item.Message)
        {
            votos[i]++; 
        }
 
    }

     for(int i=0;i<=4;i++)
    {
        if(candidatos[i]!=item.Message)
        {
            nulos=nulos+1;
        }
 
    }
    nulos=nulos-4;
    Console.WriteLine("Candidato: "+candidatos[0]+"votos"+votos[0]);
    Console.WriteLine("Candidato: "+candidatos[1]+"votos"+votos[1]);
    Console.WriteLine("Candidato: "+candidatos[2]+"votos"+votos[2]);
    Console.WriteLine("Candidato: "+candidatos[3]+"votos"+votos[3]);
    Console.WriteLine("Candidato: "+candidatos[4]+"votos"+votos[4]);
    Console.WriteLine("Candidato: nulos  "+"votos"+nulos);
    
   }
   else if(palabra=="terminar")
   {
   Console.WriteLine("votaciones ya han sido terminadas");
   }
    return Results.Ok();
   
});

app.MapPost("/C", [Topic("pubsub", "C")] (ILogger<Program> logger, Dictionary<string, string> item) => {
    Console.WriteLine($"{item["messageType"]}: {item["message"]}");
   palabra="terminar";
    Console.WriteLine("votaciones terminadas");
    Console.WriteLine("Candidato: "+candidatos[0]+"votos"+votos[0]);
    Console.WriteLine("Candidato: "+candidatos[1]+"votos"+votos[1]);
    Console.WriteLine("Candidato: "+candidatos[2]+"votos"+votos[2]);
    Console.WriteLine("Candidato: "+candidatos[3]+"votos"+votos[3]);
    Console.WriteLine("Candidato: "+candidatos[4]+"votos"+votos[4]);
    Console.WriteLine("Candidato: nulos  "+"votos"+nulos);
    
    return Results.Ok();
});

app.Run();

internal record MessageEvent(string MessageType, string Message);