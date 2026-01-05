namespace Interface;

interface ITuote{
    string Nimi {get;set;}
    double Hinta {get;set;}
    string Valuutta {get;set;}
    
    int Varasto {get;set;}
    void Laheta(Asiakas _asiakas);
}

class FyysinenTuote : ITuote{
    public string Nimi {get;set;}
    public double Hinta {get;set;}
    public string Valuutta {get;set;}  
    public int Varasto {get;set;}
    public FyysinenTuote(string _nimi, double _hinta, string _valuutta, int _varasto){
        Nimi = _nimi;
        Hinta = _hinta;
        Valuutta = _valuutta;
        Varasto = _varasto;
    }

    public void Laheta(Asiakas _asiakas){
        Console.WriteLine($"Tuote lähti matkaan osoitteeseen {_asiakas.osoite}");
    }    
}
class LadattavaTuote : ITuote{
    public string Nimi {get;set;}
    public double Hinta {get;set;}
    public string Valuutta {get;set;}
    public int Varasto {get;set;}
    public int Koodi {get;set;}

    public LadattavaTuote(string _nimi, double _hinta, string _valuutta, int _varasto, int _koodi){
        Nimi = _nimi;
        Hinta = _hinta;
        Valuutta = _valuutta;
        Varasto = _varasto;
        Koodi = _koodi;
    }

    public void Laheta(Asiakas _asiakas){
        Console.WriteLine($"Tuote koodi on lähetetty osoitteeseen {_asiakas.email}");
    }    
}

class Asiakas{
    public string nimi;
    public string osoite;
    public string email;

    public Asiakas(string _nimi, string _osoite, string _email){
        nimi = _nimi;
        osoite = _osoite;
        email = _email;
    }
}

static class UI{
    static void Kuitti(Asiakas asiakas, List<ITuote> tuotteet){
        string tiedostonNimi = "./kuittiOstoksistasi.txt";

        using (StreamWriter writer = new StreamWriter(tiedostonNimi))
        {
            writer.WriteLine("-------------Verkkis.fi----------------");
            writer.WriteLine("---------------------------------------");
            writer.WriteLine("Kiitos asioinnistasi verkkokaupassamme!");
            writer.WriteLine("---------------------------------------");
            writer.WriteLine("Asiakas:");
            writer.WriteLine(asiakas.nimi);
            writer.WriteLine(asiakas.osoite);
            writer.WriteLine(asiakas.email);
            writer.WriteLine();
            writer.WriteLine("\nTuotteet:");
            foreach (var tuote in tuotteet)
            {
                writer.WriteLine($"{tuote.Nimi} - {tuote.Hinta}€");
            }
            writer.WriteLine($"Yhteensä (sis. ALV 25%): {tuotteet.Sum(t=>t.Hinta)}€");
            writer.WriteLine("\nTuotteet lähetetään n. kolmen (3) arkipäivän sisällä tilauksestanne.");
        }
        Console.Clear();
        foreach (var rivi in File.ReadLines(tiedostonNimi))
        {
            Console.WriteLine(rivi);
        }
    }
    public static bool Tilaa(List<ITuote> tuotteet){
        double loppusumma = tuotteet.Sum(t=>t.Hinta);
        Console.Clear();
        Console.WriteLine("### Tilauslomake ###");
        Console.WriteLine("Ostoskorin loppusumma: " + loppusumma);
        Console.Write("Nimi: ");
        string nimi = Console.ReadLine()!;
        Console.Write("Osoite: ");
        string osoite = Console.ReadLine()!;
        Console.Write("Email (@ pakollinen merkki): ");
        string email = Console.ReadLine()!;

        if(nimi != "" && osoite != "" && email != "" && email.Contains('@')){
            Asiakas asiakas = new Asiakas(nimi,osoite,email);
            Kuitti(asiakas,tuotteet);
            return true;
        }
        
        return false;
    }

