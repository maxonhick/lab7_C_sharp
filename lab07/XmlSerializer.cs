using AnimalLibrary;
using MyLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AnimalSerialization
{
    /// <summary>
    /// Provides XML serialisation and desirialization functional for anumal objects
    /// </summary>
    public class AnimalXmlSerializer
    {
        private readonly Logger _logger;

        /// <summary>
        /// Initialize a new instance of AnimalXmlSerializer
        /// </summary>
        /// <param name="logger"></param>
        public AnimalXmlSerializer(Logger logger) { _logger = logger; }

        /// <summary>
        /// Serialize Animal object to XML file
        /// </summary>
        /// <typeparam name="T">Type of Animal to serialize</typeparam>
        /// <param name="animal">Animal object to serialize</param>
        /// <param name="filePath">Path to output XML file</param>
        public void SerializeToXml<T>(T animal, string filePath) where T: Animal
        {
            try
            {
                _logger.Info($"Starting XML serialisation of {typeof(T).Name} to {filePath}");

                var serializer = new XmlSerializer(typeof(T));

                using (var writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, animal);
                }

                _logger.Info($"Succesfully serialized {typeof(T).Name} to {filePath}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to serialized {typeof(T).Name} to XML");
            }
        }

        /// <summary>
        /// Deserialize an Animal object from XML file
        /// </summary>
        /// <typeparam name="T">Type of Animal to deserialize</typeparam>
        /// <param name="filePath">Path to input XML file</param>
        /// <returns>Deserialized Animal object</returns>
        public T DeserializeFromXml<T>(string filePath) where T: Animal
        {
            try
            {
                _logger.Info($"Starting XML deserialization of {typeof(T).Name} from {filePath}");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"XML file not found {filePath}");
                }

                var serializer = new XmlSerializer(typeof(T));

                using (var reader = new StreamReader(filePath))
                {
                    var animal = (T)serializer.Deserialize(reader);
                    _logger.Info($"Successfully deserialized {typeof(T).Name} from {filePath}");

                    return animal;
                }
            } 
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failedto deserialize {typeof(T).Name} from XML");
                throw;
            }
        }
    }
}
