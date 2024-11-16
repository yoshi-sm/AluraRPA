using AluraRPA.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluraRPA.Application.Interfaces
{
    public interface IBancoLocal
    {
        void InserirDadosCursos(List<Curso> registros);
        IEnumerable<T> BuscarDadosPorPalavraChave<T>(string pesquisa);
    }
}
