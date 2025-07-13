using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAlunos.Models;

namespace CrudAlunos.ViewModels
{
    public class RespostaContato
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public string DataNascimento { get; set; }
        public int Idade { get; set; }

        public static RespostaContato FromModel(ModeloContato modeloContato)
        {
            return new RespostaContato
            {
                Id = modeloContato.Id,
                Nome = modeloContato.Nome,
                Sexo = modeloContato.Sexo,
                DataNascimento = modeloContato.DataNascimento.ToString("dd/MM/yyyy"),
                Idade = modeloContato.Idade
            };
        }
    }
}