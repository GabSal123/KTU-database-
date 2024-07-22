namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class AktoriusRepo{


    public static List<AktoriusList> List(){
        var query = $@"SELECT *
        FROM aktoriai";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<AktoriusList>(drc, (dre, T) => {
            T.Id = dre.From<int>("id");
            T.Vardas = dre.From<string>("vardas");
            T.Pavarde = dre.From<string>("pavarde");
            T.Lytis = dre.From<string>("lytis");
            T.Gimimo_metai = dre.From<DateTime>("gimimo_metai");
        });

        return result;
    }
}