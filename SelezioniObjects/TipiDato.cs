using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

/// <summary>
/// Summary description for TipiDato
/// </summary>
namespace SelezioniObjects
{
	[Flags]
	public enum ModOp : int
	{
		Inserisci = 1,
		Modifica = 2,
		Duplica = 3,
		Visualizza = 4
	}

	[Flags]
	public enum StatoElab : int
	{
		NoStato = 0,
		SCrea = 10,
        SInd = 20,
        SIndDet = 30,
        SRiga = 40,
        SAttivo = 50
	}

	[Flags]
	public enum TipoOp : int
	{
		NoOp = 0,
        InsT = 14,
        DelT = 15,
		ModT = 1,
		InsInd = 2,
		ModInd = 3,
		DuplInd = 4,
		DelInd = 5,
		InsIndDet = 6,
        ModIndDet = 7,
        DuplIndDet = 8,
        DelIndDet = 9,
        InsRiga = 10,
        ModRiga = 11,
        DuplRiga = 12,
        DelRiga = 13,
        InsPunt = 14,
        ModPunt = 15,
        DuplPunt = 16,
        DelPunt = 17
	}

    [Flags]
    public enum Stato : int
    {
        NoTst = 0,
        Tst = 1,
        Ind = 2,
        Det = 3, 
        Riga = 4,
        Punt = 5,
        RigaPunt = 6,
        SoloTst = 7
    }
}