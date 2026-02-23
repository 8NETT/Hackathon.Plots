using API.Endpoints;
using API.Requests;
using API.Security;
using Application.DTOs;
using Application.UseCases.Propriedades.AlterarPropriedade;
using Application.UseCases.Propriedades.CadastrarPropriedade;
using Application.UseCases.Propriedades.ObterPropriedade;
using Application.UseCases.Propriedades.ObterPropriedadesDoProprietario;
using Application.UseCases.Propriedades.RemoverPropriedade;
using Ardalis.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hackathon.Plots.Controller.Tests.Endpoints;

public class PropriedadeEndpointsTests
{
    #region Dados Compartilhados

    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid PropriedadeId = Guid.NewGuid();

    private static readonly EnderecoDTO EnderecoDTOFake = new()
    {
        Logradouro = "Rua Teste",
        Numero = "123",
        Complemento = "Apto 1",
        Bairro = "Centro",
        Cidade = "São Paulo",
        UF = "SP",
        CEP = "01001-000"
    };

    private static readonly PropriedadeDTO PropriedadeDTOFake = new()
    {
        Id = PropriedadeId,
        ProprietarioId = UserId,
        Nome = "Fazenda Teste",
        Descricao = "Descrição teste",
        Endereco = EnderecoDTOFake
    };

    private static readonly CadastrarPropriedadeRequest CadastrarRequestValido = new()
    {
        Nome = "Fazenda Teste",
        Descricao = "Descrição teste",
        Endereco = new EnderecoRequest
        {
            Logradouro = "Rua Teste",
            Numero = "123",
            Complemento = "Apto 1",
            Bairro = "Centro",
            Cidade = "São Paulo",
            UF = "SP",
            CEP = "01001-000"
        }
    };

    private static readonly AlterarPropriedadeRequest AlterarRequestValido = new()
    {
        Nome = "Fazenda Alterada",
        Descricao = "Descrição alterada",
        Endereco = new EnderecoRequest
        {
            Logradouro = "Rua Nova",
            Numero = "456",
            Cidade = "Campinas",
            UF = "SP"
        }
    };

    #endregion

    #region Factory

    private static HttpClient CreateClient(Action<
        Mock<IObterPropriedadesDoProprietarioUseCase>,
        Mock<IObterPropriedadeUseCase>,
        Mock<ICadastrarPropriedadeUseCase>,
        Mock<IAlterarPropriedadeUseCase>,
        Mock<IRemoverPropriedadeUseCase>> configure)
    {
        var mockObterDoProprietario = new Mock<IObterPropriedadesDoProprietarioUseCase>();
        var mockObter = new Mock<IObterPropriedadeUseCase>();
        var mockCadastrar = new Mock<ICadastrarPropriedadeUseCase>();
        var mockAlterar = new Mock<IAlterarPropriedadeUseCase>();
        var mockRemover = new Mock<IRemoverPropriedadeUseCase>();

        configure(mockObterDoProprietario, mockObter, mockCadastrar, mockAlterar, mockRemover);

        var mockCurrentUser = new Mock<ICurrentUser>();
        mockCurrentUser.Setup(u => u.Id).Returns(UserId);

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Logging.ClearProviders();

        builder.Services
            .AddRouting()
            .AddAuthentication("Test")
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

        builder.Services
            .AddAuthorization()
            .AddSingleton(mockObterDoProprietario.Object)
            .AddSingleton(mockObter.Object)
            .AddSingleton(mockCadastrar.Object)
            .AddSingleton(mockAlterar.Object)
            .AddSingleton(mockRemover.Object)
            .AddSingleton(mockCurrentUser.Object);

        var app = builder.Build();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapPropriedadeEndpoints();

        app.StartAsync().GetAwaiter().GetResult();

        return app.GetTestClient();
    }

    #endregion

    #region GET /propriedades

