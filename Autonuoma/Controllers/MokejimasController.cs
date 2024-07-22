namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class MokejimasController : Controller{

	//ideti property i db, kad mokejimas apmoketas arba ne!
	//pakeisti calculate sum pagal kieki, 

    [HttpGet]
    public ActionResult Index(){

        return View(MokejimasRepo.List());
    }

    
    [HttpGet]
    public ActionResult Create(){

        var mokCE = new MokejimasCE();
        mokCE.mokejimas.Mokejimo_data = DateTime.Now;

        PopulateLists(mokCE);

        return View(mokCE);
        
    }


    [HttpPost]
	public ActionResult Create(int? save, int? add, int? remove, MokejimasCE mokCE)
	{
		//addition of new 'UzsakytosPaslaugos' record was requested?
		if( add != null )
		{

			//add entry for the new record
			var up =
				new MokejimasCE.PirktasFilmas {
					InListID =
						mokCE.uzsakytiFilmai.Count > 0 ?
						mokCE.uzsakytiFilmai.Max(it => it.InListID) + 1 :
						0,

					Filmas = null,
					Kaina = 0,
					Kiekis = 0
				};
			mokCE.uzsakytiFilmai.Add(up);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(mokCE);
			return View(mokCE);
		}

		//removal of existing 'UzsakytosPaslaugos' record was requested?
		if( remove != null )
		{
			//filter out 'UzsakytosPaslaugos' record having in-list-id the same as the given one
			mokCE.uzsakytiFilmai =
				mokCE
					.uzsakytiFilmai
					.Where(it => it.InListID != remove.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(mokCE);
			return View(mokCE);
		}

		//save of the form data was requested?
		if( save != null )
		{
			//check for attemps to create duplicate 'UzsakytaPaslauga'records
			for( var i = 0; i < mokCE.uzsakytiFilmai.Count-1; i ++ )
			{
				var refUp = mokCE.uzsakytiFilmai[i];

				for( var j = i+1; j < mokCE.uzsakytiFilmai.Count; j++ )
				{
					var testUp = mokCE.uzsakytiFilmai[j];
					
					if( testUp.Filmas == refUp.Filmas )
						ModelState.AddModelError($"UzsakytiFilmai[{j}].Paslauga", "Kartojasi filmai, nedaryti taip.");
				}
			}

			//form field validation passed?
			if( ModelState.IsValid )
			{
                mokCE.RemoveNull();
				//tikriname ar uzsakytas bent 1 filmas
				if(mokCE.uzsakytiFilmai.Count==0){
					ModelState.AddModelError(string.Empty, "Mokejimas turi buti bent uz viena filma");
					return View(mokCE);
				}
				
				

                //skaiciuosim suma
				
                var films = InventoriusRepo.List();

				ChangeList(mokCE);
				//tikriname ar uztenka filmu
				foreach(var filmas in mokCE.uzsakytiFilmai){
					bool enough = InventoriusRepo.EnoughMovies(filmas.Inventorius,filmas.Kiekis);
					if(!enough){
						ModelState.AddModelError(string.Empty, $"{filmas.Filmas} nera pakankamai, viso liko {filmas.Kiekis}");
						return View(mokCE);
					}
				}

            mokCE.mokejimas.Id = MokejimasRepo.InsertMokejimas(mokCE);
				
				//create new 'UzsakytosPaslaugos' records
				foreach( var upVm in mokCE.uzsakytiFilmai ){
					MokejimasRepo.InsertUzsakytaPaslauga(mokCE.mokejimas.Id, upVm);
					//updatinu inventory reiksmes
					int senasKiekis = InventoriusRepo.GetInventoryCount(upVm.Inventorius);
					InventoriusRepo.UpdateInventoryValues(upVm.Inventorius,-upVm.Kiekis,senasKiekis);
				}
					

				//save success, go back to the entity list
				
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			else
			{
				PopulateLists(mokCE);
				return View(mokCE);
			}
		}


		
        
        throw new Exception("Should not reach here.");
	}


	[HttpGet]
	public ActionResult Edit(int id)
	{
		var mokCE = MokejimasRepo.FindMokejimasNr(id);
		
		mokCE.uzsakytiFilmai = MokejimasRepo.ListUzsakytasFilmas(id);			

		PopulateLists(mokCE);
		ChangeList(mokCE);
		return View(mokCE);
	}


	[HttpPost]
	public ActionResult Edit(int? save, int? add, int? remove, MokejimasCE mokCE)
	{
		//addition of new 'UzsakytosPaslaugos' record was requested?
		if( add != null )
		{
			ChangeList(mokCE);
			//add entry for the new record
			var up =
				new MokejimasCE.PirktasFilmas {
					InListID =
						mokCE.uzsakytiFilmai.Count > 0 ?
						mokCE.uzsakytiFilmai.Max(it => it.InListID) + 1 :
						0,

					Filmas = null,
					Kaina = 0
				};
			mokCE.uzsakytiFilmai.Add(up);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form

			PopulateLists(mokCE);
			return View(mokCE);
		}

		//removal of existing 'UzsakytosPaslaugos' record was requested?
		if( remove != null )
		{
			//updatinam sena inventoriaus kieki
			var trinamasFilmas = mokCE.uzsakytiFilmai.Where(it => it.InListID == remove.Value).ToList();;
			int senasKiekis = InventoriusRepo.GetInventoryCount(trinamasFilmas[0].Inventorius);
			int senasUzsakytasKiekis = InventoriusRepo.GetOldOrderedInventoryCount(trinamasFilmas[0].Inventorius,mokCE.mokejimas.Id);
			InventoriusRepo.UpdateInventoryValues(trinamasFilmas[0].Inventorius,senasUzsakytasKiekis,senasKiekis);
			//filter out 'UzsakytosPaslaugos' record having in-list-id the same as the given one
			mokCE.uzsakytiFilmai =
				mokCE
					.uzsakytiFilmai
					.Where(it => it.InListID != remove.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(mokCE);
			ChangeList(mokCE);
			return View(mokCE);
		}

		//save of the form data was requested?
		if( save != null )
		{
			//check for attemps to create duplicate 'UzsakytaPaslauga'records
			for( var i = 0; i < mokCE.uzsakytiFilmai.Count-1; i ++ )
			{
				var refUp = mokCE.uzsakytiFilmai[i];

				for( var j = i+1; j < mokCE.uzsakytiFilmai.Count; j++ )
				{
					var testUp = mokCE.uzsakytiFilmai[j];
					
					if( testUp.Filmas == refUp.Filmas )
						ModelState.AddModelError($"UzsakytiFilmai[{j}].Paslauga", "Kartojasi filmai, nedaryti taip.");
				}
			}

			//form field validation passed?
			if( ModelState.IsValid )
			{
                mokCE.RemoveNull();

				if(mokCE.uzsakytiFilmai.Count==0){
					ModelState.AddModelError(string.Empty, "Mokejimas turi buti bent uz 1 filma.");
					//jei filmas buvo istrintas, ir duombazej atnaujinom kieki, taciau nepavyko issaugoti, reikia atgrazinti kieki

					//jei istrinem pasirinkta filma ir duombazej atnaujinom kieki, taciau nepavyko uzbaigti operacijos
					//grazinam sena busena

		
					mokCE.uzsakytiFilmai = MokejimasRepo.ListUzsakytasFilmas(mokCE.mokejimas.Id);
					if(mokCE.uzsakytiFilmai.Count != 0){
						foreach( var upVm in mokCE.uzsakytiFilmai ){
							//updatinu inventory reiksmes
							int senasKiekis = InventoriusRepo.GetInventoryCount(upVm.Inventorius);
							InventoriusRepo.UpdateInventoryValues(upVm.Inventorius,-upVm.Kiekis,senasKiekis);
						}
					}

					return View(mokCE);
				}
				
				
                var films = InventoriusRepo.List();

			ChangeList(mokCE);

			foreach(var filmas in mokCE.uzsakytiFilmai){
					bool enough = InventoriusRepo.EnoughMovies(filmas.Inventorius,filmas.Kiekis);
					if(!enough){
						ModelState.AddModelError(string.Empty, $"{filmas.Filmas} nera pakankamai, viso liko {filmas.Kiekis}");
						return View(mokCE);
					}
				}

			
            MokejimasRepo.UpdateMokejimas(mokCE);


			
				
				foreach( var upVm in mokCE.uzsakytiFilmai ){
					int senasKiekis = InventoriusRepo.GetInventoryCount(upVm.Inventorius);
					int senasUzsakytasKiekis = InventoriusRepo.GetOldOrderedInventoryCount(upVm.Inventorius,mokCE.mokejimas.Id);
					InventoriusRepo.UpdateInventoryValues(upVm.Inventorius,senasUzsakytasKiekis,senasKiekis);
				}
				//delete all old 'uzsakyti filmai' records
			MokejimasRepo.DeleteUzsakytasFilmas(mokCE.mokejimas.Id);

				//create new 'UzsakytosPaslaugos' records
				foreach( var upVm in mokCE.uzsakytiFilmai ){
					MokejimasRepo.InsertUzsakytaPaslauga(mokCE.mokejimas.Id, upVm);
					int senasKiekis = InventoriusRepo.GetInventoryCount(upVm.Inventorius);
					InventoriusRepo.UpdateInventoryValues(upVm.Inventorius,-upVm.Kiekis,senasKiekis);
				}
					

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			else
			{
				PopulateLists(mokCE);
				return View(mokCE);
			}
		}


		
        
        throw new Exception("Should not reach here.");
	}

  


	[HttpGet]
	public ActionResult Delete(int id)
	{
		var mokCE = MokejimasRepo.FindMokejimasNr(id);
		return View(mokCE);
	}

	[HttpPost]
	public ActionResult DeleteConfirm(int id)
	{
		//load 'Sutartis'
		var mokCE = MokejimasRepo.FindMokejimasNr(id);

		//'Sutartis' is in the state where deletion is permitted?
		if(!mokCE.mokejimas.ApmoketasB)
		{
			//delete the entity
			MokejimasRepo.DeleteFilmaiForMokejimas(id);
			MokejimasRepo.DeleteMokejimas(id);

			return RedirectToAction("Index");
		}
		//'Sutartis' is in state where deletion is not permitted
		else
		{
			//enable explanatory message and show delete form
			ViewData["deletionNotPermitted"] = true;
			return View("Delete", mokCE);
		}
	}

    private void PopulateLists(MokejimasCE mokCE){
        var darbuotojai = DarbuotojasRepo.List();
        var klientai = KlientasRepo.List();
        var filmai = InventoriusRepo.List();

         mokCE.Lists.Darbuotojai = darbuotojai.Select(
            x=>{
                return new SelectListItem{
                    Value = Convert.ToString(x.Id),
                    Text = x.Vardas + " " + x.Pavarde
                };
            }
        ).ToList();


        mokCE.Lists.Klientai = klientai.Select(
            x=>{
                return new SelectListItem{
                    Value = Convert.ToString(x.Id),
                    Text = x.Vardas + " " + x.Pavarde
                };
            }
        ).ToList();


        mokCE.Lists.Filmai = filmai.Select(
            x=>{
                return new SelectListItem{
                    Value = x.Filmo_pavadinimas,
                    Text = $"{x.Filmo_pavadinimas} ({x.Kaina}) EUR"
                };
            }
        ).ToList();  

		IList<SelectListItem> listas = new List<SelectListItem>();

		var item1 = new SelectListItem{
			Value = "Apmoketas",
			Text = "Apmoketas"
		};  
		var item2 = new SelectListItem{
			Value = "Neapmoketas",
			Text = "Neapmoketas"
		};
		listas.Add(item1);  
		listas.Add(item2);  

		mokCE.Lists.Apmokejimas = listas;
    }


	private void ChangeList(MokejimasCE mokCE){
		var films = InventoriusRepo.List();

		foreach(var film in films){
			foreach(var f in mokCE.uzsakytiFilmai){
				if(film.Id == f.Inventorius){
					f.Filmas = film.Filmo_pavadinimas;
					f.Kaina = film.Kaina;
					f.Inventorius = film.Id;
			}else if(film.Filmo_pavadinimas == f.Filmas){			
					f.Filmas = film.Filmo_pavadinimas;
					f.Kaina = film.Kaina;
					f.Inventorius = film.Id;
			}		
		}
	}
    
}}