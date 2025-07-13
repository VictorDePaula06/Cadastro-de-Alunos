using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CrudAlunos.Data;
using CrudAlunos.Controllers;
using CrudAlunos.Models;
using CrudAlunos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace PastaDeTestes
{
    public class ContatoTestes
    {
        [Fact] // Teste para cadastrar um contato valido
        public async Task PostAsync_CriarContatoValido()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            var model = new CreateContatoViewModel
            {
                Nome = "Victor de Paula",
                Sexo = "Masculino",
                DataNascimento = "13/07/2007" // Esta completando 18 anos hoje, logo o sistema deve aceitar sem problemas
            };

            //act
            var resultado = await controller.PostAsync(context, model);

            //assert
            Assert.IsType<CreatedResult>(resultado);

            var contato = await context.Contatos.FirstOrDefaultAsync(contato => contato.Nome == "Victor de Paula");
            Assert.NotNull(contato);
            Assert.Equal("Victor de Paula", contato.Nome);
            Assert.Equal("Masculino", contato.Sexo);
            Assert.True(contato.Ativo);
        }

        [Fact] //Teste para validar o formato da data
        public async Task PostAsync_FormatoData()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            var model = new CreateContatoViewModel
            {
                Nome = "Victor de Paula",
                Sexo = "Masculino",
                DataNascimento = "3/07/2007" // Formato da data esta invalido
            };

            //act
            var resultado = await controller.PostAsync(context, model);
            //assert
            Assert.IsType<BadRequestObjectResult>(resultado);
            var BadRequest = resultado as BadRequestObjectResult;
            Assert.Equal("Data de nascimento no formato invalido! Tente novamente seguindo o formato -> dia/mes/ano(dd/MM/yyyy)", BadRequest.Value);
        }

        [Fact] //Teste para validar a idade
        public async Task PostAsync_ValidarIdade()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            var model = new CreateContatoViewModel
            {
                Nome = "Victor de Paula",
                Sexo = "Masculino",
                DataNascimento = "14/07/2007" // Vai fazer 18 amanha
            };

            //act
            var resultado = await controller.PostAsync(context, model);
            //assert
            Assert.IsType<BadRequestObjectResult>(resultado);
            var BadRequest = resultado as BadRequestObjectResult;
            Assert.Equal("Nao podemos cadastrar menores de idade", BadRequest.Value);
        }


        [Fact] //Teste pegar os contatos ativos
        public async Task PostAsync_GetAtivos()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = true });
            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = true });
            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = true });
            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = true });
            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = false });

            await context.SaveChangesAsync();

            var resultado = await controller.GetAtivosAsync(context);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var contatos = Assert.IsAssignableFrom<IEnumerable<RespostaContato>>(okResult.Value);
            Assert.Equal(4, contatos.Count());
            Assert.All(contatos, contatos => Assert.True(contatos.Nome != null));
        }


        [Fact] //Teste pegar os contatos inativos
        public async Task PostAsync_GetInativos()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = false });
            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = false });
            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = false });
            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = false });
            context.Contatos.Add(new ModeloContato { Nome = "Victor", Ativo = true });

            await context.SaveChangesAsync();

            var resultado = await controller.GetInativosAsync(context);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var contatos = Assert.IsAssignableFrom<IEnumerable<RespostaContato>>(okResult.Value);
            Assert.Equal(4, contatos.Count());
            Assert.All(contatos, contatos => Assert.True(contatos.Nome != null));
        }

        [Fact] // Teste para pegar os detalhes do contato pelo Id

        public async Task PostAsync_GetDetalhes()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            var contato = new ModeloContato
            {
                Nome = "Victor",
                Sexo = "Masculino",
                DataNascimento = new DateTime(1998, 11, 06),
                Idade = 26,
                Ativo = true
            };

            context.Contatos.Add(contato);
            await context.SaveChangesAsync();

            var resultado = await controller.DetalhesAsync(context, contato.Id);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<RespostaContato>(okResult.Value);
            Assert.Equal(contato.Nome, retorno.Nome);

        }

        [Fact] // Teste para inativar um contato pelo Id
        public async Task PostAsync_Inativar()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            var contato = new ModeloContato
            {
                Nome = "Victor",
                Ativo = true
            };

            context.Contatos.Add(contato);
            await context.SaveChangesAsync();

            var resultado = await controller.InativarAsync(context, contato.Id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal($"Contato {contato.Nome} Desativado com sucesso", okResult.Value);

            var contatoInativo = await context.Contatos.FindAsync(contato.Id);
            Assert.False(contatoInativo.Ativo);

        }


        [Fact] // Teste para ativar um contato pelo Id
        public async Task PostAsync_Ativar()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            var contato = new ModeloContato
            {
                Nome = "Victor",
                Ativo = false
            };

            context.Contatos.Add(contato);
            await context.SaveChangesAsync();

            var resultado = await controller.AtivarAsync(context, contato.Id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal($"Contato {contato.Nome} Ativado com sucesso", okResult.Value);

            var contatoAtivo = await context.Contatos.FindAsync(contato.Id);
            Assert.True(contatoAtivo.Ativo);

        }

        [Fact] // Teste para deletar um contato pelo Id
        public async Task PostAsync_Deletar()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            var contato = new ModeloContato
            {
                Nome = "Victor",
                Ativo = true
            };

            context.Contatos.Add(contato);
            await context.SaveChangesAsync();

            var resultado = await controller.DeletarAsync(context, contato.Id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal($"Contato {contato.Nome} Removido com sucesso", okResult.Value);

            var contatoDeletado = await context.Contatos.FindAsync(contato.Id);
            Assert.Null(contatoDeletado);

        }

        [Fact] // Teste para Editar um contato ativo
        public async Task PostAsync_EditarContato()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var controller = new ControladorContatos(context);

            var contato = new ModeloContato
            {
                Nome = "Victor",
                Sexo = "Masculino",
                DataNascimento = new DateTime(1998, 11, 06),
                Idade = 26,
                Ativo = true
            };

            context.Contatos.Add(contato);
            await context.SaveChangesAsync();

            var model = new CreateContatoViewModel
            {
                Nome = "Victor Atualizado",
                Sexo = "Masculino",
                DataNascimento = "06/11/1998"
            };

            var resultado = await controller.PutAsync(context, model, contato.Id);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal($"Contato {contato.Nome} Atualizado com sucesso", okResult.Value);

            var contatoAtualizado = await context.Contatos.FirstOrDefaultAsync(contato => contato.Id == contato.Id);
            Assert.Equal("Victor Atualizado", contatoAtualizado.Nome);

        }


    }
}