    public class ObterPropriedadesDoProprietarioTests : PropriedadeEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((obterDoProprietario, _, _, _, _) =>
            {
                obterDoProprietario
                    .Setup(uc => uc.HandleAsync(UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<IEnumerable<PropriedadeDTO>>.Success(new[] { PropriedadeDTOFake }));
            });

            var response = await client.GetAsync("/propriedades");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

    #endregion

    #region GET /propriedades/{id}

    public class ObterPropriedadeTests : PropriedadeEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((_, obter, _, _, _) =>
            {
                obter
                    .Setup(uc => uc.HandleAsync(
                        It.Is<ObterPropriedadeDTO>(d => d.Id == PropriedadeId && d.UsuarioId == UserId),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Success(PropriedadeDTOFake));
            });

            var response = await client.GetAsync($"/propriedades/{PropriedadeId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar404_QuandoPropriedadeNaoEncontrada()
        {
            var client = CreateClient((_, obter, _, _, _) =>
            {
                obter
                    .Setup(uc => uc.HandleAsync(It.IsAny<ObterPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.NotFound());
            });

            var response = await client.GetAsync($"/propriedades/{PropriedadeId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar403_QuandoUsuarioNaoEProprietario()
        {
            var client = CreateClient((_, obter, _, _, _) =>
            {
                obter
                    .Setup(uc => uc.HandleAsync(It.IsAny<ObterPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Forbidden());
            });

            var response = await client.GetAsync($"/propriedades/{PropriedadeId}");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    #endregion

    #region POST /propriedades

    public class CadastrarPropriedadeTests : PropriedadeEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar201_QuandoSucesso()
        {
            var client = CreateClient((_, _, cadastrar, _, _) =>
            {
                cadastrar
                    .Setup(uc => uc.HandleAsync(
                        It.Is<CadastrarPropriedadeDTO>(d =>
                            d.UsuarioId == UserId &&
                            d.Nome == CadastrarRequestValido.Nome),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Success(PropriedadeDTOFake));
            });

            var response = await client.PostAsJsonAsync("/propriedades", CadastrarRequestValido);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains($"/propriedades/{PropriedadeId}", response.Headers.Location?.ToString());
        }

        [Fact]
        public async Task DeveRetornar400_QuandoDadosInvalidos()
        {
            var client = CreateClient((_, _, cadastrar, _, _) =>
            {
                cadastrar
                    .Setup(uc => uc.HandleAsync(It.IsAny<CadastrarPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Invalid(new List<ValidationError>
                    {
                        new() { Identifier = "Nome", ErrorMessage = "Nome é obrigatório." }
                    }));
            });

            var response = await client.PostAsJsonAsync("/propriedades", CadastrarRequestValido);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    #endregion

    #region PUT /propriedades/{id}

    public class AlterarPropriedadeTests : PropriedadeEndpointsTests
    {
        private static readonly PropriedadeDTO PropriedadeAlteradaDTOFake = new()
        {
            Id = PropriedadeId,
            ProprietarioId = UserId,
            Nome = "Fazenda Alterada",
            Descricao = "Descrição alterada",
            Endereco = new EnderecoDTO
            {
                Logradouro = "Rua Nova",
                Numero = "456",
                Cidade = "Campinas",
                UF = "SP"
            }
        };

        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((_, _, _, alterar, _) =>
            {
                alterar
                    .Setup(uc => uc.HandleAsync(
                        It.Is<AlterarPropriedadeDTO>(d =>
                            d.Id == PropriedadeId &&
                            d.UsuarioId == UserId &&
                            d.Nome == AlterarRequestValido.Nome),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Success(PropriedadeAlteradaDTOFake));
            });

            var response = await client.PutAsJsonAsync($"/propriedades/{PropriedadeId}", AlterarRequestValido);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar400_QuandoDadosInvalidos()
        {
            var client = CreateClient((_, _, _, alterar, _) =>
            {
                alterar
                    .Setup(uc => uc.HandleAsync(It.IsAny<AlterarPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Invalid(new List<ValidationError>
                    {
                        new() { Identifier = "Nome", ErrorMessage = "Nome é obrigatório." }
                    }));
            });

            var response = await client.PutAsJsonAsync($"/propriedades/{PropriedadeId}", AlterarRequestValido);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar404_QuandoPropriedadeNaoEncontrada()
        {
            var client = CreateClient((_, _, _, alterar, _) =>
            {
                alterar
                    .Setup(uc => uc.HandleAsync(It.IsAny<AlterarPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.NotFound());
            });

            var response = await client.PutAsJsonAsync($"/propriedades/{PropriedadeId}", AlterarRequestValido);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar403_QuandoUsuarioNaoEProprietario()
        {
            var client = CreateClient((_, _, _, alterar, _) =>
            {
                alterar
                    .Setup(uc => uc.HandleAsync(It.IsAny<AlterarPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Forbidden());
            });

            var response = await client.PutAsJsonAsync($"/propriedades/{PropriedadeId}", AlterarRequestValido);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    #endregion

    #region DELETE /propriedades/{id}

    public class RemoverPropriedadeTests : PropriedadeEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((_, _, _, _, remover) =>
            {
                remover
                    .Setup(uc => uc.HandleAsync(
                        It.Is<RemoverPropriedadeDTO>(d => d.Id == PropriedadeId && d.UsuarioId == UserId),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Success(PropriedadeDTOFake));
            });

            var response = await client.DeleteAsync($"/propriedades/{PropriedadeId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar404_QuandoPropriedadeNaoEncontrada()
        {
            var client = CreateClient((_, _, _, _, remover) =>
            {
                remover
                    .Setup(uc => uc.HandleAsync(It.IsAny<RemoverPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.NotFound());
            });

            var response = await client.DeleteAsync($"/propriedades/{PropriedadeId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar403_QuandoUsuarioNaoEProprietario()
        {
            var client = CreateClient((_, _, _, _, remover) =>
            {
                remover
                    .Setup(uc => uc.HandleAsync(It.IsAny<RemoverPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<PropriedadeDTO>.Forbidden());
            });

            var response = await client.DeleteAsync($"/propriedades/{PropriedadeId}");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    #endregion
}