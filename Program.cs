

namespace Interface;

//ideana on tehdä yksinkertainen verkkokauppa tuote, joka voi olla digitaalinen tai fyysinen
//erona on, että toinen lähtee sähköpostilla ja toinen postitetaan
//esimerkki perustuu: https://www.youtube.com/watch?v=A7qwuFnyIpM

//kaikki interfacessa on julkista (vaikka merkkaat ominaisuudet ilman public:ia ne ovat silti public ) 
//interfacen nimeämisessä käytetään I-kirjainta alussa (toimii ilmankin, mutta se on tapa)
interface ITuote{
    string Nimi {get;set;}
    double Hinta {get;set;}
    string Valuutta {get;set;}
    
    int Varasto {get;set;}
    //int varasto; //interface ei voi sisältää kenttiä (fields)

    //ainoa toiminto on tuotteen lähetys (ei bodyä)
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
    static void Tilaa(List<ITuote> tuotteet){
        double loppusumma = tuotteet.Sum(t=>t.Hinta);
        Console.Clear();
        Console.WriteLine("----");
        Console.WriteLine("Tilauslomake");
        Console.WriteLine("Ostoskorin loppusumma: " + loppusumma);
        Console.Write("Nimi: ");
        string nimi = Console.ReadLine()!;
        Console.Write("Osoite: ");
        string osoite = Console.ReadLine()!;
        Console.Write("Email: ");
        string email = Console.ReadLine()!;

        if(nimi != "" && osoite != "" && email != "" && email.Contains('@')){
            Asiakas asiakas = new Asiakas(nimi,osoite,email);
            Kuitti(asiakas,tuotteet);

        }else{
            Console.WriteLine("#######################################");
            Console.WriteLine("Kaikki pyydetyt tiedot ovat pakollisia!");
            Tilaa(tuotteet);
        }
    }
    static int Kassa(string action){
        int valinta = 0;
        try
        {
            Console.WriteLine("----");
            Console.WriteLine("1. Jatka ostoksia");
            Console.WriteLine("2. Poista tuote ostoskorista (Tehtävä: rakenna tämä ominaisuus)");
            Console.WriteLine("3. Tilaa tuotteet");
            Console.WriteLine("0. Poistu kaupasta\n----");
            Console.Write("Valinta: ");
            valinta = int.Parse(Console.ReadLine()!);
        }catch{
            Console.WriteLine("Virhe!");
            UI.Prompti(action);
        }
        return valinta;
    }
    public static int Prompti(string action, List<ITuote>? tuotteet = null ){
        int valinta = 0;
        switch (action)
        {
            case "kassalle": valinta = Kassa(action); break;
            case "maksa": Tilaa(tuotteet!); break;
            case "poista": Poista(tuotteet!); break;
            default: valinta = Kassa(action); break;
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

        ITuote[] tuotteet = new ITuote[] {tuote1,tuote2,tuote3,tuote4};
        
        Console.Clear();
        Console.WriteLine("--Verkkokauppa 4000---");
        Console.WriteLine("Valitse tuotenumero");
        foreach (var (item,i) in tuotteet.Select((value, index) => (value, index)))
        {
            Console.WriteLine($"#{i}. {item.Nimi}");
        }

        if(ostoskori.Count() > 0)
            Console.WriteLine("\nOstoskori: ");
            foreach (var item in ostoskori)
            {
                Console.Write(item.Nimi + " ");
            }

        try
        {
            Console.Write("\nValinta: ");
            int valinta = int.Parse(Console.ReadLine()!);
            ostoskori.Add(tuotteet[valinta]);

            Console.Clear();
            Console.WriteLine($"Tuote [{tuotteet[valinta].Nimi}] lisätty ostoskoriin.");
            Console.WriteLine("\nOstoskori: ");
            foreach (var item in ostoskori)
            {
                Console.WriteLine(item.Nimi + " ");
            }

            int promptiValinta = UI.Prompti("kassalle");

            switch (promptiValinta)
            {
                case 0: 
                    Console.WriteLine("Poistutaan...");
                    Environment.Exit(0);
                    break;
                case 1: Main(new string[]{}); break;
                case 2: 
                    Console.WriteLine("Rakenna tämä ominaisuus"); 
                    UI.Prompti("poista",ostoskori);
                    break;
                case 3: 
                    Console.WriteLine("Tämä ominaisuus on vajaavainen: KORJAA"); 
                    UI.Prompti("maksa",ostoskori);
                    break;
                default: Main(new string[]{}); break;
            }
        }
        catch (Exception e)
        {
           Console.WriteLine("Virhe " + e.Message);
           Main(new string[]{});
        }
    }
}
