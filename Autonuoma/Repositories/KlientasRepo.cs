namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;

public class KlientasRepo{


    public static List<KlientasList> List(){

        var query = $@"SELECT * from klientai";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<KlientasList>(drc, (dre,T)=>{

            T.Id = dre.From<int>("id");
            T.Lojalus = dre.From<bool>("lojalus");
            T.Lytis = dre.From<string>("lytis");
            T.Parduotuve_id = dre.From<int>("Parduotuve_id");
            T.El_pastas = dre.From<string>("pastas");
            T.Pavarde = dre.From<string>("pavarde");
            T.Tel_numeris = dre.From<string>("telefono_numeris");
            T.Vardas = dre.From<string>("vardas");
        });

        return result;

    }

    public static List<KlientasList> List(int shopId){

        var query = $@"SELECT * from klientai WHERE Parduotuve_id={shopId}";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<KlientasList>(drc, (dre,T)=>{

            T.Id = dre.From<int>("id");
            T.Lojalus = dre.From<bool>("lojalus");
            T.Lytis = dre.From<string>("lytis");
            T.Parduotuve_id = dre.From<int>("Parduotuve_id");
            T.El_pastas = dre.From<string>("pastas");
            T.Pavarde = dre.From<string>("pavarde");
            T.Tel_numeris = dre.From<string>("telefono_numeris");
            T.Vardas = dre.From<string>("vardas");
        });

        return result;

    }
}