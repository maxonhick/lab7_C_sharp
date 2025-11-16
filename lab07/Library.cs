using System.ComponentModel.Design.Serialization;
using System.Xml.Serialization;

namespace AnimalLibrary
{
    /// <summary>
    /// Animal classification by feeding type
    /// </summary>
    [Serializable]
    public enum ClassificationAnimal
    {
        Herbivores,
        Carnivores,
        Omnivores
    }

    /// <summary>
    /// Favorite food of animals
    /// </summary>
    [Serializable]
    public enum FavoriteFood
    {
        Meat,
        Plants,
        Everything
    }

    /// <summary>
    /// Attribute for adding comments to classes and methods
    /// </summary>
    [Serializable]
    public class CommentAttribute : Attribute
    {
        /// <summary>
        /// Comment text
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Initializes a new instance of CommentAttribute
        /// </summary>
        /// <param name="comment">The comment text</param>
        public CommentAttribute(string comment)
        {
            Comment = comment;
        }
    }

    /// <summary>
    /// Abstract base class for all animals
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(Cow))]
    [XmlInclude(typeof(Lion))]
    [XmlInclude(typeof(Pig))]
    [Comment("Abstract base class for all animals")]
    public abstract class Animal
    {
        /// <summary>
        /// Country where the animal lives
        /// </summary>
        public string Country {  get; set; }
        /// <summary>
        /// Whether the animal hides from other animals
        /// </summary>
        public bool HideFromOtherAnimals { get; set; }
        /// <summary>
        /// Name of the animal
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Type of animal
        /// </summary>
        public string WhatAnimal { get; set; }

        /// <summary>
        /// Default constructor for XML serialization
        /// </summary>
        protected Animal()
        {
            // Required for XML serialization
        }

        /// <summary>
        /// Initializes a new instance of Animal
        /// </summary>
        /// <param name="country">Country where the animal lives</param>
        /// <param name="hideFromOtherAnimals">Whether the animal hides from other animals</param>
        /// <param name="name">Name of the animal</param>
        /// <param name="whatAnimal">Type of animal</param>
        protected Animal(string country, bool hideFromOtherAnimals, string name, string whatAnimal)
        {
            Country = country;
            HideFromOtherAnimals = hideFromOtherAnimals;
            Name = name;
            WhatAnimal = whatAnimal;
        }

        /// <summary>
        /// Deconstructs the animal into its components
        /// </summary>
        /// <param name="country">Country where the animal lives</param>
        /// <param name="hideFromOtherAnimals">Whether the animal hides from other animals</param>
        /// <param name="name">Name of the animal</param>
        /// <param name="whatAnimal">Type of animal</param>
        public void Deconstruct(out string country, out bool hideFromOtherAnimals, out string name, out string whatAnimal)
        {
            country = Country;
            hideFromOtherAnimals = HideFromOtherAnimals;
            name = Name;
            whatAnimal = WhatAnimal;
        }

        /// <summary>
        /// Gets the classification of the animal
        /// </summary>
        /// <returns>Animal classification</returns>
        public abstract ClassificationAnimal GetClassificationAnimal();

        /// <summary>
        /// Gets the favorite food of the animal
        /// </summary>
        /// <returns>Favorite food</returns>
        public abstract FavoriteFood GetFavoriteFood();

        /// <summary>
        /// Makes the animal say hello
        /// </summary>
        public abstract void SayHello();
    }

    /// <summary>
    /// A class representing cows
    /// </summary>
    [Serializable]
    [Comment("A class representing cows")]
    public class Cow : Animal
    {
        /// <summary>
        /// Default constructor for XML serialization
        /// </summary>
        public Cow() : base()
        {
            WhatAnimal = "Cow";
        }

        /// <summary>
        /// Initializes a new instance of Cow
        /// </summary>
        /// <param name="country">Country where the cow lives</param>
        /// <param name="hideFromOtherAnimals">Whether the cow hides from other animals</param>
        /// <param name="name">Name of the cow</param>
        public Cow(string country, bool hideFromOtherAnimals, string name) 
            : base(country, hideFromOtherAnimals, name, "Cow") { }

        /// <summary>
        /// Gets the classification of the cow
        /// </summary>
        /// <returns>Herbivores classification</returns>
        public override ClassificationAnimal GetClassificationAnimal()
        {
            return ClassificationAnimal.Herbivores;
        }

        /// <summary>
        /// Gets the favorite food of the cow
        /// </summary>
        /// <returns>Plants as favorite food</returns>
        public override FavoriteFood GetFavoriteFood()
        {
            return FavoriteFood.Plants;
        }

        /// <summary>
        /// Makes the cow say hello
        /// </summary>
        public override void SayHello()
        {
            Console.WriteLine("Muuu! I'm a cow " + Name);
        }
    }

    /// <summary>
    /// Class representing lions
    /// </summary>
    [Serializable]
    [Comment("A class representing lions")]
    public class Lion : Animal
    {
        /// <summary>
        /// Default constructor for XML serialization
        /// </summary>
        public Lion() : base()
        {
            WhatAnimal = "Lion";
        }

        /// <summary>
        /// Initializes a new instance of Lion
        /// </summary>
        /// <param name="country">Country where the lion lives</param>
        /// <param name="hideFromOtherAnimals">Whether the lion hides from other animals</param>
        /// <param name="name">Name of the lion</param>
        public Lion(string country, bool hideFromOtherAnimals, string name)
            : base(country, hideFromOtherAnimals, name, "Lion") { }

        /// <summary>
        /// Gets the classification of the lion
        /// </summary>
        /// <returns>Carnivores classification</returns>
        public override ClassificationAnimal GetClassificationAnimal()
        {
            return ClassificationAnimal.Carnivores;
        }

        /// <summary>
        /// Gets the favorite food of the lion
        /// </summary>
        /// <returns>Meat as favorite food</returns>
        public override FavoriteFood GetFavoriteFood()
        {
            return FavoriteFood.Meat;
        }

        /// <summary>
        /// Makes the lion say hello
        /// </summary>
        public override void SayHello()
        {
            Console.WriteLine("Rrrrr! I'm a lion " + Name);
        }
    }

    /// <summary>
    /// Class representing pigs
    /// </summary>
    [Serializable]
    [Comment("A class representing pigs")]
    public class Pig : Animal
    {
        /// <summary>
        /// Default constructor for XML serialization
        /// </summary>
        public Pig() : base()
        {
            WhatAnimal = "Pig";
        }

        /// <summary>
        /// Initializes a new instance of Pig
        /// </summary>
        /// <param name="country">Country where the pig lives</param>
        /// <param name="hideFromOtherAnimals">Whether the pig hides from other animals</param>
        /// <param name="name">Name of the pig</param>
        public Pig(string country, bool hideFromOtherAnimals, string name)
            : base(country, hideFromOtherAnimals, name, "Pig") { }

        /// <summary>
        /// Gets the classification of the pig
        /// </summary>
        /// <returns>Omnivores classification</returns>
        public override ClassificationAnimal GetClassificationAnimal()
        {
            return ClassificationAnimal.Carnivores;
        }

        /// <summary>
        /// Gets the favorite food of the pig
        /// </summary>
        /// <returns>Everything as favorite food</returns>
        public override FavoriteFood GetFavoriteFood()
        {
            return FavoriteFood.Meat;
        }

        /// <summary>
        /// Makes the pig say hello
        /// </summary>
        public override void SayHello()
        {
            Console.WriteLine("Piggy Piggy! I'm a pig " + Name);
        }
    }
}
