using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAlunos.ViewModels
{
    public class CreateContatoViewModel
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Sexo { get; set; }

        [Required]
        public string DataNascimento { get; set; }

        public DateTime GetDataNascimento()
        {
            if (!DateTime.TryParseExact(DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
                throw new Exception("Data de nascimento no formato invalido! Tente novamente seguindo o formato -> dia/mes/ano(dd/MM/yyyy)");
            if (data > DateTime.Today)
                throw new Exception("A data de nascimento nao pode ser hoje ou no futuro ! ");
            return data;

        }

        public int calcularIdade(DateTime nascimento)
        {
            var hoje = DateTime.Today;
            int idade = hoje.Year - nascimento.Year;
            if (nascimento > hoje.AddYears(-idade)) idade--;
            if (idade < 18)
                throw new Exception("Nao podemos cadastrar menores de idade");
            return idade;
        }
    }
}