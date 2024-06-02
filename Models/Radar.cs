using System.Text;

namespace Models
{
    public class Radar
    {
        public StringBuilder GETALL = new StringBuilder();

        public string GetAllSQL()
        {
            GETALL.Append("SELECT ");
            GETALL.Append("CONCESSIONARIA, ANO_PNV,");
            GETALL.Append("TIPO_RADAR, RODOVIA,");
            GETALL.Append("UF, KM, MUNICIPIO,");
            GETALL.Append("TIPO_PISTA, SENTIDO, SITUACAO,");
            GETALL.Append("DATA_INATIVACAO, LATITUDE, LONGITUDE, VELOCIDADE_LEVE ");
            GETALL.Append("FROM DADOS_RADAR");

            return GETALL.ToString();
        }
    }
}
