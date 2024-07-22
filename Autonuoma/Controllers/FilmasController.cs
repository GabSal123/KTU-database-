namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class FilmasController : Controller {

    [HttpGet]
    public ActionResult Index(){

        return View(FilmasRepo.Sarasas());
    }


    [HttpGet]
    public ActionResult Create(){

        var filmCE = new FilmasCE();

        PopulateLists(filmCE);
        
        return View(filmCE);
    }


    private void PopulateLists(FilmasCE filmCE){

        var inventoriai = InventoriusRepo.List();
        var aktoriai = AktoriusRepo.List();
        var ivertinimai = IvertinimaiRepo.List();
        var tiekejai = TiekejasRepo.List();
    }
}