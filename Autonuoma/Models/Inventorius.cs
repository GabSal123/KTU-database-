namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class InventoriusList{

    [DisplayName("ID")]
    public int Id{get;set;}


    [DisplayName("Filmo pavadinimas")]
    public string Filmo_pavadinimas {get;set;}

    [DisplayName("Filmo ID")]
    public int Filmo_id {get;set;}


    [DisplayName("Kiekis")]
    public int Kiekis {get;set;}


    [DisplayName("Parduotuves ID")]
    public int Parduotuve_id{get;set;}

    [DisplayName("Trukme")]
    public int Trukme{get;set;}

    [DisplayName("Vidutinis ivertinimas")]
    public double Ivertinimas{get;set;}

    [DisplayName("Kaina")]
    public decimal Kaina{get;set;}

    [DisplayName("Kategorija")]
    public string Kategorija{get;set;}


    [DisplayName("Isleidimo data")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    [Required]
    public DateTime? Isleidimo_data {get;set;}

}

public class InventoriusCE {

    public class InventoriusM{
        
    }
}