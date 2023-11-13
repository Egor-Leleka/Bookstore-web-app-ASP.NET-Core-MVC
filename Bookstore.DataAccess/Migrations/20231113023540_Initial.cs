using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookstore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price50 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price100 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
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
                    { 1, 1, "Action" },
                    { 2, 2, "Sci-Fi" },
                    { 3, 3, "Comics" },
                    { 4, 4, "Fantasy" },
                    { 5, 5, "Horror" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "J. K. Rowling", 1, "Harry Potter has never even heard of Hogwarts when the letters start dropping on the doormat at number four, Privet Drive. Addressed in green ink on yellowish parchment with a purple seal, they are swiftly confiscated by his grisly aunt and uncle. Then, on Harry’s eleventh birthday, a great beetle-eyed giant of a man called Rubeus Hagrid bursts in with some astonishing news: Harry Potter is a wizard, and he has a place at Hogwarts School of Witchcraft and Wizardry. An incredible adventure is about to begin!\r\nJ.K. Rowling’s internationally bestselling Harry Potter books continue to captivate new generations of readers. Harry’s first adventure alongside his friends, Ron and Hermione, will whisk you away to Hogwarts, an enchanted, turreted castle filled with disappearing staircases, pearly-white ghosts and magical paintings that flit from frame to frame. This gorgeous paperback edition features a spectacular cover by award-winning artist Jonny Duddle, as well as refreshed bonus material including fun factsexploring the origins of names such as Albus Dumbledore, Hedwig and other favourite characters. This is the perfect starting point for anyone who’s ready to lose themselves in the biggest children’s books of all time.", "9781408855652", 0m, 21.99m, 15.99m, 18.99m, "Harry Potter and the Philosopher's Stone" },
                    { 2, "J. K. Rowling", 3, "Harry Potter’s summer has included the worst birthday ever, doomy warnings from a house-elf called Dobby and rescue from the Dursleys by his friend Ron Weasley in a magical flying car! Back at Hogwarts School of Witchcraft and Wizardry for his second year, Harry hears strange whispers echo through empty corridors – and then the attacks start. Students are found as though turned to stone … Dobby’s sinister predictions seem to be coming true.\r\nJ.K. Rowling’s internationally bestselling Harry Potter books continue to captivate new generations of readers. Harry’s second adventure alongside his friends, Ron and Hermione, invites you to explore even more of the wizarding world; from the waving, walloping branches of the Whomping Willow to the thrills of a rain-streaked Quidditch pitch. This gorgeous hardback edition features a spectacular cover by award-winning artist Jonny Duddle, plus refreshed bonus material, allowing readers to learn about wand woods and get to know the many members of the Weasley family. Perfect for anyone who’s ready to lose themselves in the biggest children’s books of all time.", "9781408855904", 0m, 21.99m, 15.99m, 18.99m, "Harry Potter and the Chamber of Secrets" },
                    { 3, "J. K. Rowling", 1, "Harry Potter, along with his best friends, Ron and Hermione, is about to start his third year at Hogwarts School of Witchcraft and Wizardry. Harary can't wait to get back to school after the summer holidays. (Who wouldn't if they lived with the horrible Dursleys?) But when Harry gets to Hogwarts, the atmosphere is tense. There's an escaped mass murderer on the loose, and the sinister prison guards of Azkaban have been called in to guard the school. 'The most eagerly awaited children's book for years.' - \"The Evening Standard\". 'The Harry Potter books are that rare thing, a series of stories adored by by parents and children alike'. - \"The Daily Telegraph\". All the Harry Potter books are now available in large print.", "9780747560777", 0m, 21.99m, 15.99m, 18.99m, "Harry Potter and the Prisoner of Azkaban" },
                    { 4, "J. K. Rowling", 2, "The Triwizard Tournament is to be held at Hogwarts. Only wizards who are over seventeen are allowed to enter – but that doesn’t stop Harry dreaming that he will win the competition. Then at Hallowe’en, when the Goblet of Fire makes its selection, Harry is amazed to find his name is one of those that the magical cup picks out. He will face death-defying tasks, dragons and Dark wizards, but with the help of his best friends, Ron and Hermione, he might just make it through – alive!\r\nJ.K. Rowling’s internationally bestselling Harry Potter books continue to captivate new generations of readers. Harry’s fourth adventure invites you to explore even more of the wizarding world; from the foggy, frozen depths of the Great Lake to the silvery secrets of the Pensieve. This gorgeous paperback edition features a spectacular cover by award-winning artist Jonny Duddle, plus refreshed bonus material, allowing readers to learn more about the different breeds of dragon. Perfect for anyone who’s ready to lose themselves in the biggest children’s books of all time.", "9781408855683", 0m, 21.99m, 15.99m, 18.99m, "Harry Potter and the Goblet of Fire" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
