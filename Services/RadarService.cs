using Repositories;
using System.Text.Json;
using System.Xml.Linq;

namespace Services
{
    public class RadarService
    {

        private RadarRepository _radarRepository;

        public RadarService()
        {
            _radarRepository = RadarRepository.Instance;
        }

        public void CreateFileAndDir(string path, string file)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }
        }

        public bool GenerateCSV(List<String> dataCSV)
        {
            bool result = false;
            for (int i = 0; i < dataCSV.Count; i++)
            {
                dataCSV[i] = dataCSV[i].Replace(".", ";");
            }

            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\FileGeneratorGabriel\\";
            string file = "Radar.csv";

            CreateFileAndDir(path, file);

            StreamWriter sw = new(path + file);

            try
            {
                foreach (var item in dataCSV)
                {
                    sw.WriteLine(item);
                }
                sw.Close();
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao gerar csv");
            }
            return result;
        }

        public bool GenerateXML(List<String> dataXML)
        {
            bool result = false;
            if (dataXML.Count > 0)
            {
                XElement root = new("Root");
                foreach (var item in dataXML)
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
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\FileGeneratorGabriel\\";
                    string file = "Radar.xml";

                    CreateFileAndDir(path, file);

                    // Escreve o documento XML no arquivo
                    using (StreamWriter sw = new StreamWriter(path + file))
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

        public bool GenerateJson(List<String> dataJson)
        {
            bool result = false;
            List<Object> formattedDataList = new();
            foreach (var item in dataJson)
            {
                string concessionaria = item.Split('.')[0];
                string ano_PNV = item.Split('.')[1];
                string tipo_radar = item.Split('.')[2];
                string rodovia = item.Split('.')[3];
                string uf = item.Split('.')[4];
                string km = item.Split('.')[5];
                string municipio = item.Split('.')[6];
                string tipo_pista = item.Split('.')[7];
                string sentido = item.Split('.')[8];
                string situacao = item.Split('.')[9];
                string data_inativacao = item.Split('.')[10];
                string latitude = item.Split('.')[11];
                string longitude = item.Split('.')[12];
                string velocidade_leve = item.Split('.')[13];
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
            try
            {
                string jsonData = JsonSerializer.Serialize(formattedDataList, op);

                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\FileGeneratorGabriel\\";
                string file = "Radar.json";

                CreateFileAndDir(path, file);

                StreamWriter sw = new(path + file);
                sw.Write(jsonData);
                sw.Close();
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro" + ex.Source.ToString());
            }
            return result;
        }

        public int Menu()
        {
            Console.Clear();
            Console.WriteLine("<<<<<<<<<<PEGAR DADOS VIA SQL/MONGO>>>>>>>>>>");
            Console.WriteLine("[ 1 ]  Pegar dados via SQL");
            Console.WriteLine("[ 2 ]  Pegar dados via Mongo");
            Console.WriteLine("[ 0 ]  Sair do programa");
            Console.Write("Insira uma das opcoes validas [ 0 - 2 ]:< > \b\b\b");

            int option = int.Parse(Console.ReadLine());
            return option;
        }

        public int MenuSQL()
        {
            Console.Clear();
            Console.WriteLine("<<<<<<<<<<GERAR ARQUIVO VIA SQL>>>>>>>>>>");
            Console.WriteLine("[ 1 ]  Gerar arquivo Json via SQL");
            Console.WriteLine("[ 2 ]  Gerar arquivo XML via SQL");
            Console.WriteLine("[ 3 ]  Gerar arquivo CSV via SQL");
            Console.WriteLine("[ 0 ]  Sair do programa");
            Console.Write("Insira uma das opcoes validas [ 0 - 3 ]:< > \b\b\b");

            int option = int.Parse(Console.ReadLine());
            return option;
        }

        public int MenuMongo()
        {
            Console.Clear();
            Console.WriteLine("<<<<<<<<<<GERAR ARQUIVO VIA MONGO>>>>>>>>>>");
            Console.WriteLine("[ 1 ]  Gerar arquivo Json via Mongo");
            Console.WriteLine("[ 2 ]  Gerar arquivo XML via Mongo");
            Console.WriteLine("[ 3 ]  Gerar arquivo CSV via Mongo");
            Console.WriteLine("[ 0 ]  Sair do programa");
            Console.Write("Insira uma das opcoes validas [ 0 - 3 ]:< > \b\b\b");

            int option = int.Parse(Console.ReadLine());
            return option;
        }

        public void ChamarMenu()
        {
            bool terminouMenu = false;
            do
            {
                switch (Menu())
                {
                    case 1:
                        var lst = _radarRepository.GetDataSql();
                        switch (MenuSQL())
                        {
                            case 1:
                                new RadarService().GenerateJson(lst);
                                break;
                            case 2:
                                new RadarService().GenerateXML(lst);
                                break;
                            case 3:
                                new RadarService().GenerateCSV(lst);
                                break;
                        }
                        break;
                    case 2:
                        var lstMongo = _radarRepository.GetDataMongo();
                        switch (MenuMongo())
                        {
                            case 1:
                                new RadarService().GenerateJson(lstMongo);
                                break;
                            case 2:
                                new RadarService().GenerateXML(lstMongo);
                                break;
                            case 3:
                                new RadarService().GenerateCSV(lstMongo);
                                break;
                        }
                        break;
                    case 0:
                        Console.WriteLine("Encerrando o programa.");
                        terminouMenu = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            } while (!terminouMenu);
        }
    }
}
