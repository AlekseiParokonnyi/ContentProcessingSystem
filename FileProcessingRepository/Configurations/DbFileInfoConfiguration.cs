using FileProcessingCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileProcessingRepository.Configurations;

public class DbFileInfoConfiguration : IEntityTypeConfiguration<FileInfoModel>
{
  public void Configure(EntityTypeBuilder<FileInfoModel> builder)
  {
    builder.HasKey(x => x.Id);

    builder.Property(x => x.FileName)
      .IsRequired()
      .HasMaxLength(255);

    builder.Property(x => x.Url)
      .IsRequired()
      .HasMaxLength(512);

    builder.Property(x => x.Status)
      .IsRequired()
      .HasMaxLength(32);

    builder.Property(x => x.CreatedAt)
      .HasDefaultValueSql("now()");
  }
}