using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAlunos.Models
{
    public class ModeloContato
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Idade { get; set; }
        public bool Ativo { get; set; }
    }
}