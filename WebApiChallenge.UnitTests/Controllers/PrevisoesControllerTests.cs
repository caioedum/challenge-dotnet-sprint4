using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WebApiChallenge.Controllers;
using WebApiChallenge.DTO;
using WebApiChallenge.Interfaces;
using WebApiChallenge.Models;
using Xunit;

namespace WebApiChallenge.Tests.Controllers
{
    public class PrevisoesControllerTests
    {
        private readonly Mock<IPrevisaoRepository> _mockRepo;
        private readonly Mock<ILogger<PrevisoesController>> _mockLogger;
        private readonly PrevisoesController _controller;

        public PrevisoesControllerTests()
        {
            _mockRepo = new Mock<IPrevisaoRepository>();
            _mockLogger = new Mock<ILogger<PrevisoesController>>();
            _controller = new PrevisoesController(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public void Get_QuandoHaPrevisoes_RetornaOkComDados()
        {
            // Arrange
            var previsoes = new List<Previsao>
            {
                new Previsao { PrevisaoId = 1, UsuarioId = 1, ImagemId = 1,
                    PrevisaoTexto = "Texto 1", Probabilidade = 0.8f, Recomendacao = "Recomendo 1" },
                new Previsao { PrevisaoId = 2, UsuarioId = 2, ImagemId = 2,
                    PrevisaoTexto = "Texto 2", Probabilidade = 0.6f, Recomendacao = "Recomendo 2" }
            };
            _mockRepo.Setup(repo => repo.ObterTodos()).Returns(previsoes);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var previsoesDto = Assert.IsAssignableFrom<IEnumerable<PrevisaoDTO>>(okResult.Value);
            Assert.Equal(2, previsoesDto.Count());
        }

        [Fact]
        public void Get_QuandoNaoHaPrevisoes_RetornaNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ObterTodos()).Returns(new List<Previsao>());

            // Act
            var result = _controller.Get();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Nenhuma previsão encontrada.", notFoundResult.Value);
        }

        [Fact]
        public void GetPorId_QuandoPrevisaoExiste_RetornaOk()
        {
            // Arrange
            var previsao = new Previsao
            {
                PrevisaoId = 1,
                UsuarioId = 1,
                ImagemId = 1,
                PrevisaoTexto = "Texto",
                Probabilidade = 0.9f,
                Recomendacao = "Recomendo",
                DataPrevisao = System.DateTime.Now
            };
            _mockRepo.Setup(repo => repo.ObterPorId(1)).Returns(previsao);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var previsaoDto = Assert.IsType<PrevisaoDTO>(okResult.Value);
            Assert.Equal(previsao.PrevisaoTexto, previsaoDto.PrevisaoTexto);
        }

        [Fact]
        public void GetPorId_QuandoPrevisaoNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ObterPorId(It.IsAny<int>())).Returns((Previsao)null);

            // Act
            var result = _controller.Get(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("A previsão não foi encontrada.", notFoundResult.Value);
        }

        [Fact]
        public void Post_QuandoModeloInvalido_RetornaBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("PrevisaoTexto", "Obrigatório");
            var previsaoDto = new PrevisaoCreateDTO();

            // Act
            var result = _controller.Post(previsaoDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_QuandoPrevisaoExiste_AtualizaComSucesso()
        {
            // Arrange
            var previsaoExistente = new Previsao
            {
                PrevisaoId = 1,
                UsuarioId = 1,
                ImagemId = 1,
                PrevisaoTexto = "Texto Antigo"
            };
            _mockRepo.Setup(repo => repo.ObterPorId(1)).Returns(previsaoExistente);

            var previsaoDto = new PrevisaoCreateDTO
            {
                PrevisaoTexto = "Texto Novo",
                Probabilidade = 0.9f,
                Recomendacao = "Nova Recomendação"
            };

            // Act
            var result = _controller.Put(1, previsaoDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(previsaoDto.PrevisaoTexto, previsaoExistente.PrevisaoTexto);
            _mockRepo.Verify(repo => repo.AtualizarPrevisao(previsaoExistente), Times.Once);
        }

        [Fact]
        public void Put_QuandoPrevisaoNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ObterPorId(It.IsAny<int>())).Returns((Previsao)null);
            var previsaoDto = new PrevisaoCreateDTO();

            // Act
            var result = _controller.Put(999, previsaoDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("A previsão não foi encontrada.", notFoundResult.Value);
        }

        [Fact]
        public void Put_QuandoDadosNulos_RetornaBadRequest()
        {
            // Act
            var result = _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Dados inválidos.", badRequestResult.Value);
        }

        [Fact]
        public void Delete_QuandoPrevisaoExiste_DeletaComSucesso()
        {
            // Arrange
            var previsaoExistente = new Previsao { PrevisaoId = 1 };
            _mockRepo.Setup(repo => repo.ObterPorId(1)).Returns(previsaoExistente);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockRepo.Verify(repo => repo.DeletarPrevisao(1), Times.Once);
        }

        [Fact]
        public void Delete_QuandoPrevisaoNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ObterPorId(It.IsAny<int>())).Returns((Previsao)null);

            // Act
            var result = _controller.Delete(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("A previsão não foi encontrada.", notFoundResult.Value);
        }
    }
}
