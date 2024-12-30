using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public static class ModelBuilderConfiguration
{
	public static void Apply(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new BookConfiguration());
		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new CategoryConfiguration());
		modelBuilder.ApplyConfiguration(new OrderConfiguration());
	}
}