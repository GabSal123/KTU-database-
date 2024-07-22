namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class KategorijaRepo{


    public static List<KategorijaList> List(){
        var query = $@"SELECT *
        FROM kategorija";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<KategorijaList>(drc, (dre, T) => {
            T.Id = dre.From<int>("id");
            T.Pavadinimas = dre.From<string>("pavadinimas");
        });

        return result;
    }
}