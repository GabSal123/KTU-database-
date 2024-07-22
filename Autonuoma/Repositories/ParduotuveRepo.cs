namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF3;


public class ParduotuveRepo{


    public static List<ParduotuveList> List(){


        var query = $@"SELECT p.id,p.ikurimo_data, CONCAT(a.salis,', ',a.miestas,', ',a.gatve) as adresas
         FROM parduotuves p 
         JOIN adresai a ON a.id=p.Adresas_id";

        var drc = Sql.Query(query);

        var results = Sql.MapAll<ParduotuveList>(drc,(dre,T)=>{
            
            T.Id = dre.From<int>("id");
            T.Ikurimo_data = dre.From<DateTime>("ikurimo_data");
            T.Adresas = dre.From<string>("adresas");
        });

        return results;
    }
}