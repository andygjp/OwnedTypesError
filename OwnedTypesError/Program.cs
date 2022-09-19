using static System.Diagnostics.Debug;
using static DataSourceType;

// 1. Run through this using v6.0.9 - no error.
// 2. Comment out the v6.0.9 package references, in the csproj file, and uncomment the rc1 package references.
// 3. Restore and rebuild the project.
// 4. Run - System.InvalidOperationException thrown inside RelationalSqlTranslatingExpressionVisitor.TryRewriteEntityEquality

var options = GetOptions(dataSourceType: Sqlite);

CreateDatabase(options);

var withOwnedEntity = new Entity
{
    Value = "Foo",
    OwnedOne = new OwnedOne
    {
        Value = "Bar"
    }
};
AddEntity(options, withOwnedEntity);

// This is fine
GetEntity(options, withOwnedEntity, _ => true);
// Microsoft.EntityFrameworkCore.Query.RelationalSqlTranslatingExpressionVisitor.TryRewriteEntityEquality throws an error here
var ownedEntity = GetEntity(options, withOwnedEntity, ownedOne => ownedOne != null);
Assert(ownedEntity.Count is 1);

var withoutOwnedEntity = new Entity
{
    Value = "Baz",
    OwnedOne = null,
};
AddEntity(options, withoutOwnedEntity);

GetEntity(options, withoutOwnedEntity, _ => true);
// Throws same error as before
var missingOwnedEntity = GetEntity(options, withoutOwnedEntity, ownedOne => ownedOne == null);
Assert(missingOwnedEntity.Count is 1);

DbContextOptions<Context> GetOptions(DataSourceType dataSourceType)
{
    ArgumentOutOfRangeException MissingDataSourceType() => 
        new(nameof(dataSourceType), "Expected " + string.Join(", ", Enum.GetNames<DataSourceType>()));

    DbConnection GetOpenConnection()
    {
        DbConnection dbConnection = dataSourceType switch
        {
            Sqlite => new SqliteConnection("Filename=:memory:"),
            // TODO update with your logon details
            Sql => new SqlConnection(
                "Server=localhost;Database=OwnedTypesError;User Id=<user>;Password=<password>;Encrypt=False"),
            _ => throw MissingDataSourceType()
        };
        dbConnection.Open();
        return dbConnection;
    }

    return (dataSourceType switch
        {
            Sqlite => new DbContextOptionsBuilder<Context>().UseSqlite(GetOpenConnection()),
            Sql => new DbContextOptionsBuilder<Context>().UseSqlServer(GetOpenConnection()),
            _ => throw MissingDataSourceType()
        })
        .LogTo(Console.WriteLine)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging().Options;
}

void CreateDatabase(DbContextOptions<Context> dbContextOptions)
{
    using var context = new Context(dbContextOptions);
    context.Database.EnsureCreated();
}

void AddEntity(DbContextOptions<Context> dbContextOptions, Entity newEntity)
{
    using var context = new Context(dbContextOptions);
    context.Database.EnsureCreated();
    context.Entities.Add(newEntity);
    context.SaveChanges();
}

IList GetEntity(DbContextOptions<Context> dbContextOptions, Entity existingEntity,
    Expression<Func<OwnedOne?, bool>> predicate)
{
    using var context = new Context(dbContextOptions);
    return context.Entities
        .AsNoTracking()
        .Where(x => x.EntityID == existingEntity.EntityID)
        .Select(x => x.OwnedOne)
        .Where(predicate)
        .ToList();
}

internal enum DataSourceType
{
    Sqlite,
    Sql
}

internal class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    public DbSet<Entity> Entities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>().HasKey(x => x.EntityID);
        var ownedNavigationBuilder = modelBuilder.Entity<Entity>().OwnsOne(x => x.OwnedOne);
        var mutableForeignKey = ownedNavigationBuilder.Metadata;
        Console.WriteLine(mutableForeignKey);
        var mutableEntityType = ownedNavigationBuilder.OwnedEntityType;
        Console.WriteLine(mutableEntityType);
    }
}

internal class Entity
{
    public int EntityID { get; set; }
    public string? Value { get; set; }
    public OwnedOne? OwnedOne { get; set; }
}

internal class OwnedOne
{
    public string? Value { get; set; }
}