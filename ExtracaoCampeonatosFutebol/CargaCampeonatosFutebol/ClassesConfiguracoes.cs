namespace CargaCampeonatosFutebol
{
    public class SeleniumConfigurations
    {
        public string CaminhoDriverChrome { get; set; }
        public string UrlPaginaClassificacaoBundesliga { get; set; }
        public string UrlPaginaClassificacaoLaLiga { get; set; }
        public string UrlPaginaClassificacaoPremierLeague { get; set; }
        public int Timeout { get; set; }
    }

    public class MongoDBConfigurations
    {
        public string Connection { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}