    public static void OstaNayttoListaus(ITuote[] tuotteet, List<ITuote> ostoskori){
        Console.WriteLine("Siirrä tuote ostoskoriin valitsemalla tuotenumero:");
        foreach (var (item, i) in tuotteet.Select((value, index) => (value, index)))
        {
            Console.WriteLine($"#{i}. {item.Nimi}");
        }
        Console.WriteLine("100. Jatka ->");

        if (ostoskori.Count() > 0)
            Console.WriteLine("\nOstoskori: ");
            
        foreach (var item in ostoskori)
        {
            Console.Write(item.Nimi + " ");
        }
    }

    public static bool OstaNayttoValinta(ITuote[] tuotteet, List<ITuote> ostoskori){
        try //try epäonnistuu jos käyttäjä syöttää jotain muuta kuin numeron
        {
            Console.Write("\nValinta: ");
            int valinta = int.Parse(Console.ReadLine()!);

            if(valinta == 100) return false; //jos käyttäjä haluaa jatkaa ostoksia

            ostoskori.Add(tuotteet[valinta]);
            Console.Clear();
            Console.WriteLine($"### Tuote [{tuotteet[valinta].Nimi}] lisätty ostoskoriin. ###");
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine("### Virheellinen valinta! ###");
        }
        return true;
    }

    public static int TuoteLisattyMitaSeuraavaksi(){
        int valinta = 0;
        try
        {
            Console.Clear();
            Console.WriteLine("Valitse seuraava vaihe:");
            Console.WriteLine("1. Jatka ostoksia");
            Console.WriteLine("2. Poista tuote ostoskorista (Tehtävä: rakenna tämä ominaisuus)");
            Console.WriteLine("3. Tilaa tuotteet");
            Console.WriteLine("0. Poistu kaupasta\n----");
            Console.Write("Valinta: ");
            valinta = int.Parse(Console.ReadLine()!);
        }catch{
            Console.WriteLine("Virhe!");
        }
        
        return valinta;
    }

}

class Program
{
    static List<ITuote> ostoskori = new List<ITuote>();
    public static void Main(string[] args)
    {
        FyysinenTuote tuote1 = new FyysinenTuote("Lompakko", 10, "euro", 2);
        LadattavaTuote tuote2 = new LadattavaTuote("Konserttilippu", 10, "euro", 2, 29348349);
        FyysinenTuote tuote3 = new FyysinenTuote("Sakset", 5, "euro", 2);
        LadattavaTuote tuote4 = new LadattavaTuote("Aktivointikoodi (Win XP)", 49, "euro", 2, 12300094);

        ITuote[] tuotteet = new ITuote[] { tuote1, tuote2, tuote3, tuote4 };

        bool UIactive = true;

        Console.Clear();
        Console.WriteLine("--Verkkokauppa 4000---");

        while (UIactive)
        {
            UI.OstaNayttoListaus(tuotteet, ostoskori); //listaa tuotteet
            if (!UI.OstaNayttoValinta(tuotteet, ostoskori)){ //lisää tuote ostoskoriin ja näytä viesti
                Console.WriteLine("jatko");
                int valinta = UI.TuoteLisattyMitaSeuraavaksi(); //mitä seuraavaksi

                switch (valinta)
                {
                    case 1: //jatka ostoksia
                        Console.Clear();
                        break;
                    case 2: //poista tuote ostoskorista
                        Console.Clear();
                        Console.WriteLine("Tämä ominaisuus ei ole vielä käytössä.");
                        break;
                    case 3: //tilaa tuotteet
                        if( UI.Tilaa(ostoskori) ){
                            UIactive = false;
                        }else{
                            Console.Clear();
                            Console.WriteLine("### Tilaus epäonnistui. Täytä tiedot oikein. ###");
                        }
                        
                        break;
                    case 0: //poistu kaupasta
                        UIactive = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Virheellinen valinta!");
                        break;
                }
            }
        }
        Console.WriteLine("Kiitos käynnistä!");

    }
}
