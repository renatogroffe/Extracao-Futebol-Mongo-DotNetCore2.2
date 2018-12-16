using MongoDB.Driver;

namespace CargaCampeonatosFutebol
{
    public class ClassificacaoRepository
    {
        private string _nomeCollection;
        private MongoClient _client;
        private IMongoDatabase _db;

        public ClassificacaoRepository(
            MongoDBConfigurations configurations)
        {
            _client = new MongoClient(
                configurations.Connection);
            _db = _client.GetDatabase(configurations.Database);
            _nomeCollection = configurations.Collection;
        }

        public void Incluir(Classificacao classificacao)
        {
            var collection =
                _db.GetCollection<Classificacao>(_nomeCollection);
            collection.InsertOne(classificacao);
        }
    }
}