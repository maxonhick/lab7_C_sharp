using AnimalLibrary;
using MyLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalSerialization
{
    /// <summary>
    /// Main program class for animal serialization demonstration
    /// </summary>
    class SerializationProgram
    {
        /// <summary>
        /// Main entry point for serialization demonstration
        /// </summary>
        static void Main()
        {
            var logger = new Logger(new LoggerConfig
            {
                WriteToConsole = true,
                WriteToFile = true,
                LogFilePath = $"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}_AnimalSerialization.log",
                MinLogLevel = LogLevel.DEBUG
            });

            try
            {
                logger.Info("Starting animal serialization demo");

                var cow = new Cow("USA", false, "Bessie");
                var lion = new Lion("Africa", true, "Simba");
                var pig = new Pig("Germany", false, "Porky");

                logger.Debug("Initializing animal properties");

                var serializer = new AnimalXmlSerializer(logger);

                SerializeAnimal(cow, "cow.xml", serializer, logger);
                SerializeAnimal(lion, "lion.xml", serializer, logger);
                SerializeAnimal(pig, "pig.xml", serializer, logger);

                logger.Info("Animal serialization demo complered saccessfully");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Animal serialization demo failed");
            }
        }

        /// <summary>
        /// Serialize an animal and displays its ifromation
        /// </summary>
        /// <typeparam name="T">Type of animal</typeparam>
        /// <param name="animal">Animal instance</param>
        /// <param name="fileName">Output XML file name</param>
        /// <param name="serializer">XML serializer instance</param>
        /// <param name="logger">Logger instance</param>
        static void SerializeAnimal<T>(T animal, string fileName, AnimalXmlSerializer serializer, Logger logger) where T: Animal
        {
            logger.Info($"Processing {animal.WhatAnimal} named {animal.Name}");

            logger.Info($"{animal.WhatAnimal} Information");
            logger.Info($"Name: {animal.Name}");
            logger.Info($"Country: {animal.Country}");
            logger.Info($"Hides from other animals: {animal.HideFromOtherAnimals}");
            logger.Info($"Classification: {animal.GetClassificationAnimal()}");
            logger.Info($"Favorite Food: {animal.GetFavoriteFood()}");

            animal.SayHello();

            serializer.SerializeToXml(animal, fileName);

            logger.Info($"Serialized to {fileName}");
        }
    }
}
