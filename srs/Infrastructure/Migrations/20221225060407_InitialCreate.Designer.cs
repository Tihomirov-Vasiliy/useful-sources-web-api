// <auto-generated />
using System;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(UsefulSourcesContext))]
    [Migration("20221225060407_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Source", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("createdby");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatedat");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updatedby");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("uri");

                    b.HasKey("Id");

                    b.ToTable("sources", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("createdby");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatedat");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updatedby");

                    b.HasKey("Id");

                    b.ToTable("tags", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("createdby");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text")
                        .HasColumnName("emailaddress");

                    b.Property<string>("GivenName")
                        .HasColumnType("text")
                        .HasColumnName("givenname");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatedat");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updatedby");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("source_tag", b =>
                {
                    b.Property<int>("source_id")
                        .HasColumnType("integer")
                        .HasColumnName("source_id");

                    b.Property<int>("tag_id")
                        .HasColumnType("integer")
                        .HasColumnName("tag_id");

                    b.HasKey("source_id", "tag_id");

                    b.HasIndex("tag_id");

                    b.ToTable("source_tag");
                });

            modelBuilder.Entity("tag_parenttag", b =>
                {
                    b.Property<int>("parenttag_id")
                        .HasColumnType("integer")
                        .HasColumnName("parenttag_id");

                    b.Property<int>("tag_id")
                        .HasColumnType("integer")
                        .HasColumnName("tag_id");

                    b.HasKey("parenttag_id", "tag_id");

                    b.HasIndex("tag_id");

                    b.ToTable("tag_parenttag");
                });

            modelBuilder.Entity("source_tag", b =>
                {
                    b.HasOne("Domain.Entities.Source", null)
                        .WithMany()
                        .HasForeignKey("source_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_sourcetag_sources_sourceid");

                    b.HasOne("Domain.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("tag_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_sourcetag_tags_tagid");
                });

            modelBuilder.Entity("tag_parenttag", b =>
                {
                    b.HasOne("Domain.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("parenttag_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_tagparenttag_parenttags_parenttagid");

                    b.HasOne("Domain.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("tag_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_tagparenttag_tags_tagid");
                });
#pragma warning restore 612, 618
        }
    }
}
