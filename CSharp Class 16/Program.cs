using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_Class_16
{
    class Program
    {
        static void Main(string[] args)
        {
            // Vegyük a következő problémát: van egy harmadik féltől származó (pl. NuGet könyvtárból) érkező típusunk!
            // Ki szeretnénk egészíteni ezt a típust valamilyen művelettel(metódussal), amire nekünk szükségünk van!
            // Mit tehetünk? Hisz nincs meg a forráskód?!

            // Örökölhetünk!
            // De mi van ha ez számunkra nem jó megoldás vagy nem is lehet (pl. sealed)?

            // Készíthetünk saját metódust ami átvesz egy példányt és azon dolgozik!
            // Példa: van egy Student osztályunk, ami egy külső konyvtárból jött, ezért nem módosíthatjuk a forráskódot, de nekünk szükségünk van arra, hogy tetszőleges Student korát könnyedén kiszámoljuk a születési dátum alapján!
            var student = new Student(new DateTime(1994, 09, 25));
            Console.WriteLine(Age(student));

            // Mi ezzel a probléma?
            // A metódus hívása nem úgy néz ki, mintha az osztályt egészítettük volna ki és nem is feltétlenül elég átlátható/érthető első ránézésre!

            // Mi lenne a jó megoldás?
            // Extension method!

            // Egy statikus osztályban (Extensions.cs) készítünk egy publikus statikus metódust
            // Paraméterként továbbra is egy Student példányt vesz át, azonban a paraméter elé írjuk oda a this kulcsszót
            // A this kulcsszóval jelezzük, hogy a Student osztályt egészítjük ki (extend) ezzel a metódussal
            // Így már úgy tűnik, mintha a metódus az osztály része lenne:
            Console.WriteLine(student.Age());

            // Szabályok:
            // Extension method csak statikus osztály statikus metódusa lehet
            // Egyetlen this paraméter lehet (egy metódus egyetlen osztályt egészíthet ki)
            // Ennek a paraméternek a metódus legelső paraméterének kell lennie, de utána ugyanúgy tetszőleges paraméterek szerepelhetnek
            // Az osztály protected és private tagjait továbbra sem érjük el (ahogyan bármely nem az osztály részét képező metódusban sem)

            // Fontos, hogy nem csak olyankor hasznos az extension method, ha nem módosíthatjuk a forráskódot! Olyankor is használhatjuk, ha a metódus nem illene az adott osztályba!
            // Például binárisba konvertálni a Student egy példányát, ha az nem célja az eredeti osztálynak!

            //--------------------------------------------------

            // C# 7.0-tól lehetőségünk van arra, hogy privát metódusokat definiáljunk más tagokban (pl. metódusban, konstruktorban, property accessor-ban (get, set) stb.)
            // Ezeket hívjuk lokális függvénynek
            // Példa:
            void ExampleLocalMethod()
            {
                Console.WriteLine("Ez egy lokális függvény egy metóduson belül létrehozva!");
            }

            ExampleLocalMethod();

            // Viszonylag ritkán használjuk, de van amikor jól jöhet, így érdemes tudni róla

            //--------------------------------------------------

            // Láttuk, hogy a delegate-ek egyik leghasznosabb funkciója az események kezelése
            // Azonban észrevehettük, hogy bizonyos esetekben, amikor csak 1-1 alkalomra van szükségünk egy metódusra vagy csak egy egyszerű metódus kell, akkor feleslegesnek érződhet, hogy ténylegesen létrehozunk egy teljesértékű metódust
            // Mint sok más nyelv, a C# is lehetővé teszi azt, hogy úgynevezett névtelen (anonim) függvényeket hozzunk létre

            // Ahogy a neve is sejteti, az anonim függvények, olyan függvények melyeknek nincs nevük
            // Ennek megfelelően nem is a szokásos módon hozzuk létre őket, hanem egy delegate-ben tároljuk őket (máshogy nem is tudnánk hivatkozni rájuk, hisz névtelenek)
            // Azaz minden olyan helyen használhatjuk, ahol egy delegate-et várunk
            // Az így létrehozott metódusokat lambdának nevezzük

            // Létrehozáshoz a => operátort használjuk, melynek a bal oldala a lambda paraméterei (a metódus paraméterei), a jobb oldala pedig a lambda törzse (a metódus törzse)

            // Példa:
            // Szokásos metódussal:
            Func<int, int, int> addMethod = Add;
            Console.WriteLine(addMethod(2, 3)); // 5

            // Lambdával:
            addMethod = (a, b) => a + b; // Létrehozzuk helyben a lambdát (névtelen metódust) (ez még nem meghívás, csak létrehozás!!! itt még nem fut le!!!)
            Console.WriteLine(addMethod(2, 3)); // 5 (lambda hívás delegate-n keresztül)

            // A példa lambdában láthatjuk, hogy a => operátor bal oldala lényegében a metódus paraméterei, pont úgy, ahogy az Add metódusnak van egy a és b paramétere
            // Továbbá a => operátor jobb oldalán a metódus törzse van, azaz lényegében pont úgy, ahogy az Add metódus törzse a "return a + b;"

            // Tehát lambdák segítségével egy delegaten keresztül helyben tudunk metódusokat definiálni

            // Szabályok:
            // A lambda paramétereinek típusát a fordító minden esetben kikövetkezteti (nem kellett kiírnom, hogy "int a, int b", csak "a, b")

            // Ha nincs a lambdának paramétere, akkor ezt üres zárójellel jelöljük:
            Func<int> lambda1 = () => 5;

            // Ha egyetlen paramétere van a lambdának, akkor opcionális a zárójel
            Func<int, int> lambda2 = a => a + 2;

            // Ha több paramétere van, akkor kötelező a zárójel és a szokásos módon vesszővel kell elválasztani a paramétereket
            Func<int, int, int> lambda3 = (a, b) => a - b;

            // Ha csak egyetlen kifejezésből áll a lambda és van visszatérési értéke, akkor nem kell blokkot létrehoznunk és a return-t sem kell kiírni
            Func<int, string> lambda4 = a => a.ToString();

            // Ha viszont több utasításból vagy kifejezésből áll, vagy nincs visszatérési értéke, akkor szükség van a blokkra és ha kell, a return-re is
            Func<int, double> lambda5 = a =>
            {
                a += 1;
                return a;
            };

            Action lambda6 = () =>
            {
                Console.WriteLine("Hello Lambda!");
            };

            // Fontos, hogy tényleg bárhol használhatjuk őket, ahol delegatet várunk
            // Például:
            Caller(() => 5);

            // Vagy:
            var dictionary = new Dictionary<string, Action>
            {
                { "A", () => { Console.WriteLine("Hello"); } }
            };
            dictionary["A"]();

            // Egy érdekes képessége a lambdáknak, hogy képesek az általuk látható (scope-ban levő) változókat tárolni/elkapni (capture)
            // Ebben az esetben a változó akkor is használható marad, ha amúgy kikerülne a scope-ból:
            var a = 2;
            CaptureTest(() => a > 2); // A CaptureTest metóduson belül már nem látható az "a" lokális változó, a lambda azonban ezen a ponton tárolja/elkapja (capture) a változót, így a lambda meghívásakor az "a" változó értéke elérhető

            // Nagyon fontos tulajdonságai a capture-nek/lambdának:
            // Az elkapott változók addig nem lesznek felszabadítva, amíg a lambdára él a referencia (azaz a változó által foglalt memóriaterület addig nem szabadul fel, amíg a lambda elérhető ~ változó, paraméter, adattag stb.)
            // A lambda által elkapott változó nem látható a lambdát tartalmazó metódusban (lent látható, hogy az "a" változó a CaptureTest-ben nem látható)
            // A lambda nem tud elkapni, in, ref vagy out paramétereket
            // Egy lambdán belüli return nem tér vissza az őt befoglaló metódusból (tehát a CaptureTest-nek nincs azért vége, mert a meghívott lambdában van egy return utasítás)
            // Egy lambdában nem lehet goto, break vagy continue, ha a cél nem a lambdán belül van (tehát ha a lambdát egy for, foreach, while stb-n belül hívod, akkor a lambdában levő goto/break/continue nem fog működni):
            for (int i = 0; i < 10; i++)
            {
                CaptureTest(() =>
                {
                    //break; // Nem jó

                    return true;
                });
            }

            //--------------------------------------------------

            // Hol tudjuk a lambdák és a delegate-ek erejét igazán kihasználni?
            // LINQ: Language Integrated Query
            // Segítségével SQL-szerű lekérdezéseket végezhetünk adatokon (esetünkben IEnumerable vagy IEnumerable<T>-t interfacet megvalósító objektumokon, ami nagyrészt jelenleg nekünk a kollekciókat jelenti, azaz List, Dictionary, Queue stb.)
            // Nagy része extension method-okkal van megvalósítva

            // Nézzük meg a használatát egy Animal (Animal.cs) osztállyal
            var animals = new List<Animal>
            {
                new Animal(10, "A"),
                new Animal(15, "B"),
                new Animal(3, "C"),
                new Animal(3, "D"),
                new Animal(4, "E")
            };


            var average = animals.Average(animal => animal.Age); // Az Average() egy úgynevezett selectort (Func) vár, azaz egy olyan delegatet, ami megmondja neki, hogy mi alapján átlagoljon
            var all = animals.All(animal => animal.Age < 16); // Az All() egy Predicate-et vár és megmondja, hogy az adott feltétel igaz-e minden elemre
            var any = animals.Any(animal => animal.Age < 5); // Az Any() egy Predicate-et vár és megmondja, hogy az adott feltétel igaz-e bármelyik elemre
            var count = animals.Count(animal => animal.Name == "A"); // A Count() egy Predicate-et vár és megmondja, hogy hány elemre igaz a megadott feltétel
            var first = animals.First(animal => animal.Name == "B"); // A First() egy Predicate-et vár és visszaadja az első olyan elemt, amelyre igaz a feltétel (ha nincs ilyen, akkor exception)
            var firstOrDefault = animals.FirstOrDefault(animal => animal.Name == "B"); // A FirstOrDefault() egy Predicate-et vár és visszaadja az első olyan elemet, amelyre igaz a feltétel (ha nincs ilyen, akkor alapértelmezett értéket ad, pl. null)
            var groupBy = animals.GroupBy(animal => animal.Age); // A GroupBy() egy olyan selectort (Func) vár, amely megmondja, hogy mi alapján csoportosítsa az elemeket
            var last = animals.Last(animal => animal.Name == "B"); // Mint a First(), csak az utolsó elemre
            var lastOrDefault = animals.LastOrDefault(animal => animal.Name == "B"); // Mint a FirstOrDefault(), csak az utolsó elemre
            var max = animals.Max(animal => animal.Age); // A Max() egy selectort (Func) vár, amely megmondja, hogy milyen tulajdonság alapján keressük a maximumot
            var min = animals.Min(animal => animal.Age); // A Min() egy selectort (Func) vár, amely megmondja, hogy milyen tulajdonság alapján keressük a minimumot
            var orderBy = animals.OrderBy(animal => animal.Name); // Az OrderBy() egy selectort (Func) vár, amely megmondja, hogy mely tulajdonság alapján rendezzen növekvő sorrendbe
            var orderByDescending = animals.OrderByDescending(animal => animal.Name); // Az OrderByDescending() egy selectort (Func) vár, amely megmondja, hogy mely tulajdonság alapján rendezzen csökkenő sorrendbe
            var select = animals.Select(animal => animal.Name); // A Select() egy olyan selectort (Func) vár, amely megmondja, hogy mely értékeket gyűjtse ki egy külön kollekcióba
            var single = animals.Single(animal => animal.Name == "A"); // A Single() egy Predicate-et vár és visszaadja az egyetlen olyan elemet, amire igaz a feltétel (ha több ilyen van, akkor exception)
            var singleOrDefault = animals.SingleOrDefault(animal => animal.Name == "A"); // A SingleOrDefault() egy Predicate-et vár és visszaadja az egyetlen olyan elemet, amire igaz a feltétel (ha több ilyen van, akkor alapértelmezett értéket ad, pl. null)
            var skip = animals.Skip(2); // A Skip() kihagy az elejéről adott számú elemet
            var skipLast = animals.SkipLast(2); // A SkipLast() kihagy a végéről adott számú elemet
            var skipWhile = animals.SkipWhile(animal => animal.Age > 5); // A SkipWhile() addig hagy ki elemet, amíg a megadott Predicate igaz
            var sum = animals.Sum(animal => animal.Age); // A Sum() egy olyan selectort (Func) vár, amely megmondja, hogy melyik tulajdonság alapján összegzünk
            var take = animals.Take(2); // A Take() az első megadott számú elemet veszi
            var takeLast = animals.TakeLast(2); // A TakeLast() az utolsó megadott számú elemet veszi
            var where = animals.Where(animal => animal.Age > 5); // A Where() egy Predicate-et vár és visszaadja azokat az elemeket, amikre a Predicate igaz

            // Rengeteg más LINQ metódus van

            // Az imént látott forma a LINQ to Object a method szintaxissal
            // Más formája is van és más adatokon is tud dolgozni
        }

        private static int Age(Student student)
        {
            return DateTime.Now.Year - student.Birthday.Year; // Nem teljesen pontos számolás, de a példához megfelelő lesz
        }

        private static int Add(int a, int b)
        {
            return a + b;
        }

        private static void Caller(Func<int> func)
        {
            Console.WriteLine(func());
        }

        private static void CaptureTest(Func<bool> func)
        {
            //Console.WriteLine(a); // Nem jó
            Console.WriteLine(func());
        }
    }
}
