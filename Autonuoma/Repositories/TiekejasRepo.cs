namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF3;

public class TiekejasRepo{


    public static List<TiekejasList> List(){

        var query = $@"SELECT t.id,t.pavadinimas,CONCAT(a.salis,', ',a.miestas,', ',a.gatve) as adresas
        FROM tiekejai t 
        JOIN adresai a ON a.Tiekejas_id=t.id;";

        var drc = Sql.Query(query);


        var results = Sql.MapAll<TiekejasList>(drc,(dre,T)=>{
            T.Id = dre.From<int>("id");
            T.Pavadinimas = dre.From<string>("pavadinimas");
            T.Adresas = dre.From<string>("adresas");
        });

        return results;
    }
}