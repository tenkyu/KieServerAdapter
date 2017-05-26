using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KieServerAdapter.Test
{
    public class Faktor
    {
        public string Kod { get; set; }
        public double? Carpan { get; set; }
    }

    public class Teminat
    {
        public string Kod { get; set; }
        public double? BazPrim { get; set; }
        public double? NetPrim { get; set; }
        public int? KomisyonOran { get; set; }
        public double? KomisyonTutar { get; set; }
    }

    public class Sigortali
    {
        public int? Yas { get; set; }
        public DateTime? TanzimTarihi { get; set; }
        public string Cinsiyet { get; set; }
        public bool YenilemeGarantisiVarMi { get; set; }
        public string EdinimTuru { get; set; }
        public int? KacYildirBizimSigortalimiz { get; set; }
        public string Plan { get; set; }
        public string IkametIl { get; set; }
        public double? FerdiKazaBedeli { get; set; }
        public string KisiTipi { get; set; }
        public double? OncekiYilSaglikPrimi { get; set; }
        public List<Faktor> Faktorler { get; set; }
        public List<Teminat> Teminatlar { get; set; }
    }

    [TestClass]
    public class KieExecuterTest
    {
        [TestMethod]
        public async Task ExecuteAsync()
        {
            var insertObject = new Sigortali { Cinsiyet = "K", TanzimTarihi = DateTime.Today, EdinimTuru = "R", Yas = 40, KacYildirBizimSigortalimiz = 1, Plan = "PLAN_D", IkametIl = "06", FerdiKazaBedeli = 25000, KisiTipi = "G", OncekiYilSaglikPrimi = 600};

            var executer = new KieExecuter { HostUrl = "http://10.100.0.67:8082", AuthUserName = "kieserver", AuthPassword = "kieserver1!" };

            executer.AddInsert(insertObject, "groupama.turkuaz.Sigortali");
            executer.AddStartProcess("turkuaz.Flow_Turkuaz");

            //var response = await executer.ExecuteAsync("turkuaz");
            var response = await executer.ExecuteAsync<Sigortali>("turkuaz");

            Assert.IsTrue(response.Type.Equals("SUCCESS"));
        }
    }
}
