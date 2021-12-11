using ATM.Model.DbModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATM.Model.DbModel.EntityConfig
{
    public class DenominationsConfiguration : IEntityTypeConfiguration<Denominations>
    {
        public void Configure(EntityTypeBuilder<Denominations> builder)
        {
            builder
                .Property(e => e.Denomination)
                .IsRequired();

            builder
                .HasIndex(e => e.Denomination)
                .IsUnique()
                .HasDatabaseName("I_DENOMINATION_UNIQUE");

            builder
                .Property(e => e.Quantity)
                .IsRequired();
        }

    }
}
