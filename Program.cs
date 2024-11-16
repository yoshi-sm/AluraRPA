using AluraRPA.Application.Interfaces;
using AluraRPA.Application.WebDriver;
using AluraRPA.Infra;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

//Settando D.I.
var serviceCollection = new ServiceCollection();
serviceCollection.AddTransient<IWebDriver, ChromeDriver>();
serviceCollection.AddTransient<IBancoLocal, BancoLocal>();
serviceCollection.AddTransient<AluraWebDriver>();

//Executar pesquisa 
ExecutarRPA(serviceCollection, "rpa");

static void ExecutarRPA(IServiceCollection serviceCollection, string pesquisa)
{
    //Buildando dependências
    var serviceProvider = serviceCollection.BuildServiceProvider();

    using AluraWebDriver webDriver = serviceProvider.GetRequiredService<AluraWebDriver>();
    webDriver.ExecutarPesquisa(pesquisa);
}
