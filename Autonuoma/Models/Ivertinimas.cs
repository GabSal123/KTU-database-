namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


public class IvertinimaiList{

    [DisplayName("ID")]
    public int Id{get;set;}
    
    [DisplayName("Filmo pavadinimas")]
    public string Filmo_pavadinimas {get;set;}

    [DisplayName("Ivertinimas")]
    public int Reitingas {get;set;}
}