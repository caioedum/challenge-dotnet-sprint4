using Microsoft.AspNetCore.Mvc;
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
    public class DentistasControllerTests
    {
        private readonly Mock<IDentistaRepository> _mockRepo;
        private readonly DentistasController _controller;

        public DentistasControllerTests()
        {
            _mockRepo = new Mock<IDentistaRepository>();
            _controller = new DentistasController(_mockRepo.Object);
        }

        // Testes para GET (lista)
        [Fact]
        public void Get_QuandoHaDentistas_RetornaOk()
        {
            // Arrange
            var dentistas = new List<Dentista>
            {
                new Dentista { DentistaId = 1, NomeDentista = "Dr. Silva" },
                new Dentista { DentistaId = 2, NomeDentista = "Dr. Costa" }
            };
            _mockRepo.Setup(repo => repo.ObterTodos()).Returns(dentistas);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dentistasDto = Assert.IsAssignableFrom<IEnumerable<DentistaDTO>>(okResult.Value);
            Assert.Equal(2, dentistasDto.Count());
        }

        // Testes para GET por ID
        [Fact]
        public void GetPorId_QuandoDentistaExiste_RetornaOk()
        {
            // Arrange
            var dentista = new Dentista
            {
                DentistaId = 1,
                NomeDentista = "Dr. Silva",
                EmailDentista = "silva@email.com"
            };
            _mockRepo.Setup(repo => repo.ObterPorId(1)).Returns(dentista);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dentistaDto = Assert.IsType<DentistaDTO>(okResult.Value);
            Assert.Equal("silva@email.com", dentistaDto.NomeDentista);
            Assert.Equal("silva@email.com", dentistaDto.EmailDentista);
        }

        [Fact]
        public void GetPorId_QuandoDentistaNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ObterPorId(It.IsAny<int>())).Returns((Dentista)null);

            // Act
            var result = _controller.Get(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Testes para POST
        [Fact]
        public void Post_QuandoModeloValido_CriaDentista()
        {
            // Arrange
            var dentistaDto = new DentistaCreateDTO
            {
                NomeDentista = "Dr. Novo",
                Especialidade = "Ortodontia",
                TelefoneDentista = "(11) 1234-5678",
                EmailDentista = "novo@email.com"
            };

            // Act
            var result = _controller.Post(dentistaDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdResult.ActionName);
            _mockRepo.Verify(repo => repo.AdicionarDentista(It.IsAny<Dentista>()), Times.Once);
        }

        [Fact]
        public void Post_QuandoModeloInvalido_RetornaBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("NomeDentista", "O campo é obrigatório");
            var dentistaDto = new DentistaCreateDTO();

            // Act
            var result = _controller.Post(dentistaDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Testes para PUT
        [Fact]
        public void Put_QuandoDentistaExiste_AtualizaComSucesso()
        {
            // Arrange
            var dentistaExistente = new Dentista
            {
                DentistaId = 1,
                NomeDentista = "Antigo",
                EmailDentista = "antigo@email.com"
            };
            _mockRepo.Setup(repo => repo.ObterPorId(1)).Returns(dentistaExistente);

            var dentistaDto = new DentistaCreateDTO
            {
                NomeDentista = "Novo",
                Especialidade = "Especialidade Nova",
                TelefoneDentista = "(11) 6543-2109",
                EmailDentista = "novo@email.com"
            };

            // Act
            var result = _controller.Put(1, dentistaDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Novo", dentistaExistente.NomeDentista);
            Assert.Equal("novo@email.com", dentistaExistente.EmailDentista);
            _mockRepo.Verify(repo => repo.AtualizarDentista(dentistaExistente), Times.Once);
        }

        [Fact]
        public void Put_QuandoDentistaNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ObterPorId(It.IsAny<int>())).Returns((Dentista)null);
            var dentistaDto = new DentistaCreateDTO { NomeDentista = "Teste" };

            // Act
            var result = _controller.Put(999, dentistaDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Put_QuandoDadosNulos_RetornaBadRequest()
        {
            // Arrange
            var dentistaDto = (DentistaCreateDTO)null;

            // Act
            var result = _controller.Put(1, dentistaDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
