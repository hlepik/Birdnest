namespace DAL.App.EF;

public class AppDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        var connectionStr =
            "Server=hlepik-birdnest.postgres.database.azure.com;Database=birdnest-hlepik;Port=5432;User Id=hlepik;Password=Birdnest123;Ssl Mode=VerifyFull;";
        optionsBuilder.UseNpgsql(connectionStr);

        return new AppDbContext(optionsBuilder.Options);
    }
}