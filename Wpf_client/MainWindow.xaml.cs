using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Wpf_client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
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
    public partial class MainWindow : Window
    {
        Color leaveGrid;
        public MainWindow()
        {
            InitializeComponent();
            leaveGrid = Color.FromArgb(100, 0, 0, 100);
            MyWindow.Background = new SolidColorBrush(leaveGrid);
        }

        private void MyWindow_Closed(object sender, EventArgs e)
        {

        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        Point p, p_, p__;
        bool flag02 = true, flag;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == buttonCancel)
                this.Close();
            else if (sender == buttonLoading)
            {
                //MessageBox.Show(proxy.GetDataTranslate().Count()+"");
                //foreach (Alfa.DataTranslate item in proxy.GetDataTranslate())
                //{
                //    DataTranslate obj = new DataTranslate();
                //    obj.Word = item.Word;
                //    obj.Description = item.Description;
                //    //MessageBox.Show(item.Word+ item.Description);
                //    Mas.dictionary.Add(obj);
                //}
                ChannelFactory<IService2> factory = null;
                try
                {
                    BasicHttpBinding binding = new BasicHttpBinding();
                    factory = new ChannelFactory<IService2>(
                        binding,
                        new EndpointAddress("http://localhost:10561/Service2.svc"));
                    IService2 channel = factory.CreateChannel();
                    PersonFromClient personFromClient = new PersonFromClient();
                    personFromClient.Name = textBoxName.Text;
                    personFromClient.Age = Int32.Parse(textBoxAge.Text);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(PersonFromClient));
                    XDocument xDocument = new XDocument();
                    using (XmlWriter xmlWriter = xDocument.CreateWriter())
                    {
                        xmlSerializer.Serialize(xmlWriter, personFromClient);
                    }
                    XElement personXElement = xDocument.Root;
                    int result = channel.addPerson(personXElement);
                    textBlockStatus.Text = "Данные загружены на сервер успешно";
                }
                catch (Exception)
                {
                    textBlockStatus.Text = "Загрузка данных не удалась";
                }
                finally
                {
                    factory.Close();
                }
            }
        }

        private void Text_Changed(object sender, TextChangedEventArgs e)
        {
            textBlockStatus.Text = "Element";
        }

        private void MyWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            flag = true;
        }

        private void MyWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            flag = false;
            flag02 = true;
        }

        private void MyWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                if (flag02)
                {
                    p__ = e.GetPosition(this);
                    flag02 = false;
                }
                p = e.GetPosition(this);
                p_ = PointToScreen(p);
                Top = p_.Y - p__.Y;
                Left = p_.X - p__.X;
            }
        }
    }
}
