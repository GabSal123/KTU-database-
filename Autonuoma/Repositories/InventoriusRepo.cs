namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class InventoriusRepo{


    public static List<InventoriusList> List(){

        var query = $@"SELECT i.id, f.pavadinimas, f.id as filmo_id, i.kiekis, i.Parduotuve_id, f.trukme, temp.vidurkis, f.kaina, kat.pavadinimas, f.isleidimo_data
         FROM inventorius i
          JOIN filmai f ON i.Filmai_id = f.id 
          JOIN kategorija kat ON f.kategorija = kat.id
          JOIN (SELECT AVG(iv.reitingas) as vidurkis, f.id as id FROM filmai f JOIN ivertinimai iv ON iv.Filmai_id=f.id GROUP BY f.id ) temp ON temp.id = f.id
          ORDER BY f.id;";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<InventoriusList>(drc, (dre, T)=>{
            T.Id = dre.From<int>("id");
            T.Filmo_id = dre.From<int>("filmo_id");
            T.Filmo_pavadinimas = dre.From<string>("Pavadinimas");
            T.Kiekis = dre.From<int>("kiekis");
            T.Parduotuve_id = dre.From<int>("Parduotuve_id");
            T.Trukme = dre.From<int>("trukme");
            T.Kaina = dre.From<int>("kaina");
            T.Isleidimo_data = dre.From<DateTime>("isleidimo_data");
            T.Kategorija = dre.From<string>("pavadinimas");
            T.Ivertinimas = dre.From<double>("vidurkis");
        });

        return result;
    }


    public static List<InventoriusList> List(int shopId){

        var query = $@"SELECT i.id, i.kiekis,
        f.pavadinimas as Pavadinimas,f.id as fid, i.Parduotuve_id
        FROM inventorius i
        LEFT JOIN filmai f ON f.id = i.Filmai_id
        WHERE i.Parduotuve_id = {shopId}";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<InventoriusList>(drc, (dre, T)=>{
            T.Id = dre.From<int>("id");
            T.Filmo_pavadinimas = dre.From<string>("Pavadinimas");
            T.Kiekis = dre.From<int>("kiekis");
            T.Parduotuve_id = dre.From<int>("Parduotuve_id");
            T.Filmo_id = dre.From<int>("fid");
        });

        return result;
    }

    public static bool EnoughMovies(int invId,int kiekis){
        var query = $@"SELECT kiekis
        FROM inventorius
        WHERE id = ?id";

         var drc =
			Sql.Query(query, args => {
				args.Add("?id", invId);
			});

         var result = Sql.MapOne<InventoriusList>(drc,(dre,T)=>{
            T.Kiekis = dre.From<int>("kiekis");
         });
         return result.Kiekis-kiekis >=0;
    }


    public static int GetInventoryCount(int invId){
         var query = $@"SELECT kiekis
        FROM inventorius
        WHERE id = ?id";

         var drc =
			Sql.Query(query, args => {
				args.Add("?id", invId);
			});

         var result = Sql.MapOne<InventoriusList>(drc,(dre,T)=>{
            T.Kiekis = dre.From<int>("kiekis");
         });
         return result.Kiekis;
    }

    public static int GetOldOrderedInventoryCount(int invId, int mokId){
         var query = $@"SELECT kiekis
        FROM mokejimas_inventorius
        WHERE Inventorius_id = ?id AND Mokejimai_id = ?mokId";

        var drc =
			Sql.Query(query, args => {
				args.Add("?id", invId);
                args.Add("?mokId", mokId);
			});

             var result = Sql.MapOne<InventoriusList>(drc,(dre,T)=>{
            T.Kiekis = dre.From<int>("kiekis");
         });
         if(result == null){
            return 0;
         }

         return result.Kiekis;
    }
    public static void UpdateInventoryValues(int invId,int kiekisAtnaujinti, int senasKiekis){

        var query =
			$@"UPDATE `inventorius`
			SET
				`kiekis` = ?kiekis
			WHERE id=?nr";

		Sql.Update(query, args => {
			args.Add("?kiekis", senasKiekis+kiekisAtnaujinti);
            args.Add("?nr", invId);
		});
    }
}