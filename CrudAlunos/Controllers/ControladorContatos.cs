using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAlunos.Data;
using CrudAlunos.Models;
using CrudAlunos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudAlunos.Controllers
{
    [ApiController]
    [Route("Api/Versao1/Contatos")]
    public class ControladorContatos : ControllerBase
    {
        private AppDbContext context;

        public ControladorContatos(AppDbContext context)
        {
            this.context = context;
        }


        //Metodos do crud
        //Metodo para listar os contatos ativos

        [HttpGet]
        [Route("Listar-Contatos")]
        public async Task<IActionResult> GetAtivosAsync(
            [FromServices] AppDbContext context)
        {
            var contatos = await context.Contatos
            .Where(contato => contato.Ativo)
            .AsNoTracking()
            .ToListAsync();

            var resultado = contatos.Select(RespostaContato.FromModel);
            return Ok(resultado);

        }

        //Metodo para listar os contato inativos
        [HttpGet]
        [Route("Listar-Contatos-Inativos")]
        public async Task<IActionResult> GetInativosAsync(
     [FromServices] AppDbContext context)
        {
            var contatos = await context.Contatos
            .Where(contato => !contato.Ativo)
            .AsNoTracking()
            .ToListAsync();

            var resultado = contatos.Select(RespostaContato.FromModel);
            return Ok(resultado);

        }
        // Metodo para listar detalhes do contato selecionado pelo ID
        [HttpGet]
        [Route("Detalhes-Do-Contato/{Id}")]
        public async Task<IActionResult> DetalhesAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int Id)
        {
            try
            {
                var contato = await context.Contatos
                    .Where(contato => contato.Id == Id && contato.Ativo)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (contato == null)
                    return NotFound("Contato não encontrado no banco de dados.");

                var resultado = RespostaContato.FromModel(contato);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Metodo para cadastrar um contato, ele ja vai inicializado como ativo

        [HttpPost]
        [Route("Cadastrar-Contato")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateContatoViewModel model)

        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Se nao por um campo obrigatorio vai ocorrer um erro
            try
            {
                var DataNascimento = model.GetDataNascimento();
                var idade = model.calcularIdade(DataNascimento);

                var contato = new ModeloContato
                {
                    Nome = model.Nome,
                    Sexo = model.Sexo,
                    DataNascimento = DataNascimento,
                    Idade = idade,
                    Ativo = true
                };

                await context.Contatos.AddAsync(contato);
                await context.SaveChangesAsync();
                return Created($"Versao1/Contatos/{contato.Id}", new
                {
                    mensagem = $"Contato {contato.Nome} cadastrado com sucesso",
                    contato
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Metodo para editar um contato ativo

        [HttpPut]
        [Route("Editar-Contato/{Id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateContatoViewModel model,
            [FromRoute] int Id)

        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contato = await context.Contatos.FirstOrDefaultAsync(contato => contato.Id == Id && contato.Ativo);
            if (contato == null)
                return NotFound("Contato nao encontrado no banco de dados");
            try
            {
                var novaDataNascimento = model.GetDataNascimento();
                var novaIdade = model.calcularIdade(novaDataNascimento);

                contato.Nome = model.Nome;
                contato.Sexo = model.Sexo;
                contato.DataNascimento = novaDataNascimento;
                contato.Idade = novaIdade;

                context.Contatos.Update(contato);
                await context.SaveChangesAsync();

                return Ok($"Contato {contato.Nome} Atualizado com sucesso");

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar o contato: {ex.Message}");
            }
        }

        //Metodo para Inativar um contato ativo pelo Id
        [HttpPut]
        [Route("Inativar-Contato/{Id}")]
        public async Task<IActionResult> InativarAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int Id)
        {
            var contato = await context.Contatos.FirstOrDefaultAsync(contato => contato.Id == Id && contato.Ativo);
            if (contato == null)
                return NotFound("Contato nao encontrado no banco de dados");

            contato.Ativo = false; // vai alterar o status para inativo

            context.Contatos.Update(contato);
            await context.SaveChangesAsync();

            return Ok($"Contato {contato.Nome} Desativado com sucesso");
        }


        //Metodo para ativar um contato inaativo pelo Id
        [HttpPut]
        [Route("Ativar-Contato/{Id}")]
        public async Task<IActionResult> AtivarAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int Id)
        {
            var contato = await context.Contatos.FirstOrDefaultAsync(contato => contato.Id == Id && !contato.Ativo);
            if (contato == null)
                return NotFound("Contato nao encontrado no banco de dados");

            contato.Ativo = true; // vai alterar o status para ativo

            context.Contatos.Update(contato);
            await context.SaveChangesAsync();

            return Ok($"Contato {contato.Nome} Ativado com sucesso");
        }


        //Metodo para deletar um contato do banco de dados
        [HttpDelete]
        [Route("Deletar-Contato/{Id}")]
        public async Task<IActionResult> DeletarAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int Id)
        {
            var contato = await context.Contatos.FirstOrDefaultAsync(contato => contato.Id == Id && contato.Ativo);
            if (contato == null)
                return NotFound("Contato nao encontrado no banco de dados");

            context.Contatos.Remove(contato);
            await context.SaveChangesAsync();

            return Ok($"Contato {contato.Nome} Removido com sucesso");


        }










    }
}
