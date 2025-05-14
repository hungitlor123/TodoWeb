using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Constants.Enums;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(x => x.EmailAddress).IsRequired();
        builder.Property(x => x.Password).IsRequired();
        
        // builder.Property(x => x.Role).HasConversion(codeToDatabase => (int)codeToDatabase, databaseToCode => (Role)databaseToCode);
    }
}