using _08A3A4HttpServerDemo.HTTP;
using _08A3A4HttpServerDemo.MTCG.Endpooints;
using _08A3A4HttpServerDemo.Services;
using System.Net;

Console.WriteLine("Our first simple HTTP-Server: http://localhost:10001/");

DatabaseService dbManager = new DatabaseService();
dbManager.OpenConnection();


HttpServer httpServer = new HttpServer(IPAddress.Any, 10001);
httpServer.RegisterEndpoint("users", new UsersEndpoint(dbManager.GetConnection));
httpServer.RegisterEndpoint("sessions", new SessionEndpoint(dbManager.GetConnection));
httpServer.RegisterEndpoint("packages", new PackageEndpoint(dbManager.GetConnection));
httpServer.RegisterEndpoint("transactions", new TransactionEndpoint(dbManager.GetConnection));
httpServer.RegisterEndpoint("cards", new CardsEndpoint(dbManager.GetConnection));
httpServer.RegisterEndpoint("deck", new DeckEndpoint(dbManager.GetConnection));
httpServer.RegisterEndpoint("stats", new StatsEndpoint(dbManager.GetConnection));
httpServer.RegisterEndpoint("battles", new BattleEndpoint(dbManager.GetConnection));
httpServer.RegisterEndpoint("tradings", new TradingEndpoint(dbManager.GetConnection));




httpServer.Run();

