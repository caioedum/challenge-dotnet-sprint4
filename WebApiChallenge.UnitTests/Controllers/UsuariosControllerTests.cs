using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiChallenge.Controllers;
using WebApiChallenge.DTO;
using WebApiChallenge.Interfaces;
using WebApiChallenge.Models;
using Xunit;

namespace WebApiChallenge.Tests.Controllers
{
    public class UsuariosControllerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepo;
        private readonly UsuariosController _controller;

        public UsuariosControllerTests()
        {
            _mockRepo = new Mock<IUsuarioRepository>();
            _controller = new UsuariosController(_mockRepo.Object);
        }

        [Fact]
        public void Get_QuandoHaUsuarios_RetornaOkComDados()
        {
            // Arrange
            var usuarios = new List<Usuario>
            {
                new Usuario { UsuarioId = 1, Nome = "João" },
                new Usuario { UsuarioId = 2, Nome = "Maria" }
            };

            _mockRepo.Setup(repo => repo.ObterTodos()).Returns(usuarios);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var usuariosDto = Assert.IsAssignableFrom<IEnumerable<UsuarioDTO>>(okResult.Value);
            Assert.Equal(2, usuariosDto.Count());
        }

        [Fact]
        public void GetPorId_QuandoUsuarioExiste_RetornaUsuario()
        {
            // Arrange
            var usuario = new Usuario { UsuarioId = 1, Nome = "João" };
            _mockRepo.Setup(repo => repo.ObterPorId(1)).Returns(usuario);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var usuarioDto = Assert.IsType<UsuarioDTO>(okResult.Value);
            Assert.Equal("João", usuarioDto.Nome);
        }

        [Fact]
        public void GetPorId_QuandoUsuarioNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ObterPorId(It.IsAny<int>())).Returns((Usuario)null);

            // Act
            var result = _controller.Get(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Post_QuandoModeloValido_RetornaCreated()
        {
            // Arrange
            var usuarioDto = new UsuarioCreateDTO
            {
                Nome = "Novo",
                Sobrenome = "Usuário",
                Cpf = "12345678901"
            };

            // Act
            var result = _controller.Post(usuarioDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdResult.ActionName);
            _mockRepo.Verify(repo => repo.AdicionarUsuario(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void Post_QuandoModeloInvalido_RetornaBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Nome", "Obrigatório");
            var usuarioDto = new UsuarioCreateDTO();

            // Act
            var result = _controller.Post(usuarioDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_QuandoUsuarioExiste_AtualizaComSucesso()
        {
            // Arrange
            var usuarioExistente = new Usuario { UsuarioId = 1, Nome = "Antigo" };
            _mockRepo.Setup(repo => repo.ObterPorId(1)).Returns(usuarioExistente);

            var usuarioDto = new UsuarioCreateDTO
            {
                Nome = "Novo",
                Sobrenome = "Atualizado"
            };

            // Act
            var result = _controller.Put(1, usuarioDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Novo", usuarioExistente.Nome);
            _mockRepo.Verify(repo => repo.AtualizarUsuario(usuarioExistente), Times.Once);
        }

        [Fact]
        public void Put_QuandoUsuarioNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ObterPorId(It.IsAny<int>())).Returns((Usuario)null);
            var usuarioDto = new UsuarioCreateDTO();

            // Act
            var result = _controller.Put(999, usuarioDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Put_QuandoDadosInvalidos_RetornaBadRequest()
        {
            // Arrange
            var usuarioDto = (UsuarioCreateDTO)null;

            // Act
            var result = _controller.Put(1, usuarioDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
