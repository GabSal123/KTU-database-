namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;


using Org.Ktu.Isk.P175B602.Autonuoma.Models;

public class MokejimasRepo{


    public static List<MokejimasList> List(){

        var query = $@"SELECT m.id, m.mokejimo_data, m.suma, CONCAT(k.vardas,', ',k.pavarde) as klientas, CONCAT(d.vardas,', ',d.pavarde) as darbuotojas, GROUP_CONCAT(CONCAT(f.pavadinimas,' (',mi.kiekis,') ')) as filmas, m.apmoketas
                    FROM mokejimai m
                    LEFT JOIN klientai k ON k.id = m.Klientas_id
                    LEFT JOIN darbuotojai d ON d.id = m.Darbuotojai_id
                    LEFT JOIN mokejimas_inventorius mi ON mi.Mokejimai_id = m.id
                    LEFT JOIN inventorius i ON i.id = mi.Inventorius_id
                    LEFT JOIN filmai f ON f.id = i.Filmai_id
                    GROUP BY m.id";

        var drc = Sql.Query(query);

        var result = Sql.MapAll<MokejimasList>(drc, (dre,T)=>{

            T.Id = dre.From<int>("id");
            T.Darbuotojas = dre.From<string>("darbuotojas");
            T.Klientas = dre.From<string>("klientas");
            T.Suma = dre.From<float>("suma");
            T.Mokejimo_data = dre.From<DateTime>("mokejimo_data");
            T.Filmai = dre.From<string>("filmas");
			T.Apmoketas = dre.From<bool>("apmoketas");
        });

        return result;

    }

    public static int InsertMokejimas(MokejimasCE mokCE){
        var query =
			$@"INSERT INTO `mokejimai`
			(
                `suma`,
				`mokejimo_data`,
				`Darbuotojai_id`,
				`Klientas_id`,
				`apmoketas`
			)
			VALUES(
				?suma,
				?data,
				?darbuotojas,
				?klientas,
				?apmoketas
			)";
            var nr =
			Sql.Insert(query, args => {
				//make a shortcut
				var sut = mokCE.mokejimas;
                var suma = mokCE.CalculatePrice();

				//
				args.Add("?suma", suma);
				args.Add("?data", sut.Mokejimo_data);
				args.Add("?darbuotojas", sut.Darbuotojo_id);
				args.Add("?klientas", sut.Kliento_id);
				args.Add("?apmoketas", sut.Apmoketas);
			});

            return (int)nr;

    }

    public static void InsertUzsakytaPaslauga(int mokejimoId, MokejimasCE.PirktasFilmas up)
	{

		var query =
			$@"INSERT INTO `mokejimas_inventorius`
				(
					Inventorius_id,
					Mokejimai_id,
					kiekis
				)
				VALUES(
					?iId,
					?mId,
					?kiekis
				)";

		Sql.Insert(query, args => {
			args.Add("?iId", up.Inventorius);
			args.Add("?mId", mokejimoId);
			args.Add("?kiekis", up.Kiekis);
		});
	}


	public static MokejimasCE FindMokejimasNr(int nr)
	{
		var query = $@"SELECT * FROM `mokejimai` WHERE id=?id";
		var drc =
			Sql.Query(query, args => {
				args.Add("?id", nr);
			});

		var result =
			Sql.MapOne<MokejimasCE>(drc, (dre, t) => {
				var mok = t.mokejimas;

				mok.Id = dre.From<int>("id");
				mok.Kaina = dre.From<decimal>("suma");
				mok.Mokejimo_data = dre.From<DateTime>("mokejimo_data");
				mok.Darbuotojo_id = dre.From<int>("Darbuotojai_id");
				mok.Kliento_id = dre.From<int>("Klientas_id");
				mok.ApmoketasB = dre.From<bool>("apmoketas");
			});
			result.mokejimas.Apmoketas = result.mokejimas.ApmoketasB == true ? "Apmoketas" : "Neapmoketas";

		return result;
	}

	public static List<MokejimasCE.PirktasFilmas> ListUzsakytasFilmas(int mokejimoId)
	{
		var query =
			$@"SELECT *
			FROM `mokejimas_inventorius`
			WHERE Mokejimai_id = ?mokejimoId";

		var drc =
			Sql.Query(query, args => {
				args.Add("?mokejimoId", mokejimoId);
			});

		var result =
			Sql.MapAll<MokejimasCE.PirktasFilmas>(drc, (dre, t) => {
				t.Inventorius = dre.From<int>("Inventorius_id");
				t.Kiekis = dre.From<int>("kiekis");
			});

		List<MokejimasCE.PirktasFilmas> result2 = new List<MokejimasCE.PirktasFilmas>();

		foreach(var res in result){
			var query2 =
			$@"SELECT f.id,i.id as iId,f.pavadinimas,f.kaina,mi.kiekis
			FROM `inventorius` i
			JOIN mokejimas_inventorius mi ON mi.Inventorius_id = i.id
			JOIN filmai f ON f.id = i.Filmai_id
			WHERE i.id = ?invId AND mi.Mokejimai_id = ?mokejimoId";


			var drc2 =
			Sql.Query(query2, args => {
				args.Add("?invId", res.Inventorius);
				args.Add("?mokejimoId", mokejimoId);
			});

			 var temp = Sql.MapAll<MokejimasCE.PirktasFilmas>(drc2, (dre, t) => {
				t.Inventorius = dre.From<int>("iId");
				t.Filmas = dre.From<string>("pavadinimas");
				t.Kaina = dre.From<decimal>("kaina");
				t.Kiekis = dre.From<int>("kiekis");
			});
			result2.AddRange(temp);
			
		}

		for( int i = 0; i < result2.Count; i++ )
			result2[i].InListID = i;

		return result2;
	}

	public static void UpdateMokejimas(MokejimasCE mokCE)
	{

		 
		mokCE.mokejimas.ApmoketasB = mokCE.mokejimas.Apmoketas == "Apmoketas" ? true : false;
		var query =
			$@"UPDATE `mokejimai`
			SET
				`suma` = ?suma,
				`mokejimo_data` = ?mokData,
				`Darbuotojai_id` = ?darbuotojas,
				`Klientas_id` = ?klientas,
				`apmoketas` = ?apmoketas
			WHERE id=?nr";

		Sql.Update(query, args => {
			//make a shortcut
			var mok = mokCE.mokejimas;

			 var suma = mokCE.CalculatePrice();

			args.Add("?suma", suma);
			args.Add("?mokData", mok.Mokejimo_data);
			args.Add("?darbuotojas", mok.Darbuotojo_id);
			args.Add("?klientas", mok.Kliento_id);
			args.Add("?apmoketas", mok.ApmoketasB);
			args.Add("?nr", mok.Id);
		});
	}

	public static void DeleteUzsakytasFilmas(int id)
	{
		var query =
			$@"DELETE FROM a
			USING `mokejimas_inventorius` as a
			WHERE a.Mokejimai_id=?fkid";

		Sql.Delete(query, args => {
			args.Add("?fkid", id);
		});
	}


	public static void DeleteMokejimas(int nr)
	{
		var query = $@"DELETE FROM `mokejimai` where id=?nr";
		Sql.Delete(query, args => {
			args.Add("?nr", nr);
		});
	}


	public static void DeleteFilmaiForMokejimas(int id)
	{
		var query =
			$@"DELETE FROM a
			USING `mokejimas_inventorius` as a
			WHERE a.Mokejimai_id=?fkid";

		Sql.Delete(query, args => {
			args.Add("?fkid", id);
		});
	}


}