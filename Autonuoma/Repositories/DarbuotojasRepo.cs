namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class DarbuotojasRepo{


    public static List<DarbuotojasList> List(){
        

        var query = $@"SELECT *
        FROM darbuotojai";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<DarbuotojasList>(drc, (dre, T) => {
            T.Id = dre.From<int>("id");
            T.Vardas = dre.From<string>("vardas");
            T.Pavarde = dre.From<string>("pavarde");
            T.Pareigos = dre.From<string>("pareigos");
            T.El_pastas = dre.From<string>("pastas");
            T.Lytis = dre.From<string>("lytis");
            T.Tel_numeris = dre.From<string>("telefono_numeris");
            T.Parduotuve_id = dre.From<int>("Parduotuve_id");
        });

        return result;
    }


     public static List<DarbuotojasList> List(int shopId){
        
        var query = $@"SELECT *
        FROM darbuotojai
        WHERE Parduotuve_id = {shopId}";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<DarbuotojasList>(drc, (dre, T) => {
            T.Id = dre.From<int>("id");
            T.Vardas = dre.From<string>("vardas");
            T.Pavarde = dre.From<string>("pavarde");
            T.Pareigos = dre.From<string>("pareigos");
            T.El_pastas = dre.From<string>("pastas");
            T.Lytis = dre.From<string>("lytis");
            T.Tel_numeris = dre.From<string>("telefono_numeris");
            T.Parduotuve_id = dre.From<int>("Parduotuve_id");
        });

        return result;
    }
}