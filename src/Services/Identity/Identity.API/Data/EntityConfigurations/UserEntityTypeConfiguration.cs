using Identity.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> userConfiguration)
        {
            userConfiguration.HasKey(p => p.Id);

            userConfiguration.HasIndex(p => p.Email)
                .IsUnique();

            userConfiguration.Property(p => p.FirstName)
                .IsRequired();

            userConfiguration.Property(p => p.LastName)
                .IsRequired();
        }
    }
}