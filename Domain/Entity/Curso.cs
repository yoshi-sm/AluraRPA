using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AluraRPA.Domain.Entity
{
    public class Curso
    {
        public Curso() { }
        public Curso(string titulo, string professor, string descricao, string cargaHoraria, string palavraChave)
        {
            Titulo = titulo;
            Professor = professor;
            Descricao = descricao;
            CargaHoraria = cargaHoraria;
            PalavraChave = palavraChave;
        }

        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Professor { get; set; }
        public string Descricao { get; set; }
        public string CargaHoraria { get; set; }
        public string PalavraChave { get; set; }

    }
}
