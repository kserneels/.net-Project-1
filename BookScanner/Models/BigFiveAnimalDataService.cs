namespace BookScanner.Models
{
    public class BigFiveAnimalDataService
    {
        public static BigFiveAnimal? GetBigFiveAnimalByTag(string tag)
        {
            return new List<BigFiveAnimal>
            {
                new BigFiveAnimal
                {
                    Tag = "lion",
                    Name = "Lion",
                    Description = "The lion is a large carnivorous feline known as the 'king of the jungle.' They are social animals living in prides and are found in grasslands and savannas.",
                    Size = new SizeInfo { Weight = "Males: 330-550 lbs, Females: 265-395 lbs", Height = "4 feet at the shoulder" },
                    SocialStructure = "Lives in prides consisting of multiple related females, their cubs, and a small number of adult males.",
                    Diet = "Carnivorous, primarily hunting large ungulates like zebras, wildebeest, and buffalo.",
                    TerritorialBehavior = "Highly territorial, with males marking their territory with scent markings and roars to ward off intruders.",
                    Lifespan = "Typically 10-14 years in the wild."
                },
                new BigFiveAnimal
                {
                    Tag = "elephant",
                    Name = "Elephant",
                    Description = "African elephants are the largest land animals on Earth, known for their intelligence, social structure, and iconic long trunks.",
                    Size = new SizeInfo { Weight = "Up to 14,000 lbs", Height = "Up to 13 feet at the shoulder" },
                    SocialStructure = "Live in matriarchal herds consisting of related females and their offspring; males are typically solitary or form bachelor groups.",
                    Diet = "Herbivorous, consuming a variety of vegetation including grasses, leaves, bark, and fruit.",
                    TerritorialBehavior = "Not strictly territorial but have large home ranges they travel through in search of food and water.",
                    Lifespan = "Around 60-70 years in the wild."
                },
                new BigFiveAnimal
                {
                    Tag = "leopard",
                    Name = "Leopard",
                    Description = "Leopards are solitary and elusive big cats, known for their spotted coats and ability to adapt to various habitats, including forests, mountains, and savannas.",
                    Size = new SizeInfo { Weight = "66-176 lbs", Height = "2-2.5 feet at the shoulder" },
                    SocialStructure = "Solitary animals except during mating or when females are raising cubs.",
                    Diet = "Carnivorous, with a versatile diet including small mammals, birds, and large ungulates.",
                    TerritorialBehavior = "Territorial, with individuals marking their range with scent markings and scratches on trees.",
                    Lifespan = "About 12-17 years in the wild."
                },
                new BigFiveAnimal
                {
                    Tag = "rhinoceros",
                    Name = "Rhinoceros",
                    Description = "Rhinos are large, thick-skinned herbivores known for their distinctive horns on their snouts, which are made of keratin.",
                    Size = new SizeInfo { Weight = "Black rhinos: 1,760-3,080 lbs, White rhinos: 4,000-6,000 lbs", Height = "Black rhinos: 4.5-5.5 feet at the shoulder, White rhinos: 5-6 feet at the shoulder" },
                    SocialStructure = "Typically solitary or found in small groups; white rhinos may form larger, loosely associated groups.",
                    Diet = "Herbivorous; black rhinos browse on leaves, shoots, and branches, while white rhinos graze on grasses.",
                    TerritorialBehavior = "Territorial, with individuals marking their territory with dung and urine.",
                    Lifespan = "Around 35-50 years in the wild."
                },
                new BigFiveAnimal
                {
                    Tag = "buffalo",
                    Name = "Cape Buffalo",
                    Description = "The Cape buffalo, also known as the African buffalo, is a large bovine found in sub-Saharan Africa, known for its unpredictable nature and strength.",
                    Size = new SizeInfo { Weight = "Up to 2,000 lbs", Height = "About 5 feet at the shoulder" },
                    SocialStructure = "Lives in large herds that can number in the hundreds, providing mutual protection.",
                    Diet = "Herbivorous, primarily feeding on grasses but also consuming shrubs and leaves.",
                    TerritorialBehavior = "Not strictly territorial but herd ranges are defended against predators and rival herds.",
                    Lifespan = "Around 20-25 years in the wild."
                }
            }.FirstOrDefault(animal => animal.Tag == tag);
        }
    }
}
