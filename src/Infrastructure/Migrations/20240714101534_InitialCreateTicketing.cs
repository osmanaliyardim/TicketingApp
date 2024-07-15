using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateTicketing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "buyer_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "event_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "eventmanager_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "manifest_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "offer_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "prices_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "seat_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "section_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "ticket_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "venue_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdentityGuid = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ShipToAddress_Street = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    ShipToAddress_City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShipToAddress_State = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ShipToAddress_Country = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    ShipToAddress_ZipCode = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(1250)", maxLength: 1250, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasketItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    BasketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItems_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethod_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemOrdered_EventId = table.Column<int>(type: "int", nullable: false),
                    ItemOrdered_EventName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Manifests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SeatMap = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manifests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manifests_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(1250)", maxLength: 1250, nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SeatType = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    ManifestId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Manifests_ManifestId",
                        column: x => x.ManifestId,
                        principalTable: "Manifests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seats_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    PricesId = table.Column<int>(type: "int", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Prices_PricesId",
                        column: x => x.PricesId,
                        principalTable: "Prices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketId",
                table: "BasketItems",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_VenueId",
                table: "Events",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Manifests_VenueId",
                table: "Manifests",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_EventId",
                table: "Offers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_BuyerId",
                table: "PaymentMethod",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_OfferId",
                table: "Prices",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ManifestId",
                table: "Seats",
                column: "ManifestId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SectionId",
                table: "Seats",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_VenueId",
                table: "Sections",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BuyerId",
                table: "Tickets",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PricesId",
                table: "Tickets",
                column: "PricesId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SeatId",
                table: "Tickets",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_VenueId",
                table: "Tickets",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItems");

            migrationBuilder.DropTable(
                name: "EventManagers");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Manifests");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropSequence(
                name: "buyer_hilo");

            migrationBuilder.DropSequence(
                name: "event_hilo");

            migrationBuilder.DropSequence(
                name: "eventmanager_hilo");

            migrationBuilder.DropSequence(
                name: "manifest_hilo");

            migrationBuilder.DropSequence(
                name: "offer_hilo");

            migrationBuilder.DropSequence(
                name: "prices_hilo");

            migrationBuilder.DropSequence(
                name: "seat_hilo");

            migrationBuilder.DropSequence(
                name: "section_hilo");

            migrationBuilder.DropSequence(
                name: "ticket_hilo");

            migrationBuilder.DropSequence(
                name: "venue_hilo");
        }
    }
}
