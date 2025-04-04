﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasMany(e => e.Orders)
			.WithOne(e => e.User)
			.HasPrincipalKey(e => e.UserName)
			.HasForeignKey(e => e.UserName)
			.OnDelete(DeleteBehavior.SetNull)
			.IsRequired();
	}
}