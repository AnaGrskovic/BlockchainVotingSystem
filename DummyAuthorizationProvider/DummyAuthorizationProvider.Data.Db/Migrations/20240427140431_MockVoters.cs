using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DummyAuthorizationProvider.Data.Db.Migrations
{
    /// <inheritdoc />
    public partial class MockVoters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Voters",
                columns: new[] { "Id", "Oib" },
                values: new object[,]
                {
                    { new Guid("0376b3e7-8876-4dcd-b71c-15dae0c0a53c"), "00000000040" },
                    { new Guid("050e0823-0cef-4764-9ed1-920d832eb1a3"), "00000000032" },
                    { new Guid("06965835-b7cc-4dc4-b7b4-cecf1ec9e46a"), "00000000043" },
                    { new Guid("0714a2e8-bc14-43f5-aecd-c904675b3f35"), "00000000004" },
                    { new Guid("0e8e628b-628b-4103-8a25-79aa44944ca3"), "00000000028" },
                    { new Guid("11f35f4d-2fd3-4a6e-baea-8dbd4a9811b0"), "00000000033" },
                    { new Guid("14efabc6-c80e-47b7-84ba-e0241f75fecb"), "00000000016" },
                    { new Guid("15dfbe09-d006-4cbc-b7be-b68a5036ea71"), "00000000044" },
                    { new Guid("23c1086e-113b-40bb-ac2c-331d7487f238"), "00000000008" },
                    { new Guid("24d63f3e-02ac-43eb-b238-0f2a1efba22a"), "00000000035" },
                    { new Guid("2ee552cc-4982-4527-9a5e-148d6b6d070b"), "00000000031" },
                    { new Guid("3d727859-8627-4d7f-802c-c8611fc82a05"), "00000000036" },
                    { new Guid("420c3031-4b2c-4d52-9b2a-806eaeac0900"), "00000000026" },
                    { new Guid("4b13bba5-2dcb-4872-9156-14a5976c7832"), "00000000006" },
                    { new Guid("4c4cdf76-d4db-4e01-8e78-eac93cf30369"), "00000000049" },
                    { new Guid("4d684fcc-37fe-4d62-8791-1e0b2c6c0dbb"), "00000000009" },
                    { new Guid("4dab138b-36ea-4bd1-85a0-c6d6adee6882"), "00000000023" },
                    { new Guid("4de98897-0873-4a06-9e05-7c1c7a5c8d39"), "00000000050" },
                    { new Guid("4f77a22d-50ec-49bd-b3a2-7592b9f54bf8"), "00000000013" },
                    { new Guid("5226970a-a424-472f-9adc-28c9a9c1f582"), "00000000030" },
                    { new Guid("59c375e6-458a-4c39-9049-9073a8a749b3"), "00000000048" },
                    { new Guid("622f67b2-026f-4027-93d3-9d58ca835de2"), "00000000025" },
                    { new Guid("6c5f2f0a-d9d2-4219-992f-75f42fce0bde"), "00000000018" },
                    { new Guid("6d87ae4c-851b-44af-a671-9855cf1c0201"), "00000000021" },
                    { new Guid("7a5aaa2a-be5a-437a-92bf-46c41c8e6081"), "00000000017" },
                    { new Guid("81eee5a4-5947-45e3-8411-1a446666e73b"), "00000000041" },
                    { new Guid("8635190e-0285-4c86-ab4b-ac3f344303e0"), "00000000019" },
                    { new Guid("895eebed-2a48-4182-9394-bfcd7b675e03"), "00000000005" },
                    { new Guid("8a0c95d0-bc55-422d-9748-d80f34e457b2"), "00000000045" },
                    { new Guid("9169b49b-3f98-4d2b-8e36-0ceb08ce65f5"), "00000000001" },
                    { new Guid("92c46b9a-cbc7-4f13-be4b-0626145d6524"), "00000000029" },
                    { new Guid("a62727fa-4d54-4277-a184-3911ceabfc3d"), "00000000027" },
                    { new Guid("ae1e3fb9-57e3-4a75-b029-1d7d7a6c69f6"), "00000000011" },
                    { new Guid("b7bbf18d-2894-4797-a2ab-356687d64f93"), "00000000038" },
                    { new Guid("c1fad324-2bb6-4f28-9b4d-9535fed61b4e"), "00000000020" },
                    { new Guid("c5b9b9cb-7f2d-4d9a-b8ea-556ddd1a6149"), "00000000034" },
                    { new Guid("c5e55d16-a17e-4a7b-88c6-09f0a1bcc0fd"), "00000000024" },
                    { new Guid("cabd51ed-4b6f-4996-a6c9-cda80dfa19a8"), "00000000046" },
                    { new Guid("cafbc536-a8b1-4cdb-b04d-05a7e3fe1500"), "00000000047" },
                    { new Guid("ce00cbc2-4589-4350-8c1e-dd6cd8e26ba0"), "00000000022" },
                    { new Guid("cf753e5a-4fd9-4927-bdba-b6520bc93950"), "00000000003" },
                    { new Guid("d37c0111-a44a-433a-a794-b8ebcba56179"), "00000000039" },
                    { new Guid("dd390018-734d-4003-a571-516ae9cef419"), "00000000042" },
                    { new Guid("df9db4f3-c6bf-4892-9f64-6f38b2883818"), "00000000002" },
                    { new Guid("e8d503a6-9e58-4eda-bf3d-a478d1c973a1"), "00000000015" },
                    { new Guid("ea7c8d1a-3d26-4ac9-b963-38a00429527c"), "00000000012" },
                    { new Guid("f0050ba1-1cdd-4da6-b563-6d0171a39cc8"), "00000000037" },
                    { new Guid("f2ed2247-6e79-45b9-8785-200e71daea61"), "00000000014" },
                    { new Guid("f4aec2a2-2963-4db2-9a10-912d6216bcbd"), "00000000007" },
                    { new Guid("fac2dbf2-f664-49fc-a018-476ba8cfdae9"), "00000000010" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("0376b3e7-8876-4dcd-b71c-15dae0c0a53c"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("050e0823-0cef-4764-9ed1-920d832eb1a3"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("06965835-b7cc-4dc4-b7b4-cecf1ec9e46a"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("0714a2e8-bc14-43f5-aecd-c904675b3f35"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("0e8e628b-628b-4103-8a25-79aa44944ca3"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("11f35f4d-2fd3-4a6e-baea-8dbd4a9811b0"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("14efabc6-c80e-47b7-84ba-e0241f75fecb"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("15dfbe09-d006-4cbc-b7be-b68a5036ea71"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("23c1086e-113b-40bb-ac2c-331d7487f238"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("24d63f3e-02ac-43eb-b238-0f2a1efba22a"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("2ee552cc-4982-4527-9a5e-148d6b6d070b"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("3d727859-8627-4d7f-802c-c8611fc82a05"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("420c3031-4b2c-4d52-9b2a-806eaeac0900"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("4b13bba5-2dcb-4872-9156-14a5976c7832"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("4c4cdf76-d4db-4e01-8e78-eac93cf30369"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("4d684fcc-37fe-4d62-8791-1e0b2c6c0dbb"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("4dab138b-36ea-4bd1-85a0-c6d6adee6882"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("4de98897-0873-4a06-9e05-7c1c7a5c8d39"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("4f77a22d-50ec-49bd-b3a2-7592b9f54bf8"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("5226970a-a424-472f-9adc-28c9a9c1f582"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("59c375e6-458a-4c39-9049-9073a8a749b3"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("622f67b2-026f-4027-93d3-9d58ca835de2"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("6c5f2f0a-d9d2-4219-992f-75f42fce0bde"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("6d87ae4c-851b-44af-a671-9855cf1c0201"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("7a5aaa2a-be5a-437a-92bf-46c41c8e6081"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("81eee5a4-5947-45e3-8411-1a446666e73b"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("8635190e-0285-4c86-ab4b-ac3f344303e0"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("895eebed-2a48-4182-9394-bfcd7b675e03"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("8a0c95d0-bc55-422d-9748-d80f34e457b2"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("9169b49b-3f98-4d2b-8e36-0ceb08ce65f5"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("92c46b9a-cbc7-4f13-be4b-0626145d6524"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("a62727fa-4d54-4277-a184-3911ceabfc3d"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("ae1e3fb9-57e3-4a75-b029-1d7d7a6c69f6"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("b7bbf18d-2894-4797-a2ab-356687d64f93"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("c1fad324-2bb6-4f28-9b4d-9535fed61b4e"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("c5b9b9cb-7f2d-4d9a-b8ea-556ddd1a6149"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("c5e55d16-a17e-4a7b-88c6-09f0a1bcc0fd"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("cabd51ed-4b6f-4996-a6c9-cda80dfa19a8"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("cafbc536-a8b1-4cdb-b04d-05a7e3fe1500"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("ce00cbc2-4589-4350-8c1e-dd6cd8e26ba0"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("cf753e5a-4fd9-4927-bdba-b6520bc93950"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("d37c0111-a44a-433a-a794-b8ebcba56179"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("dd390018-734d-4003-a571-516ae9cef419"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("df9db4f3-c6bf-4892-9f64-6f38b2883818"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("e8d503a6-9e58-4eda-bf3d-a478d1c973a1"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("ea7c8d1a-3d26-4ac9-b963-38a00429527c"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("f0050ba1-1cdd-4da6-b563-6d0171a39cc8"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("f2ed2247-6e79-45b9-8785-200e71daea61"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("f4aec2a2-2963-4db2-9a10-912d6216bcbd"));

            migrationBuilder.DeleteData(
                table: "Voters",
                keyColumn: "Id",
                keyValue: new Guid("fac2dbf2-f664-49fc-a018-476ba8cfdae9"));
        }
    }
}
