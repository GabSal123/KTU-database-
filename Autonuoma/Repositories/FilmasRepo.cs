namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;

public class FilmasRepo{



    public static List<FilmasList> Sarasas()
	{
		var query =
			$@"SELECT f.id, f.pavadinimas, f.trukme, f.kaina,
             f.isleidimo_data, k.pavadinimas as kategorija, 
             SUM(i.kiekis) as Kiekis, AVG(iv.reitingas) as Vidurkis
                FROM filmai f
                LEFT JOIN inventorius i ON i.Filmai_id=f.id
                LEFT JOIN ivertinimai iv ON iv.Filmai_id=f.id
				LEFT JOIN kategorija k ON k.id=f.kategorija
                GROUP BY f.id
                ORDER BY Vidurkis DESC, KIEKIS DESC;";

		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<FilmasList>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Trukme = dre.From<int>("trukme");
				t.Kaina = dre.From<int>("kaina");
				t.Isleidimo_data = dre.From<DateTime>("isleidimo_data");
                t.Kategorija = dre.From<string>("kategorija");
                t.Kiekis = dre.From<int>("Kiekis");
                t.Vidurkis = dre.From<float>("Vidurkis");
			});

		return result;
	}



	private void PopulateLists(FilmasCE filmasCE){

	}
}