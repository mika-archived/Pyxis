using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Pyxis.Models.Caching;

namespace Pyxis.Migrations
{
    [DbContext(typeof(CacheContext))]
    internal class CacheContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("Pyxis.Models.Caching.CacheFile", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();

                b.Property<int>("Category");

                b.Property<DateTime>("CreatedAt");

                b.Property<string>("Path");

                b.Property<long>("Size");

                b.Property<DateTime>("UpdatedAt");

                b.HasKey("Id");

                b.ToTable("CacheFiles");
            });
        }
    }
}