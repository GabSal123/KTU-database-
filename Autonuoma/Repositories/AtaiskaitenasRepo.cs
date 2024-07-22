namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;


using ContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.Ataiskaitenas;



/// <summary>
/// Database operations related to reports.
/// </summary>
public class AtaiskaitenasRepo
{
	

	public static List<ContractsReport.Ataiskaitenas> GetContracts(DateTime? dateFrom, DateTime? dateTo,int? apmoketas,decimal? sumaNuo)
	{
        if(apmoketas == null){
            apmoketas = 2;
        }
        if(sumaNuo == null){
            sumaNuo = 0;
        }

       var query = @" 
    SELECT k.pastas ,m.apmoketas,k.id,m.id AS mokId, CONCAT(UPPER(LEFT(k.vardas,1)), UPPER(LEFT(k.pavarde,1))) AS Inicialai, CONCAT(k.vardas,' ',k.pavarde) AS `Pilnas Vardas`,
        IFNULL(k.telefono_numeris,'Nenurodytas') AS `Telefono numeris`, m.mokejimo_data AS `Mokejimo Data`, spm.bendra_suma AS `Suma uz Mokejima`,
        mpk.max_mok AS `Maksimalus mokejimas`,spk.bendra_suma AS `Is Viso`, km.atlikta_mokejimu AS `Atlikta mokejimu`
    FROM mokejimai m
    JOIN mokejimas_inventorius mi ON mi.Mokejimai_id = m.id
    JOIN inventorius i ON i.id = mi.Inventorius_id
    JOIN filmai f ON f.id = i.Filmai_id
    RIGHT JOIN klientai k ON k.id = m.Klientas_id
    LEFT JOIN (
        SELECT SUM(mi.kiekis*f.kaina) as bendra_suma, m.id
        FROM klientai k
        LEFT JOIN mokejimai m ON m.Klientas_id = k.id
        LEFT JOIN mokejimas_inventorius mi ON mi.Mokejimai_id = m.id
        LEFT JOIN inventorius i ON i.id = mi.Inventorius_id
        LEFT JOIN filmai f ON f.id = i.Filmai_id
        WHERE (m.mokejimo_data >= IFNULL(?nuo,m.mokejimo_data) AND m.mokejimo_data <= IFNULL(?iki,m.mokejimo_data))
        AND (m.apmoketas != ?apmoketas)
        GROUP BY m.id
    ) AS spm ON spm.id = m.id
    LEFT JOIN (
        SELECT SUM(mi.kiekis*f.kaina) as bendra_suma, k.id
        FROM klientai k
        LEFT JOIN mokejimai m ON m.Klientas_id = k.id
        LEFT JOIN mokejimas_inventorius mi ON mi.Mokejimai_id = m.id
        LEFT JOIN inventorius i ON i.id = mi.Inventorius_id
        LEFT JOIN filmai f ON f.id = i.Filmai_id
        WHERE (m.mokejimo_data >= IFNULL(?nuo,m.mokejimo_data) AND m.mokejimo_data <= IFNULL(?iki,m.mokejimo_data))
        AND (m.apmoketas != ?apmoketas)
        GROUP BY k.id
    ) AS spk ON spk.id = k.id
    LEFT JOIN (
	SELECT COUNT(m.id) AS atlikta_mokejimu, k.id
    FROM klientai k 
    JOIN mokejimai m ON m.Klientas_id = k.id
    WHERE (m.mokejimo_data >= IFNULL(?nuo,m.mokejimo_data) AND m.mokejimo_data <= IFNULL(?iki,m.mokejimo_data))
    AND (m.apmoketas != ?apmoketas)
    GROUP BY k.id
) AS km ON km.id = k.id
LEFT JOIN (
	SELECT MAX(sub.sumele) as max_mok, sub.id
    FROM (
    	SELECT k.id, SUM(mi.kiekis* f.kaina) as sumele
        FROM klientai k
        LEFT JOIN mokejimai m ON m.Klientas_id = k.id
        LEFT JOIN mokejimas_inventorius mi ON mi.Mokejimai_id = m.id
        LEFT JOIN inventorius i ON i.id = mi.Inventorius_id
        LEFT JOIN filmai f ON f.id = i.Filmai_id
        WHERE (m.mokejimo_data >= IFNULL(?nuo,m.mokejimo_data) AND m.mokejimo_data <= IFNULL(?iki,m.mokejimo_data))
        AND (m.apmoketas != ?apmoketas)
        GROUP BY m.id
    ) AS sub
    GROUP BY id
) AS mpk ON mpk.id = k.id
    WHERE 
        (m.mokejimo_data >= IFNULL(?nuo,m.mokejimo_data) AND m.mokejimo_data <= IFNULL(?iki,m.mokejimo_data))
        AND (m.apmoketas != ?apmoketas) AND (spk.bendra_suma >= ?sumaNuo)
    GROUP BY k.id, m.id
    ORDER BY Inicialai, `Pilnas Vardas` ";




		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
                args.Add("?apmoketas",apmoketas);
                args.Add("?sumaNuo",sumaNuo);
			});

		var result = 
			Sql.MapAll<ContractsReport.Ataiskaitenas>(drc, (dre, t) => {
				t.Nr = dre.From<int>("mokId");
                t.kID = dre.From<int>("id");
				t.MokejimoData = dre.From<DateTime>("Mokejimo Data");
				t.Apmoketas = dre.From<int>("apmoketas");
				t.VardasPilnas = dre.From<string>("Pilnas Vardas");
				t.Kaina = dre.From<decimal>("Suma uz Mokejima");
				t.AtliktaMokejimu = dre.From<int>("Atlikta mokejimu");
				t.BendraSuma = dre.From<decimal>("Is Viso");
				t.TelefonoNumeris = dre.From<string>("Telefono numeris");
                t.Email = dre.From<string>("pastas");
                t.Maksimalus = dre.From<decimal>("Maksimalus mokejimas");
                t.Inicialai = dre.From<string>("Inicialai");
			});

		return result;
	}

	
}
