using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using MongoDB.Bson;
using Models;
using System.Text;

namespace Repositories
{
    public class RadarRepository
    {
        private static readonly Lazy<RadarRepository> singletonInstance = new Lazy<RadarRepository>(() => new RadarRepository());

        private string _strConnMongo = "mongodb://root:Mongo%402024%23@localhost:27017/";
        private readonly IMongoCollection<BsonDocument> _radarCollection;

        private string _strConnSQL = "Data Source = 127.0.0.1; Initial Catalog=5BY5-PERSIST; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=True;";
        private SqlConnection _conn;
        private SqlCommand _cmd;

        private RadarRepository()
        {
            var client = new MongoClient(_strConnMongo);
            var database = client.GetDatabase("Radar");
            _radarCollection = database.GetCollection<BsonDocument>("DadosRadar");
            _cmd = new();
            _conn = new(_strConnSQL);
            _conn.Open();
        }

        public static RadarRepository Instance => singletonInstance.Value;

        public List<String> GetDataSql()
        {
            List<String> dataSql = new List<String>();
            try
            {
                _cmd.CommandText = new Radar().GetAllSQL();
                _cmd.Connection = _conn;
                using (SqlDataReader reader = _cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StringBuilder sb = new();
                        sb.Append($"{reader.GetString(0)}.");
                        sb.Append($"{reader.GetString(1)}.");
                        sb.Append($"{reader.GetString(2)}.");
                        sb.Append($"{reader.GetString(3)}.");
                        sb.Append($"{reader.GetString(4)}.");
                        sb.Append($"{reader.GetString(5)}.");
                        sb.Append($"{reader.GetString(6)}.");
                        sb.Append($"{reader.GetString(7)}.");
                        sb.Append($"{reader.GetString(8)}.");
                        sb.Append($"{reader.GetString(9)}.");
                        sb.Append($"{reader.GetString(10)}.");
                        sb.Append($"{reader.GetString(11)}.");
                        sb.Append($"{reader.GetString(12)}.");
                        sb.Append($"{reader.GetString(13)}");
                        dataSql.Add(sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pegar dados do SQL: " + ex.Source?.ToString());
            }
            finally
            {
                _conn.Close();
            }
            return dataSql;
        }

        public List<String> GetDataMongo()
        {
            List<String> dataMongo = new();
            var filter = Builders<BsonDocument>.Filter.Empty;
            var cursor = _radarCollection.Find(filter).ToCursor();

            foreach (var item in cursor.ToEnumerable())
            {
                StringBuilder sb = new();
                sb.Append($"{item.GetValue("Concessionaria")}.");
                sb.Append($"{item.GetValue("AnoPNV")}.");
                sb.Append($"{item.GetValue("TipoRadar")}.");
                sb.Append($"{item.GetValue("Rodovia")}.");
                sb.Append($"{item.GetValue("UF")}.");
                sb.Append($"{item.GetValue("KM")}.");
                sb.Append($"{item.GetValue("Municipio")}.");
                sb.Append($"{item.GetValue("TipoPista")}.");
                sb.Append($"{item.GetValue("Sentido")}.");
                sb.Append($"{item.GetValue("Situacao")}.");
                sb.Append($"{item.GetValue("DataInativacao")}.");
                sb.Append($"{item.GetValue("Latitude")}.");
                sb.Append($"{item.GetValue("Longitude")}.");
                sb.Append($"{item.GetValue("VelocidadeLeve")}");
                dataMongo.Add(sb.ToString());
            }
            return dataMongo;
        }
    }
}
