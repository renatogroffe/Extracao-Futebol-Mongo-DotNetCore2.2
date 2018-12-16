using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CargaCampeonatosFutebol
{
    class Program
    {
        private static SeleniumConfigurations _seleniumConfigurations;
        private static MongoDBConfigurations _mongoDBConfigurations;

        static void Main(string[] args)
        {
            Console.WriteLine(
                "*** Extração de Dados da Web com " +
                ".NET Core 2.2, Selenium WebDriver, " +
                "Chrome Driver e MongoDB ***");
            Console.WriteLine("Carregando configurações...");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");
            var configuration = builder.Build();

            _seleniumConfigurations = new SeleniumConfigurations();
            new ConfigureFromConfigurationOptions<SeleniumConfigurations>(
                configuration.GetSection("SeleniumConfigurations"))
                    .Configure(_seleniumConfigurations);

            _mongoDBConfigurations = new MongoDBConfigurations();
            new ConfigureFromConfigurationOptions<MongoDBConfigurations>(
                configuration.GetSection("MongoDBConfigurations"))
                    .Configure(_mongoDBConfigurations);

            ExtrairDadosCampeonato("Bundesliga", "Alemanha",
                _seleniumConfigurations.UrlPaginaClassificacaoBundesliga);
            ExtrairDadosCampeonato("La Liga", "Espanha",
                _seleniumConfigurations.UrlPaginaClassificacaoLaLiga);
            ExtrairDadosCampeonato("Premier League", "Inglaterra",
                _seleniumConfigurations.UrlPaginaClassificacaoPremierLeague);

            Console.WriteLine(
                Environment.NewLine +
                "Carga de dados concluída com sucesso!");

            Console.ReadKey();
        }

        private static void ExtrairDadosCampeonato(
            string nomeCampeonato, string pais, string urlClassificacao)
        {
            Console.Write(Environment.NewLine);
            Console.WriteLine(
                "Carregando driver do Selenium para Chrome em modo headless...");
            var paginaClassificacao = new PaginaClassificacao(
                _seleniumConfigurations,
                nomeCampeonato, pais, urlClassificacao);

            Console.WriteLine(
                $"Carregando página com classificações - {nomeCampeonato}...");
            paginaClassificacao.CarregarPagina();

            Console.WriteLine(
                "Extraindo dados...");
            var classificacao = paginaClassificacao.ObterClassificacao();
            paginaClassificacao.Fechar();

            Console.WriteLine("Gravando dados extraídos...");
            new ClassificacaoRepository(_mongoDBConfigurations)
                .Incluir(classificacao);
        }
    }
}