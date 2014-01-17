
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Clinic.Model;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Documents;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.Threading;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clinic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Report2 : Page , INotifyPropertyChanged
    {

        // A pointer back to the main page which is used to gain access to the input and output frames and their content.
        //NewPatient rootPage = null;
        public Report2()
        {
            this.InitializeComponent();
            printableArea.DataContext = this;
        }

        private Patient pa;
        public Patient Pa
        {
            get { return pa; }
            set
            {
                pa = value;
                OnPropertyChanged("Pa");
            }
        }
        //Date                  Cholestrol                  Pulse                  Temperature
        List<PatientHistory> phList = new List<PatientHistory>();
        List<Appointment> appList = new List<Appointment>();
        List<PatientEncounter> peList = new List<PatientEncounter>();
        public Report2(Patient p)
        {
            this.InitializeComponent();
            printableArea.DataContext = this;
            PatientName = p.LName + ", " + p.FName;
            ImageSource = p.ImageUri;
            Pa = p;
            getList();

            }



        private static IMobileServiceTable<PatientHistory> patientHistory = Connection.MobileService.GetTable<PatientHistory>();

        private async void getList()
        {

            // <Paragraph FontSize="22" FontWeight="Bold">
            //    Date                  Cholestrol                  Pulse                  Temperature
            //</Paragraph>
            Run myRun0 = new Run();
            myRun0.Text = "Date                  Cholestrol                  Pulse                  Temperature";
            myRun0.FontSize = 22;
            myRun0.FontWeight = FontWeights.Bold; 
            Paragraph myParagraph0 = new Paragraph();
            myParagraph0.Inlines.Add(myRun0);
            textContent.Blocks.Add(myParagraph0);

            appList = (await Appointment.ReadAppointmentsList()).Where(app => app.PatientID.Equals(Pa.PatientID)).OrderByDescending(d => d.Date).ToList();
           // peList = (await PatientEncounter.ReadPatientEncountersList()).Where(pe=>pe.pa)
            phList = (await ReadPatientHistoryList()).Where(ph => ph.PatientId.Equals(Pa.PatientID)).OrderByDescending(d => d.InfoDate).ToList();
            Run myRun1;
            Paragraph myParagraph ;
            foreach (PatientHistory ph in phList)
            {
                myRun1 = new Run();
                myRun1.Text = ph.InfoDate.Date.ToString()+ "                  " + ph.Cholestrol + "                  " + ph.Pulse + "                  " + ph.Temperature;
                // myRun1.Foreground =  ;
                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);
             



            }

            myRun1 = new Run();
            myRun1.Text = "                                                              ";
               
            myParagraph = new Paragraph();
            myParagraph.Inlines.Add(myRun1);
            textContent.Blocks.Add(myParagraph);

             myRun1 = new Run();

                myRun1.Text ="Patient Appointments";
                myRun1.FontSize = 24;
                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);
                //myRun1.Text = "Date                  Time                  Complain                  Doctor                  Diagnostics                  ";
                //myRun1.FontSize = 22;
                //myParagraph = new Paragraph();
                //myParagraph.Inlines.Add(myRun1);
                //textContent.Blocks.Add(myParagraph);
            foreach(Appointment app in appList)
            {
                myRun1 = new Run();
                myRun1.Text = "                                                              ";

                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);

                myRun1 = new Run();
                myRun1.Text = "                                                              ";

                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);


                User Doc = (await User.getUser(app.UserID));
                PatientEncounter pe;
                try
                {
                    pe = (await PatientEncounter.getPatientEncounter(app));
                }
                catch
                {
                     pe = null;
                }
                myRun1 = new Run();
                myRun1.Text ="Date :"+ app.Date.Date.ToString();
               
                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);

                myRun1 = new Run();
                myRun1.Text = "Time :" + app.TimeFrom.ToString();

                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);

            

                myRun1 = new Run();
                myRun1.Text = "Doctor :" + Doc.LName + ", " + Doc.FName;

                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);

                myRun1 = new Run();
                myRun1.Text = "Complain :" + app.Complaint;

                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);


                myRun1 = new Run();
                try
                {
                    myRun1.Text = "Diagnostics :" + pe.Diagnostics;
                }
                catch
                {
                    myRun1.Text = "Diagnostics :" + "No patient encounter has been made for this appointment";
                }

                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);

                myRun1 = new Run();
                try
                {
                    myRun1.Text = "Notes :" + pe.Notes;

                }
                catch
                {
                    myRun1.Text = "Notes :" + "No patient encounter has been made for this appointment";
                }

                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);

                myRun1 = new Run();
                try
                {
                    myRun1.Text = "Drugs :" + pe.Drugs;
                }
                catch
                {
                    myRun1.Text = "Drugs :" + "No patient encounter has been made for this appointment";

                }

                myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun1);
                textContent.Blocks.Add(myParagraph);
            }


            //for (int i = 0; i < phList.Count; i++)
            //{
            //    try
            //    {
            //        //items2.Add(new NameValueItem { Name = PhList.ElementAt(i).InfoDate.Year.ToString(), Value = Convert.ToInt32(PhList.ElementAt(i).Cholestrol) });
            //        items2.Add(new NameValueItem { Name = (phList.ElementAt(i).InfoDate.Year.ToString() + "/" + phList.ElementAt(i).InfoDate.Month.ToString() + "/" + phList.ElementAt(i).InfoDate.Day.ToString()), Value = Convert.ToDouble(phList.ElementAt(i).Cholestrol) });

            //    }
            //    catch
            //    {

            //    }
            //    try
            //    {
            //        //items.Add(new NameValueItem { Name = PhList.ElementAt(i).InfoDate.Year.ToString(), Value = Convert.ToInt32(PhList.ElementAt(i).Temperature) });
            //        items.Add(new NameValueItem { Name = (phList.ElementAt(i).InfoDate.Year.ToString() + "/" + phList.ElementAt(i).InfoDate.Month.ToString() + "/" + phList.ElementAt(i).InfoDate.Day.ToString()), Value = Convert.ToDouble(phList.ElementAt(i).Temperature) });

            //    }
            //    catch
            //    {
            //    }

            //    try
            //    {
            //        //items3.Add(new NameValueItem { Name = PhList.ElementAt(i).InfoDate.Year.ToString(), Value = Convert.ToInt32(PhList.ElementAt(i).BloodPressure) });
            //        items3.Add(new NameValueItem { Name = (phList.ElementAt(i).InfoDate.Year.ToString() + "/" + phList.ElementAt(i).InfoDate.Month.ToString() + "/" + phList.ElementAt(i).InfoDate.Day.ToString()), Value = Convert.ToDouble(phList.ElementAt(i).Pulse) });

            //    }
            //    catch
            //    {

            //    }

            //}



            //((LineSeries)this.LineChart.Series[0]).ItemsSource = items2;
            //((LineSeries)this.LineChart3.Series[0]).ItemsSource = items;
            //((LineSeries)this.LineChart2.Series[0]).ItemsSource = items3;
           


        }

        //List<NameValueItem> items = new List<NameValueItem>();

        //List<NameValueItem> items2 = new List<NameValueItem>();
        //List<NameValueItem> items3 = new List<NameValueItem>();

        public static async Task<List<PatientHistory>> ReadPatientHistoryList()
        {

            List<PatientHistory> phList = await patientHistory.ToListAsync();
            return phList;

        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private string patientName;
        public string PatientName
        {
            get { return patientName; }
            set
            {
                patientName = value;
                OnPropertyChanged("PatientName");
            }
        }

        private string imageSource;
        public string ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        NewPatient  rootPage = null;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = ((Dictionary<Page, Patient>)e.Parameter).Keys.First() as NewPatient;

            if (e.Parameter != null)
            {
                Patient patient = patient = ((Dictionary<Page, Patient>)e.Parameter).Values.First();
               
                PatientName = patient.LName + ", " + patient.FName;
                ImageSource = patient.ImageUri;
                Pa = patient;
                //phList = (await PatientHistory.ReadPatientHistoryList()).Where(ph => ph.PatientId.Equals(Pa.PatientID)).ToList();

               
                //   Run myRun1;
                //foreach(PatientHistory ph in phList){
                //    myRun1 = new Run();
                //    myRun1.Text = ph.InfoDate.Date +"                  "+ph.Cholestrol+"                  "+ph.Pulse+"                  "+ph.Temperature  ;
                //    Paragraph myParagraph = new Paragraph();
                //    myParagraph.Inlines.Add(myRun1);
                //    textContent.Blocks.Add(myParagraph);
                //}

                getList();
            
              
            }
        }

        public class NameValueItem
        {
            public string Name { get; set; }
            public double Value { get; set; }
        }
    }
}
