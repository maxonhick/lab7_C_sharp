using AnimalLibrary;
using System.Reflection;
using System.Xml.Linq;
using MyLogger;

namespace AnimalReflection
{
    /// <summary>
    /// Main program class for generating class diagram using reflection
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point of the application
        /// </summary>
        static void Main()
        {
            var logger = new Logger(new LoggerConfig
            {
                WriteToConsole = true,
                WriteToFile = true,
                LogFilePath = $"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}_AnimalReflection.log",
                MinLogLevel = LogLevel.DEBUG
            });
            GenerateClassDiagramXml(logger);
        }

        /// <summary>
        /// Generates XML class diagram from animal assembly using reflection
        /// </summary>
        /// <param name="logger">Logger instance for logging operations</param>
        static void GenerateClassDiagramXml(Logger logger)
        {
            logger.Info("Getting an assembly with animal classes");
            Assembly animalAssembly = typeof(Animal).Assembly;

            logger.Info("Creating the root XML element");
            XElement root = new XElement("ClassDiagram",
                new XAttribute("GeneratedAt", DateTime.Now),
                new XAttribute("Assembly", animalAssembly.FullName)
            );

            logger.Info("We get all types from the assembly");
            var types = animalAssembly.GetTypes()
                .Where(t => t.Namespace == "AnimalLibrary")
                .OrderBy(t => t.Name);
            foreach (var type in types)
            {
                XElement typeElement = CreateTypeElement(type);
                root.Add(typeElement);
                logger.Debug($"Add element type {typeElement.Name.ToString()}");
            }

            XDocument doc = new XDocument(root);
            doc.Save("ClassDiagram.xml");
            logger.Info("Save diagram to file ClassDiagram.xml");
        }

        /// <summary>
        /// Creates XML element for a type with its properties and methods
        /// </summary>
        /// <param name="type">The type to create XML element for</param>
        /// <returns>XElement representing the type</returns>
        static XElement CreateTypeElement(Type type)
        {
            XElement typeElement = new XElement(type.IsEnum ? "Enum" : "Class",
                new XAttribute("Name", type.Name),
                new XAttribute("FullName", type.FullName),
                new XAttribute("IsAbstract", type.IsAbstract)
            );

            var commentAttributes = type.GetCustomAttributes(typeof(CommentAttribute), false);
            foreach (CommentAttribute attr in commentAttributes)
            {
                typeElement.Add(new XElement("Comment", attr.Comment));
            }

            if (type.IsEnum)
            {
                var values = Enum.GetValues(type);
                foreach (var value in values)
                {
                    typeElement.Add(new XElement("Value", value.ToString()));
                }
            } else
            {
                if (type.BaseType != null && type.BaseType != typeof(object))
                {
                    typeElement.Add(new XElement("BaseType", type.BaseType.Name));
                }

                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                if (properties.Any())
                {
                    XElement propertiesElement = new XElement("Properties");
                    foreach (var prop in properties)
                    {
                        XElement propElement = new XElement("Property",
                            new XAttribute("Name", prop.Name),
                            new XAttribute("Type", prop.PropertyType.Name)
                        );
                        propertiesElement.Add(propElement);
                    }
                    typeElement.Add(propertiesElement);
                }

                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => !m.IsSpecialName);
                if (methods.Any())
                {
                    XElement methodsElement = new XElement("Methods");
                    foreach (var method in methods)
                    {
                        XElement methodElement = new XElement("Method",
                            new XAttribute("Name", method.Name),
                            new XAttribute("ReturnType", method.ReturnType.Name),
                            new XAttribute("IsAbstract", method.IsAbstract)
                        );

                        var parameters = method.GetParameters();
                        if (parameters.Any())
                        {
                            XElement paramsElement = new XElement("Parameters");
                            foreach (var param in parameters)
                            {
                                paramsElement.Add(new XElement("Parameter",
                                    new XAttribute("Name", param.Name),
                                    new XAttribute("Type", param.ParameterType.Name)
                                ));
                            }
                            methodElement.Add(paramsElement);
                        }

                        methodsElement.Add(methodElement);
                    }

                    typeElement.Add(methodsElement);
                }
            }

            return typeElement;
        }
    }
}