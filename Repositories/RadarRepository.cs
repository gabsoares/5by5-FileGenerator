using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using MongoDB.Bson;
using Models;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;

namespace Repositories
{
    public class RadarRepository
    {
        private string _strConnMongo = "mongodb://root:Mongo%402024%23@localhost:27017/";
        private readonly IMongoCollection<BsonDocument> _radarCollection;
        private string _strConnSQL = "Data Source = 127.0.0.1; Initial Catalog=5BY5-PERSIST; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=True;";
        private SqlConnection _conn;
        private SqlCommand _cmd;

        public RadarRepository()
        {
            var client = new MongoClient(_strConnMongo);
            var database = client.GetDatabase("Radar");
            _radarCollection = database.GetCollection<BsonDocument>("DadosRadar");
            _cmd = new();
            _conn = new(_strConnSQL);
            _conn.Open();
        }

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

        public List<String> GenerateCSV(List<String> dataSqlCSV)
        {
            for (int i = 0; i < dataSqlCSV.Count; i++)
            {
                dataSqlCSV[i] = dataSqlCSV[i].Replace(".", ";");
            }

            return dataSqlCSV;
        }

        public bool GenerateXML(List<String> dataSqlXML)
        {
            bool result = false;
            if (dataSqlXML.Count > 0)
            {
                XElement root = new("Root");
                foreach (var item in dataSqlXML)
                {
                    string[] fields = item.Split('.');

                    var radares = new XElement("radar",
                                            new XElement("concessionaria", fields[0]),
                                            new XElement("ano_pnv", fields[1]),
                                            new XElement("tipo_radar", fields[2]),
                                            new XElement("rodovia", fields[3]),
                                            new XElement("UF", fields[4]),
                                            new XElement("KM", fields[5]),
                                            new XElement("municipio", fields[6]),
                                            new XElement("tipo_pista", fields[7]),
                                            new XElement("sentido", fields[8]),
                                            new XElement("situacao", fields[9]),
                                            new XElement("data_inativacao", fields[10]),
                                            new XElement("latitude", fields[11]),
                                            new XElement("longitude", fields[12]),
                                            new XElement("velocidade_leve", fields[13]));
                    root.Add(radares);
                }
                try
                {
                    // Escreve o documento XML no arquivo
                    using (StreamWriter sw = new StreamWriter(@"C:\Users\adm\Documents\5by5-TrabalhoDesignPattern\5by5-FileGenerator\saida.xml"))
                    {
                        sw.Write(root);
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao gravar o arquivo XML: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Lista vazia.");
            }
            return result;
        }

        public string GenerateJson(List<String> dataSqlJson)
        {
            List<Object> formattedDataList = new();
            foreach (var item in dataSqlJson)
            {
                string concessionaria = item.Split('.')[0].Trim('"');
                string ano_PNV = item.Split('.')[1].Trim('"');
                string tipo_radar = item.Split('.')[2].Trim('"');
                string rodovia = item.Split('.')[3].Trim('"');
                string uf = item.Split('.')[4].Trim('"');
                string km = item.Split('.')[5].Trim('"');
                string municipio = item.Split('.')[6].Trim('"');
                string tipo_pista = item.Split('.')[7].Trim('"');
                string sentido = item.Split('.')[8].Trim('"');
                string situacao = item.Split('.')[9].Trim('"');
                string data_inativacao = item.Split('.')[10].Trim('"');
                string latitude = item.Split('.')[11].Trim('"');
                string longitude = item.Split('.')[12].Trim('"');
                string velocidade_leve = item.Split('.')[13].Trim('"');
                var formattedData = new
                {
                    concessionaria = concessionaria,
                    ano_PNV = ano_PNV,
                    tipo_radar = tipo_radar,
                    rodovia = rodovia,
                    uf = uf,
                    km = km,
                    municipio = municipio,
                    tipo_pista = tipo_pista,
                    sentido = sentido,
                    situacao = situacao,
                    data_inativacao = data_inativacao,
                    latitude = latitude,
                    longitude = longitude,
                    velocidade_leve = velocidade_leve
                };

                formattedDataList.Add(formattedData);
            }
            JsonSerializerOptions op = new()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            string jsonData = JsonSerializer.Serialize(formattedDataList, op);
            StreamWriter sw = new(@"C:\Users\adm\Documents\5by5-TrabalhoDesignPattern\5by5-FileGenerator\saida.json");
            sw.Write(jsonData);
            sw.Close();
            return jsonData;
        }
    }
}
