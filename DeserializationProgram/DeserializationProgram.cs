using AnimalLibrary;
using AnimalSerialization;
using MyLogger;

namespace AnimalDeserialization
{
    /// <summary>
    /// Main rpogram class for animal deserialization demostrate
    /// </summary>
    class DeserializationProgram
    {
        /// <summary>
        /// Main entry point for deserialization demonstration
        /// </summary>
        static void Main()
        {
            var logger = new Logger(new LoggerConfig
            {
                WriteToConsole = true,
                WriteToFile = true,
                LogFilePath = $"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}_AnimalDeserialization.log",
                MinLogLevel = LogLevel.DEBUG
            });

            try
            {
                logger.Info("Starting Animal Deserialization Demo");

                var serializer = new AnimalXmlSerializer(logger);

                DeserializeAndDisplayAnimal<Cow>("cow.xml", serializer, logger);
                DeserializeAndDisplayAnimal<Lion>("lion.xml", serializer, logger);
                DeserializeAndDisplayAnimal<Pig>("pig.xml", serializer, logger);

                logger.Info("Animal Deserialization Demo completed successfully");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Animal Deserialization Demo failed");
            }
        }
        /// <summary>
        /// Deserializes an animal and displays its information
        /// </summary>
        /// <typeparam name="T">Type of animal</typeparam>
        /// <param name="fileName">Input XML file name</param>
        /// <param name="serializer">XML serializer instance</param>
        /// <param name="logger">Logger instance</param>
        static void DeserializeAndDisplayAnimal<T>(string fileName, AnimalXmlSerializer serializer, Logger logger) where T : Animal
        {
            try
            {
                logger.Info($"Deserializing {typeof(T).Name} from {fileName}");

                if (!System.IO.File.Exists(fileName))
                {
                    logger.Warning($"XML file not found: {fileName}");
                    return;
                }

                var animal = serializer.DeserializeFromXml<T>(fileName);

                logger.Info($"{animal.WhatAnimal} Information");
                logger.Info($"Name: {animal.Name}");
                logger.Info($"Country: {animal.Country}");
                logger.Info($"Hides from other animals: {animal.HideFromOtherAnimals}");
                logger.Info($"Classification: {animal.GetClassificationAnimal()}");
                logger.Info($"Favorite Food: {animal.GetFavoriteFood()}");

                animal.SayHello();

                logger.Info($"Deserialized from: {fileName}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Failed to deserialize {typeof(T).Name} from {fileName}");
            }
        }
    }
}