using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvestments.Migrations
{
    /// <inheritdoc />
    public partial class Added_ClasseAtivoId_AND_Setor_To_Ativo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "ValorCorretagem",
                schema: "MyInvestments",
                table: "AppOperacoes",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<Guid>(
                name: "ClasseAtivoId",
                schema: "MyInvestments",
                table: "AppAtivos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SetorId",
                schema: "MyInvestments",
                table: "AppAtivos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppAtivos_ClasseAtivoId",
                schema: "MyInvestments",
                table: "AppAtivos",
                column: "ClasseAtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAtivos_SetorId",
                schema: "MyInvestments",
                table: "AppAtivos",
                column: "SetorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAtivos_AppClasseAtivos_ClasseAtivoId",
                schema: "MyInvestments",
                table: "AppAtivos",
                column: "ClasseAtivoId",
                principalSchema: "MyInvestments",
                principalTable: "AppClasseAtivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppAtivos_AppSetores_SetorId",
                schema: "MyInvestments",
                table: "AppAtivos",
                column: "SetorId",
                principalSchema: "MyInvestments",
                principalTable: "AppSetores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAtivos_AppClasseAtivos_ClasseAtivoId",
                schema: "MyInvestments",
                table: "AppAtivos");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAtivos_AppSetores_SetorId",
                schema: "MyInvestments",
                table: "AppAtivos");

            migrationBuilder.DropIndex(
                name: "IX_AppAtivos_ClasseAtivoId",
                schema: "MyInvestments",
                table: "AppAtivos");

            migrationBuilder.DropIndex(
                name: "IX_AppAtivos_SetorId",
                schema: "MyInvestments",
                table: "AppAtivos");

            migrationBuilder.DropColumn(
                name: "ClasseAtivoId",
                schema: "MyInvestments",
                table: "AppAtivos");

            migrationBuilder.DropColumn(
                name: "SetorId",
                schema: "MyInvestments",
                table: "AppAtivos");

            migrationBuilder.AlterColumn<float>(
                name: "ValorCorretagem",
                schema: "MyInvestments",
                table: "AppOperacoes",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
