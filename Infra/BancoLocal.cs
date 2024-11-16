using AluraRPA.Application.Interfaces;
using AluraRPA.Domain.Entity;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Transactions;

namespace AluraRPA.Infra
{
    public class BancoLocal : IBancoLocal
    {
        private SqliteConnection _connection = null!;
        private readonly string _caminhoDb = $@"{Path.GetTempPath()}AluraDb\";

        public BancoLocal()
        {
            CriarPasta();
            DbConnection();
            CriarTabelaSQLite();
        }

        private void CriarPasta()
        {
            Directory.CreateDirectory(_caminhoDb);
        }

        private void DbConnection()
        {
            _connection = new SqliteConnection($"Data Source = {_caminhoDb}.sqlite");
            _connection.Open();
        }

        private void CriarTabelaSQLite()
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS " +
                "CURSOS(" +
                "ID INTEGER, " +
                "TITULO VARCHAR(9999), " +
                "DESCRICAO VARCHAR(9999), " +
                "PROFESSOR VARCHAR(9999), " +
                "CARGA_HORARIA VARCHAR(9999)," +
                "PALAVRA_CHAVE VARCHAR(9999)," +
                "PRIMARY KEY (ID)" +
                ")";
            cmd.ExecuteNonQuery();
        }

        public void InserirDadosCursos(List<Curso> registros)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "BEGIN TRANSACTION; ";
            foreach (var registro in registros)
            {

                cmd.CommandText += "insert into CURSOS (ID, TITULO, DESCRICAO, PROFESSOR, CARGA_HORARIA, PALAVRA_CHAVE) values  " +
                    $"(null, '{registro.Titulo}', '{registro.Descricao}', '{registro.Professor}', " +
                    $"'{registro.CargaHoraria}', '{registro.PalavraChave.Trim().ToLower()}'); ";
            }
            cmd.CommandText += "COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<T> BuscarDadosPorPalavraChave<T>(string pesquisa)
        {
            pesquisa = pesquisa.Trim().ToLower();
            var query = $"SELECT " +
                $"CARGA_HORARIA as CargaHoraria, " +
                $"PALAVRA_CHAVE as PalavraChave, " +
                $"*  FROM CURSOS WHERE PALAVRA_CHAVE = '{pesquisa}'";
            var retornoConsulta = _connection.Query<T>(query);
            return retornoConsulta;
        }


    }
}
