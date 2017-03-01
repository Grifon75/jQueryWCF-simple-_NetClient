using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WCF_MyClient1
{
    //Модель данных для отправки серверной службе
    public class PersonFromClient
    {
        public Int32 Age { get; set; }
        public String Name { get; set; }
    }
    //Контракт для передачи данных серверной службе 
    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        int addPerson(XElement _personXElement);
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Именно такой тип привязки необходим для работы со службами, размещенными в ASP.NET-приложении
            BasicHttpBinding binding = new BasicHttpBinding();
            //Для фабрики указываем адрес, который можно узнать, запустив серверную службу с тестовым клиентом
            ChannelFactory<IService2> factory = new ChannelFactory<IService2>(
                binding,
                new EndpointAddress("http://localhost:10561/Service2.svc"));
            //Создаем канал для работы с серверной службой
            IService2 channel = factory.CreateChannel();
            //Создаем пустой объект контракта данных и инициализируем его поля
            PersonFromClient personFromClient = new PersonFromClient();
            personFromClient.Age = 30;
            personFromClient.Name = "Petia";
            //Создаем сериализатор
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PersonFromClient));
            //Создаем объект XDocument и сериализуем в него объект с данными
            XDocument xDocument = new XDocument();
            using (XmlWriter xmlWriter = xDocument.CreateWriter())
            {
                xmlSerializer.Serialize(xmlWriter, personFromClient);
            }
            //Сохраняем сериализованные данные в объект XElement, подходящий для отправки серверной службе
            XElement personXElement = xDocument.Root;
            //Передаем серверной службе данные и ожидаем в ответ значение 0
            int result = channel.addPerson(personXElement);
            //Выводи в консоль ответ сервера, подтверждающий, что операция выполнена
            Console.WriteLine("result: {0}", result);
            Console.WriteLine("Для завершения нажмите <ENTER>\n");
            Console.ReadLine();
            factory.Close();
        }
    }
}
