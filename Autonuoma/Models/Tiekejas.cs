namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF3;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class TiekejasList{

    [DisplayName("ID")]
    public int Id{get;set;}

    [DisplayName("Pavadinimas")]
    public string Pavadinimas{get;set;}

    [DisplayName("Adresas")]
    public string Adresas{get;set;}
}