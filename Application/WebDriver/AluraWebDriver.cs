using AluraRPA.Application.Interfaces;
using AluraRPA.Domain.Entity;
using OpenQA.Selenium;

namespace AluraRPA.Application.WebDriver
{
    public class AluraWebDriver : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly IBancoLocal _bancoLocal;
        private string _pesquisa = "";

        public AluraWebDriver(IWebDriver driver, IBancoLocal bancoLocal) 
        {
            _driver = driver;
            _bancoLocal = bancoLocal;
        }

        public void ExecutarPesquisa(string pesquisa)
        {
            _pesquisa = pesquisa;

            //Realiza a pesquisa inicial e verifica se existe algum resultado
            if (!ExistemResultados())
                return;

            //Agrupa os links dos cursos
            var listaLinks = BuscarLinks();

            //Vai até as urls e obtém os dados necessarios, retornando uma lista de entidades "curso"
            var cursos = BuscarCursos(listaLinks);

            //Verifica existem registros existente no banco de dados entre os cursos retornados para a palavra-chave
            EliminarDuplicados(cursos);

            //Insere os registros
            _bancoLocal.InserirDadosCursos(cursos);
        }

        private void EliminarDuplicados(List<Curso> cursos)
        {
            var cursosGravados = _bancoLocal.BuscarDadosPorPalavraChave<Curso>(_pesquisa);
            List<Curso> cursoRemover = new();
            foreach ( var curso in cursos)
            {
                if (cursosGravados.Any(x =>
                x.Titulo == curso.Titulo &&
                x.Professor == curso.Professor &&
                x.CargaHoraria == curso.CargaHoraria &&
                x.Descricao == curso.Descricao))
                    cursoRemover.Add(curso);
            }
            foreach (var curso in cursoRemover)
                cursos.Remove(curso);
        }

        private bool ExistemResultados()
        {
            //filtros iniciais
            _driver.Navigate().GoToUrl("https://www.alura.com.br/");
            _driver.FindElement(By.XPath(@"//*[@id=""header-barraBusca-form-campoBusca""]")).SendKeys(_pesquisa);
            _driver.FindElement(By.XPath(@"/html/body/main/section[1]/header/div/nav/div[2]/form/button")).Click();

            //verifica se existe algum resultado;
            if (string.IsNullOrWhiteSpace(_driver.FindElement(By.XPath(@"//*[@id=""busca--filtros--tipos""]/li[1]/label/div/span[2]")).Text))
                return false;

            //realiza filtro por cursos
            _driver.FindElement(By.XPath(@"//*[@id=""busca--filtros--tipos""]/li[1]/label")).Click();
            _driver.FindElement(By.XPath(@"//*[@id=""busca-form""]/form/input[3]")).Click();

            return true;
        }


        private List<string> BuscarLinks()
        {
            var listaLinks = new List<string>();
            var continuar = true;
            do
            {
                var linkProximo = _driver.FindElement(By.LinkText(@"Próximo"));
                listaLinks.AddRange(_driver.FindElements(By.ClassName("busca-resultado-link")).Select(x => x.GetAttribute("href")));

                try
                {
                    linkProximo.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    continuar = false;
                }
            }
            while (continuar);
            
            return listaLinks;
        }

        private List<Curso> BuscarCursos(List<string> listaLinks)
        {
            var cursos = new List<Curso>();

            foreach (string link in listaLinks)
            {
                var titulo = "Título não encontrado";
                var cargaHoraria = "Carga horária não encontrada";
                var professor = "Nome do professor não encontrado";
                var descricao = "Descrição não encontrada";
                _driver.Navigate().GoToUrl(link);
                try
                {
                    titulo = _driver.FindElement(By.XPath(@"/html/body/section[1]/div/div[1]/h1")).Text.Replace("'", "\"");
                    titulo += " " + _driver.FindElement(By.XPath(@"/html/body/section[1]/div/div[1]/p[2]")).Text.Replace("'", "\"");
                }
                catch { }
                try
                {
                    cargaHoraria = _driver.FindElement(By.XPath(@"/html/body/section[1]/div/div[2]/div[1]/div/div[1]/div/p[1]")).Text;
                }
                catch { }
                try
                {
                    var listaProfessor = _driver.FindElements(By.ClassName("instructor-title--name")).Select(x => x.Text).ToList();
                    professor = listaProfessor.First(x => !string.IsNullOrWhiteSpace(x));
                }
                catch { }
                try
                {
                    descricao = _driver.FindElement(By.XPath(@"/html/head/meta[3]")).GetAttribute("content").Replace("'", "\"");
                }
                catch { }

                cursos.Add(new Curso(titulo, professor, descricao, cargaHoraria, _pesquisa));
            }

            return cursos;
        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }

    }
}
