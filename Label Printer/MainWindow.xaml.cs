using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Input;

namespace Label_Printer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //when the application loaded
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //display the label printer
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                
                listBox1.Items.Add(printer);
                

            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        //boolean for check the generate button click
        private bool button1WasClicked = false;

        //generate button
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            //ensure the part order number is not empty
            if (textBoxPONo.Text.Length > 12)
            {
                MessageBox.Show("Sorry, PO No. only accept 12 digits!");
                textBoxPON.Text = "PO: " + "Error!";
            }
            else if (textBoxPONo.Text.Length <= 0)
            {
                MessageBox.Show("Sorry, PO No. cannot be empty!");
                textBoxPON.Text = "PO: " + "Error!";
            }
            else
            {
                textBoxPON.Text = "PO: " + textBoxPONo.Text;

                //ensure the part number is not empty
                if (textBoxPartNo.Text.Length <= 0)
                {
                    MessageBox.Show("Sorry, Part No. cannot be empty!");
                    textBoxBarcode.Text = "*" + "Error!" + "*";
                    textBoxPN.Text = "Error!";
                }
                else
                {
                    textBoxBarcode.Text = "*" + textBoxPartNo.Text + "*";
                    textBoxPN.Text = textBoxPartNo.Text;
                    textBoxDate.Text = DateTime.Now.ToString("MMM dd,yyyy");
                    button1WasClicked = true;
                }

            }
            //get selected copies
            //set number of copies to print
            int selectedValue = (int)comboBox1.SelectedIndex + 1;

            //show selected printer name
            //string printerName = listBox1.GetItemText(listBox1.SelectedItem);
            try
            {
                string printerName = listBox1.SelectedItem.ToString();
                toolStripStatusLabel1.Content = "Status:\t"+"Print Copies: " + selectedValue + ". Printer: " + printerName;
            }
            catch (Exception)
            {

                MessageBox.Show("Please select a printer!");
                toolStripStatusLabel1.Content = "Status:\t" + "Please select a printer!";
            }
            
            
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            textBoxPONo.Focus();

            if (textBoxPONo.Text.Length > 12)
            {
                MessageBox.Show("Sorry, PO No. only accept maximum 12 digits!");
            }
            else if (textBoxPONo.Text.Length <= 0)
            {
                MessageBox.Show("Sorry, PO No. cannot be empty!");
            }
            else if (textBoxPartNo.Text.Length <= 0)
            {
                MessageBox.Show("Sorry, Part No. cannot be empty!");
            }
            else if (button1WasClicked == false)
            {
                MessageBox.Show("Sorry, you have to generate before print!");
            }
            else
            {
                //get selected printer name
                string printerName = listBox1.SelectedItem.ToString();

                //print doc object
                System.Drawing.Printing.PrintDocument myPrintDocument1 = new System.Drawing.Printing.PrintDocument();

                //PrintDialog myPrinDialog1 = new PrintDialog();

                myPrintDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(myPrintDocument2_PrintPage);

                //myPrinDialog1.Document = myPrintDocument1;
                //if (myPrinDialog1.ShowDialog() == DialogResult.OK)
                //{
                //    myPrintDocument1.Print();
                //}


                //use the selected printer name
                myPrintDocument1.PrinterSettings.PrinterName = printerName;
                //set number of copies to print
                int selectedValue = (int)comboBox1.SelectedIndex + 1;
                myPrintDocument1.PrinterSettings.Copies = (short)selectedValue;
                try
                {
                    toolStripStatusLabel1.Content = "Status:\t" + "Printing...";
                    myPrintDocument1.Print();
                    toolStripStatusLabel1.Content = "Status:\t" + "Succeed.";
                }
                catch (Exception)
                {

                    MessageBox.Show("Please select a printer!");
                    toolStripStatusLabel1.Content = "Status:\t" + "Please select a printer!";
                }

            }

            textBoxPONo.Text = "";
            textBoxPartNo.Text = "";
        }

        private void myPrintDocument2_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            #region
            //Bitmap myBitmap2 = new Bitmap(panel1.Width, panel1.Height);


            //panel1.DrawToBitmap(myBitmap2, new Rectangle(0, 0, panel1.Width, panel1.Height));


            ////e.Graphics.DrawImage(myBitmap2, 0, 0);
            ////e.Graphics.DrawImage(this.ConvertTextToImage(panel1.Text, "Code 3 de 9", 10, Color.White, Color.Black, panel1.Width, panel1.Height), 0, 0);

            //myBitmap2.Dispose();
            #endregion

            //set font
            Font printFont = new Font("Code 3 de 9", 14);
            Font printFont2 = new Font("Arial", 6);
            Font printFont3 = new Font("Arial", 8);

            //set the bar-code width
            Matrix m = new Matrix();
            m.Scale(1.5F, 0.8F); //1.5times width than original width, 0.8 times height than original height

            //begin scale the bar-code width
            e.Graphics.Transform = m;
            e.Graphics.DrawString(textBoxBarcode.Text, printFont, Brushes.Black, 22, 2, new StringFormat());
            //reset the font to regular width
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(textBoxPON.Text, printFont2, Brushes.Black, 5, 20, new StringFormat());
            e.Graphics.DrawString(textBoxPartNo.Text, printFont3, Brushes.Black, 80, 18, new StringFormat());
            e.Graphics.DrawString(DateTime.Now.ToString("MMM dd,yyyy"), printFont2, Brushes.Black, 175, 20, new StringFormat());


        }

        private void textBoxPONo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxPartNo.Focus();

                e.Handled = true;
            }
        }

        private void textBoxPartNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxPONo.Focus();

                e.Handled = true;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
