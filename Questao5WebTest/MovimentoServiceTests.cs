// Questao5Web.Tests/Controllers/MovimentoControllerTests.cs
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Questao5Web.Controllers;
using Questao5Web.Models; // Adapte conforme o nome real do seu modelo
using System;
using Questao5Web.Dto;

public class MovimentoControllerTests
{
    private readonly MovimentoController _controller;

    public MovimentoControllerTests()
    {
        // Configure mocks aqui se necessário
        _controller = new MovimentoController(/* Dependências aqui */);
    }

    [Fact]
    public async Task TestarCadastroMovimento_Valido_Retorna200()
    {
        var request = new MovimentoRequest
        {
            IdContaCorrente = "1",
            Valor = 100.00m,
            TipoMovimento = "C"
        };

        var result = await _controller.CadastrarMovimento(request) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<string>(result.Value); // Verifique o tipo do valor retornado
    }

    [Fact]
    public async Task TestarCadastroMovimento_ContaInvalida_Retorna400()
    {
        var request = new MovimentoRequest
        {
            IdContaCorrente = "9999",
            Valor = 100.00m,
            TipoMovimento = "C"
        };

        var result = await _controller.CadastrarMovimento(request) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        var response = result.Value as dynamic;
        Assert.Equal("INVALID_ACCOUNT", response.Tipo);
        Assert.Equal("Conta corrente não encontrada.", response.Mensagem);
    }

    [Fact]
    public async Task TestarCadastroMovimento_ValorInvalido_Retorna400()
    {
        var request = new MovimentoRequest
        {
            IdContaCorrente = "1",
            Valor = -100.00m,
            TipoMovimento = "C"
        };

        var result = await _controller.CadastrarMovimento(request) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        var response = result.Value as dynamic;
        Assert.Equal("INVALID_VALUE", response.Tipo);
        Assert.Equal("O valor da movimentação deve ser positivo.", response.Mensagem);
    }
}
