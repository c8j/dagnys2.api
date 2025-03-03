﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dagnys2.api.Data;

#nullable disable

namespace dagnys2.api.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("dagnys2.api.Entities.Address", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<string>("PostalCode")
                        .HasColumnType("longtext");

                    b.Property<string>("StreetLine")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("dagnys2.api.Entities.AddressType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("AddressTypes");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Batch", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateOnly>("ExpirationDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("ManufactureDate")
                        .HasColumnType("date");

                    b.HasKey("ID");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Entity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("ContactName")
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("varchar(8)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Entities");

                    b.HasDiscriminator().HasValue("Entity");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("dagnys2.api.Entities.EntityAddress", b =>
                {
                    b.Property<int>("EntityID")
                        .HasColumnType("int");

                    b.Property<int>("AddressID")
                        .HasColumnType("int");

                    b.Property<int>("AddressTypeID")
                        .HasColumnType("int");

                    b.HasKey("EntityID", "AddressID", "AddressTypeID");

                    b.HasIndex("AddressID");

                    b.HasIndex("AddressTypeID");

                    b.ToTable("EntityAddresses");
                });

            modelBuilder.Entity("dagnys2.api.Entities.EntityPhone", b =>
                {
                    b.Property<int>("EntityID")
                        .HasColumnType("int");

                    b.Property<int>("PhoneID")
                        .HasColumnType("int");

                    b.Property<int>("PhoneTypeID")
                        .HasColumnType("int");

                    b.HasKey("EntityID", "PhoneID", "PhoneTypeID");

                    b.HasIndex("PhoneID");

                    b.HasIndex("PhoneTypeID");

                    b.ToTable("EntityPhones");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Ingredient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("ItemNumber")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("ItemNumber")
                        .IsUnique();

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateOnly>("DateCreated")
                        .HasColumnType("date");

                    b.Property<int>("GeneratedNumber")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("dagnys2.api.Entities.OrderItem", b =>
                {
                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Phone", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Number")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Phones");
                });

            modelBuilder.Entity("dagnys2.api.Entities.PhoneType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PhoneTypes");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AmountInPackage")
                        .HasColumnType("int");

                    b.Property<string>("ItemNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<decimal>("PriceKrPerUnit")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("WeightInGrams")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("dagnys2.api.Entities.ProductBatch", b =>
                {
                    b.Property<int>("BatchID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("BatchID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductBatches");
                });

            modelBuilder.Entity("dagnys2.api.Entities.SupplierIngredient", b =>
                {
                    b.Property<int>("SupplierID")
                        .HasColumnType("int");

                    b.Property<int>("IngredientID")
                        .HasColumnType("int");

                    b.Property<decimal>("PriceKrPerKg")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("SupplierID", "IngredientID");

                    b.HasIndex("IngredientID");

                    b.ToTable("SupplierIngredients");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Customer", b =>
                {
                    b.HasBaseType("dagnys2.api.Entities.Entity");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Supplier", b =>
                {
                    b.HasBaseType("dagnys2.api.Entities.Entity");

                    b.HasDiscriminator().HasValue("Supplier");
                });

            modelBuilder.Entity("dagnys2.api.Entities.EntityAddress", b =>
                {
                    b.HasOne("dagnys2.api.Entities.Address", "Address")
                        .WithMany("EntityAddresses")
                        .HasForeignKey("AddressID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dagnys2.api.Entities.AddressType", "AddressType")
                        .WithMany("EntityAddresses")
                        .HasForeignKey("AddressTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dagnys2.api.Entities.Entity", "Entity")
                        .WithMany("EntityAddresses")
                        .HasForeignKey("EntityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("AddressType");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("dagnys2.api.Entities.EntityPhone", b =>
                {
                    b.HasOne("dagnys2.api.Entities.Entity", "Entity")
                        .WithMany("EntityPhones")
                        .HasForeignKey("EntityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dagnys2.api.Entities.Phone", "Phone")
                        .WithMany("EntityPhones")
                        .HasForeignKey("PhoneID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dagnys2.api.Entities.PhoneType", "PhoneType")
                        .WithMany("EntityPhones")
                        .HasForeignKey("PhoneTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entity");

                    b.Navigation("Phone");

                    b.Navigation("PhoneType");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Order", b =>
                {
                    b.HasOne("dagnys2.api.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("dagnys2.api.Entities.OrderItem", b =>
                {
                    b.HasOne("dagnys2.api.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dagnys2.api.Entities.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("dagnys2.api.Entities.ProductBatch", b =>
                {
                    b.HasOne("dagnys2.api.Entities.Batch", "Batch")
                        .WithMany("ProductBatches")
                        .HasForeignKey("BatchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dagnys2.api.Entities.Product", "Product")
                        .WithMany("ProductBatches")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Batch");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("dagnys2.api.Entities.SupplierIngredient", b =>
                {
                    b.HasOne("dagnys2.api.Entities.Ingredient", "Ingredient")
                        .WithMany("SupplierIngredients")
                        .HasForeignKey("IngredientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dagnys2.api.Entities.Supplier", "Supplier")
                        .WithMany("SupplierIngredients")
                        .HasForeignKey("SupplierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Address", b =>
                {
                    b.Navigation("EntityAddresses");
                });

            modelBuilder.Entity("dagnys2.api.Entities.AddressType", b =>
                {
                    b.Navigation("EntityAddresses");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Batch", b =>
                {
                    b.Navigation("ProductBatches");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Entity", b =>
                {
                    b.Navigation("EntityAddresses");

                    b.Navigation("EntityPhones");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Ingredient", b =>
                {
                    b.Navigation("SupplierIngredients");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Phone", b =>
                {
                    b.Navigation("EntityPhones");
                });

            modelBuilder.Entity("dagnys2.api.Entities.PhoneType", b =>
                {
                    b.Navigation("EntityPhones");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Product", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("ProductBatches");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("dagnys2.api.Entities.Supplier", b =>
                {
                    b.Navigation("SupplierIngredients");
                });
#pragma warning restore 612, 618
        }
    }
}
