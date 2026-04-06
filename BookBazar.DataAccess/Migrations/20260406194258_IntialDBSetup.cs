using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookBazar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IntialDBSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Self Help" },
                    { 2, 2, "Biography" },
                    { 3, 3, "Fiction" },
                    { 4, 4, "Historical Fiction" },
                    { 5, 5, "Thriller" },
                    { 6, 6, "Fantasy" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Thomas Erikson", 1, "A popular self-help book about understanding personality types and improving communication with different kinds of people.", "9781250179944", "/images/products/books/surrounded-by-idiots.jpg", 18.0, 16.0, 12.0, 14.0, "Surrounded by Idiots" },
                    { 2, "Michelle Obama", 2, "A memoir by Michelle Obama about her life, family, values, and journey from childhood to the White House.", "9781524763138", "/images/products/books/becoming.jpg", 20.0, 18.0, 14.0, 16.0, "Becoming" },
                    { 3, "Héctor García and Francesc Miralles", 1, "A self-help book exploring the Japanese idea of ikigai, or finding purpose, meaning, and joy in life.", "9780143130727", "/images/products/books/ikigai.jpg", 17.0, 15.0, 11.0, 13.0, "Ikigai" },
                    { 4, "James Clear", 1, "A bestselling self-improvement book about building good habits, breaking bad ones, and making small changes that produce big results.", "9780735211292", "/images/products/books/atomic-habits.jpg", 20.0, 18.0, 14.0, 16.0, "Atomic Habits" },
                    { 5, "Virginia Evans", 3, "A literary fiction novel centered on relationships, emotional depth, and human connection.", "9780593654477", "/images/products/books/the-correspondent.jpg", 19.0, 17.0, 13.0, 15.0, "The Correspondent" },
                    { 6, "Allen Levi", 3, "A reflective and emotional novel about life, memory, and meaningful human experiences.", "9781637632697", "/images/products/books/theo-of-golden.jpg", 18.0, 16.0, 12.0, 14.0, "Theo of Golden" },
                    { 7, "Joseph Murphy", 1, "A motivational self-help classic focused on confidence, belief, and the power of the mind.", "9789355994653", "/images/products/books/believe-in-yourself.jpg", 15.0, 13.0, 9.0, 11.0, "Believe in Yourself" },
                    { 8, "Anthony Doerr", 4, "A bestselling historical fiction novel set during World War II, following the lives of a blind French girl and a German boy.", "9781501173219", "/images/products/books/all-the-light-we-cannot-see.jpg", 19.0, 17.0, 13.0, 15.0, "All the Light We Cannot See" },
                    { 9, "Stacy Willingham", 5, "A psychological thriller with suspense, secrets, and disturbing twists tied to crimes from the past.", "9781250803825", "/images/products/books/a-flicker-in-the-dark.jpg", 18.0, 16.0, 12.0, 14.0, "A Flicker in the Dark" },
                    { 10, "Shubham Kumar Singh", 1, "A motivational and mindset-focused self-help book about mastering thoughts and shaping your life through positive thinking.", "9780143475279", "/images/products/books/you-become-what-you-think.jpg", 16.0, 14.0, 10.0, 12.0, "You Become What You Think" },
                    { 11, "Morgan Housel", 1, "A personal finance and self-development book explaining how behavior, emotions, and decision-making shape financial success.", "9780857197689", "/images/products/books/the-psychology-of-money.jpg", 18.0, 16.0, 12.0, 14.0, "The Psychology of Money" },
                    { 12, "Rebecca Yarros", 6, "A fantasy novel filled with dragons, war college training, danger, and romance.", "9781649374042", "/images/products/books/fourth-wing.jpg", 22.0, 20.0, 16.0, 18.0, "Fourth Wing" },
                    { 13, "Mel Robbins", 1, "A self-help and mindset book focused on letting go of control, reducing stress, and living with more freedom.", "9781401971366", "/images/products/books/the-let-them-theory.jpg", 19.0, 17.0, 13.0, 15.0, "The Let Them Theory" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
