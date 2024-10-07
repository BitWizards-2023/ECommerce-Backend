using ECommerceBackend.Models;
using ECommerceBackend.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ECommerceBackend.Data.Contexts
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoDbContext> _logger;

        public MongoDbContext(IOptions<DatabaseSettings> dbSettings, ILogger<MongoDbContext> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            try
            {
                var client = new MongoClient(dbSettings.Value.ConnectionString.ToString());
                _database = client.GetDatabase(dbSettings.Value.DatabaseName);

                _logger.LogInformation(
                    "Successfully connected to MongoDB Atlas database: {DatabaseName}",
                    dbSettings.Value.DatabaseName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while connecting to MongoDB Atlas.");
                throw;
            }
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Category> Categories =>
            _database.GetCollection<Category>("Categories");

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
        public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");
    }
}
