using API.Endpoints;
using API.Requests;
using API.Security;
using Application.DTOs;
using Application.UseCases.Talhoes.AlterarTalhao;
using Application.UseCases.Talhoes.CadastrarTalhao;
using Application.UseCases.Talhoes.ObterTalhao;
using Application.UseCases.Talhoes.ObterTalhoesDaPropriedade;
using Application.UseCases.Talhoes.ObterTalhoesDoProprietario;
using Application.UseCases.Talhoes.RemoverTalhao;
using Ardalis.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Xunit;

namespace Hackathon.Plots.Controller.Tests.Endpoints;

#region Auth Handler

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "test-user") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

#endregion

public class TalhaoEndpointsTests
{
    #region Dados Compartilhados

    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid TalhaoId = Guid.NewGuid();
    private static readonly Guid PropriedadeId = Guid.NewGuid();

    private static readonly CoordenadasDTO CoordenadasFake = new()
    {
        Latitude = -23.5505,
        Longitude = -46.6333
    };

    private static readonly TalhaoDTO TalhaoDTOFake = new()
    {
        Id = TalhaoId,
        PropriedadeId = PropriedadeId,
        Nome = "Talhão Teste",
        Descricao = "Descrição teste",
        Coordenadas = CoordenadasFake,
        Area = 10.5m
    };

    private static readonly CadastrarTalhaoRequest CadastrarRequestValido = new()
    {
        PropriedadeId = PropriedadeId,
        Nome = "Talhão Teste",
        Descricao = "Descrição teste",
        Coordenadas = new CoordenadasRequest { Latitude = -23.5505, Longitude = -46.6333 },
        Area = 10.5m
    };

    private static readonly AlterarTalhaoRequest AlterarRequestValido = new()
    {
        PropriedadeId = PropriedadeId,
        Nome = "Talhão Alterado",
        Descricao = "Descrição alterada",
        Coordenadas = new CoordenadasRequest { Latitude = -23.5505, Longitude = -46.6333 },
        Area = 20.0m
    };

    #endregion

    #region Factory

    private static HttpClient CreateClient(Action<
        Mock<IObterTalhoesDoProprietarioUseCase>,
        Mock<IObterTalhaoUseCase>,
        Mock<IObterTalhoesDaPropriedadeUseCase>,
        Mock<ICadastrarTalhaoUseCase>,
        Mock<IAlterarTalhaoUseCase>,
        Mock<IRemoverTalhaoUseCase>> configure)
    {
        var mockObterDoProprietario = new Mock<IObterTalhoesDoProprietarioUseCase>();
        var mockObterTalhao = new Mock<IObterTalhaoUseCase>();
        var mockObterDaPropriedade = new Mock<IObterTalhoesDaPropriedadeUseCase>();
        var mockCadastrar = new Mock<ICadastrarTalhaoUseCase>();
        var mockAlterar = new Mock<IAlterarTalhaoUseCase>();
        var mockRemover = new Mock<IRemoverTalhaoUseCase>();

        configure(mockObterDoProprietario, mockObterTalhao, mockObterDaPropriedade,
                  mockCadastrar, mockAlterar, mockRemover);

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
            .AddSingleton(mockObterTalhao.Object)
            .AddSingleton(mockObterDaPropriedade.Object)
            .AddSingleton(mockCadastrar.Object)
            .AddSingleton(mockAlterar.Object)
            .AddSingleton(mockRemover.Object)
            .AddSingleton(mockCurrentUser.Object);

        var app = builder.Build();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapTalhaoEndpoints();

        app.StartAsync().GetAwaiter().GetResult();

        return app.GetTestClient();
    }

    #endregion

    #region GET /talhao

