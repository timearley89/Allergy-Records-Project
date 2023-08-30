namespace Allergy_Records_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Allergies> allergylist = new List<Allergies>();

            string? userinput = "";

            do
            {
                Console.WriteLine("\nEnter command. Type 'Help' for a list of commands.\n");
                userinput = Console.ReadLine();
                if (userinput == null) { userinput = ""; }
                if (userinput.ToLower() == "exit" || userinput == "") { continue; }

                switch (userinput.ToLower())
                {
                    case "help":
                        {
                            Console.WriteLine("\nAvailable commands:\n");
                            Console.WriteLine("Add Record\t\tAdds a new allergy record to the datastore.");                 //implemented
                            Console.WriteLine("Delete Record\t\tRemoves an existing allergy record from the datastore.");   //implemented
                            Console.WriteLine("Add Allergy\t\tAdds an allergy to an existing record.");                     //implemented
                            Console.WriteLine("Delete Allergy\t\tRemoves an allergy from an existing record.");             //implemented
                            Console.WriteLine("Show All\t\tDisplays all stored records and their allergies.");              //implemented
                            Console.WriteLine("Delete All\t\tRemoves all records in the datastore.");                       //implemented
                            Console.WriteLine("Help\t\t\tDisplays this information.");                                      //implemented
                            Console.WriteLine("Exit\t\t\tTerminates the allergy record system program.\n");                 //implemented
                            break;
                        }
                    case "show all":
                        {
                            Console.WriteLine("");
                            if (allergylist.Count == 0) { Console.WriteLine("\nNo records in datastore. Add a record by entering the 'Add Record' command.\n"); break; }
                            foreach (Allergies allergy in allergylist)
                            {
                                Console.WriteLine($"\nRecord Name: {allergy.Name}");
                                if (allergy.Allergens.Count == 0) { Console.WriteLine("\t\tNo Allergies"); }
                                allergy.Sort();
                                foreach (Allergies.Allergen allergen in allergy.Allergens)
                                {
                                    Console.WriteLine("\t\t" + allergen.ToString());
                                }
                                Console.WriteLine("");
                            }
                            break;
                        }
                    case "add record":
                        {
                            Console.WriteLine("\nEnter name for new record:");
                            string? tempname = Console.ReadLine();
                            if (tempname != "" && tempname != null)
                            {
                                if (!allergylist.Exists(z => z.Name == tempname))
                                {
                                    allergylist.Add(new Allergies(tempname));
                                    Console.WriteLine($"\nRecord for {tempname} added!\n");
                                }
                                else
                                {
                                    Console.WriteLine($"\nRecord for {tempname} already exists!\n");
                                }

                            }
                            else { Console.WriteLine("Invalid name entry. Aborting add."); }
                            break;
                        }
                    case "add allergy":
                        {
                            Console.WriteLine("\nEnter record name to add allergy for:\n");
                            string? recname = Console.ReadLine();
                            if (recname == null || recname == "") { Console.WriteLine("Invalid name entered. Aborting."); break; }
                            if (allergylist.Exists(X => X.Name == recname))
                            {
                                Console.WriteLine($"\nRecord {recname} found. Enter allergy or allergies to add. Possible allergies include:\n");
                                foreach (string allword in Enum.GetNames(typeof(Allergies.Allergen)))
                                {
                                    Console.WriteLine(allword);
                                }
                                Console.WriteLine("");
                                string? tempallergen = Console.ReadLine();
                                try
                                {
                                    ((Allergies?)allergylist.Find(x => x.Name == recname)).AddAllergy(tempallergen);
                                    Console.WriteLine("\nAdded!");
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine($"\nInvalid allergy entered. {ex.Message} Aborting.\n");
                                    break;
                                }
                            }
                            else { Console.WriteLine($"\nRecord {recname} not found. Aborting."); }
                            break;
                        }
                    case "delete all":
                        {
                            Console.WriteLine($"\n{allergylist.Count.ToString()} records exist currently. Are you sure you want to delete ALL records?\n");
                            if (Console.ReadLine().ToLower().Trim() == "yes")
                            {
                                allergylist.Clear();
                                Console.WriteLine("\nAll records deleted.\n");
                            }
                            else { Console.WriteLine("\nAborting delete command.\n"); }
                            break;
                        }
                    case "delete record":
                        {
                            Console.WriteLine("\nEnter record name to delete:\n");
                            string? recname = Console.ReadLine();
                            if (recname != null && recname != "")
                            {
                                if (allergylist.Exists(x => x.Name == recname))
                                {
                                    Console.WriteLine($"\nRecord {recname} found. Delete record?");
                                    if (Console.ReadLine().ToLower().Trim() == "yes")
                                    {
                                        allergylist.Remove(allergylist.Find(y => y.Name == recname));
                                        Console.WriteLine("\nRecord Deleted.\n");

                                    }
                                    else { Console.WriteLine("\nAborting Delete.\n"); }
                                }
                                else { Console.WriteLine($"\nRecord {recname} not found. Aborting."); }
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid record name entered. Aborting.\n");
                            }
                            break;
                        }
                    case "delete allergy":
                        {
                            Console.WriteLine("\nEnter record to delete allergy from:\n");
                            string? recname = Console.ReadLine();
                            if (recname != null && recname != "")
                            {
                                if (allergylist.Exists(z => z.Name == recname))
                                {
                                    Console.WriteLine($"\nRecord {recname} found. Enter allergy or allergies to delete:\n");
                                    string? allergys = Console.ReadLine();
                                    if (allergys != null && allergys != "")
                                    {
                                        allergylist.Find(y => y.Name == recname).DeleteAllergy(allergys);
                                        Console.WriteLine("\nDeleted.\n");
                                        break;
                                    }
                                    else { Console.WriteLine("\nInvalid entry. Aborting.\n"); break; }
                                }
                                else { Console.WriteLine($"\nRecord {recname} not found. Aborting.\n"); break; }
                            }
                            else { Console.WriteLine("\nInvalid record name entered. Aborting.\n"); break; }
                        }
                }

            } while (userinput.ToLower() != "exit");
            Console.WriteLine("\nThank you for testing the allergy record system! Goodbye!");
        }
    }
    public class Allergies : IEquatable<Allergies>//functional
    {

        [Flags]
        public enum Allergen
        {
            //All constants defined here must have a value equal to double that of the constant before it!
            Eggs = 1,           //2^0
            Peanuts = 2,        //2^1
            Shellfish = 4,      //2^2
            Strawberries = 8,   //2^3
            Tomatoes = 16,      //2^4
            Chocolate = 32,     //2^5
            Pollen = 64,        //2^6
            Cats = 128,         //2^7
            Dogs = 256,         //2^8
            Penicillin = 512,   //2^9
            Poison_Ivy = 1024   //2^10
        }
        private List<Allergen> allergens;
        private string name;


        // constructors
        public Allergies(string inname)
        {
            this.name = inname;
            this.allergens = new List<Allergen>();
        }
        public Allergies(string inname, int inscore)
        {
            this.name = inname;
            this.allergens = new List<Allergen>();
            for (int i = Enum.GetNames<Allergen>().Length - 1; i >= 0; i--)
            {
                if (inscore - Math.Pow(2, i) >= 0)
                {
                    inscore -= (int)Math.Pow(2, i);
                    this.allergens.Add((Allergen)((int)Math.Pow(2, i)));
                }
                else
                {
                    continue;
                }
            }

        }
        public Allergies(string inname, string inallergies)
        {
            this.name = inname;
            this.allergens = new List<Allergen>();
            string[] allergywords = inallergies.Split(' ');
            foreach (string word in allergywords)
            {
                Allergen thisallergen = (Allergen)Enum.Parse(typeof(Allergen), word);
                this.allergens.Add(thisallergen);
            }

        }

        // properties (readonly)
        public string Name
        {
            get { return this.name; }
        }
        public int Score
        {
            get
            {
                int sc = 0;
                foreach (Allergen allergen in this.allergens)
                {
                    sc += (int)allergen;
                }
                return sc;
            }
        }
        public List<Allergen> Allergens
        {
            get { return this.allergens; }
        }

        // methods
        public bool IsAllergicTo(string allergy)
        {
            for (int i = 0; i < this.allergens.Count; i++)
            {
                if (this.allergens[i].ToString() == allergy) { return true; }
            }
            return false;
        }
        public bool IsAllergicTo(Allergen allergy)
        {
            return this.allergens.Contains(allergy);
        }
        public void AddAllergy(string allergy)
        {
            string[] allergyinput = allergy.Split(' ');
            foreach (string word in allergyinput)
            {
                Allergen thisallergen = (Allergen)Enum.Parse(typeof(Allergen), word);
                if (!this.allergens.Contains(thisallergen))
                {
                    this.allergens.Add(thisallergen);
                }
            }

            return;
        }
        public void AddAllergy(Allergen allergy)
        {
            if (!this.allergens.Contains(allergy))
            {
                this.allergens.Add(allergy);
            }
            return;
        }
        public void DeleteAllergy(string allergy)
        {
            string[] delallergies = allergy.Split(' ');
            foreach (string thisallergy in delallergies)
            {
                if (this.allergens.Exists(z => z == (Allergen)Enum.Parse(typeof(Allergen), thisallergy)))
                {
                    this.allergens.Remove((Allergen)Enum.Parse(typeof(Allergen), thisallergy));
                }
            }
            return;
        }
        public void DeleteAllergy(Allergen allergy)
        {
            if (this.allergens.Contains(allergy))
            {
                this.allergens.Remove(allergy);
            }
            return;
        }
        public override string ToString()
        {
            this.Sort();
            if (this.allergens.Count == 0)
            {
                return $"{this.name} has no allergies!";
            }
            else if (this.allergens.Count == 1)
            {
                return $"{this.name} is allergic to {this.allergens[0].ToString()}.";
            }
            else if (this.allergens.Count == 2)
            {
                return $"{this.name} is allergic to {this.allergens[0].ToString()} and {this.allergens[1].ToString()}.";
            }
            else
            {
                string output = $"{this.name} is allergic to ";
                for (int i = 0; i < this.allergens.Count - 2; i++)
                {
                    output += $"{this.allergens[i].ToString()}, ";
                }

                output += $"{this.allergens[this.allergens.Count - 2].ToString()} and {this.allergens[this.allergens.Count - 1].ToString()}.";
                return output;
            }
        }
        public void Sort()
        {
            List<Allergen> templist = new List<Allergen>();
            int myscore = 0;
            foreach (Allergen allergy in this.allergens)
            {
                myscore += (int)allergy;
            }
            for (int i = Enum.GetNames<Allergen>().Length - 1; i >= 0; i--)
            {
                if (myscore - Math.Pow(2, i) >= 0)
                {
                    myscore -= (int)Math.Pow(2, i);
                    templist.Insert(0, (Allergen)((int)Math.Pow(2, i)));
                }
            }
            this.allergens = templist;
        }
        public bool Equals(Allergies? other)
        {
            if (this == null || other == null) { return false; }
            if (this.allergens == null || other.allergens == null) { return false; }
            if (this.Name != other.Name) { return false; }
            if (this.allergens.Count != other.allergens.Count) { return false; }
            this.Sort(); other.Sort();
            for (int i = 0; i < this.allergens.Count; i++)
            {
                if (this.allergens[i] != other.allergens[i]) { return false; }
            }
            return true;
        }
    }
}