namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class KategorijaList{

    [DisplayName("ID")]
    public int Id{get;set;}

    [DisplayName("Pavadinimas")]
    public string Pavadinimas{get;set;}

}