    public class ObterTalhoesDoProprietarioTests : TalhaoEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((obterDoProprietario, _, _, _, _, _) =>
            {
                obterDoProprietario
                    .Setup(uc => uc.HandleAsync(UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<IEnumerable<TalhaoDTO>>.Success(new[] { TalhaoDTOFake }));
            });

            var response = await client.GetAsync("/talhao");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

    #endregion

    #region GET /talhao/{id}

    public class ObterTalhaoTests : TalhaoEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((_, obterTalhao, _, _, _, _) =>
            {
                obterTalhao
                    .Setup(uc => uc.HandleAsync(
                        It.Is<ObterTalhaoDTO>(d => d.Id == TalhaoId && d.UsuarioId == UserId),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Success(TalhaoDTOFake));
            });

            var response = await client.GetAsync($"/talhao/{TalhaoId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar404_QuandoTalhaoNaoEncontrado()
        {
            var client = CreateClient((_, obterTalhao, _, _, _, _) =>
            {
                obterTalhao
                    .Setup(uc => uc.HandleAsync(It.IsAny<ObterTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.NotFound());
            });

            var response = await client.GetAsync($"/talhao/{TalhaoId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar403_QuandoUsuarioNaoEProprietario()
        {
            var client = CreateClient((_, obterTalhao, _, _, _, _) =>
            {
                obterTalhao
                    .Setup(uc => uc.HandleAsync(It.IsAny<ObterTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Forbidden());
            });

            var response = await client.GetAsync($"/talhao/{TalhaoId}");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    #endregion

    #region GET /talhao/propriedade/{propriedadeId}

    public class ObterTalhoesDaPropriedadeTests : TalhaoEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((_, _, obterDaPropriedade, _, _, _) =>
            {
                obterDaPropriedade
                    .Setup(uc => uc.HandleAsync(
                        It.Is<ObterTalhoesDaPropriedadeDTO>(d =>
                            d.PropriedadeId == PropriedadeId && d.UsuarioId == UserId),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<IEnumerable<TalhaoDTO>>.Success(new[] { TalhaoDTOFake }));
            });

            var response = await client.GetAsync($"/talhao/propriedade/{PropriedadeId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar403_QuandoUsuarioNaoEProprietario()
        {
            var client = CreateClient((_, _, obterDaPropriedade, _, _, _) =>
            {
                obterDaPropriedade
                    .Setup(uc => uc.HandleAsync(It.IsAny<ObterTalhoesDaPropriedadeDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<IEnumerable<TalhaoDTO>>.Forbidden());
            });

            var response = await client.GetAsync($"/talhao/propriedade/{PropriedadeId}");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    #endregion

    #region POST /talhao

    public class CadastrarTalhaoTests : TalhaoEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar201_QuandoSucesso()
        {
            var client = CreateClient((_, _, _, cadastrar, _, _) =>
            {
                cadastrar
                    .Setup(uc => uc.HandleAsync(
                        It.Is<CadastrarTalhaoDTO>(d =>
                            d.UsuarioId == UserId &&
                            d.PropriedadeId == PropriedadeId &&
                            d.Nome == CadastrarRequestValido.Nome &&
                            d.Area == CadastrarRequestValido.Area),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Success(TalhaoDTOFake));
            });

            var response = await client.PostAsJsonAsync("/talhao", CadastrarRequestValido);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains($"/talhao/{TalhaoId}", response.Headers.Location?.ToString());
        }

        [Fact]
        public async Task DeveRetornar400_QuandoDadosInvalidos()
        {
            var client = CreateClient((_, _, _, cadastrar, _, _) =>
            {
                cadastrar
                    .Setup(uc => uc.HandleAsync(It.IsAny<CadastrarTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Invalid(new List<ValidationError>
                    {
                        new() { Identifier = "Nome", ErrorMessage = "Nome é obrigatório." }
                    }));
            });

            var response = await client.PostAsJsonAsync("/talhao", CadastrarRequestValido);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar404_QuandoPropriedadeNaoEncontrada()
        {
            var client = CreateClient((_, _, _, cadastrar, _, _) =>
            {
                cadastrar
                    .Setup(uc => uc.HandleAsync(It.IsAny<CadastrarTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.NotFound());
            });

            var response = await client.PostAsJsonAsync("/talhao", CadastrarRequestValido);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar403_QuandoUsuarioNaoEProprietario()
        {
            var client = CreateClient((_, _, _, cadastrar, _, _) =>
            {
                cadastrar
                    .Setup(uc => uc.HandleAsync(It.IsAny<CadastrarTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Forbidden());
            });

            var response = await client.PostAsJsonAsync("/talhao", CadastrarRequestValido);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    #endregion

    #region PUT /talhao/{id}

    public class AlterarTalhaoTests : TalhaoEndpointsTests
    {
        private static readonly TalhaoDTO TalhaoAlteradoDTOFake = new()
        {
            Id = TalhaoId,
            PropriedadeId = PropriedadeId,
            Nome = "Talhão Alterado",
            Descricao = "Descrição alterada",
            Coordenadas = CoordenadasFake,
            Area = 20.0m
        };

        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((_, _, _, _, alterar, _) =>
            {
                alterar
                    .Setup(uc => uc.HandleAsync(
                        It.Is<AlterarTalhaoDTO>(d =>
                            d.Id == TalhaoId &&
                            d.UsuarioId == UserId &&
                            d.Nome == AlterarRequestValido.Nome &&
                            d.Area == AlterarRequestValido.Area),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Success(TalhaoAlteradoDTOFake));
            });

            var response = await client.PutAsJsonAsync($"/talhao/{TalhaoId}", AlterarRequestValido);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar400_QuandoDadosInvalidos()
        {
            var client = CreateClient((_, _, _, _, alterar, _) =>
            {
                alterar
                    .Setup(uc => uc.HandleAsync(It.IsAny<AlterarTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Invalid(new List<ValidationError>
                    {
                        new() { Identifier = "Area", ErrorMessage = "Área deve ser maior que zero." }
                    }));
            });

            var response = await client.PutAsJsonAsync($"/talhao/{TalhaoId}", AlterarRequestValido);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar404_QuandoTalhaoNaoEncontrado()
        {
            var client = CreateClient((_, _, _, _, alterar, _) =>
            {
                alterar
                    .Setup(uc => uc.HandleAsync(It.IsAny<AlterarTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.NotFound());
            });

            var response = await client.PutAsJsonAsync($"/talhao/{TalhaoId}", AlterarRequestValido);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar403_QuandoUsuarioNaoEProprietario()
        {
            var client = CreateClient((_, _, _, _, alterar, _) =>
            {
                alterar
                    .Setup(uc => uc.HandleAsync(It.IsAny<AlterarTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Forbidden());
            });

            var response = await client.PutAsJsonAsync($"/talhao/{TalhaoId}", AlterarRequestValido);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    #endregion

    #region DELETE /talhao/{id}

    public class RemoverTalhaoTests : TalhaoEndpointsTests
    {
        [Fact]
        public async Task DeveRetornar200_QuandoSucesso()
        {
            var client = CreateClient((_, _, _, _, _, remover) =>
            {
                remover
                    .Setup(uc => uc.HandleAsync(
                        It.Is<RemoverTalhaoDTO>(d => d.Id == TalhaoId && d.UsuarioId == UserId),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Success(TalhaoDTOFake));
            });

            var response = await client.DeleteAsync($"/talhao/{TalhaoId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar404_QuandoTalhaoNaoEncontrado()
        {
            var client = CreateClient((_, _, _, _, _, remover) =>
            {
                remover
                    .Setup(uc => uc.HandleAsync(It.IsAny<RemoverTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.NotFound());
            });

            var response = await client.DeleteAsync($"/talhao/{TalhaoId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar403_QuandoUsuarioNaoEProprietario()
        {
            var client = CreateClient((_, _, _, _, _, remover) =>
            {
                remover
                    .Setup(uc => uc.HandleAsync(It.IsAny<RemoverTalhaoDTO>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result<TalhaoDTO>.Forbidden());
            });

            var response = await client.DeleteAsync($"/talhao/{TalhaoId}");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

    #endregion
}