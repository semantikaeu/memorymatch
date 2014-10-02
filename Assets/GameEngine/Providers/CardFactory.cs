namespace Assets.GameEngine.Providers
{
    using System;
    using System.Collections.Generic;
#if UNITY_WINRT && !UNITY_EDITOR
    using Path = System.IO.Path;
    using File = UnityEngine.Windows.File;
#else
    using System.IO;
#endif
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Assets.GameEngine.Data;

    using UnityEngine;

    public static class CardFactory
    {
        private static List<DeckData> savedDecks = new List<DeckData>();
        private static List<DeckData> customDecks = new List<DeckData>();

        /// <summary>
        /// Initializes static members of the <see cref="CardFactory" /> class.
        /// </summary>
        static CardFactory()
        {
            LoadCustomDecks();

            savedDecks.AddRange(customDecks);

            savedDecks.Add(new DeckData { Id = "animals_kids", Cards = GetAnimalsKids().ToList(), Title = "Animals", ResourceName = "AnimalsKids", ImageLocation = "Resources" });
            savedDecks.Add(new DeckData { Id = "minerals", Cards = GetMinerals().ToList(), Title = "Minerals", ResourceName = "Minerals", ImageLocation = "Resources" });
            savedDecks.Add(new DeckData { Id = "insects", Cards = GetInsects().ToList(), Title = "Insects", ResourceName = "Insects", ImageLocation = "Resources" });
            savedDecks.Add(new DeckData { Id = "fossils", Cards = GetFossils().ToList(), Title = "Fossils", ResourceName = "Fossils", ImageLocation = "Resources" });
        }

        public static List<DeckData> GetAllOfflineDecks()
        {
            return new List<DeckData>(savedDecks);
        }

        public static List<CardData> GetRandomDeck(int numberOfCards)
        {
            return GetInsects().Take(numberOfCards).ToList();
        }

        private static IEnumerable<CardData> GetMinerals()
        {
            List<QuestionData> questions = new List<QuestionData>();
            questions.Add(new QuestionData("rhodonite", "Rhodonite has the chemical formula (Mn, Fe, Mg, Ca)SiO<ss>3</ss>. What group of minerals does it belong to?", "Silicates,", "Oxides,", "Carbonates", 0, "chem") { CardId = 1 });
            questions.Add(new QuestionData("rhodonite", "Rhodonite can be used as", "ore of iron,", "ore of gold,", "ornamental stone", 2, "chem") { CardId = 1 });
            questions.Add(new QuestionData("torbenite", "Torbenite has the chemical formula Cu(UO<ss>2</ss>)<ss>2</ss>(PO<ss>4</ss>)<ss>2</ss>•<ss>12</ss> H<ss>2</ss>O. What group of minerals does it belong to?", "Silicates,", "Phosphates,", "Carbonates", 1, "chem") { CardId = 2 });
            questions.Add(new QuestionData("torbenite", "Torbernite is radioactive and produces outgas of", "xenon (Xe),", "radon (Rn),", "oxygen (O<ss>2</ss>).", 1, "chem") { CardId = 2 });
            questions.Add(new QuestionData("diamond", "Weight of a diamond is measured in:", "carats [ct],", "grams [g],", "becquerel [Bq]", 0, "chem") { CardId = 3 });
            questions.Add(new QuestionData("diamond", "Diamonds can be used mainly:", "in space industry,", "in chemical industry,", "as gemstones", 2, "chem") { CardId = 3 });
            questions.Add(new QuestionData("diamond", "Find the true statement.", "Diamond is the hardest known natural material with a hardness of 12 in Mohs scale,", "Diamond has the same chemical formula as graphite,", "Diamond is composed of sulphur.", 1, "chem") { CardId = 4 });
            questions.Add(new QuestionData("almandine", "Almandine has the chemical formula of Fe<ss>3</ss>Al<ss>2</ss>Si<ss>3</ss>O<ss>12</ss>. What group of minerals does it belong to?", "Sulphides,", "Oxides,", "Silicates", 2, "chem") { CardId = 5 });
            questions.Add(new QuestionData("almandine", "Almandine has a deep red colour to purple because it includes", "chrome (Cr) and aluminium (Al)", "iron (Fe) and aluminium (Al)", "potassium (K), aluminium (Al)", 1, "chem") { CardId = 5 });
            questions.Add(new QuestionData("topaz", "Find the true statement.", "Topaz has a hardness of 8 in Mohs scale,", "Its blue form is called sapphire,", "Topaz can only be yellow", 0, "chem") { CardId = 6 });
            questions.Add(new QuestionData("topaz", "Topaz belong to Silicates and its chemical formula is:", "CaCO<ss>3</ss>,", "Al<ss>2</ss>SiO<ss>4</ss>(F,OH)<ss>2</ss>,", "SiO<ss>2</ss>", 1, "chem") { CardId = 6 });
            questions.Add(new QuestionData("spessartite", "Spessartite has a chemical formula of Mn<ss>3</ss>Al<ss>2</ss>(SiO<ss>4</ss>)<ss>3</ss>. What group of minerals does it belong to?", "Sulphates,", "Oxides,", "Silicates", 2, "chem") { CardId = 7 });
            questions.Add(new QuestionData("spessartite", "Find the true statement.", "Spessartite is a manganese aluminium garnet,", "Spessartite has a typical green colour,", "The gem variety of spessartite is called ruby.", 0, "chem") { CardId = 7 });
            questions.Add(new QuestionData("arsenopyrite", "Arsenopyrite is often associated with", "gold (Au),", "mercury (Hg),", "copper (Cu)", 0, "chem") { CardId = 8 });
            questions.Add(new QuestionData("arsenopyrite", "When arsenopyrite is heated it gives off fumes, which are:", "green,", "toxic,", "radioactive.", 1, "chem") { CardId = 8 });
            questions.Add(new QuestionData("cinnabar", "Cinnabar is the common ore of", "lead (Pb),", "zinc (Zn),", "mercury (Hg).", 2, "chem") { CardId = 9 });
            questions.Add(new QuestionData("cinnabar", "Find the false statement.", "Cinnabar has been used for decoration in e.g., Southern America and China,", "Cinnabar has been used for gold and silver mining,", "Cinnabar belongs to sulphates and is common ore of mercury.", 2, "chem") { CardId = 9 });
            questions.Add(new QuestionData("malachite", "Malachite includes one important element which is directly linked to its colour:", "copper (Cu),", "aluminium (Al),", "sulphur (S)", 0, "chem") { CardId = 10 });
            questions.Add(new QuestionData("malachite", "What group of minerals does malachite belong to?", "sulphates,", "sulphides ,", "carbonates", 2, "chem") { CardId = 10 });
            questions.Add(new QuestionData("malachite", "Malachite is common ore of", "iron (Fe),", "copper (Cu),", "zinc (Zn)", 1, "chem") { CardId = 11 });
            questions.Add(new QuestionData("malachite", "Find the false statement.", "Malachite was used as a mineral pigment in green paints,", "Malachite often results from weathering of copper ores,", "Malachite is often found together with pyrite and quartz", 2, "chem") { CardId = 11 });
            questions.Add(new QuestionData("dolomite", "Dolomite is closely related to", "calcite and magnetite,", "calcite and magnesite,", "quartz and siderite.", 2, "chem") { CardId = 12 });
            questions.Add(new QuestionData("dolomite", "Dolomite in rock form is known from the Dolomite Alps, which are located in:", "Italy,", "France,", "Switzerland", 0, "chem") { CardId = 12 });
            questions.Add(new QuestionData("gold", "The carat rating of pure gold is:", "24,", "32,", "18", 0, "chem") { CardId = 13 });
            questions.Add(new QuestionData("gold", "Gold is usually alloyed with:", "silver (Ag), copper (Cu),", "palladium (Pd), zinc (Zn),", "mercury (Hg), uranium (U)", 0, "chem") { CardId = 13 });
            questions.Add(new QuestionData("gold", "Gold is linked with one famous physicist:", "Isaac Newton,", "Archimedes of Syracuse,", "Tomas A. Edison", 1, "chem") { CardId = 14 });
            questions.Add(new QuestionData("gold", "Gold is almost insoluble, but can be dissolved in Aqua Regia (\"king's water\"), which is:", "mixture of concentrated nitric acid and hydrochloric acid,", "concentrated hydrochloric acid (HCl),", "concentrated sulfuric acid (H<ss>2</ss>SO<ss>4</ss>)", 0, "chem") { CardId = 14 });
            questions.Add(new QuestionData("epidote", "Epidote has a complicated chemical formula: Ca<ss>2</ss>Al<ss>2</ss>(Fe,Al)(SiO<ss>4</ss>)(Si<ss>2</ss>O<ss>7</ss>)O(OH). What group of minerals does it belong to?", "Silicates,", "Oxides,", "Sulphides.", 0, "chem") { CardId = 15 });
            questions.Add(new QuestionData("epidote", "Epidote is often coloured", "shades of blue,", "shades of red,", "shades of green", 2, "chem") { CardId = 15 });
            questions.Add(new QuestionData("scheelite", "Scheelite is an important ore of:", "tungsten (W),", "tin (Sn),", "Manganese (Mn)", 0, "chem") { CardId = 16 });
            questions.Add(new QuestionData("scheelite", "Scheelite emits a bright sky-blue glow under ultraviolet light. The phenomen is called:", "fluorescence,", "phosphorescence,", "bioluminescence", 0, "chem") { CardId = 16 });
            questions.Add(new QuestionData("schorl", "Schorl is the most common species of", "garnet,", "tourmaline,", "quartz", 1, "chem") { CardId = 17 });
            questions.Add(new QuestionData("schorl", "A bluish to brownish black schorl belongs to the group of tourmaline as well as light blue to bluish green", "verdelit,", "rubellite,", "indigolite", 2, "chem") { CardId = 17 });
            questions.Add(new QuestionData("bauxite", "Bauxite is the world's main source of", "iron (Fe),", "aluminium (Al),", "manganese (Mn).", 1, "chem") { CardId = 18 });
            questions.Add(new QuestionData("bauxite", "Sources of bauxites are found mostly in the countries of:", "Tropical Zone,", "Subtropical Zone", "Temperate Zone.", 0, "chem") { CardId = 18 });
            questions.Add(new QuestionData("antimonite", "Antimonite is an ore of antimony (Sb) and is very often associated with:", "gold (Au),", "copper (Cu),", "iron (Fe).", 0, "chem") { CardId = 19 });
            questions.Add(new QuestionData("antimonite", "Antimonite is also used for:", "medicine,", "production of matchsticks and steel,", "food (processing) industry", 1, "chem") { CardId = 19 });
            questions.Add(new QuestionData("Gypsum", "Fill in a blank place in the sentence by suitable world. Gypsum is the definition of a hardness of______ on the Mohs scale of mineral hardness.", "5,", "2,", "4", 1, "chem") { CardId = 20 });
            questions.Add(new QuestionData("Gypsum", "Gypsum is a common mineral, with thick and extensive ___________ beds in association with sedimentary rocks.", "silicified,", "calcareous,", "evaporite", 2, "chem") { CardId = 20 });
            questions.Add(new QuestionData("Calcite", "Calcite is the most stable polymorph of calcium carbonate (CaCO<ss>3</ss>). The other polymorph is", "aragonite,", "magnetite,", "dolomite.", 0, "chem") { CardId = 21 });
            questions.Add(new QuestionData("Calcite", "Higher level of acidity can cause dissolution of calcite and release a gas of:", ", sulfur dioxide (SO<ss>2</ss>),", "nitrogen dioxide (NO<ss>2</ss>)", "carbon dioxide (CO<ss>2</ss>)", 2, "chem") { CardId = 21 });
            questions.Add(new QuestionData("azurite", "Name of this mineral corresponds to its colour. Is it:", "olivine", "rhodonite", "azurite", 2, "chem") { CardId = 22 });
            questions.Add(new QuestionData("azurite", "Azurite includes one important element associated with its colour. Is it:", "sodium (Na),", "copper (Cu),", "aluminium (Al),", 1, "chem") { CardId = 22 });
            questions.Add(new QuestionData("aragonite", "Aragonite has a chemical formula of calcium carbonate (CaCO<ss>3</ss>) and it is a polymorph of", "calcite,", "dolomite,", "magnetite", 0, "chem") { CardId = 23 });
            questions.Add(new QuestionData("aragonite", "The typical location for aragonite is Molina de Aragón located in", "France,", "Brazil,", "Spain", 2, "chem") { CardId = 23 });
            questions.Add(new QuestionData("galena", "Galena is the most important lead ore often associated with significant amounts of", "silver (Ag),", "gold (Au),", "mercury (Hg).", 0, "chem") { CardId = 24 });
            questions.Add(new QuestionData("galena", "Galena was used in the past for", "production of weapons,", "an ancient eye cosmetic (kohl),", "fertilizer in agriculture", 1, "chem") { CardId = 24 });
            questions.Add(new QuestionData("chrysoberyl", "Chrysoberyl has a chemical formula of BeAl<ss>2</ss>O<ss>4</ss>. What group of minerals does it belong to?", "Silicates,", "Oxides,", "Carbonates", 1, "chem") { CardId = 25 });
            questions.Add(new QuestionData("pyrite", "Pyrite has a chemical formula of iron sulfide (FeS<ss>2</ss>) and it is a polymorph of", "marcasite,", "magnetite,", "magnesite", 0, "chem") { CardId = 26 });
            questions.Add(new QuestionData("pyrite", "Find the true sentence. Pyrite is commercially used for", "manufacture of sulfuric acid (H<ss>2</ss>SO<ss>4</ss>),<br />", "main ore of iron (Fe),", "poison for rats", 0, "chem") { CardId = 26 });
            questions.Add(new QuestionData("fluorite", "Fluorite has a chemical formula of CaF<ss>2</ss>. What group of minerals does it belong to?", "Silicates,", "Oxides,", "Halides", 2, "chem") { CardId = 27 });
            questions.Add(new QuestionData("fluorite", "Find the false sentence", "Many samples of fluorite exhibit fluorescence under ultraviolet light, a property that takes its name from fluorite,", "Fluorite belongs to trigonal crystal system,", "Fluorite is a major source of hydrogen fluoride and hydrofluoric acid (HF)", 1, "chem") { CardId = 27 });
            questions.Add(new QuestionData("augite", "Augite is a common rock forming mineral belongs to the group of:", "pyroxene,", "amphibole", "plagioklase", 0, "chem") { CardId = 28 });
            questions.Add(new QuestionData("augite", "Augite is an essential mineral in", "mafic igneous rocks,", "felsic igneous rocks,", "quartzite rocks", 0, "chem") { CardId = 28 });
            questions.Add(new QuestionData("orthoclase", "Find the correct chemical formula of orthoclase. Known as a hint, its alternative name is potassium (K)-feldspar.", "NaAlSi<ss>3</ss>O<ss>8</ss>,", "KMgCl<ss>3</ss>•<ss>6</ss>(H<ss>2</ss>O),", "KAlSi<ss>3</ss>O<ss>8</ss>", 2, "chem") { CardId = 29 });
            questions.Add(new QuestionData("orthoclase", "Orthoclase is a common constituent of", "most granites and other felsic igneous rocks,", "most evaporite sedimentary rocks,", "basic and ultrabasic igneous rocks", 0, "chem") { CardId = 29 });
            questions.Add(new QuestionData("quartz", "A variety of quartz, whose colour ranges from a pale yellow to brown is called:", "citrine,", "rose quartz,", "amethyst", 0, "chem") { CardId = 30 });
            questions.Add(new QuestionData("quartz", "A variety of quartz, whose colour ranges from a bright to dark or dull purple is called:", "milky quartz,", "rose quartz,", "amethys", 2, "chem") { CardId = 30 });
            questions.Add(new QuestionData("quartz", "A variety of quartz, whose colour ranges from pale pink to rose red hue is called:", "citrine,", "rose quartz,", "opal", 1, "chem") { CardId = 31 });
            questions.Add(new QuestionData("quartz", "Quartz belongs to", "triclinic crystal system,", "cubic crystal system,", "trigonal crystal system", 2, "chem") { CardId = 31 });
            questions.Add(new QuestionData("opal", "Opal is a hydrated amorphous form of", "silicon dioxide (SiO<ss>2</ss>),", "calcium carbonate (CaCO<ss>3</ss>),", "Sodium chloride (NaCl).", 0, "chem") { CardId = 32 });
            questions.Add(new QuestionData("opal", "Transparent to translucent opals with warm body colours of yellow, orange, orange-yellow or red are called \"fire\" opals. The most famous source of these opals is located in", "Canada,", "Australia,", "Mexico", 2, "chem") { CardId = 32 });
            questions.Add(new QuestionData("tourmaline", "Find the mineral which does not belong to tourmaline group:", "verdelit,", "ruby,", "elbaite", 1, "chem") { CardId = 33 });
            questions.Add(new QuestionData("tourmaline", "A dark yellow to brownish black tourmaline is called", "verdelit,", "rubellite,", "dravite", 2, "chem") { CardId = 33 });
            questions.Add(new QuestionData("pyrite", "Pyrite belongs to", "orthorhombic crystal system,", "cubic crystal system,", "trigonal crystal system", 1, "chem") { CardId = 34 });
            questions.Add(new QuestionData("pyrite", "Pyrite has a chemical formula of iron sulfide (FeS<ss>2</ss>). What group of minerals does it belong to?", "Silicates,", "Oxides,", "Sulfides", 2, "chem") { CardId = 34 });
            questions.Add(new QuestionData("pyrite", "Find the false sentence.", "Pyrite was used as a source of ignition in early firearms in the 16th and 17th centuries,", "Pyrite remains in commercial use for the production of sulfur dioxide (use in e.g., paper industry),", "Pyrite is often associated with harder chalcopyrite (CuFeS<ss>2</ss>).", 2, "chem") { CardId = 35 });
            questions.Add(new QuestionData("olivine", "The mineral olivine when of gem quality is also called:", "ruby, sapphire,", "peridot, chrysolite", "amethyst, topaz", 1, "chem") { CardId = 36 });
            questions.Add(new QuestionData("olivine", "Olivin´s typically olive-green colour is thought to be a result of traces of:", "nickel (Ni),", "chrome (Cr),", "manganese (Mn)", 0, "chem") { CardId = 36 });
            questions.Add(new QuestionData("corundum", "Corundum is commonly used for:", "abrasives,", "sculptures,", "pavements", 0, "chem") { CardId = 37 });
            questions.Add(new QuestionData("corundum", "Transparent specimens of corundum used as gemstones are called:", "malachite and amethyst,", "topaz and citrine,", "ruby and sapphire", 2, "chem") { CardId = 37 });
            questions.Add(new QuestionData("halite", "Halite is commonly known as:", "common mica,", "rock salt,", "potash", 1, "chem") { CardId = 38 });
            questions.Add(new QuestionData("halite", "Which of these ways of mining is NOT suitable for halite:", "mining extraction,", "deepwater drilling,", "evaporation of sea water", 1, "chem") { CardId = 38 });
            questions.Add(new QuestionData("quartz", "Which of the following semi-precious gemstones are not a variety of quartz?", "ruby and sapphire,", "smoky quartz and citrine,", "amethyst and rose quartz", 0, "chem") { CardId = 39 });
            questions.Add(new QuestionData("quartz", "Pure quartz is transparent and is called:", "rock ice,", "obsidian,", "rock crystal", 2, "chem") { CardId = 39 });
            questions.Add(new QuestionData("cassiterite", "Cassiterite is the most important source of:", "zinc (Zn),", "lead (Pb),", "tin (Sn)", 2, "chem") { CardId = 40 });
            questions.Add(new QuestionData("cassiterite", "Quality crystals of cassiterite are also used as:", "gemstones,", "abrasive,", "weight", 0, "chem") { CardId = 40 });
            questions.Add(new QuestionData("opal", "Opal is the national gemstone of:", "Brazil,", "Australia,", "China", 1, "chem") { CardId = 41 });
            questions.Add(new QuestionData("opal", "Opal is classified as a mineraloid because of its character. Is it:", "amorphous,", "regularly shaped,", "stable composed", 0, "chem") { CardId = 41 });
            questions.Add(new QuestionData("muscovite", "Muscovite is in demand for the manufacture of:", "extra hard materials,", "water-resistant materials,", "fire-proofing materials", 2, "chem") { CardId = 42 });
            questions.Add(new QuestionData("muscovite", "Muscovite is also known as:", "common mica,", "rock salt,", "potash", 0, "chem") { CardId = 42 });

            int i = questions.Min(q => q.CardId);
            int size = questions.Max(q => q.CardId) + i;

            var cards = new List<CardData>();
            for (; i < size; ++i)
            {
                var card = new CardData(i, "res://Decks/Minerals/" + i);
                card.Title = "Minerals";
                
                var qs = questions.Where(q => q.CardId == i && q.CorrectAnswerIndex >= 0).ToList();
                card.Questions.AddRange(qs);

                cards.Add(card);
            }

            return cards;
        }

        private static IEnumerable<CardData> GetInsects()
        {
            List<QuestionData> questions = new List<QuestionData>();
            questions.Add(new QuestionData("Andrena isis canaria", "Andrena is one of the largest of all genera of", "wasps,", "sawflies,", "bees.", 2, string.Empty) { CardId = 1 });
            questions.Add(new QuestionData("Andrena isis canaria", "Fill in the suitable word. Females of Andrena bees usually prefer ____________ soils for a nesting substrate, near or under shrubs to be protected from heat and frost.", "sandy,", "clayey,", "stony", 0, string.Empty) { CardId = 1 });
            questions.Add(new QuestionData("Andrena humilis indigena Imh.", "Andreana bees belong to order Hymenoptera originated in the", "Permian,", "Jurassic,", "Triassic.", 2, string.Empty) { CardId = 2 });
            questions.Add(new QuestionData("Andrena humilis indigena Imh.", "Andrena females can be distinguished from most other small bees by", "long hair around sting,", "velvety areas in between eyes and antennas,", "long and thick antennas.", 1, string.Empty) { CardId = 2 });
            questions.Add(new QuestionData("Panurgus variegatus berberus", "Panurgus is a genus of mining bees. They prefer yellow flowers and are dependent on pollen of", "Asteraceae,", "Orchidaceae,", "Rosaceae.", 0, string.Empty) { CardId = 3 });
            questions.Add(new QuestionData("Panurgus variegatus berberus", "The nests of Panurgus bees are laid in", "clayey soil,", "sandy soil or loess,", "stony soil.", 1, string.Empty) { CardId = 3 });
            questions.Add(new QuestionData("Inachis io (Linnaeus, 1758)", "Inachis io is commonly known as", "the Cat eye butterfly,", "the Peacock butterfly,", "the Tiger butterfly.", 0, string.Empty) { CardId = 4 });
            questions.Add(new QuestionData("Inachis io (Linnaeus, 1758)", "When faced with avian predators, the Peacock butterfly threateningly displays its eyespots as well as", "quickly waves its antennae,", "lifts the abdomen,", "makes a hissing noise.", 2, string.Empty) { CardId = 4 });
            questions.Add(new QuestionData("Vanessa atalanta (Linnaeus, 1758)", "Vanessa atalanta is commonly known as", "Red admiral,", "Red major,", "Red general.", 0, string.Empty) { CardId = 5 });
            questions.Add(new QuestionData("Vanessa atalanta (Linnaeus, 1758)", "The caterpillars of Vanessa atalanta feed on", "sunflowers,", "nettles,", "lettuce.", 1, string.Empty) { CardId = 5 });
            questions.Add(new QuestionData("Apatura iris (Linnaeus, 1758)", "This butterfly, the purple emperor, does not feed from", "flowers,", "honeydew secreted by aphids,", "urine and animal carcasses.", 0, string.Empty) { CardId = 6 });
            questions.Add(new QuestionData("Apatura iris (Linnaeus, 1758)", "Purple emperor (Apatura iris) is widely distributed in Central Europe, and in suitably temperate parts of Asia, including", "Japan,", "Korea,", "China.", 2, string.Empty) { CardId = 6 });
            questions.Add(new QuestionData("Colias hyale (Linnaeus, 1758)", "Colias hale is a small butterfly of the family Pieridae, that is", "the Whites and Blues,", "the Yellows and Whites,", "the Yellows and Blues.", 2, string.Empty) { CardId = 7 });
            questions.Add(new QuestionData("Colias hyale (Linnaeus, 1758)", "Species of Lepidoptera, as this Pale Clouded Yellow, undergo holometabolism or \"complete metamorphosis\", where the insect changes in four stages. Which stage, comparing to the incomplete metamorphosis is extra?", "pupa,", "larva,", "egg", 1, string.Empty) { CardId = 7 });
            questions.Add(new QuestionData("Sesia apiformis (Clerck, 1759)", "The Hornet Moth (Sesia apiformis) is a large bulky moth which is a brilliant natural imitation of a hornet, however the moth is perfectly harmless. This phenomenon is called", "Batesian mimicry,", "aposematism,", "crypsis.", 0, string.Empty) { CardId = 8 });
            questions.Add(new QuestionData("Sesia apiformis (Clerck, 1759)", "The Hornet Moth cannot be found in", "Germany, Holland,", "Austria, Bohemia,", "Italy, Slovenia.", 2, string.Empty) { CardId = 8 });
            questions.Add(new QuestionData("Sesia apiformis (Clerck, 1759)", "Catocala fraxini is a moth also known as", "yellow underwing,", "blue underwing,", "green underwing.", 1, string.Empty) { CardId = 9 });
            questions.Add(new QuestionData("Sesia apiformis (Clerck, 1759)", "Catocala fraxini, also known as Clifden Nonpareil, is a moth. A moth is an insect related to the butterfly, both being of the order", "Lepidoptera,", "Notoptera,", "Plecoptera.", 0, string.Empty) { CardId = 9 });
            questions.Add(new QuestionData("Agrius convolvuli (Linnaeus, 1758)", "Agrius convolvuli has a very long “proboscis” (longer than its body) that enables it to", "lay eggs in slits,", "copulate,", "feed on long trumpet like flowers.", 2, string.Empty) { CardId = 10 });
            questions.Add(new QuestionData("Agrius convolvuli (Linnaeus, 1758)", "The Convolvulus Hawk-moth, Agrius convolvuli, is a large hawkmoth. It is seen in gardens hovering over the flowers and its favorite time is around", "sunrise,", "twilight,", "midnight.", 1, string.Empty) { CardId = 10 });
            questions.Add(new QuestionData("Tasgius pedator Grav.", "Beetles have different types of jaw depending on their eating habits. This representative of rove beetles is", "predator of insects,", "herbivore,", "scavenger.", 0, string.Empty) { CardId = 11 });
            questions.Add(new QuestionData("Tasgius pedator Grav.", "Genus Tasgius belongs to family of rove beetles. It is an ancient group, with fossil rove beetles known from the", "Triassic,", "Jurassic,", "Permian.", 0, string.Empty) { CardId = 11 });
            questions.Add(new QuestionData("Parajungiella bohdanecensis Ježek & Hájek, 2007", "This insect belongs to order Diptera which is characterized by", "two independent ovipositors,", "only two pairs of legs,", "only single pair of wings.", 2, string.Empty) { CardId = 12 });
            questions.Add(new QuestionData("Parajungiella bohdanecensis Ježek & Hájek, 2007", "The first true dipterans are known from the", "Jurassic,", "Triassic,", "Permian.", 1, string.Empty) { CardId = 12 });
            questions.Add(new QuestionData("Tipula Lunatipula livida aspasia B. Mannheims, 1968", "Tipula lunatipula is a species of crane fly. Adults are mostly feed on", "blood,", "other insects ,", "nectar.", 2, string.Empty) { CardId = 13 });
            questions.Add(new QuestionData("Tipula Lunatipula livida aspasia B. Mannheims, 1968", "Tipula lunatipula belongs to order Diptera. Which of following insects does not belong to the same order?", "true flies,", "bees,", "mosquitos", 1, string.Empty) { CardId = 13 });
            questions.Add(new QuestionData("Diphyus tricolor Kriechbaumer, 1890", "Females of ichneumon wasps (genus Diphyus) have long", "ovipositor for laying eggs,", "wings for better flying,", "sting for fighting with the enemy", 0, string.Empty) { CardId = 14 });
            questions.Add(new QuestionData("Diphyus tricolor Kriechbaumer, 1890", "The most species of ichneumon wasps (genus Diphyus) inject their eggs into", "the water,", "a host's body (larva or pupa),", "fruits.", 1, string.Empty) { CardId = 14 });
            questions.Add(new QuestionData("Bombus imitator Pittioni, 1949", "A bumblebee is any member of the bee genus Bombus. The buzzing sound of bees is caused by", "the beating of their wings,", "the beating of their legs,", "vibrating flight muscles.", 2, string.Empty) { CardId = 15 });
            questions.Add(new QuestionData("Bombus imitator Pittioni, 1949", "Queen and worker bumblebees can sting", "repeatedly,", "cannot sting,", "just once.", 0, string.Empty) { CardId = 15 });
            questions.Add(new QuestionData("Anoplognathus frenchi", "This beetle is a member of genus Anoplognathus which occurs in", "Africa,", "Australia,", "South America.", 1, string.Empty) { CardId = 16 });
            questions.Add(new QuestionData("Anoplognathus frenchi", "The genus Anoplognathus includes 35 species, several of which have been implicated in dieback of", "eucalyptus,", "bamboo,", "myrtle.", 0, string.Empty) { CardId = 16 });
            questions.Add(new QuestionData("Calodema regalis blairi", "Calodema is a genus of beetles in the family Buprestidae. The elytra of some Buprestidae species have been traditionally used in", "jewelry and decoration,", "gastronomy,", "medicine.", 0, string.Empty) { CardId = 17 });
            questions.Add(new QuestionData("Calodema regalis blairi", "The genus Calodema is found only in", "Australia and New Guinea,", "Central and South America,", "Mexico.", 0, string.Empty) { CardId = 17 });
            questions.Add(new QuestionData("Vesta saturnalis", "Vesta saturnalis is a member of beetle family Lampyridae which is commonly called", "lampflies,", "fireflies,", "spiritflies.", 1, string.Empty) { CardId = 18 });
            questions.Add(new QuestionData("Vesta saturnalis", "The beetle family Lampyridae (fireflies) is typical with light production due to a type of chemical reaction called bioluminescence – the enzyme luciferase acts on the luciferin. Firefly luciferase is used in", "art and painting,", "gastronomy,", "forensics and medicine.", 2, string.Empty) { CardId = 18 });
            questions.Add(new QuestionData("Hexarthrius buquetii", "Males of this “stag beetle” use their jaws to", "hunt their pray,", "fight each other over females,", "bite branches and build the nest.", 1, string.Empty) { CardId = 19 });
            questions.Add(new QuestionData("Hexarthrius buquetii", "Hexarthrius buqueti is a quite common species of “stag beetle” from", "Indonesia,", "Sri Lanka,", "Costa Rica.", 0, string.Empty) { CardId = 19 });
            questions.Add(new QuestionData("Eudicella gralli", "This species of flower beetle (Eudicella gralli) lives in the rainforests of", "South America,", "Africa,", "India.", 1, string.Empty) { CardId = 20 });
            questions.Add(new QuestionData("Eudicella gralli", "The males have a \"Y\"-shaped horn, used to fight over females, females do not have this horn. Phenotypic difference between males and females of the same species is called", "sexual disparity,", "male dominance,", "sexual dimorphism.", 2, string.Empty) { CardId = 20 });
            questions.Add(new QuestionData("Zygaena osterodensis Reiss, 1921", "The Zygaenidae moths have bright colors that are a warning to predators that the moths are distasteful - they contain", "ammonia (NH<ss>3</ss>),", "hydrogen cyanide (HCN),", "sulfuric acid (H<ss>2</ss>SO<ss>4</ss>).", 1, string.Empty) { CardId = 21 });
            questions.Add(new QuestionData("Zygaena osterodensis Reiss, 1921", "Zygaena osterodensis is a typical element of", "the coniferous forests,", "the mixed forests,", "the deciduous forests.", 2, string.Empty) { CardId = 21 });
            questions.Add(new QuestionData("Sternotomis pulchra", "Beetles of the family Cerambycidae have extremely long antennae, which are often as long as or longer than the beetle's body. That is why this family is sometimes called", "longtentacle beetles,", "longantenna beetles,", "longhorn beetles.", 2, string.Empty) { CardId = 22 });
            questions.Add(new QuestionData("Sternotomis pulchra", "These beetles of Crambycidae family feed on", "tea plant (Thea sinensis),", "liberian coffee (Coffea liberica),", "cacao tree (Theobroma cacao)", 1, string.Empty) { CardId = 22 });
            questions.Add(new QuestionData("Strategus syphax", "Strategu syphax belongs to subfamily rhinoceros beetles (Dynastinae). If they are disturbed, some can release very loud, hissing squeaks which are created by:", "rubbing their front legs against the horns,", "rubbing their abdomens against the ends of their wing covers,", "rubbing their horns against wings.", 1, string.Empty) { CardId = 23 });
            questions.Add(new QuestionData("Strategus syphax", "This ox beetle is unique to", "Guadeloupe Islands,", "Galápagos Islands,", "Maldive Islands.", 0, string.Empty) { CardId = 23 });
            questions.Add(new QuestionData("Lamprima latreilli", "Lamprima latreillii species can be found in", "Africa,", "Australia,", "South America.", 1, string.Empty) { CardId = 24 });
            questions.Add(new QuestionData("Lamprima latreilli", "Males have the mandibles enlarged and prolonged forwards hence the common name of the Lucanidae beetle family is", "Hart Beetles,", "Deer Beetles,", "Stag Beetles.", 2, string.Empty) { CardId = 24 });
            questions.Add(new QuestionData("Deilephila elpenor elpenor (Linnaeus, 1758)", "The anterior of the caterpillar of Deilephila elpenor appears to have the shape of a trunk-like snout. That is why this moth is also known as", "the Elephant Hawk-moth,", "the Ant-eater Hawk-moth,", "the Pinnocchio Hawk-moth.", 0, string.Empty) { CardId = 25 });
            questions.Add(new QuestionData("Deilephila elpenor elpenor (Linnaeus, 1758)", "This species possesses good night vision. Its eye receptors have peak absorption in the three parts of the spectrum (green, blue, ultra violet). This phenomenon is called", "three dimensional channel,", "trichromaticism,", "tricolor.", 1, string.Empty) { CardId = 25 });
            questions.Add(new QuestionData("Mirollia hamata S. Ingrisch, 1998", "Mirollia hamata belongs to order Orthoptera. This order of insects has paurometabolous or \"incomplete metamorphosis\", where the insect changes in three stages. Which stage, comparing to the complete metamorphosis is missing?", "larva,", "egg,", "pupa", 2, string.Empty) { CardId = 26 });
            questions.Add(new QuestionData("Mirollia hamata S. Ingrisch, 1998", "Mirolla hamate, one of the bush crickets produce sound known as a \"stridulation\" by:", "rubbing their legs against each other,", "rubbing their wings against each other,", "rubbing their legs against their wings.", 1, string.Empty) { CardId = 26 });
            questions.Add(new QuestionData("Chrysis auxifera Buysson, 1900", "Chrysis auxifera belongs to the family Chrysididae which is commonly known as cuckoo wasps because of", "calling the same sound as the Common Cuckoo,", "coexistence Common Cuckoo,", "laying their eggs in host nests.", 2, string.Empty) { CardId = 27 });
            questions.Add(new QuestionData("Chrysis auxifera Buysson, 1900", "Chrysis auxifera and other cuckoo wasps is a cosmopolitan group of parasitoid or cleptoparasitic wasps. Cleptoparasitism means that:", "one animal takes food from another,", "the strongest offspring eats the other descendants,", "the descendants feed on the body of the female.", 0, string.Empty) { CardId = 27 });
            questions.Add(new QuestionData("Cerceris diabolica Giner Mari, 1942", "The adult female Cerceris wasp generally digs a nest in the soil and provisions it with living prey items she has paralyzed with venom. The prey are usually", "earthworms,", "beetles and bees,", "other wasps.", 1, string.Empty) { CardId = 28 });
            questions.Add(new QuestionData("Cerceris diabolica Giner Mari, 1942", "Cerceris is the largest genus of wasps in the family Crabronidae. The distribution of this genus is:", "Eurasia, South and North America,", "only Africa,", "cosmopolitan.", 2, string.Empty) { CardId = 28 });
            questions.Add(new QuestionData("Pallenis tricolor", "Pallenis tricolor is one of the beetles of the family Cleridae commonly known as checkered beetles. Some of Clerids have a minor significance in", "gastronomy,", "forensic entomology,", "medicine.", 1, string.Empty) { CardId = 29 });
            questions.Add(new QuestionData("Pallenis tricolor", "Certain species of the family Cleridae are used as biological controls because they hunt for other insects. This is a very effective technique for controlling", "bark beetles,", "ants,", "mosquitos.", 0, string.Empty) { CardId = 29 });
            questions.Add(new QuestionData("Argynnis (Argynnis) paphia (Linnaeus, 1758)", "Butterflies of the family Nymphalidae have underwings in contrast often dull and in some species look remarkably like dead leaves. This phenomenon is called", "crypsis,", "aposematism,", "Batesian mimicry.", 0, string.Empty) { CardId = 30 });
            questions.Add(new QuestionData("Argynnis (Argynnis) paphia (Linnaeus, 1758)", "Adults of the family Nymphalidae have one part of their body small or even reduced. It is:", "proboscis,", "antennae,", "front legs.", 2, string.Empty) { CardId = 30 });
            questions.Add(new QuestionData("Anthelephila irula Kejval, 2007", "Anthelephila irula belongs to family Anthicidae, it is family of", "ants,", "beetles,", "bedbugs.", 1, string.Empty) { CardId = 31 });
            questions.Add(new QuestionData("Anthelephila irula Kejval, 2007", "Many members of the family Anthelephila are attracted to chemical compound “cantharidin”, which they seem to accumulate and that", "deters possible predators,", "entice possible partner,", "defines territory.", 0, string.Empty) { CardId = 31 });
            questions.Add(new QuestionData("Syntelia sp. 1", "Syntelia is a monotypic genus in the family Synteliidae. They have quite powerful jaws because the feed on", "hard fruits,", "insect larvae,", "body of molluscs.", 1, string.Empty) { CardId = 32 });
            questions.Add(new QuestionData("Syntelia sp. 1", "There are only few known species, spread in", "Asia and Australia,", "Asia and Peru,", "Mexico and Asia.", 2, string.Empty) { CardId = 32 });

            List<CardData> cards = new List<CardData>();

            int id = 1;
            int size = questions.Count;
            for (int i = 0; i < size; i += 2)
            {
                cards.Add(new CardData(id, "res://Decks/Insects/" + id, questions[i], questions[i + 1]));
                ++id;
            }

            return cards;
        }

        private static IEnumerable<CardData> GetFossils()
        {
            List<QuestionData> questions = new List<QuestionData>();
            questions.Add(new QuestionData("Jincelites vogeli Valent et al., 2009", "This enigmatic animal fossil belongs to the Hyolitha class, which existed during the", "Palaeozoic Era,", "Mesozoic Era,", "Cenozoic Era", 0, string.Empty) { CardId = 1 });
            questions.Add(new QuestionData("Jincelites vogeli Valent et al., 2009", "The small conical shells of Hyolitha are made by", "aragonite,", "quartz,", "apatite", 0, string.Empty) { CardId = 1 });
            questions.Add(new QuestionData("Temnocidaris Stereocidaris sp.", "The calcareous endosceleton of this organism belongs to which animal group?", "trilobite,", "belemnite,", "sea urchin", 2, string.Empty) { CardId = 2 });
            questions.Add(new QuestionData("Temnocidaris Stereocidaris sp.", "This marine animal fossil belongs to the echinoderm and is closely related to", "sea cucumbers,", "starfishes,", "crinoids", 0, string.Empty) { CardId = 2 });
            questions.Add(new QuestionData("Schlueteria tetracheles Fritsch in Fritsch et Kafka, 1887", "This fossil represents", "head of carnivorous ringed worms,", "chela of decapods (crayfish, crab, lobster),", "trilobite leg", 1, string.Empty) { CardId = 3 });
            questions.Add(new QuestionData("Schlueteria tetracheles Fritsch in Fritsch et Kafka, 1887", "Choose the modern group of living animals that do not have a morphologically close type of chelae (claws):", "crabs,", "scorpions,", "shrimps", 2, string.Empty) { CardId = 3 });
            questions.Add(new QuestionData("Pinna nodulosa Reuss", "This Pinna pen shell pictured from late Cretaceous belongs to", "calms (bivalvia),", "gastropods,", "tusk shells (scaphopods).", 0, string.Empty) { CardId = 4 });
            questions.Add(new QuestionData("Pinna nodulosa Reuss", "A modern large species of Pinna nobilis is not used for", "production of sea silk,", "flesh and pearls,", "production of musk", 2, string.Empty) { CardId = 4 });
            questions.Add(new QuestionData("Peronopsis cuneifEra", "Trilobites are a well-known fossil group of extinct marine", "arthropods,", "molluscs,", "echinoderms.", 0, string.Empty) { CardId = 5 });
            questions.Add(new QuestionData("Peronopsis cuneifEra", "Why the trilobites became extinct is not clear however the extinction occurred during", "the Cambrian extinction,", "the Palaeogene extinction,", "the Permian extinction.", 2, string.Empty) { CardId = 5 });
            questions.Add(new QuestionData("TrochocEras inclytum Barrande", "Choose the true statement:", "this fossil represents a nautiloid cephalopod, which lived during the Devonian Period;", "this fossil represents an extinct ammonite, which lived during the Ordovician Period;",  "this fossil represents extinct gastropods, which lived during the Permian Period.", 0, string.Empty) { CardId = 6 });
            questions.Add(new QuestionData("TrochocEras inclytum Barrande", "Morphologically similar shells from the genus, Trochoceras are not known as representatives of", "gastropods,", "ammonites,", "brachiopods.", 2, string.Empty) { CardId = 6 });
            questions.Add(new QuestionData("Eophacops trapeziceps", "Find the false statement:", "trilobites grew through successive moult stages called instars,", "trilobites are a well-known fossil group of Mesozoic marine arthropods,", "trilobites typically had compound eyes.", 1, string.Empty) { CardId = 7 });
            questions.Add(new QuestionData("Eophacops trapeziceps", "Which lifestyle is not typical for trilobites:", "predators and scavengers,", "filter feeders,", "parasite.", 2, string.Empty) { CardId = 7 });
            questions.Add(new QuestionData("Aulacopleura konincki (Barrande, 1846)", "Find the false statement:", "a) trilobite exoskeletons are composed of calcite and calcium phosphate minerals in a protein lattice of chitin;", "trilobites have three distinctive sections – cephalon (head), thorax (body) and pygidium (tail),", "soft body parts are still unknown", 2, string.Empty) { CardId = 8 });
            questions.Add(new QuestionData("Aulacopleura konincki (Barrande, 1846)", "Find the false statement. A trilobite's cephalon, or head section always includes the following morphological features:", "glabella,", "eyes,", "hypostome and librigena (\"free cheeks\")", 1, string.Empty) { CardId = 8 });
            questions.Add(new QuestionData("Umbonula granulata sp.n.", "Moss animals (Bryozoa) are aquatic invertebrates, mostly colonial and predominantly live in", "tropical marine environments,", "freshwater environments,", "brackish water.", 0, string.Empty) { CardId = 9 });
            questions.Add(new QuestionData("Umbonula granulata sp.n.", "Moss animal exoskeletons may be organic (chitin, polysaccharide or protein) or made of minerals:", "calcium carbonate (CaCO<ss>3</ss>),", "silicon dioxide (SiO<ss>2</ss>),", "strontium sulfate (SrSO<ss>4</ss>).", 0, string.Empty) { CardId = 9 });
            questions.Add(new QuestionData("Orthoceras bronni Barrande", "Orthoceras (\"straight horn\") is a genus of extinct", "nautiloid gastropod,", "nautiloid ammonite,", "nautiloid cephalopod.", 2, string.Empty) { CardId = 10 });
            questions.Add(new QuestionData("Orthoceras bronni Barrande", "Orthoceras fossils are common and have a global distribution from the Early Paleozoic. They can be found in any marine rock but especially in", "limestone,", "sandstone,", "granite.", 0, string.Empty) { CardId = 10 });
            questions.Add(new QuestionData("Trochus engelhardti Geinitz", "Trochus engelhardti is a typical marine gastropod from the Cretaceous preiod. Choose a group of animals that Trochus engelhardti could not normally meet at that time:", "ammonites,", "dinosaurs,", "trilobites.", 2, string.Empty) { CardId = 11 });
            questions.Add(new QuestionData("Trochus engelhardti Geinitz", "Similar to some other clams, gastropods, and cephalopods, the interior of the Trochus shell is pearly and iridescent because of a layer of nacre. Narce, also known as mother of pearl, is composed of hexagonal platelets of", "calcite,", "quartz,", "aragonite", 2, string.Empty) { CardId = 11 });
            questions.Add(new QuestionData("Dicellograptus laticeps", "Find the true statement. This organism belongs to the Graptolithina (Hemichordata) class , which includes", "colonial animals from the Cambrian through the early Carboniferous Periods,", "colonial animals from the Triassic through the Early Cretaceous Periods,", "colonial animals still alive today.", 0, string.Empty) { CardId = 12 });
            questions.Add(new QuestionData("Dicellograptus laticeps", "Find the true statement. Graptolite fossils are often found in shale and mud rocks where sea-bed fossils are rare. This type of rock is formed from sediment deposited in", "fresh-water environment only,", "relatively shallow water with high bottom circulation and enough oxygen,", "relatively deep water with poor bottom circulation and deficient in oxygen.", 2, string.Empty) { CardId = 12 });
            questions.Add(new QuestionData("Kettnerites sp.", "Kettnerites belongs to a group of Scolecodonta. This is a picture of", "a polychaete annelid jaw,", "the jaw of a primitive vertebrate,", "the jaw of unknown animals", 0, string.Empty) { CardId = 13 });
            questions.Add(new QuestionData("Kettnerites sp.", "Scolecodonts are common and diverse microfossils known to have existed during", "the Cambrian to the Devonian,", "the Cambrian to the Cretaceous,", "the Cambrian to the present.", 0, string.Empty) { CardId = 13 });
            questions.Add(new QuestionData("Inoceramus planus Münster in Goldfuss, 1836", "The thick shells of Inoceramus (\"strong pot\") are very important index fossils (guide or indicator fossils) used to define and identify", "the Cambrian Period,", "the Cretaceous Period,", "the Palaeocene Period", 1, string.Empty) { CardId = 14 });
            questions.Add(new QuestionData("Inoceramus planus Münster in Goldfuss, 1836", "Genus Inoceramus is extinct and belongs to", "calms (bivalves),", "ammonites,", "brachiopods.", 0, string.Empty) { CardId = 14 });
            questions.Add(new QuestionData("Peronopsis cuneifera", "This very small trilobite belongs to the Agnostida order, which existed at the beginning of trilobite’s evolution during", "the Cambrian Period,", "the Cretaceous Period,", "the Carboniferous Period.", 0, string.Empty) { CardId = 15 });
            questions.Add(new QuestionData("Drahomira glaserae Barrande in Perner, 1903", "Drahomira is an extinct genus of Palaeozoic animals, which belong to", "calms (bivalves),", "brachiopods,", "monoplacophorans.", 0, string.Empty) { CardId = 16 });
            questions.Add(new QuestionData("Drahomira glaserae Barrande in Perner, 1903", "The Drahomira glaserae species was described by the famous Joachim Barrande. J. Barrande was a French geologist and palaeontologist, who described more than 3500 fossil species including graptolites, brachiopods, molluscs, trilobites and fishes. He lived and worked in the Czech Republic during the", "18th century,", "19th century,", "20th century.", 1, string.Empty) { CardId = 16 });
            questions.Add(new QuestionData("Ichthyostega sp.", "Ichthyostega is an early tetrapod genus that lived in East Greenland at the end of", "the Devonian Period,", "the Lower Carboniferous Period,", "the Silurian Period.", 0, string.Empty) { CardId = 17 });
            questions.Add(new QuestionData("Ichthyostega sp.", "Fill in the suitable word. Ichthyostega stands alone as the transitional fossil between ___________ and tetrapods, combining all features including;  tail and gills with a new amphibian skull and limbs.", "reptiles,", "sharks,", "lobe-finned fish", 2, string.Empty) { CardId = 17 });
            questions.Add(new QuestionData("Gyroceras tenue Barrande", "Gyroceras is part of the prehistoric nautiloid genus. Find the true statement.", "Nautiloids is a large and diverse group of marine cephalopods dominanant during the Palaeogene and Neogene,", "Nautiloids are an extinct group of cephalopods,", "Nautiloids are represented today by the living Nautilus and Allonautilus.", 2, string.Empty) { CardId = 18 });
            questions.Add(new QuestionData("Gyroceras tenue Barrande", "Gyroceras is part of the prehistoric nautiloid genus. Find the false statement.", "Nautiloids flourished during the early Paleozoic Era,", "Nautiloids became extinct during Late Paleozoic Era,", "Nautiloids constituted the main predatory animals during the early Paleozoic Era.", 1, string.Empty) { CardId = 18 });
            questions.Add(new QuestionData("Ptenoceras proximum (Barrande, 1865)", "Species of Ptenoceras proximum represent a shell of the Lower Devonian nautiloids, which stratigraphically belong to the", "Palaeozoic Era,", "Mesozoic Era,", "Cenozoic Era", 0, string.Empty) { CardId = 19 });
            questions.Add(new QuestionData("Ptenoceras proximum (Barrande, 1865)", "Sea shells of Palaeozoic nautiloids morphologically resemble exoskeleton of", "belemnites,", "ammonites,", "brachiopods", 1, string.Empty) { CardId = 19 });
            questions.Add(new QuestionData("Protopteris punctata (Sternberg) Sternberg", "Protopteris punctata represents a fragment of the stem covered by helically arranged leaf scars, which systematically belongs to", "ferns,", "lycopods,", "seed ferns", 0, string.Empty) { CardId = 20 });
            questions.Add(new QuestionData("Protopteris punctata (Sternberg) Sternberg", "Find the false statement.", "Genus Protopteris represents stem casts of tree ferns known from the Mesozoic sediments,", "Leaf scares of Protopteris are similar to those of extant tree ferns from the Zone,", "Leaf scar of Protopteris bearing one horseshoe shaped continuous scar of vascular bundle.", 1, string.Empty) { CardId = 20 });
            questions.Add(new QuestionData("Lepidodendron aculeatum", "Find the false statement.", "Lepidodendron had tall, thick trunks that rarely branched and were topped with a crown of bifurcating branches bearing clusters of leaves,", "Lepidodendron likely lived in the wettest parts of the coal swamps that existed during the Permian Period,", "Lepidodendron reached heights of over 30 metres and the trunks were often over 1 m in diameter", 1, string.Empty) { CardId = 21 });
            questions.Add(new QuestionData("Lepidodendron aculeatum", "Find the true statement.", "Lepidodendron is now an extinct genus of primitive, vascular, arborescent (tree-like) plant related to the horsetails,", "Petrified trunks of Lepidodendron were frequently exhibited at fairgrounds by amateurs as giant fossilized lizards or snakes in the 19th century,", "Lepidodendron likely lived in the driest parts of the coal swamps that existed during the Carboniferous Period.", 1, string.Empty) { CardId = 21 });
            questions.Add(new QuestionData("Calamites paleaceus", "Find the false statement.", "The trunks of Calamites had a distinctive segmented, bamboo-like appearance and vertical ribbing,", "Calamites is a genus of extinct arborescent (tree-like) horsetails,", "Calamites were herbaceous elements of the understories of coal swamps during the Carboniferous Period.", 2, string.Empty) { CardId = 22 });
            questions.Add(new QuestionData("Calamites paleaceus", "Find the true statement.", "Calamites reproduced by means of spores, which were produced in small sacs organized into cones,", "The Calamitaceae finally became extinct during Early Carboniferous Period,", "Calamites reproduced via asexual mode of reproduction only.", 0, string.Empty) { CardId = 22 });
            questions.Add(new QuestionData("Sigillaria sp.", "Sigillaria is a genus of extinct, spore-bearing, arborescent (tree-like)", "ferns,", "lycopods,", "horsetails.", 1, string.Empty) { CardId = 23 });
            questions.Add(new QuestionData("Sigillaria sp.", "Sigillaria was a tree-like plant characterized by a tall trunk with diamond-shaped leaf scars. It commonly occurred in freshwater sediments of", "the Carboniferous Period,", "the Silurian Period,", "the Triassic Period.", 0, string.Empty) { CardId = 23 });
            questions.Add(new QuestionData("Pecopteris miltoni", "Pecopteris is a genus of leaves, which are usually associated with", "ferns and seed ferns,", "arborescent lycopods and ferns,", "primitive flowering plants.", 0, string.Empty) { CardId = 24 });
            questions.Add(new QuestionData("Pecopteris miltoni", "Leaves of Pecopteris first appeared in the Devonian Period, but flourished in the Carboniferous and went extinct around the beginning of", "the Silurian Period,", "Triassic Period,", "Permian Period.", 2, string.Empty) { CardId = 24 });
            questions.Add(new QuestionData("Sphenophyllum wingfieldense Hemingway 1931", "Find the false statement.", "Sphenophylls are herbaceous horsetails,", "Sphenophylls were common from the Carboniferous to Early Permian time slice,", "Sphenophylls is an extinct group of ferns.", 1, string.Empty) { CardId = 25 });
            questions.Add(new QuestionData("Sphenophyllum wingfieldense Hemingway 1931", "Genus Sphenopyllum has a close affinity to", "extinct Calamites,", "arborescent fern of Pecopteris,", "herbaceous lycopod of Protolepidodendron.", 0, string.Empty) { CardId = 25 });
            questions.Add(new QuestionData("Calamostachys tuberculata (Sternberg) Weiss", "Find the true statement.", "Calamostachys represents special organ allowing asexual reproduction of horsetails,", "Calamostachys represents the reproductive organ or cone of the Calamite tree,", "Cones of Calamostachys are commonly described from the Upper Devonian marine sediments.", 2, string.Empty) { CardId = 26 });
            questions.Add(new QuestionData("Calamostachys tuberculata (Sternberg) Weiss", "Calamostachys represents the reproductive organs, which are usually associated to", "ferns,", "horsetails,", "seed ferns.", 0, string.Empty) { CardId = 26 });
            questions.Add(new QuestionData("Sphenophyllum emarginatum", "Sphenophylls are small, slender branching plants, usually growing to a height of less than 1 m tall. This group of plants belongs to", "horsetails,", "lycopods,", "ferns.", 0, string.Empty) { CardId = 27 });
            questions.Add(new QuestionData("Sphenophyllum emarginatum", "Find the true statement.", "Sphenophylls are arborescent (tree-like) ferns,", "Sphenophylls are main coal forming element during Devonian Period,", "Taxonomy of sphenophylls may be based on the morphology and anatomy of sterile plant parts and fructifications.", 2, string.Empty) { CardId = 27 });
            questions.Add(new QuestionData("Palaeostachya distachya (Sternberg) Jongmans", "Fill in a suitable word. Horsetails, lycopods, _______________, seed ferns were important coal-forming plants during Carboniferous to Permian Periods,", "flowering plants,", "ferns,", "rhyniophytes.", 1, string.Empty) { CardId = 28 });
            questions.Add(new QuestionData("Palaeostachya distachya (Sternberg) Jongmans", "A group of plants, which do not reproduce via spores, is:", "ferns,", "ginkgos,", "horsetails", 1, string.Empty) { CardId = 28 });
            questions.Add(new QuestionData("Cephalaspis aarhusi Wängsjö, 1952", "Cephalaspis (head shield) was an armoured jawless fish, which occured in freshwater and brackish sediments during", "Devonian Period,", "Jurassic Period,", "Cambrian Period.", 0, string.Empty) { CardId = 29 });
            questions.Add(new QuestionData("Cephalaspis aarhusi Wängsjö, 1952", "Body of Cephalaspis was heavily armored, presumably to defend against predators such as:", "trilobites and sharks,", "placoderms and eurypterids,", "marine reptiles and amphibians", 1, string.Empty) { CardId = 29 });
            questions.Add(new QuestionData("Lamna denticulata Agassiz", "Lamna is a genus of mackerel sharks, which belongs along with sharks, rays, skates and chimaeras to", "lobe-finned fishes,", "bony fishes,", "cartilaginous fishes.", 2, string.Empty) { CardId = 30 });
            questions.Add(new QuestionData("Lamna denticulata Agassiz", "The earliest known evidence of sharks dates from", "Cambrian,", "Silurian,", "Permian", 1, string.Empty) { CardId = 30 });
            questions.Add(new QuestionData("Mammuthus primigenius (Blumenbach, 1799)", "The woolly mammoth (Mammuthus primigenius) was a species of mammoth which has closest extant relation to", "modern Asian elephant,", "modern African elephant,", "no close relation to living elephants.", 0, string.Empty) { CardId = 31 });
            questions.Add(new QuestionData("Mammuthus primigenius (Blumenbach, 1799)", "Find the true statement.", "A mammoth had short, curved tusks and four molars, which were replaced six times during the lifetime of an individual,", "a diet of the woolly mammoth was mainly meet and shellfishes,", "a mammoth was well adapted to the cold environment. Its body is covered in fur, ears and tail was short to minimise frostbite and heat loss.", 2, string.Empty) { CardId = 31 });
            questions.Add(new QuestionData("Grandostoma grande (Barrande in Perner, 1903)", "Grandostoma grande is an interesting extinct animal species from the Ordovician Period of the Czech republic. The Ordovician Period covers the time between 488 to 444 million years ago, and belongs to", "Mesozoic Era,", "Cenozoic Era,", "Palaeozoic Era", 2, string.Empty) { CardId = 32 });
            questions.Add(new QuestionData("Grandostoma grande (Barrande in Perner, 1903)", "The species of Grandostoma grande belongs to systematic group of", "gastropods,", "calms (bivalves),", "brachiopods", 0, string.Empty) { CardId = 32 });

            int i = questions.Min(q => q.CardId);
            int size = questions.Max(q => q.CardId) + i;

            var cards = new List<CardData>();
            for (; i < size; ++i)
            {
                var card = new CardData(i, "res://Decks/Fossils/" + i);
                card.Title = "Minerals";

                var qs = questions.Where(q => q.CardId == i && q.CorrectAnswerIndex >= 0).ToList();
                card.Questions.AddRange(qs);

                cards.Add(card);
            }

            return cards;
        }

        private static IEnumerable<CardData> GetAnimalsKids()
        {
            List<QuestionData> questions = new List<QuestionData>();
            questions.Add(new QuestionData("kangaroo", "Except one genus, the tree-kangaroo, kangaroos are endemic to", "Madagascar,", "Australia,", "New Zealand.", 1, string.Empty) { CardId = 1 });
            questions.Add(new QuestionData("kangaroo", "The young kangaroo, or joey, is born alive when it is about", "20 cm,", "10 cm,", "2 cm.", 2, string.Empty) { CardId = 1 });
            questions.Add(new QuestionData("giraffe", "How many vertebrae are in giraffe´s neck?", "Five, which is less than humans have.", "Seven, which is the same number that humans have.", "Nine, which is more than humans have.", 1, string.Empty) { CardId = 2 });
            questions.Add(new QuestionData("giraffe", "Giraffes are", "carnivorous (meat eaters),", "herbivorous (plant eaters),", "omnivorous (meat and plant eaters).", 1, string.Empty) { CardId = 2 });
            questions.Add(new QuestionData("elephant", "Which elephant feature is commonly used as a metaphor?", "elephant skin,", "elephant thirst,", "elephant memory.", 2, string.Empty) { CardId = 3 });
            questions.Add(new QuestionData("elephant", "Which part of the body IS NOT important while distinguishing between African and Indian elephant?", "tusks,", "tail,", "ears", 1, string.Empty) { CardId = 3 });
            questions.Add(new QuestionData("elephant", "A common cause of death is starvation because the elephant", "is weak for food searching,", "no longer has teeth,", "has digestive system dysfunction.", 1, string.Empty) { CardId = 3 });
            questions.Add(new QuestionData("fox", "This animal is related to", "cats,", "dogs,", "none of them.", 1, string.Empty) { CardId = 4 });
            questions.Add(new QuestionData("fox", "The Fox is one of the animals which transmit rabies. It is a viral disease that causes acute", "indigestion,", "pulmonary disease,", "inflammation of the brain.", 2, string.Empty) { CardId = 4 });
            questions.Add(new QuestionData("cat", "The domestic cat (housecat) is NOT related to the", "lion,", "fox,", "tiger.", 1, string.Empty) { CardId = 5 });
            questions.Add(new QuestionData("cat", "Housecats are able to", "retract their claws,", "regenerate their tails,", "completely recolor their hair.", 0, string.Empty) { CardId = 5 });
            questions.Add(new QuestionData("lamb", "A young sheep is called a", "calf,", "lamb,", "kid.", 1, string.Empty) { CardId = 6 });
            questions.Add(new QuestionData("lamb", "Which parts of the world are most closely associated with sheep production?", "Brazil and Argentina,", "India and Tibet,", "Australia and New Zealand.", 2, string.Empty) { CardId = 6 });
            questions.Add(new QuestionData("swan", "The swan’s close relatives DO NOT include", "geese,", "seagulls,", "ducks.", 1, string.Empty) { CardId = 7 });
            questions.Add(new QuestionData("swan", "Swans’ legs are normally", "grey,", "orange,", "yellow.", 0, string.Empty) { CardId = 7 });
            questions.Add(new QuestionData("butterfly", "A butterfly has the same number of pairs of legs as other insects. This number is", "two,", "three,", "four.", 1, string.Empty) { CardId = 8 });
            questions.Add(new QuestionData("butterfly", "The larval stadium of butterflies is called", "nymph,", "imago,", "caterpillar.", 2, string.Empty) { CardId = 8 });
            questions.Add(new QuestionData("giant panda", "The giant panda is native to", "Japan,", "China,", "Madagascar.", 1, string.Empty) { CardId = 9 });
            questions.Add(new QuestionData("giant panda", "The giant panda's diet is primarily herbivorous, consisting almost exclusively of", "bamboo,", "eucalypt,", "avocado.", 0, string.Empty) { CardId = 9 });
            questions.Add(new QuestionData("moose", "Moose typically live in which environment?", "Mediterranean,", "Taiga,", "Rainforest.", 1, string.Empty) { CardId = 10 });
            questions.Add(new QuestionData("moose", "Female moose (cows) select mates based on", "antler size,", "height at the shoulder,", "strength of their voice.", 0, string.Empty) { CardId = 10 });
            questions.Add(new QuestionData("St. Bernard dog", "The St. Bernard dog became famous through tales of rescues from", "fire,", "avalanche,", "tsunami.", 1, string.Empty) { CardId = 11 });
            questions.Add(new QuestionData("St. Bernard dog", "Where is the original St. Bernard dog breeding place?", "Switzerland,", "Italy,", "Netherlands", 0, string.Empty) { CardId = 11 });
            questions.Add(new QuestionData("hippo", "Hippo is", "carnivorous (meat eater),", "herbivorous (plant eater),", "omnivorous (meat and plant eater).", 1, string.Empty) { CardId = 12 });
            questions.Add(new QuestionData("hippo", "Hippos are still found in the rivers and lakes of", "South America,", "Australia,", "Africa.", 2, string.Empty) { CardId = 12 });
            questions.Add(new QuestionData("koala", "A koala’s diet mostly consists of", "bamboo,", "eucalypt,", "avocado.", 1, string.Empty) { CardId = 13 });
            questions.Add(new QuestionData("koala", "The koala IS NOT related to the", "bear,", "kangaroo,", "wombat.", 0, string.Empty) { CardId = 13 });
            questions.Add(new QuestionData("seal", "The animal in the picture is called", "walrus,", "true seal,", "sea lion.", 2, string.Empty) { CardId = 14 });
            questions.Add(new QuestionData("seal", "Sea lions are characterized by", "long hair,", "external ear flaps,", "short foreflippers.", 1, string.Empty) { CardId = 14 });
            questions.Add(new QuestionData("whale", "The whale is related to", "the shark,", "the dolphin,", "the clown fish.", 1, string.Empty) { CardId = 15 });
            questions.Add(new QuestionData("whale", "Some whales communicate using", "melodic sounds,", "tail movements,", "swimming direction.", 0, string.Empty) { CardId = 15 });
            questions.Add(new QuestionData("cow", "Most cows have horns on their head. Its composition is", "bony,", "dermal,", "spongy.", 1, string.Empty) { CardId = 16 });
            questions.Add(new QuestionData("cow", "The meat of adult cattle (cow) is known as", "pork,", "lamb,", "beef.", 2, string.Empty) { CardId = 16 });
            questions.Add(new QuestionData("deer", "All male deer have antlers on their head. Its composition is", "bony,", "dermal,", "spongy.", 0, string.Empty) { CardId = 17 });
            questions.Add(new QuestionData("deer", "Deer are widely distributed and live in a variety of biomes, ranging from tundra to the tropical rainforest. In which continent it is not possible to meet any species of deer?", "Europe,", "Australia,", "Africa", 1, string.Empty) { CardId = 17 });
            questions.Add(new QuestionData("rhinoceros", "Rhinoceros are killed by humans for their horns, which are used by some cultures for", "gastronomy,", "medicinal purposes,", "cosmetic purposes.", 1, string.Empty) { CardId = 18 });
            questions.Add(new QuestionData("rhinoceros", "Which one of following species is not the one of two living African species of rhinoceros?", "Indian rhinoceros,", "white rhinoceros,", "black rhinoceros.", 0, string.Empty) { CardId = 18 });
            questions.Add(new QuestionData("wild boar", "Wild boar piglets are", "colored the same as adults,", "having cream stripes,", "naked.", 1, string.Empty) { CardId = 19 });
            questions.Add(new QuestionData("wild boar", "Find the right statement about wild pigs.", "adult females develop tusks,", "adult males develop tusks,", "adult females and males develop tusks", 1, string.Empty) { CardId = 19 });
            questions.Add(new QuestionData("zebra", "Zebra´s skin is thought to be", "black with white stripes,", "white with black stripes,", "it depends on the species.", 0, string.Empty) { CardId = 20 });
            questions.Add(new QuestionData("zebra", "Which one of following animals IS NOT the closest relative of zebra?", "horse,", "donkey,", "antelope.", 2, string.Empty) { CardId = 20 });
            questions.Add(new QuestionData("black dog", "The domestic dog is a subspecies of the", "jackal,", "wolf,", "dingo.", 1, string.Empty) { CardId = 21 });
            questions.Add(new QuestionData("black dog", "How is the offspring of the dog called until it is about a year old?", "puppy,", "kitten,", "kid", 0, string.Empty) { CardId = 21 });
            questions.Add(new QuestionData("beaver", "Beavers are known for their natural trait of building", "burrows under trees,", "stony nests,", "dams on rivers.", 2, string.Empty) { CardId = 22 });
            questions.Add(new QuestionData("beaver", "Beavers can stay under water for as long as", "15 minutes,", "30 minutes,,", "45 minutes.", 0, string.Empty) { CardId = 22 });
            questions.Add(new QuestionData("lion", "Lions CAN NOT be found in", "Africa,", "South America,", "Asia.", 1, string.Empty) { CardId = 23 });
            questions.Add(new QuestionData("lion", "The lion in the picture is", "male,", "female,", "can be both.", 0, string.Empty) { CardId = 23 });
            questions.Add(new QuestionData("bear", "Bears are primarily found in", "both Hemispheres,", "the Southern Hemisphere,", "the Northern Hemisphere", 2, string.Empty) { CardId = 24 });
            questions.Add(new QuestionData("bear", "Which one of the following animals is not related to the bear in the picture?", "koala bear,", "giant panda,", "polar bear", 0, string.Empty) { CardId = 24 });
            questions.Add(new QuestionData("European badger", "European badger is", "carnivorous (meat eater),", "herbivorous (plant eater),", "omnivorous (meat and plant eater).", 0, string.Empty) { CardId = 25 });
            questions.Add(new QuestionData("European badger", "European badgers have few natural enemies. There are", "bears and cats,", "wolves and lynxes,", "dogs and martens.", 1, string.Empty) { CardId = 25 });
            questions.Add(new QuestionData("European pine marten", "European pine marten is mainly active", "at night and dusk,", "during a day,", "during winter", 0, string.Empty) { CardId = 26 });
            questions.Add(new QuestionData("European pine marten", "At birth young European pine martens weigh around", "3 kilograms,", "300 grams,", "30 grams.", 2, string.Empty) { CardId = 26 });
            questions.Add(new QuestionData("chimpanzee", "Find the false statement.", "Chimpanzees are the closest living relatives to humans,", "Chimpanzees are members of the family Hominidae, along with gorillas, humans, and orangutans,", "Chimpanzees are still found in Asia.", 2, string.Empty) { CardId = 27 });
            questions.Add(new QuestionData("chimpanzee", "Find the false statement.", "Chimpanzees make tools and use them to acquire foods and for social displays,", "Chimpanzee cannot walk upright on two legs,", "Chimpanzees live social groups, which are called communities.", 1, string.Empty) { CardId = 27 });
            questions.Add(new QuestionData("white horse", "Humans began to domesticate horses around", "4000 years BC,", "40 000 years BC,", "20 000 years BC.", 0, string.Empty) { CardId = 28 });
            questions.Add(new QuestionData("white horse", "Find the false statement.", "Przewalski's horse has never been domesticated and remains a truly wild animal today,", "Przewalski's horse has been reintroduced to its native habitat in Canada,", "Przewalski horse’s diet consists mostly of vegetation.", 1, string.Empty) { CardId = 28 });
            questions.Add(new QuestionData("dog", "The ancestors of domestic dogs are", "wolves,", "foxes,", "wildcats.", 0, string.Empty) { CardId = 29 });
            questions.Add(new QuestionData("dog", "Find the false statement.", "Dogs can see blue and yellow, but have difficulty differentiating red and green colours,", "The height of dogs measured to the withers ranges from 15.2 centimetres in the Chihuahua to about 76 cm in the Irish Wolfhound,", "The dog was the last domesticated animal.", 2, string.Empty) { CardId = 29 });
            questions.Add(new QuestionData("Black-eared Wheatear (Birds)", "The fossil record indicates that birds emerged within the theropod dinosaurs during", "the Jurassic period,", "the Neogene period,", "the Carboniferous period.", 0, string.Empty) { CardId = 30 });
            questions.Add(new QuestionData("Black-eared Wheatear (Birds)", "Black-eared wheatear belongs to bird’s group of", "falcons,", ", owls,", "songbirds.", 2, string.Empty) { CardId = 30 });
            questions.Add(new QuestionData("brown horse", "The horse skeleton averages", "205 bones,", "25 bones,", "2500 bones.", 0, string.Empty) { CardId = 31 });
            questions.Add(new QuestionData("brown horse", "Horses belong to", "even-toed ungulates (Artiodactyla),", "proboscideans (Proboscidea),", "odd-toed ungulates (Perissodactyla).", 2, string.Empty) { CardId = 31 });
            questions.Add(new QuestionData("white horse", "Find the false statement.", "The height of horses is measured at the highest point of the withers,", "The English-speaking world measures the height of horses in legs,", "The world's smallest horse is Thumbelina which is 43 cm tall and weighs 26 kg.", 1, string.Empty) { CardId = 32 });
            questions.Add(new QuestionData("white horse", "Gestation of horses lasts approximately", "340 days,", "850 days,", "170 days", 0, string.Empty) { CardId = 32 });

            int i = questions.Min(q => q.CardId);
            int size = questions.Max(q => q.CardId) + i;

            var cards = new List<CardData>();
            for (; i < size; ++i)
            {
                var card = new CardData(i, "res://Decks/AnimalsKids/Animals_" + i);
                card.Title = "Animals kids";

                var qs = questions.Where(q => q.CardId == i && q.CorrectAnswerIndex >= 0).ToList();
                card.Questions.AddRange(qs);

                cards.Add(card);
            }

            return cards;
        }

        public static void Add(string name, List<CardData> cards)
        {
            var deck = new DeckData { Cards = new List<CardData>(cards), ResourceName = name };
            savedDecks.Insert(0, deck);
            customDecks.Insert(0, deck);

            SaveCustomDeck();
        }

        private static void SaveCustomDeck()
        {
#if !UNITY_WP8 && !UNITY_WINRT
            try
            {
                UnityEngine.Debug.Log("Initialize serializer...");
                XmlSerializer serializer = new XmlSerializer(typeof(List<DeckData>));

                UnityEngine.Debug.Log("Start serializing...");
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, customDecks);
                    stream.Position = 0;

                    UnityEngine.Debug.Log("Writing to storage...");
                    var data = stream.ToArray();
                    File.WriteAllText(Application.persistentDataPath + "/decks.xml", Encoding.UTF8.GetString(data));

                    UnityEngine.Debug.Log("Serialization completed!");
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Serialization failed: " + ex.Message);
            }
#endif
        }

        private static void LoadCustomDecks()
        {
#if !UNITY_WP8 && !UNITY_WINRT
            string fileName = Application.persistentDataPath + "/decks.xml";
            if (!File.Exists(fileName))
            {
                return;
            }

            try
            {
                UnityEngine.Debug.Log("Initialize deserializer...");
                XmlSerializer serializer = new XmlSerializer(typeof(List<DeckData>));

                var data = File.ReadAllBytes(Application.persistentDataPath + "/decks.xml");

                UnityEngine.Debug.Log("Start deserializing...");
                using (MemoryStream stream = new MemoryStream())
                {
                    UnityEngine.Debug.Log("Reading from storage...");
                    stream.Write(data, 0, data.Length);
                    stream.Position = 0;

                    UnityEngine.Debug.Log("Serializing...");
                    customDecks = serializer.Deserialize(stream) as List<DeckData>;
                    if (customDecks == null)
                    {
                        customDecks = new List<DeckData>();
                    }

                    UnityEngine.Debug.Log("Deserialization completed!");
                }
            }
            catch (Exception)
            {
            }

            if (customDecks != null)
            {
                bool needToBeUpdated = false;
                foreach (var customDeck in customDecks)
                {
                    if (string.IsNullOrEmpty(customDeck.Id) || customDeck.Id.Trim().Length == 0)
                    {
                        customDeck.Id = Guid.NewGuid().ToString();
                        needToBeUpdated = true;
                    }

                    if (string.IsNullOrEmpty(customDeck.Title) || customDeck.Title.Length == 0)
                    {
                        customDeck.Title = customDeck.ResourceName;
                        needToBeUpdated = true;
                    }
                }

                if (needToBeUpdated)
                {
                    SaveCustomDeck();
                }
            }
#endif
        }
    }
}
