namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;

public class IvertinimaiRepo{

    public static List<IvertinimaiList> List(){

        var query = $@"SELECT i.id, i.reitingas,
        f.pavadinimas as Pavadinimas
        FROM ivertinimai i
        LEFT JOIN filmai f ON f.id = i.Filmai_id";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<IvertinimaiList>(drc, (dre, T)=>{
            T.Id = dre.From<int>("id");
            T.Filmo_pavadinimas = dre.From<string>("Pavadinimas");
            T.Reitingas = dre.From<int>("reitingas");
        });

        return result;
    }
}